using Rotativa.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

IWebHostEnvironment env = app.Environment;
var architectureFolder = (IntPtr.Size == 8) ? "64 bit" : "32 bit";
var wkHtmlToPdfPath = Path.Combine(env.ContentRootPath, $"v0.12.4\\{architectureFolder}\\libwkhtmltox");
//CustomAssemblyLoadContext context = new CustomAssemblyLoadContext();
//context.LoadUnmanagedLibrary(wkHtmlToPdfPath)

RotativaConfiguration.Setup((Microsoft.AspNetCore.Hosting.IHostingEnvironment) env);

app.Run();

//https://github.com/HakanL/WkHtmlToPdf-DotNet/blob/master/src/WkHtmlToPdf-DotNet/WkHtmlModuleLinux64.cs
//https://tecnobyte.com.br/124613544/Tecnobyte-SAC/O-servidor-RPC-nao-esta-disponivel-O-que-fazer
//https://github.com/webgio/Rotativa.AspNetCore/blob/master/Rotativa.AspNetCore.Demo/Startup.cs