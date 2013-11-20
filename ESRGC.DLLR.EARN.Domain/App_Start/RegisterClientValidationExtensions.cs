using DataAnnotationsExtensions.ClientValidation;

[assembly: WebActivator.PreApplicationStartMethod(typeof(ESRGC.DLLR.EARN.Domain.App_Start.RegisterClientValidationExtensions), "Start")]
 
namespace ESRGC.DLLR.EARN.Domain.App_Start {
    public static class RegisterClientValidationExtensions {
        public static void Start() {
            DataAnnotationsModelValidatorProviderExtensions.RegisterValidationExtensions();            
        }
    }
}