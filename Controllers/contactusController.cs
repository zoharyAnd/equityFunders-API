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
    public class contactusController : Controller
    {
        private readonly AppliContext _context;

        public contactusController(AppliContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> getAllMessages()
        {
            List<user> users = _context.users.ToList(); 
            return Json(await _context.contactsus.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> detailMessage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactus = await _context.contactsus.FirstOrDefaultAsync(m => m.id == id);

            if (contactus == null)
            {
                return NotFound();
            }
            List<user> users = _context.users.ToList(); 
            return Json(contactus);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int userid, string sendername, string senderemail, string mailsubject, string mailmessage)
        {
            contactus contactus = new contactus(); 
            if (ModelState.IsValid)
            {
                var currentUser = await _context.users.FirstOrDefaultAsync(m => m.id == userid);

                if (currentUser == null)
                {
                    return NotFound();
                }
                else {
                    contactus.fkuser = currentUser;
                    contactus.sendername = sendername;
                    contactus.senderemail = senderemail;
                    contactus.mailsubject = mailsubject;
                    contactus.mailmessage = mailmessage;

                    _context.Add(contactus);
                    await _context.SaveChangesAsync();
                    return Redirect("http://149.202.210.119:8080/");
                }
                
            }
            return Redirect("http://149.202.210.119:8080/");
        }

        

    }
}