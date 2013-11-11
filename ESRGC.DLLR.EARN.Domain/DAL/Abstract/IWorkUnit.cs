using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRGC.DLLR.EARN.Domain.Model;

namespace ESRGC.DLLR.EARN.Domain.DAL.Abstract
{
  public interface IWorkUnit
  {
    IRepository<Account> AccountRepository { get; }
    IRepository<Picture> PictureRepository { get; }
    IRepository<Contact> ContactRepository { get; }
    IRepository<Profile> ProfileRepository { get; }
    IRepository<Organization> OrganizationRepository { get; }
    IRepository<UserGroup> UserGroupRepository { get; }
    IRepository<Category> CategoryRepository { get; }
    IRepository<Tag> TagRepository { get; }
    IRepository<ProfileTag> ProfileTagRepository { get; }
    //IRepository<Connection> ConnectionRepository { get; }
    IRepository<Partnership> PartnershipRepository { get; }
    IRepository<PartnershipDetail> PartnershipDetailRepository { get; }
    IRepository<Request> RequestRepository { get; }
    IRepository<Notification> NotificationRepository { get; }
    IRepository<Comment> CommentRepository { get; }
    IRepository<PartnershipTag> PartnershipTagRepository { get; }

    void saveChanges();
    void Dispose();
  }
}
