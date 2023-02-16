using Microsoft.AspNetCore.Mvc;
using Relatorio.Models;
using Rotativa.AspNetCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Rotativa.AspNetCore.Options;
using DinkToPdf.Contracts;
using DinkToPdf;
using System.Net.Mime;

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
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment, IConverter converter)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _converter = converter;
        }

        public IActionResult Index()
        {
            return View();  
        }

        public IActionResult ImprimirPedido()
        {
            var pedido = new PedidoViewModel();

            var pdf = new ViewAsPdf(pedido, null);

            return pdf;
        }

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




        public IActionResult Privacy()
        {
            var htmlContent = $@"
	<!DOCTYPE html>
	<html lang=""en"">
	<head>
		<style>
		p{{
			width: 80%;
		}}
		</style>
	</head>
	<body>
		<h1>Some heading</h1>
		<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>
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
                HtmlContent = htmlContent,
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}