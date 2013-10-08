using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Models;
using ESRGC.DLLR.EARN.Helpers;
using System.Web.Security;
using ESRGC.DLLR.EARN.Domain.Model;
using ESRGC.DLLR.EARN.Domain.DAL.Abstract;

namespace ESRGC.DLLR.EARN.Controllers
{
  [Authorize]
  public class AccountController : BaseController
  {
    public AccountController(IWorkUnit workUnit) : base(workUnit) { }

    public ActionResult Index() {

      return View();
    }
    [AllowAnonymous]
    public ActionResult SignUp() {
      var signUpModel = new SignUpModel() {
        //SecretQuestions = Utility.SecurityQuestions
      };
      //render form
      return View(signUpModel);
    }

    [HttpPost]
    [AllowAnonymous]
    public ActionResult SignUp(SignUpModel model) {
      if (ModelState.IsValid) {
        var existingEmails = _workUnit.AccountRepository.Entities.Where(x => x.EmailAddress == model.Email);
        if (existingEmails.Count() > 0) {
          ModelState.AddModelError("", "This email address \"" + model.Email + "\" has already been in use. Please try again with a different email address");
          return View(model);
        }
        //we're good to go
        var newAccount = new Account() {
          EmailAddress = model.Email,
          //SecretQuestion = model.SecretQuestion,
          InitialPassword = Utility.RandomString(8)
        };
        //check password
        if (model.Password == model.ConfirmPassword) {
          try {
            //encrypt password
            var encryptedPassword = SHA1PasswordSecurity.encrypt(model.Password);
            newAccount.Password = encryptedPassword;
            //encrypt secret answer for password recovery
            //newAccount.AnswerToSecretQuestion = SHA1PasswordSecurity.encrypt(model.SecretAnswer.ToUpper());//use upper case
            //add role
            newAccount.Role = "user";//by default user is assigned to the account

            newAccount.LastLogin = DateTime.Now;
            newAccount.MemberSince = DateTime.Now;
            newAccount.LastUpdate = DateTime.Now;
            //store to database
            _workUnit.AccountRepository.InsertEntity(newAccount);
            _workUnit.saveChanges();
            //make sure any signed on account is signed out
            FormsAuthentication.SignOut();
            //authenticated--add session cookies
            FormsAuthentication.SetAuthCookie(newAccount.EmailAddress, false);
            //send email notification
            //notifyNewAccount(newAccount);
            //updateTempDataMessage("Your account has been created. Please create a new contact for your account");
            //redirect to create new user contact

            return RedirectToAction("Create", "Profile");
          }
          catch (Exception) {
            ModelState.AddModelError("", "Error saving data to database. Please try again later");

          }

        }
        else
          ModelState.AddModelError("", "New password and confirmation password do not match. Please try again");
      }

      // If we got this far, something failed, redisplay form
      return View(model);
    }
    [AllowAnonymous]
    public ActionResult SignIn() {
      //sign out any previous session

      FormsAuthentication.SignOut();
      return View(new SignInModel());
    }

    [HttpPost]
    [AllowAnonymous]
    public ActionResult SignIn(SignInModel model, string returnUrl) {
      if (ModelState.IsValid) {
        if (Authentication.authenticate(_workUnit, model)) {
          //authenticate user
          FormsAuthentication.SetAuthCookie(model.Email, model.RememberMe);
          //return to previous url
          if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
              && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\")) {
            return Redirect(returnUrl);
          }
          else {
            return RedirectToAction("Index", "Profile");
          }
        }
        else
          ModelState.AddModelError("", "The username/email or password provided is incorrect.");
      }

      // If we got this far, something failed, redisplay form
      return View(model);
    }

    public ActionResult SignOut() {
      //record last login
      var account = _workUnit.AccountRepository.Entities.First(x => x.EmailAddress == User.Identity.Name);
      account.LastLogin = DateTime.Now;
      _workUnit.AccountRepository.UpdateEntity(account);
      _workUnit.saveChanges();
      FormsAuthentication.SignOut();
      return RedirectToAction("Index", "Home");
    }



  }
}
