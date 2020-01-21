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
using Microsoft.AspNetCore.Http.Extensions;

namespace cFunding.Controllers
{
    public class favoriteController : Controller
    {
        private readonly AppliContext _context;

        public favoriteController(AppliContext context)
        {
            _context = context;
        }

        public IActionResult getAllFavorites(int userid)
        {
            
            var listFavorites = _context.favorites.Where(f => f.fkuser.id == userid);
            List<user> users = _context.users.ToList(); 
            List<project> projects = _context.projects.ToList(); 
            return Json(listFavorites);
        }

        [HttpPost]
        public async Task<IActionResult> createFavorite(int userid, int projectid)
        {
            favorite favorite = new favorite(); 
            if (ModelState.IsValid)
            {
                var currentUser = await _context.users.FirstOrDefaultAsync(m => m.id == userid);
                
                if (currentUser == null) {
                    return NotFound();
                }
                else {
                    var currentProject = await _context.projects.FirstOrDefaultAsync(m => m.id == projectid);
                    
                    if (currentProject == null) {
                        return NotFound();
                    }
                    else {
                        favorite.fkproject = currentProject;
                        favorite.fkuser = currentUser;
                        

                        _context.Add(favorite);
                        await _context.SaveChangesAsync();
                        return Json(favorite);
                    }
                    
                }
                
            }
            return Json(favorite);
        }

        // POST: favorite/Delete/5
        [HttpPost]
        public async Task<IActionResult> deleteFavorite(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else{
                var favorite = await _context.favorites.FirstOrDefaultAsync(m => m.id == id);
                List<user> users = _context.users.ToList(); 
                List<project> projects = _context.projects.ToList(); 

                _context.favorites.Remove(favorite);
                await _context.SaveChangesAsync();

                return Json(favorite);
            }
            
        }
    }
}