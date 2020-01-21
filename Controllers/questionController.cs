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
    public class questionController : Controller
    {
        private readonly AppliContext _context;

        public questionController(AppliContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> getAllQuestions()
        {
            List<user> users = _context.users.ToList(); 
            return Json(await _context.questions.ToListAsync());
        }

        public async Task<IActionResult> detailQuestion(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.questions.FirstOrDefaultAsync(m => m.id == id);
            List<user> users = _context.users.ToList(); 

            if (question == null)
            {
                return NotFound();
            }

            return Json(question);
        }

        [HttpPost]
        public async Task<IActionResult> createQuestion(int userid, [Bind("questionmessage")] question question)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _context.users.FirstOrDefaultAsync(m => m.id == userid);

                if (currentUser == null) {
                    return NotFound();
                }
                else {
                    question.fkuser = currentUser;
                    //.ToString("yyyy-MM-dd' 'HH:mm:ssK")
                    question.questiondate = DateTimeOffset.Now;
                    question.validatedQuestion = false; 

                    _context.Add(question);
                    await _context.SaveChangesAsync();

                    return Json(question);
                }
            }
            return Json(question);
        }


    }
}