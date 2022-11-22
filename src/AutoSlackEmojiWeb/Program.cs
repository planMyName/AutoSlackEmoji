using Amazon.S3;
using AutoSlackEmoji.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();

// register DI
builder.Services.AddTransient<IAmazonS3, AmazonS3Client>(s =>
{
    var awsAccessKey = "accesskey";
    var awsSecretKey = "secretKey";
    var s3Client = new AmazonS3Client(awsAccessKey, awsSecretKey, Amazon.RegionEndpoint.APSoutheast2);
    return s3Client;
});
builder.Services.AddTransient<IFileRepository, S3Repository>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
