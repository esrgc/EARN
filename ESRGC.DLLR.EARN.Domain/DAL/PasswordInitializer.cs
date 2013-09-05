using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRGC.DLLR.EARN.Domain.DAL.Abstract;

namespace ESRGC.DLLR.EARN.Domain.DAL
{
    public class PasswordInitializer
    {
        public static void initializePassword(IWorkUnit workUnit)
        {

            //var accountRepo = workUnit.AccountRepository;
            //var accounts = accountRepo.Entities;

            //foreach (var account in accounts)
            //{
            //    if (account.InitialPassword != null)
            //    {
            //        var encodedPassword = SHA1PasswordSecurity.encrypt(account.InitialPassword);
            //        account.Password = encodedPassword;
            //        account.Role = "user";
            //        accountRepo.UpdateEntity(account);
            //    }
            //}

            //workUnit.saveChanges();
        }
    }
}
