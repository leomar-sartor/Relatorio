using Microsoft.AspNetCore.Mvc;
using Relatorio.Models;
using Rotativa.AspNetCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Rotativa.AspNetCore.Options;

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

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
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
                PageOrientation = Orientation.Portrait,
                CustomSwitches = customSwitches
            };

            return pdf;
        }

        public IActionResult Privacy()
        {
            return View();
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
                PageOrientation = Orientation.Landscape,
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