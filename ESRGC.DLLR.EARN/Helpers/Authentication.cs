using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ESRGC.DLLR.EARN.Domain.DAL.Abstract;
using ESRGC.DLLR.EARN.Domain.Helpers;
using ESRGC.DLLR.EARN.Domain.Model;
using ESRGC.DLLR.EARN.Models;

namespace ESRGC.DLLR.EARN.Helpers
{
  /// <summary>
  /// Helper class used to authenticate users
  /// </summary>
  public class Authentication
  {
    public static bool authenticate(IWorkUnit workUnit, SignInModel logOnModel) {
      var password = logOnModel.Password;
      Account account = null;
      try {
          //get the contact with the provided email
          account = workUnit.AccountRepository.Entities.First(x => x.EmailAddress.ToUpper() == logOnModel.Email.ToUpper());
      }
      catch {
        return false;//failed to find user account
      }

      //encode password
      var encodedPassword = SHA1PasswordSecurity.encrypt(password);

      //authenticate
      var databasePassword = account.Password;

      var pwValidation = comparePassword(encodedPassword, databasePassword);
      //if valid record log in datetime
      if (pwValidation) {
        try {
          account.LastLogin = DateTime.Now;
          workUnit.AccountRepository.UpdateEntity(account);
          workUnit.saveChanges();
        }
        catch {
          //do error logging here
        }
      }
      return pwValidation;
    }
    /// <summary>
    /// compare provided passwords.
    /// </summary>
    /// <param name="password"></param>
    /// <param name="databasePassword"></param>
    /// <returns>True if 2 passwords are identical, false otherwise</returns>
    public static bool comparePassword(byte[] password, byte[] databasePassword) {
      bool authenticated = false;

      if (password.Length != databasePassword.Length)
        return authenticated;//always false

      int i = 0;
      while ((i < password.Length) && (password[i] == databasePassword[i])) {
        i++;
      }
      if (i == password.Length) {
        authenticated = true;
      }
      return authenticated;
    }
  }
}