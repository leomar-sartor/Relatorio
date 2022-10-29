using Microsoft.AspNetCore.Mvc;
using Relatorio.Models;
using Rotativa.AspNetCore;
using System.Diagnostics;

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
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}