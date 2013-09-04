using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRGC.DLLR.EARN.Domain.DAL.Abstract;
using ESRGC.DLLR.EARN.Domain.DAL;

namespace ESRGC.MSGIC.Membership.Domain.DAL.Concrete
{
  public class WorkUnit : IWorkUnit, IDisposable
  {
    DomainContext _context;
    //IRepository<Contact> _contactRepo;
    //IRepository<Caucus> _caucusRepo;
    //IRepository<Company> _companyRepo;
    //IRepository<SubCommittee> _subCommittRepo;
    //IRepository<Account> _accountRepo;
    //IRepository<Picture> _pictureRepo;
    //IRepository<MembershipType> _membershipType;
    //IRepository<Subscription> _subscriptionRepo;
    //IRepository<Transaction> _transactionRepo;
    //IRepository<PayPalLog> _paypalLogRepo;

    public WorkUnit(DomainContext context) {
      _context = context;
    }

    #region IWorkUnit Members

    public void saveChanges() {
      _context.SaveChanges();
    }

    public void Dispose() {
      _context.Dispose();
    }

    //public IRepository<Model.Contact> ContactRepository {
    //  get {
    //    if (_contactRepo == null) {
    //      _contactRepo = new Repository<Contact>(_context);
    //    }
    //    return _contactRepo;
    //  }
    //}

    //public IRepository<Model.Caucus> CaucusRepository {
    //  get {
    //    if (_caucusRepo == null)
    //      _caucusRepo = new Repository<Caucus>(_context);
    //    return _caucusRepo;
    //  }
    //}

    //public IRepository<Model.SubCommittee> SubCommittRepository {
    //  get {
    //    if (_subCommittRepo == null)
    //      _subCommittRepo = new Repository<SubCommittee>(_context);
    //    return _subCommittRepo;
    //  }
    //}


    //public IRepository<Company> CompanyRepository {
    //  get {
    //    if (_companyRepo == null)
    //      _companyRepo = new Repository<Company>(_context);
    //    return _companyRepo;
    //  }
    //}


    //public IRepository<Account> AccountRepository {
    //  get {
    //    if (_accountRepo == null)
    //      _accountRepo = new Repository<Account>(_context);
    //    return _accountRepo;
    //  }
    //}

    //public IRepository<Picture> PictureRepository {
    //  get {
    //    if (_pictureRepo == null)
    //      _pictureRepo = new Repository<Picture>(_context);
    //    return _pictureRepo;
    //  }
    //}

    //public IRepository<MembershipType> MembershipType {
    //  get {
    //    if (_membershipType == null)
    //      _membershipType = new Repository<MembershipType>(_context);
    //    return _membershipType;
    //  }
    //}

    #endregion


    //public IRepository<Subscription> SubscriptionRepository {
    //  get {
    //    if (_subscriptionRepo == null)
    //      _subscriptionRepo = new Repository<Subscription>(_context);
    //    return _subscriptionRepo;
    //  }

    //}

    //public IRepository<Transaction> TransactionRepository {
    //  get {
    //    if (_transactionRepo == null)
    //      _transactionRepo = new Repository<Transaction>(_context);
    //    return _transactionRepo;
    //  }

    //}
    //public IRepository<PayPalLog> PayPalLogRepository {
    //  get {
    //    if (_paypalLogRepo == null)
    //      _paypalLogRepo = new Repository<PayPalLog>(_context);
    //    return _paypalLogRepo;
    //  }
    //}
  }
}
