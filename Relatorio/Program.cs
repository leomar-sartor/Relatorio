using Microsoft.CodeAnalysis.CSharp.Syntax;
using Relatorio;
using Rotativa.AspNetCore;
using System.Runtime.InteropServices;
using System.Runtime.Loader;

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

//DinkPDF
List<string> bibliotecas = new List<string>() { "libwkhtmltox" };
var customLoad = new CustomAssemblyLoadContext();
var arquitetura = RuntimeInformation.OSArchitecture.ToString().ToLower();
var arquiteturaProcesso = RuntimeInformation.ProcessArchitecture.ToString().ToLower();

var extensao = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "dll" :
                             RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "so" :
                               RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "dylib" : "";

foreach (var biblioteca in bibliotecas)
{
    try
    {
        var caminhoBiblioteca = Path.Combine(Directory.GetCurrentDirectory(), $"BibliotecaTerceiros\\{arquiteturaProcesso}\\{biblioteca}.{extensao}");
        if (File.Exists(caminhoBiblioteca))
            customLoad.LoadUnmanagedLibrary(caminhoBiblioteca);
        //else
        //LogService.WriteLine($"Não foi possível localizar a biblioteca {biblioteca} no caminho {caminhoBiblioteca}");
    }
    catch (Exception ex)
    {
        //LogService.WriteLine($"Ocorreu uma exceção ao carregar a biblioteca {biblioteca}.{Environment.NewLine}Mensagem:{Environment.NewLine}    {ex.Message}{Environment.NewLine}Stack trace:{Environment.NewLine}{ex.StackTrace}");
        throw new Exception(ex.StackTrace);
    }
}

//Rotativa
IWebHostEnvironment env = app.Environment;
RotativaConfiguration.Setup((Microsoft.AspNetCore.Hosting.IHostingEnvironment) env);

app.Run();

//https://github.com/HakanL/WkHtmlToPdf-DotNet/blob/master/src/WkHtmlToPdf-DotNet/WkHtmlModuleLinux64.cs
//https://tecnobyte.com.br/124613544/Tecnobyte-SAC/O-servidor-RPC-nao-esta-disponivel-O-que-fazer
//https://github.com/webgio/Rotativa.AspNetCore/blob/master/Rotativa.AspNetCore.Demo/Startup.cs

//https://medium.com/volosoft/convert-html-and-export-to-pdf-using-dinktopdf-on-asp-net-boilerplate-e2354676b357

//SelectPDf tem limitação