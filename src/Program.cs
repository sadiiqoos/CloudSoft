using MongoDB.Driver;
using CloudSoft.Repositories;
using CloudSoft.Services;
using CloudSoft.Models;
using CloudSoft.Configurations;
using CloudSoft.Storage;

var builder = WebApplication.CreateBuilder(args);
// ... the rest of your code ...var builder = WebApplication.CreateBuilder(args);

// 1. Add basic MVC services
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor(); // Required for URL generation in Storage

// --- MONGODB SETUP ---
bool useMongoDb = builder.Configuration.GetValue<bool>("FeatureFlags:UseMongoDb");

if (useMongoDb)
{
    builder.Services.Configure<MongoDbOptions>(
        builder.Configuration.GetSection(MongoDbOptions.SectionName));

    builder.Services.AddSingleton<IMongoClient>(serviceProvider => {
        var options = builder.Configuration.GetSection(MongoDbOptions.SectionName).Get<MongoDbOptions>();
        return new MongoClient(options?.ConnectionString);
    });

    builder.Services.AddSingleton<IMongoCollection<Subscriber>>(serviceProvider => {
        var options = builder.Configuration.GetSection(MongoDbOptions.SectionName).Get<MongoDbOptions>();
        var mongoClient = serviceProvider.GetRequiredService<IMongoClient>();
        var database = mongoClient.GetDatabase(options?.DatabaseName);
        return database.GetCollection<Subscriber>(options?.SubscribersCollectionName);
    });

    builder.Services.AddSingleton<ISubscriberRepository, MongoDbSubscriberRepository>();
    Console.WriteLine("✅ System is using: Azure MongoDB Repository");
}
else
{
    builder.Services.AddSingleton<ISubscriberRepository, InMemorySubscriberRepository>();
    Console.WriteLine("ℹ️ System is using: In-Memory Repository");
}

// --- AZURE BLOB STORAGE SETUP ---
builder.Services.Configure<AzureBlobOptions>(
    builder.Configuration.GetSection(AzureBlobOptions.SectionName));

bool useAzureStorage = builder.Configuration.GetValue<bool>("FeatureFlags:UseAzureStorage");

if (useAzureStorage)
{
    builder.Services.AddSingleton<IImageService, AzureBlobImageService>();
    Console.WriteLine("✅ System is using: Azure Blob Storage for images");
}
else
{
    builder.Services.AddSingleton<IImageService, LocalImageService>();
    Console.WriteLine("ℹ️ System is using: Local storage for images");
}

// 2. Register Business Logic (NewsletterService)
builder.Services.AddScoped<INewsletterService, NewsletterService>();

var app = builder.Build();

// --- PIPELINE SETUP ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Added this to serve local images correctly
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();