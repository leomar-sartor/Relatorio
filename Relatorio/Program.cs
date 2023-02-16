using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Relatorio;
using Rotativa.AspNetCore;
using System.Runtime.InteropServices;
using System.Runtime.Loader;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

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
        var caminhoBiblioteca = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\BibliotecaTerceiros\\{arquiteturaProcesso}\\{biblioteca}.{extensao}");
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

builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
//End Dink To PDF


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




//Rotativa
IWebHostEnvironment env = app.Environment;
RotativaConfiguration.Setup((Microsoft.AspNetCore.Hosting.IHostingEnvironment) env);
//EndRotativa



app.Run();

//https://github.com/HakanL/WkHtmlToPdf-DotNet/blob/master/src/WkHtmlToPdf-DotNet/WkHtmlModuleLinux64.cs
//https://tecnobyte.com.br/124613544/Tecnobyte-SAC/O-servidor-RPC-nao-esta-disponivel-O-que-fazer
//https://github.com/webgio/Rotativa.AspNetCore/blob/master/Rotativa.AspNetCore.Demo/Startup.cs

//https://github.com/GilbertoCastro/Rotativo/tree/master/Rotativo_exemplo
//https://bhavdiptala.blogspot.com/2016/05/displaying-headers-and-footers-in-pdf.html
//SelectPDf tem limitação


//Outro links
//https://jeminpro.com/net-core-create-pdf-using-dinktopdf/
//https://www.mikesdotnetting.com/article/364/exploring-generating-pdf-files-from-html-in-asp-net-core
//https://medium.com/volosoft/convert-html-and-export-to-pdf-using-dinktopdf-on-asp-net-boilerplate-e2354676b357
