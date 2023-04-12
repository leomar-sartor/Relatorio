using Microsoft.AspNetCore.Mvc;
using Relatorio.Models;
using Rotativa.AspNetCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Rotativa.AspNetCore.Options;
using DinkToPdf.Contracts;
using DinkToPdf;
using System.Net.Mime;
using ChromeHtmlToPdfLib;
using ChromeHtmlToPdfLib.Settings;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Relatorio.Controllers
{
    public class PedidoViewModel
    {
        public PedidoViewModel()
        {
            Lote = new Lote();
            Locais = new List<Local>();
        }

        public Lote Lote { get; set; }

        public List<Local> Locais { get; set; }

    }

    public class Lote
    {
        public Lote()
        {
            Data = DateTime.Today;
        }
        public DateTime Data { get; set; }

        public string Identificador { get; set; }

        public string Peoduto { get; set; }

    }

    public class Local
    {
        public Local()
        {
            Data = DateTime.Today;
        }
        public DateTime Data { get; set; }

        public string Nome { get; set; }
    }



    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<HomeController> _logger;
        private readonly IConverter _converter;
        protected readonly ICompositeViewEngine _compositeViewEngine;

        public HomeController(
            ILogger<HomeController> logger,
            IWebHostEnvironment webHostEnvironment,
            IConverter converter,
            ICompositeViewEngine compositeViewEngine
            )
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _converter = converter;
            _compositeViewEngine = compositeViewEngine;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Rotativa
        public IActionResult ImprimirPedido()
        {
            var pedido = new PedidoViewModel();

            var pdf = new ViewAsPdf(pedido, null);

            return pdf;
        }

        //Rotativa
        public IActionResult ImprimirPedidoComCabecalho()
        {
            var pedido = new PedidoViewModel();
            //string webRootPath = _webHostEnvironment.WebRootPath;
            string contentRootPath = _webHostEnvironment.ContentRootPath;
            //var pathHeader = Path.Combine(webRootPath, "Report", "Header.html");

            //var pathHeader = Path.Combine(contentRootPath, "Views", "Shared", "Header.html");
            var pathFooter = Path.Combine(contentRootPath, "Views", "Shared", "Footer.html");

            string customSwitches = string.Format(//"--header-html  \"{0}\" " +
                                                  //"--header-spacing \"2\" " +
                                   "--footer-html \"{0}\" " +
                                   "--footer-spacing \"2\" " +
                                   "--footer-font-size \"8\" " +
                                   "--footer-font-name \"Open Sans\" " +
                                   "--footer-right \"[page] de [toPage]\" "
                                   //"--header-font-size \"10\" "
                                   , pathFooter);
            //"--footer-font-size \"8\" " +
            //);

            var pdf = new ViewAsPdf
            {
                Model = pedido,
                FileName = "Contact.pdf",
                PageSize = Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                CustomSwitches = customSwitches
            };

            return pdf;
        }



        //DinkToPDF
        public async Task<IActionResult> Privacy()
        {
            //using (var stringWriter = new StringWriter())
            //{
            //    var viewResult = _compositeViewEngine.FindView(ControllerContext, "ImprimirPedidoComCabecalho", false);

            //    if (viewResult.View == null)
            //    {
            //        throw new Exception();
            //    }

            //    var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());

            //    var viewContext = new ViewContext(
            //        ControllerContext,
            //        viewResult.View,
            //        viewDictionary,
            //        TempData,
            //        stringWriter,
            //        new HtmlHelperOptions()
            //        );

            //    await viewResult.View.RenderAsync(viewContext);

            //    var pageSettings = new PageSettings(ChromeHtmlToPdfLib.Enums.PaperFormat.A4);
            //    var stream = new MemoryStream();
            //    using var converter = new Converter();

            //    converter.ConvertToPdf(stringWriter.ToString(), stream, pageSettings);



            //    converter.ConvertToPdf(stringWriter.ToString(), stream, pageSettings);


            //    var globalSettings = new GlobalSettings
            //    {
            //        ColorMode = ColorMode.Color,
            //        Orientation = DinkToPdf.Orientation.Portrait,
            //        PaperSize = PaperKind.A4,
            //        Margins = new MarginSettings { Top = 18, Bottom = 18 },
            //    };

            //    var objectSettings = new ObjectSettings
            //    {
            //        PagesCount = true,
            //        HtmlContent = stringWriter.ToString(),
            //        WebSettings = { DefaultEncoding = "utf-8" },
            //        HeaderSettings = { FontSize = 10, Right = "Page [page] of [toPage]", Line = true },
            //        FooterSettings = { FontSize = 8, Center = "PDF demo from JeminPro", Line = true },
            //    };

            //    var htmlToPdfDocument = new HtmlToPdfDocument()
            //    {
            //        GlobalSettings = globalSettings,
            //        Objects = { objectSettings },
            //    };

            //    var pdf = _converter.Convert(htmlToPdfDocument);

            //    return File(pdf, MediaTypeNames.Application.Pdf, "Teste.pdf");
            //}

            var htmlContent = $@"
	<!DOCTYPE html>
	<html lang=""en"">
	<head>

<link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha3/dist/css/bootstrap.min.css'/>

        <style>
		p{{
			width: 80%;
		}}
		</style>
	</head>
	<body>
		<h1>Some heading</h1>
		<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>


<section class='section header'>
    <div class='row'>

        <div class='col-xl-4'>
            <div class='card' style='border: none;'>
                <div class='card-body profile-card pt-4 d-flex flex-column align-items-center'>
                    <img src='~/MeuAgro.png' alt='Logo' style='max-height: 120px;'>
                    <h2>Fenos Pinhal</h2>
                </div>
            </div>
        </div>

        <div class='col-xl-8'>
            <div class='card' style='border: none;'>
                <div class='card-body pt-3'>
                    <h5 class='card-title'>Parametrôs de Relatório</h5>

                    <div class='row pt-2'>
                        <div class='col-lg-3 col-md-4 label '>Data Inicial:</div>
                        <div class='col-lg-9 col-md-8'>01/04/2023</div>
                    </div>

                    <div class='row'>
                        <div class='col-lg-3 col-md-4 label'>Data Final</div>
                        <div class='col-lg-9 col-md-8'>30/04/2023</div>
                    </div>

                    <div class='row'>
                        <div class='col-lg-3 col-md-4 label'>Agrupamento</div>
                        <div class='col-lg-9 col-md-8'>Sem Agrupamento</div>
                    </div>

                    <div class='row'>
                        <div class='col-lg-3 col-md-4 label'>Analitico / Sintetico</div>
                        <div class='col-lg-9 col-md-8'>Sintetico</div>
                    </div>
                </div>
            </div>
        </div>

    </div>
</section>

<hr class='border border-success border-1 opacity-50'>

	</body>
	</html>
	";

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 18, Bottom = 18 },
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlContent.ToString(),
                WebSettings = { DefaultEncoding = "utf-8" },
                HeaderSettings = { FontSize = 10, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontSize = 8, Center = "PDF demo from JeminPro", Line = true },
            };

            var htmlToPdfDocument = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings },
            };

            var pdf = _converter.Convert(htmlToPdfDocument);

            return File(pdf, MediaTypeNames.Application.Pdf, "Teste.pdf");
        }

        //Rotativa
        public IActionResult Contact()
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string contentRootPath = _webHostEnvironment.ContentRootPath;

            var pathHeader = Path.Combine(contentRootPath, "Views", "Shared", "Header.html");
            var pathFooter = Path.Combine(contentRootPath, "Views", "Shared", "Footer.html");

            string header = pathHeader;
            string footer = pathFooter;

            string customSwitches = string.Format("--header-html  \"{0}\" " +
                                   "--header-spacing \"0\" " +
                                   "--footer-html \"{1}\" " +
                                   "--footer-spacing \"10\" " +
                                   "--footer-font-size \"10\" " +
                                   "--header-font-size \"10\" ", header, footer);

            var pdf = new ViewAsPdf
            {
                FileName = "Contact.pdf",
                PageSize = Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                CustomSwitches = customSwitches
            };

            return pdf;

        }


        //sample: https://www.mikesdotnetting.com/article/364/exploring-generating-pdf-files-from-html-in-asp-net-core
        //ChromeHtmlToPdf
        public async Task<IActionResult> ImprimirGoogle()
        {

            using (var stringWriter = new StringWriter())
            {
                var viewResult = _compositeViewEngine.FindView(ControllerContext, "ImprimirPedidoComCabecalho", false);

                if (viewResult.View == null)
                {
                    throw new Exception();
                }

                var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());

                var viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    viewDictionary,
                    TempData,
                    stringWriter,
                    new HtmlHelperOptions()
                    );

                await viewResult.View.RenderAsync(viewContext);

                var pageSettings = new PageSettings(ChromeHtmlToPdfLib.Enums.PaperFormat.A4);
                var stream = new MemoryStream();
                using var converter = new Converter();

                converter.ConvertToPdf(stringWriter.ToString(), stream, pageSettings);



                converter.ConvertToPdf(stringWriter.ToString(), stream, pageSettings);

                return File(stream.ToArray(), MediaTypeNames.Application.Pdf, "ReportChrome.pdf");
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}