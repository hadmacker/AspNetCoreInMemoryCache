using AspNetCoreInMemoryCache.Web.Models;
using AspNetCoreInMemoryCache.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreInMemoryCache.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICustomerRepository customerRepository;

        public HomeController(ILogger<HomeController> logger, ICustomerRepository customerRepository)
        {
            _logger = logger;
            this.customerRepository = customerRepository;
        }

        public async Task<IActionResult> Index()
        {
            var customers = await customerRepository.RetrieveAllEntities();
            return View(customers);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CustomerEntity model)
        {
            if(ModelState.IsValid)
            {
                customerRepository.InsertOrMergeEntityAsync(model);
                return RedirectToAction(nameof(Create));
            }
            throw new Exception("bad post");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
