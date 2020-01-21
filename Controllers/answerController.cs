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
    public class answerController : Controller
    {
        private readonly AppliContext _context;

        public answerController(AppliContext context)
        {
            _context = context;
        }

        public IActionResult getAllAnswers(int questionid)
        {
            var listAnswers =  _context.answers.Where(p => p.fkquestion.id == questionid);
            List<user> users = _context.users.ToList(); 
            List<question> questions = _context.questions.ToList(); 
            return Json(listAnswers);
        }

        public async Task<IActionResult> detailAnswer(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.answers.FirstOrDefaultAsync(m => m.id == id);
            List<user> users = _context.users.ToList(); 
            List<question> questions = _context.questions.ToList(); 

            if (answer == null)
            {
                return NotFound();
            }

            return Json(answer);
        }

        [HttpPost]
        public async Task<IActionResult> createAnswer([Bind("answermessage")] answer answer, int questionid, int userid)
        {
            if (ModelState.IsValid)
            {
                List<user> users = _context.users.ToList(); 
                var currentUser = await _context.users.FirstOrDefaultAsync(m => m.id == userid);
                
                if (currentUser == null) {
                    return NotFound();
                }
                else {
                    var currentQuestion = await _context.questions.FirstOrDefaultAsync(m => m.id == questionid);
                    
                    if (currentQuestion == null) {
                        return NotFound();
                    }
                    else {
                        answer.fkquestion = currentQuestion;
                        answer.fkuser = currentUser;
                        //.ToString("yyyy-MM-dd' 'HH:mm:ssK")
                        answer.answerdate = DateTimeOffset.Now;
                        

                        _context.Add(answer);
                        await _context.SaveChangesAsync();
                        return Redirect("http://149.202.210.119:8080/");
                    }
                    
                }
                
            }
            return Redirect("http://149.202.210.119:8080/");
        }

        

    }
}