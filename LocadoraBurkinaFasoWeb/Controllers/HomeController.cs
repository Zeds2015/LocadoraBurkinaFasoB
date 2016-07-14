using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LocadoraBurkinaFasoWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Chama_Outra_Action()
        {
            return RedirectToAction("Criar_Cliente", "Cliente");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Sobre a Locadora Burkina Faso";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contatos - Locadora Burkina Faso";

            return View();
        }
    }
}