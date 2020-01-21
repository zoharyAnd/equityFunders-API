using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using cFunding.Models;
using cFunding.Data;
using Newtonsoft.Json;
using PayPal.Api;


namespace cFunding.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppliContext _context;

        public HomeController(AppliContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            /* filter : take only validated projects */
            List<user> users = _context.users.ToList(); 
            List<category> categories = _context.categories.ToList(); 
            var listValidPj = _context.projects.Where(p => p.validatedProject == true);
            return Json(listValidPj);
        }
         // GET: home/login/
        public async Task<IActionResult> login(string useremail, string userpassword)
        {
            var currentUser =  await _context.users.FirstOrDefaultAsync(m => m.useremail == useremail && m.userpassword == userpassword);
            
            if (currentUser == null)
            {
                return NotFound(); 
            }
            
            return Json(currentUser); 
        }
        

    }
}
