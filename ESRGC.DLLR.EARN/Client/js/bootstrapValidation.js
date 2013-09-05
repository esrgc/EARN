var bootstrapValidation = {
    debug: true,
    highlight: function (label) {
        $(label).closest('.control-group').addClass('error');
        $(label).closest('.control-group').removeClass('success');
    },
    success: function (label) {
        $(label).text('OK!').addClass('valid')
                    .closest('.control-group').addClass('success');
    },
    submitHandler: function (form) {
        if ($(form).valid()) {
            form.submit();
        }
        return false; // prevent normal form posting
    }
}