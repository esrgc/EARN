using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRGC.DLLR.EARN.Domain.DAL.Abstract;
using ESRGC.DLLR.EARN.Domain.DAL;
using ESRGC.DLLR.EARN.Domain.Model;

namespace ESRGC.DLLR.EARN.Domain.DAL.Concrete
{
  public class WorkUnit : IWorkUnit, IDisposable
  {
    DomainContext _context;
    IRepository<Contact> _contactRepo;
    IRepository<Account> _accountRepo;
    IRepository<Picture> _pictureRepo;
    IRepository<Profile> _profileRepo;
    IRepository<Organization> _organizationRepo;
    IRepository<Industry> _industryRepo;
    IRepository<UserGroup> _userGroupRepo;

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

    public IRepository<Model.Contact> ContactRepository {
      get {
        if (_contactRepo == null) {
          _contactRepo = new Repository<Contact>(_context);
        }
        return _contactRepo;
      }
    }

    public IRepository<Account> AccountRepository {
      get {
        if (_accountRepo == null)
          _accountRepo = new Repository<Account>(_context);
        return _accountRepo;
      }
    }

    public IRepository<Picture> PictureRepository {
      get {
        if (_pictureRepo == null)
          _pictureRepo = new Repository<Picture>(_context);
        return _pictureRepo;
      }
    }

    public IRepository<Profile> ProfileRepository {
      get { return _profileRepo ?? (_profileRepo = new Repository<Profile>(_context)); }
    }

    public IRepository<Organization> OrganizationRepository {
      get { return _organizationRepo ?? (_organizationRepo = new Repository<Organization>(_context)); }
    }

    public IRepository<UserGroup> UserGroupRepository {
      get { return _userGroupRepo ?? (_userGroupRepo = new Repository<UserGroup>(_context)); }
    }

    public IRepository<Industry> IndustryRepository {
      get {
        if (_industryRepo == null)
          _industryRepo = new Repository<Industry>(_context);
        return _industryRepo;
      }
    }
    #endregion
  }
}
