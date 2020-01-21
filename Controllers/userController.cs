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
using System.Web;
using System.Net.Mail;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace cFunding.Controllers
{
    public class userController : Controller
    {
        private readonly AppliContext _context;

        public userController(AppliContext context)
        {
            _context = context;
        }

        // GET: user
        public async Task<IActionResult> getAllUsers()
        {
            return Json(await _context.users.ToListAsync());
        }

        // GET: user/Details/5
        public async Task<IActionResult> detailUser(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.users.FirstOrDefaultAsync(m => m.id == id);
            if (user == null)
            {
                return NotFound();
            }
            
            return Json(user);
        }

        // user Registration Home Page 
        [HttpPost]
        
        public async Task<IActionResult> register([Bind("useremail,userpassword")] user user)
        {
            if (ModelState.IsValid)
            {
                user.userfname = null; 
                user.userlname = null; 
                user.username = null; 
                user.userphoto = "images/avatar.jpg"; 
                user.userdob = DateTimeOffset.Now; 
                user.useraddress = null;
                user.usercountry = null;
                user.companyname = null; 
                user.companylogo = "images/noImageAvailable.jpg"; 
                user.companydescription = null; 
                user.nbshareordinary = 0;
                user.sharevalueordinary = 0;
                user.descordinary = null;
                user.additionalordinary = null;
                user.nbsharepreferencial = 0;
                user.sharevaluepreferencial = 0;
                user.descpreferencial = null;
                user.additionalpreferencial = null;
                user.nbsharenonvoting = 0;
                user.sharevaluenonvoting = 0;
                user.descnonvoting = null;
                user.additionalnonvoting = null;
                user.nbshareredeemable = 0;
                user.sharevalueredeemable = 0;
                user.descredeemable = null;
                user.additionalredeemable = null;
                user.userassets = 0;
                user.isadmin=false; 
                user.validatedUser=false;
                
                
                _context.Add(user);
                await _context.SaveChangesAsync();

                // configuration of email 
                var emailSender = "cFundingZrsi@gmail.com";
                var password = "cFundingZrsi2019";
                var activateUrl = HttpContext.Session.GetString("urlHome")+"user/ActivateEmail/"+user.id;

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587); 
                client.EnableSsl = true; 
                client.Timeout = 100000; 
                client.DeliveryMethod = SmtpDeliveryMethod.Network; 
                client.UseDefaultCredentials = false;  
                client.Credentials = new NetworkCredential(emailSender, password);
                 
                var subject = "Cfunding Account Validation"; 
                var body = "Thank you for registering!<br>Please activate your account by clicking the link below:<br><a>"+activateUrl+"</a><br>Regards,<br>Cfunding Team.";
                MailMessage mailMessage = new MailMessage (emailSender, user.useremail, subject, body); 
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = UTF8Encoding.UTF8; 

                client.Send(mailMessage); 
                
                 return Json(user);
            }
            return Json(user);
        }

        // POST: user/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> editUser(int id, string userfname, string userlname, string username, string useremail, string userpassword, DateTimeOffset userdob, string useraddress, string usercountry, IFormFile fileUserphotoEdit)
        {
            
            var user = await _context.users.FirstOrDefaultAsync(m => m.id == id);
            if (user == null)
            {
                return NotFound();
            }else{
                // upload new user photo
                if (fileUserphotoEdit == null || fileUserphotoEdit.Length == 0){
                    user.userphoto = user.userphoto;
                }
                else {
                    var newUserPhoto = "images/"+fileUserphotoEdit.FileName;
                    if (newUserPhoto == user.userphoto) {
                        user.userphoto = user.userphoto;
                    } 
                    else {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/", fileUserphotoEdit.FileName);  

                        using (var stream = new FileStream(path, FileMode.Create))  
                        {  
                            await fileUserphotoEdit.CopyToAsync(stream);  
                        }  
                        user.userphoto = "images/" + fileUserphotoEdit.FileName; 
                    } 
                }
                if(userfname == null){user.userfname = user.userfname; }else{user.userfname = userfname;}
                if(userlname == null){user.userlname = user.userlname; }else{user.userlname = userlname;}
                if(username == null){user.username = user.username; }else{user.username = username;}
                if(useremail == null){user.useremail = user.useremail; }else{user.useremail = useremail;}
                if(userpassword == null){user.userpassword = user.userpassword; }else{user.userpassword = userpassword;}
                if(userdob == null){user.userdob = user.userdob; }else{user.userdob = userdob;}
                if(useraddress == null){user.useraddress = user.useraddress; }else{user.useraddress = useraddress;}
                if(usercountry == null){user.usercountry = user.usercountry; }else{user.usercountry = usercountry;}
                
                user.companyname = user.companyname;
                user.companydescription = user.companydescription;
                user.nbshareordinary = user.nbshareordinary; 
                user.sharevalueordinary = user.sharevalueordinary; 
                user.descordinary = user.descordinary;
                user.additionalordinary = user.additionalordinary; 
                user.nbsharepreferencial = user.nbsharepreferencial; 
                user.sharevaluepreferencial = user.sharevaluepreferencial; 
                user.descpreferencial = user.descpreferencial; 
                user.additionalpreferencial = user.additionalpreferencial;
                user.nbsharenonvoting = user.nbsharenonvoting; 
                user.sharevaluenonvoting = user.sharevaluenonvoting;
                user.descnonvoting = user.descnonvoting;
                user.additionalnonvoting = user.additionalnonvoting; 
                user.nbshareredeemable = user.nbshareredeemable; 
                user.sharevalueredeemable = user.sharevalueredeemable;
                user.descredeemable = user.descredeemable;
                user.additionalredeemable = user.additionalredeemable;
                user.userassets = user.userassets;

                user.isadmin=user.isadmin;
                user.validatedUser=user.validatedUser;}
                
                _context.Update(user);
                await _context.SaveChangesAsync();
                
                return Json(user);
            
        }

         [HttpPost]
        public async Task<IActionResult> editCompany(int id, string companyname, string companydescription, int nbshareordinary, double sharevalueordinary, string descordinary, string additionalordinary, int nbsharepreferencial, double sharevaluepreferencial, string descpreferencial, string additionalpreferencial, int nbsharenonvoting, double sharevaluenonvoting, string descnonvoting, string additionalnonvoting, int nbshareredeemable, double sharevalueredeemable, string descredeemable, string additionalredeemable, double userassets, IFormFile fileCompanyLogoEdit)
        {
            
            var user = await _context.users.FirstOrDefaultAsync(m => m.id == id);
            if (user == null)
            {
                return NotFound();
            }else{
                // upload new company logo
                if (fileCompanyLogoEdit == null || fileCompanyLogoEdit.Length == 0){
                    user.companylogo = user.companylogo;
                }
                else {
                    var newcLogo = "images/"+fileCompanyLogoEdit.FileName;
                    if (newcLogo == user.companylogo) {
                        user.companylogo = user.companylogo;
                    } 
                    else {
                        var path1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/", fileCompanyLogoEdit.FileName);  

                        using (var stream1 = new FileStream(path1, FileMode.Create))  
                        {  
                            await fileCompanyLogoEdit.CopyToAsync(stream1);  
                        }  
                        user.companylogo = "images/" + fileCompanyLogoEdit.FileName; 
                    } 
                }

                user.userfname = user.userfname; 
                user.userlname = user.userlname;
                user.username = user.username; 
                user.useremail = user.useremail; 
                user.userpassword = user.userpassword;
                user.userdob = user.userdob;
                user.useraddress = user.useraddress;
                user.usercountry = user.usercountry; 


                if(companyname == null){user.companyname = user.companyname; }else{user.companyname = companyname;}
                if(companydescription == null){user.companydescription = user.companydescription; }else{user.companydescription = companydescription; }
                if(nbshareordinary == 0){user.nbshareordinary = user.nbshareordinary; }else{user.nbshareordinary = nbshareordinary; }
                if(sharevalueordinary == 0){user.sharevalueordinary = user.sharevalueordinary; }else{user.sharevalueordinary = sharevalueordinary; }
                if(descordinary == null){user.descordinary = user.descordinary; }else{user.descordinary = descordinary; }
                if(additionalordinary == null){user.additionalordinary = user.additionalordinary; }else{user.additionalordinary = additionalordinary; }
                if(nbsharepreferencial == 0){user.nbsharepreferencial = user.nbsharepreferencial; }else{user.nbsharepreferencial = nbsharepreferencial; }
                if(sharevaluepreferencial == 0){user.sharevaluepreferencial = user.sharevaluepreferencial; }else{user.sharevaluepreferencial = sharevaluepreferencial; }
                if(descpreferencial == null){user.descpreferencial = user.descpreferencial; }else{user.descpreferencial = descpreferencial; }
                if(additionalpreferencial == null){user.additionalpreferencial = user.additionalpreferencial; }else{user.additionalpreferencial = additionalpreferencial; }
                if(nbsharenonvoting == 0){user.nbsharenonvoting = user.nbsharenonvoting; }else{user.nbsharenonvoting = nbsharenonvoting; }
                if(sharevaluenonvoting == 0){user.sharevaluenonvoting = user.sharevaluenonvoting; }else{user.sharevaluenonvoting = sharevaluenonvoting; }
                if(descnonvoting == null){user.descnonvoting = user.descnonvoting; }else{user.descnonvoting = descnonvoting; }
                if(additionalnonvoting == null){user.additionalnonvoting = user.additionalnonvoting; }else{user.additionalnonvoting = additionalnonvoting; }
                if(nbshareredeemable ==0 ){user.nbshareredeemable = user.nbshareredeemable; }else{user.nbshareredeemable = nbshareredeemable; }
                if(sharevalueredeemable ==0 ){user.sharevalueredeemable = user.sharevalueredeemable; }else{user.sharevalueredeemable = sharevalueredeemable; }
                if(descredeemable == null){user.descredeemable = user.descredeemable; }else{user.descredeemable = descredeemable; }
                if(additionalredeemable == null){user.additionalredeemable = user.additionalredeemable; }else{user.additionalredeemable = additionalredeemable; }
                if(userassets == 0){user.userassets = user.userassets; }else{user.userassets = userassets;}
                
                user.isadmin=user.isadmin;
                user.validatedUser=user.validatedUser;}
                
                _context.Update(user);
                await _context.SaveChangesAsync();
                
                return Json(user);
            
        }

        public async Task<IActionResult> deleteUser(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.users.FirstOrDefaultAsync(m => m.id == id);
            if (user == null)
            {
                return NotFound();
            }
            else{
                _context.users.Remove(user);
                await _context.SaveChangesAsync();

                return Json(user);

            }
        }

        public IActionResult ActivateEmail(int id)
        {
            // validate the user having the userid == id 
            return View();
        }
       
    }
}
