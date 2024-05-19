using FileUploadDownloader.FileInterfaces;
using FileUploadDownloader.Services;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Inject wwwroot/Uploads path to dependency container via IFileProvider. 
builder.Services.AddSingleton<IFileProvider>(services =>
{ 
    var env = services.GetRequiredService<IWebHostEnvironment>();
    return new PhysicalFileProvider(
        Path.Combine(env.WebRootPath, "Uploads"));
});

//Register this service in the dependency container so that it can be injected into a controller
builder.Services.AddTransient<IBufferedFileUploadService, BufferedFileUploadLocalService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
