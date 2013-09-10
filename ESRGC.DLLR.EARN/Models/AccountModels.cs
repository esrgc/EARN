using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Domain.ValidationAttributes;

namespace ESRGC.DLLR.EARN.Models
{

    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password did not match.")]
        public string ConfirmPassword { get; set; }
    }
    public class ResetPasswordModel
    {
        //[Required]
        //[Display(Name = "User name")]
        //public string UserName { get; set; }

        [Required]
        [EmailValidation(ErrorMessage = "Invalid email address")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }
    }
    public class SecurityCheckModel
    {
        public int AccountID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        [Display(Name = "Security Question")]
        public string SecretQuestion { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Secret Answer")]
        public string SecretAnswer { get; set; }
    }
    public class RecoverPasswordModel
    {
        public int AccountID { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    public class SignInModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
        [Required]
        [EmailValidation(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public string logOnType { get; set; }
    }

    public class SignUpModel
    {
        
        [Required]
        [EmailValidation(ErrorMessage = "Invalid email address")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public List<string> SecretQuestions { get; set; }
        [Required]
        [Display(Name = "Security question")]
        public string SecretQuestion { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Secret answer")]
        public string SecretAnswer { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm answer")]
        [Compare("SecretAnswer", ErrorMessage = "The answer and confirmation answer do not match.")]
        public string ConfirmAnswer { get; set; }
    }

    public class UserEditModel
    {
        public int AccountID { get; set; }
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        [EmailValidation(ErrorMessage = "Invalid email address")]
        public string EmailAddress { get; set; }
        [Display(Name="Accept notification")]
        public bool AcceptNotification { get; set; }
    }

    public class SecurityQuestionEditModel
    {
        public int AccountID { get; set; }
        public List<string> SecurityQuestions { get; set; }
        [Required]
        [Display(Name="Security question")]
        public string SecurityQuestion { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name="Secret answer")]
        public string SecurityAnswer { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm secret answer")]
        [Compare("SecurityAnswer", ErrorMessage = "The answer and confirmation answer do not match.")]
        public string ConfirmSecurityAnswer { get; set; }
    }
}
