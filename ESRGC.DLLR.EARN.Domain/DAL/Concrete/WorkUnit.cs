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
    IRepository<Category> _categoryRepo;
    IRepository<UserGroup> _userGroupRepo;
    IRepository<Tag> _tagRepo;
    IRepository<ProfileTag> _profileTagRepo;
    IRepository<Partnership> _partnershipRepo;
    IRepository<PartnershipDetail> _parnershipDetailRepo;
    IRepository<Notification> _notificationRepo;
    IRepository<Request> _requestRepo;
    IRepository<Comment> _commentRepo;
    IRepository<PartnershipTag> _partnershipTagRepo;
    IRepository<Document> _documentRepo;

    public WorkUnit(DomainContext context) {
      _context = context;
    }

    #region IWorkUnit Members

    public void saveChanges() {
      _context.SaveChanges();
    }

    public void Dispose() {
      this.Dispose();
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

    public IRepository<Category> CategoryRepository {
      get {
        if (_categoryRepo == null)
          _categoryRepo = new Repository<Category>(_context);
        return _categoryRepo;
      }
    }
    public IRepository<Tag> TagRepository {
      get {
        return _tagRepo ?? (_tagRepo = new Repository<Tag>(_context));
      }
    }
    public IRepository<ProfileTag> ProfileTagRepository {
      get {
        return _profileTagRepo ?? (_profileTagRepo = new Repository<ProfileTag>(_context));
      }
    }   
    public IRepository<Partnership> PartnershipRepository {
      get {
        return _partnershipRepo ?? (_partnershipRepo = new Repository<Partnership>(_context));
      }
    }
    public IRepository<PartnershipDetail> PartnershipDetailRepository {
      get {
        return _parnershipDetailRepo ?? (_parnershipDetailRepo = new Repository<PartnershipDetail>(_context));
      }
    }

    public IRepository<Request> RequestRepository {
      get { return _requestRepo ?? (_requestRepo = new Repository<Request>(_context)); }
    }

    public IRepository<Notification> NotificationRepository {
      get { return _notificationRepo ?? (_notificationRepo = new Repository<Notification>(_context)); }
    }
    public IRepository<Comment> CommentRepository {
      get { return _commentRepo ?? (_commentRepo = new Repository<Comment>(_context)); }
    }
    public IRepository<PartnershipTag> PartnershipTagRepository {
      get { return _partnershipTagRepo ?? (_partnershipTagRepo = new Repository<PartnershipTag>(_context)); }
    }
    public IRepository<Document> DocumentRepository {
      get { return _documentRepo ?? (_documentRepo = new Repository<Document>(_context)); }
    }
    #endregion
  }
}
