﻿using Microsoft.AspNetCore.Mvc;

namespace ProfileProject.Areas.Main.Controllers
{
    [Area("Main")]
    [Route("/")]
    public class MainController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //TO-DO
        //[Route("forbidden/")]
        //public IActionResult Forbidden()
        //{
        //    return View();
        //}
    }
}
