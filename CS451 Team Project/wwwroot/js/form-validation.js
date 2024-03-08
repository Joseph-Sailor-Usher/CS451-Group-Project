document.addEventListener('DOMContentLoaded', function () {
    'use strict';

    var forms = document.querySelectorAll('.needs-validation');

    Array.prototype.slice.call(forms).forEach(function (form) {
        form.addEventListener('submit', function (event) {
            var password = document.getElementById('password').value;
            var isValidPassword = validatePassword(password);
            
            if (!form.checkValidity() || !isValidPassword) {
                event.preventDefault();
                event.stopPropagation();

                // If password is invalid, show custom invalid feedback
                if (!isValidPassword) {
                    document.getElementById('password').classList.add('is-invalid');
                }
            }

            form.classList.add('was-validated');
        }, false);
    });

    function validatePassword(password) {
        var specialChars = /[^A-Za-z0-9]/g;
        var uppercase = /[A-Z]/g;
        var lowercase = /[a-z]/g;
        var numbers = /\d/g;

        if (password.length >= 16 && password.length <= 24 &&
            (password.match(specialChars) || []).length >= 4 &&
            (password.match(uppercase) || []).length >= 4 &&
            (password.match(lowercase) || []).length >= 4 &&
            (password.match(numbers) || []).length >= 4) {
            return true;
        } else {
            return false;
        }
    }
});
