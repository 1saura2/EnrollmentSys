﻿using ABKS_project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ABKS_project.Controllers
{
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
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        } 
        public IActionResult Product()
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
