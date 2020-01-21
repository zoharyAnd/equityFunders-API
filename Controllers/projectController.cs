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
using System.IO;
using Newtonsoft.Json;

namespace cFunding.Controllers
{
    public class projectController : Controller
    {
        private readonly AppliContext _context;

        public projectController(AppliContext context)
        {
            _context = context;
        }

        // GET: project
        public IActionResult getValidProjects()
        {
            List<user> users = _context.users.ToList(); 
            List<category> categories = _context.categories.ToList(); 
            var listValidPj = _context.projects.Where(p => p.validatedProject == true);
            return Json(listValidPj);
        }

        // GET: project/getAllProjects
        public async Task<ActionResult> getAllProjects(){
            List<user> users = _context.users.ToList(); 
            List<category> categories = _context.categories.ToList(); 
            return Json(await _context.projects.ToListAsync());   
        }

        // GET: project/Details/5
        public async Task<IActionResult> detailProject(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.projects.FirstOrDefaultAsync(m => m.id == id);
            if (project == null)
            {
                return NotFound();
            }
 
            return Json(project);
        }
      
        // GET: project/deleteProject/5
        public async Task<IActionResult> deleteProject(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

           var project = await _context.projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            else{
                _context.projects.Remove(project);
                await _context.SaveChangesAsync();
                return Json(project);
            }
        }

        private bool projectExists(int id)
        {
            return _context.projects.Any(e => e.id == id);
        }

    }
}
