using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ESRGC.DLLR.EARN.Domain.ValidationAttributes;
using System.ComponentModel.DataAnnotations.Schema;
using ESRGC.DLLR.EARN.Domain.Helpers;

namespace ESRGC.DLLR.EARN.Domain.Model
{
  public class Account
  {
    public Account() {
      AccountID = 0;
      Active = true;
      Notifications = new List<Notification>();
      SentRequests = new List<Request>();
      ReceivedRequests = new List<Request>();
      EmailVerified = false;
      VerificationCode = Helpers.Utility.RandomString(30);
      InitialPassword = Utility.RandomString(8);
      IsProfileOwner = false;
    }

    public int AccountID { get; set; }

    [MaxLength(50)]
    [EmailValidation(ErrorMessage = "Invalid email address")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email address")]
    public string EmailAddress { get; set; }

    [MaxLength(32)]
    [DataType(DataType.Password)]
    public byte[] Password { get; set; }

    [MaxLength(32)]
    [ScaffoldColumn(false)]
    public string InitialPassword { get; set; }

    [MaxLength(20)]
    public string Role { get; set; }

    public bool Active { get; set; }
    [MaxLength(50)]
    public string SecretQuestion { get; set; }

    [MaxLength(50)]
    [DataType(DataType.Password)]
    public byte[] AnswerToSecretQuestion { get; set; }

    [Display(Name = "Member since")]
    public DateTime? MemberSince { get; set; }

    [Display(Name = "Last login")]
    public DateTime? LastLogin { get; set; }

    [Display(Name = "Last update")]
    public DateTime? LastUpdate { get; set; }

    public int? ProfileID { get; set; }
    [ForeignKey("ProfileID")]
    public virtual Profile Profile { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; }
    public virtual ICollection<Request> SentRequests { get; set; }
    public virtual ICollection<Request> ReceivedRequests { get; set; }

    public bool EmailVerified { get; set; }
    public string VerificationCode { get; set; }
    /// <summary>
    /// indicates this account is verified with the associated profile 
    /// </summary>
    public bool IsProfileOwner { get; set; }
    public void newVerificationCode() {
      VerificationCode = Helpers.Utility.RandomString(30);
    }
  }
}
