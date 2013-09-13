using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRGC.DLLR.EARN.Domain.Model;

namespace ESRGC.DLLR.EARN.Domain.DAL.Abstract
{
  public interface IWorkUnit
  {
    IRepository<Contact> ContactRepository { get; }
    //IRepository<Caucus> CaucusRepository { get; }
    //IRepository<SubCommittee> SubCommittRepository { get; }
    //IRepository<Company> CompanyRepository { get; }
    IRepository<Account> AccountRepository { get; }
    //IRepository<Picture> PictureRepository { get; }
    //IRepository<MembershipType> MembershipType { get; }
    //IRepository<Subscription> SubscriptionRepository { get; }
    //IRepository<Transaction> TransactionRepository { get; }
    //IRepository<PayPalLog> PayPalLogRepository { get; }
    void saveChanges();
    void Dispose();
  }
}
