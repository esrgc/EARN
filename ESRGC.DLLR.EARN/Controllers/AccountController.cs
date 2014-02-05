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
using ESRGC.DLLR.EARN.Filters;
using ESRGC.DLLR.EARN.Domain.Helpers;

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
    [CheckCookie]
    public ActionResult SignUp() {
      var signUpModel = new SignUpModel() {
        //SecretQuestions = Utility.SecurityQuestions
      };
      //render form
      return View(signUpModel);
    }

    [HttpPost]
    [AllowAnonymous]
    [CheckCookie]
    public ActionResult SignUp(SignUpModel model) {
      if (ModelState.IsValid) {
        var existingEmails = _workUnit.AccountRepository.Entities.Where(x => x.EmailAddress.ToLower() == model.Email.ToLower());
        if (existingEmails.Count() > 0) {
          ModelState.AddModelError("", "This email address \"" + model.Email + "\" has already been in use. Please try again with a different email address");
          return View(model);
        }
        //we're good to go
        var newAccount = new Account() {
          EmailAddress = model.Email
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
            //verify email
            EmailHelper.SendVerificationEmail(newAccount);
            return RedirectToAction("Create", "Profile");
          }
          catch (Exception ex) {
            ModelState.AddModelError("", "Error saving data to database. Please try again later. " + ex.Message);

          }

        }
        else
          ModelState.AddModelError("", "New password and confirmation password do not match. Please try again");
      }

      // If we got this far, something failed, redisplay form
      return View(model);
    }
    [AllowAnonymous]
    [SetGuestCookie]
    public ActionResult SignIn(string returnUrl) {
      //sign out any previous session

      FormsAuthentication.SignOut();
      ViewBag.returnUrl = returnUrl;
      return View(new SignInModel());
    }

    [HttpPost]
    [AllowAnonymous]
    [CheckCookie]
    public ActionResult SignIn(SignInModel model, string returnUrl) {
      if (ModelState.IsValid) {
        if (Authentication.authenticate(_workUnit, model)) {
          //authenticate user
          FormsAuthentication.SetAuthCookie(model.Email, model.RememberMe);

          try {
            var currentAccount = _workUnit.AccountRepository
                    .Entities.First(x => x.EmailAddress == model.Email);

            //check password reset
            var initPassword = SHA1PasswordSecurity.encrypt(currentAccount.InitialPassword);
            if (Authentication.comparePassword(initPassword, currentAccount.Password)) {
              updateTempMessage("Please change your password. If you choose not to do so, you will be prompted to change your password everytime you sign in!");
              return RedirectToAction("ChangePassword");
            }
          }
          catch {
            //just in case..ofcourse account would have been 
            //authenticated at this point

            //if could not find the account
            //proceeds as usual  
          }

          //return to previous url
          if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
              && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\")) {
            return Redirect(returnUrl);
          }
          else {
            return RedirectToAction("Detail", "Profile");
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
      if (account != null) {
        account.LastLogin = DateTime.Now;
        _workUnit.AccountRepository.UpdateEntity(account);
        _workUnit.saveChanges();
      }
      FormsAuthentication.SignOut();
      return RedirectToAction("Index", "Home");
    }

    public ActionResult VerifyEmail(int accountID, string verificationCode) {
      var account = _workUnit.AccountRepository.GetEntityByID(accountID);
      if (account == null) {
        updateTempMessage("Invalid account ID. Error verifying email.");
        return RedirectToAction("Index", "Home");
      }
      if (string.IsNullOrEmpty(verificationCode)) {
        updateTempMessage("Could not verify email. Verification code is empty.");
        return RedirectToAction("Index", "Home");
      }
      if (account.EmailVerified)
        updateTempMessage("Your email is already verified.");
      else {
        if (verificationCode == account.VerificationCode) {
          account.EmailVerified = true;
          _workUnit.AccountRepository.UpdateEntity(account);
          _workUnit.saveChanges();
          updateTempMessage("Your email address has been verified.");
        }
      }
      return RedirectToAction("Settings");
    }
    [VerifyAccount]
    public ActionResult Settings() {
      return View(CurrentAccount);
    }

    [VerifyAccount]
    public ActionResult ResendVerificationEmail(string returnUrl) {
      if (!CurrentAccount.EmailVerified) {
        EmailHelper.SendVerificationEmail(CurrentAccount);
        updateTempMessage("Verification email has been resent. Please check your inbox and verify your email.");
      }
      //return to previous url
      if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
          && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\")) {
        return Redirect(returnUrl);
      }
      else {
        return RedirectToAction("Index", "Home");
      }
    }
    [VerifyAccount]
    public ActionResult ChangePassword() {
      return View();
    }
    [HttpPost]
    [VerifyAccount]
    public ActionResult ChangePassword(ChangePasswordModel model) {
      var account = CurrentAccount;
      if (ModelState.IsValid) {
        // ChangePassword will throw an exception rather
        // than return false in certain failure scenarios.
        bool changePasswordSucceeded;
        try {
          if (model.NewPassword != model.ConfirmPassword) {
            throw new Exception("Confirmed password does not match new password. Please try again");
          }

          if (model.NewPassword.Length < 6) {
            throw new Exception("Minimum password is too short. Minimum 6 characters");
          }

          //encrypt new password
          var newPassword = SHA1PasswordSecurity.encrypt(model.NewPassword);
          var oldPassword = SHA1PasswordSecurity.encrypt(model.OldPassword);
          if (Authentication.comparePassword(oldPassword, account.Password)) {
            account.Password = newPassword;//change password
            _workUnit.AccountRepository.UpdateEntity(account);
            _workUnit.saveChanges();
            changePasswordSucceeded = true;
            updateTempMessage("Your password has been changed successfully.");
            logLastUpdate(account);
          }
          else {
            throw new Exception("Old password is incorrect. Please try again");
          }

        }
        catch (Exception ex) {
          ModelState.AddModelError("", ex.Message);
          changePasswordSucceeded = false;
        }

        if (changePasswordSucceeded) {
          return RedirectToAction("Settings", "Account");
        }

      }

      // If we got this far, something failed, redisplay form
      return View(model);
    }
    [AllowAnonymous]
    public ActionResult ForgotPassword() {
      return View();
    }
    [AllowAnonymous]
    [HttpPost]
    public ActionResult ForgotPassword(ResetPasswordModel model) {
      if (ModelState.IsValid) {
        var emailAddress = model.Email;
        Account account = null;
        try {
          account = _workUnit.AccountRepository.Entities
              .Where(x => x.EmailAddress.ToLower() == emailAddress.ToLower())
              .First();
        }
        catch {// no email found in the system
          ModelState.AddModelError("", "We could not find the email address you entered. Please enter a different email address!");
        }
        if (account != null) {
          //set temp password
          account.Password = SHA1PasswordSecurity.encrypt(account.InitialPassword);
          _workUnit.AccountRepository.UpdateEntity(account);
          _workUnit.saveChanges();
          //send email
          EmailHelper.SendPasswordEmail(account);
          updateTempMessage("An email with password reset information has been sent to your email address. Please check your inbox to change your password.");
          return RedirectToAction("Index", "Home");
        }
      }
      return View(model);
    }

    [AllowAnonymous]
    public ActionResult ResetPassword(int accountID, string verificationCode) {
      var account = _workUnit.AccountRepository.GetEntityByID(accountID);
      if (account == null) {
        updateTempMessage("Invalid account ID. Error resetting your password.");
        return RedirectToAction("Index", "Home");
      }
      if (string.IsNullOrEmpty(verificationCode)) {
        updateTempMessage("Could reset your password. Verification code is empty.");
        return RedirectToAction("Index", "Home");
      }
      if (verificationCode == account.VerificationCode) {
        updateTempMessage("Please enter a new password");
        //sign the user in
        FormsAuthentication.SetAuthCookie(account.EmailAddress, false);
        //change the password
        return RedirectToAction("ChangePassword");
      }
      return RedirectToAction("Index", "Home");
    }
    [VerifyAccount]
    public ActionResult ChangeSignInEmail() {
      var account = CurrentAccount;

      return View(new ChangeEmailModel { CurrentEmail = CurrentAccount.EmailAddress });
    }
    [HttpPost]
    [VerifyAccount]    
    public ActionResult ChangeSignInEmail(ChangeEmailModel model) {
      var account = CurrentAccount;
      if (ModelState.IsValid) {
        var newEmail = model.NewEmail;
        if (newEmail.ToLower() == account.EmailAddress.ToLower()) {
          ModelState.AddModelError("", "New email address must be different emailAddress the current email address. Please try again!");
          return View(model);
        }

        //check for existing email
        try {
          var existing = _workUnit.AccountRepository
                .Entities
                .First(x => x.EmailAddress.ToLower() == newEmail.ToLower());
          ModelState.AddModelError("", "The email address you entered already exists in our database. Please use another email address");
          return View(model);
        }
        catch  {
          //no existing email! We're good
          //does nothing and keep going
        }

        //everything is good, so go ahead and store it
        account.EmailAddress = newEmail;
        account.EmailVerified = false;
        account.newVerificationCode();
        //store to database
        _workUnit.AccountRepository.UpdateEntity(account);
        _workUnit.saveChanges();
        //sign out old email and sign in with new email
        FormsAuthentication.SignOut();
        FormsAuthentication.SetAuthCookie(newEmail, false);
        //resend verification email
        EmailHelper.SendReverificationEmail(account);
        
        updateTempMessage("Your sign-in email address has been changed. Please make sure to verify the email address.");
        return RedirectToAction("Settings");
      }
      return View(model);
    }
    
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////// Private functions //////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
    private void logLastUpdate(Account account) {
      account.LastUpdate = DateTime.Now;
      _workUnit.AccountRepository.UpdateEntity(account);
      _workUnit.saveChanges();
    }

  }
}
