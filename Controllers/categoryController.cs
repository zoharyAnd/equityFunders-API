using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using cFunding.Data;
using cFunding.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace cFunding.Controllers
{
    public class categoryController : Controller
    {
        private readonly AppliContext _context;

        public categoryController(AppliContext context)
        {
            _context = context;
        }

        // GET: category
        public async Task<IActionResult> getAllCategories()
        {
            return Json(await _context.categories.ToListAsync());
        }

        // GET: category/Details/5
        public async Task<IActionResult> detailCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.categories
                .FirstOrDefaultAsync(m => m.id == id);
            if (category == null)
            {
                return NotFound();
            }

            return Json(category);
        }

        private bool categoryExists(int id)
        {
            return _context.categories.Any(e => e.id == id);
        }
    }
}
