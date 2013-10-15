//var bootstrapValidation = {
//    debug: true,
//    highlight: function (label) {
//        $(label).closest('.form-group').addClass('has-error');
//        $(label).closest('.form-group').removeClass('has-success');
//    },
//    success: function (label) {
//        $(label).text('OK!').addClass('valid')
//                    .closest('.form-group').addClass('has-success');
//    },
//    submitHandler: function (form) {
//        if ($(form).valid()) {
//            form.submit();
//        }
//        return false; // prevent normal form posting
//    }
//}
function boostrapHighlight(element, errorClass, validClass) {
    $(element).closest(".form-group").addClass("has-error");
    $(element).trigger('highlated');
};

function boostrapUnhighlight(element, errorClass, validClass) {
    $(element).closest(".form-group").removeClass("has-error");
    $(element).trigger('unhighlated');
};

$.validator.setDefaults({
    ignore: "",
    highlight: function (element, errorClass, validClass) {
        boostrapHighlight(element, errorClass, validClass);
    },
    unhighlight: function (element, errorClass, validClass) {
        boostrapUnhighlight(element, errorClass, validClass);
    }
});
$('.input-validation-error').closest('.form-group').addClass('has-error');