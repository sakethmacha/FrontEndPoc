$(document).ready(function () {

    // ========== STRICT EMAIL VALIDATOR ==========
    $.validator.addMethod("emailvalidate", function (value, element, params) {
        if (!value) return true;
        value = value.trim();
        if (value.indexOf('@') === -1) return false;
        var atIndex = value.indexOf('@');
        var dotIndex = value.lastIndexOf('.');
        if (dotIndex <= atIndex) return false;
        if (value.substring(dotIndex + 1).length < 2) return false;
        return new RegExp(params.pattern).test(value);
    });

    $.validator.unobtrusive.adapters.add("emailvalidate", ["pattern"], function (options) {
        options.rules["emailvalidate"] = { pattern: options.params.pattern };
        options.messages["emailvalidate"] = options.message;
    });

    // ========== PASSWORD VALIDATOR ==========
    $.validator.addMethod("passwordvalidate", function (value, element, params) {
        if (!value) return true;
        return new RegExp(params.pattern).test(value);
    });

    $.validator.unobtrusive.adapters.add("passwordvalidate", ["pattern"], function (options) {
        options.rules["passwordvalidate"] = { pattern: options.params.pattern };
        options.messages["passwordvalidate"] = options.message;
    });

    // ========== NAME VALIDATOR ==========
    $.validator.addMethod("namevalidate", function (value, element, params) {
        if (!value) return true;
        return new RegExp(params.pattern).test(value);
    });

    $.validator.unobtrusive.adapters.add("namevalidate", ["pattern"], function (options) {
        options.rules["namevalidate"] = { pattern: options.params.pattern };
        options.messages["namevalidate"] = options.message;
    });

    // ========== REAL-TIME EMAIL FEEDBACK ==========
    $('input[name="Email"]').on('blur keyup input', function () {
        var email = $(this).val().trim();
        var $feedback = $('#emailFeedback');
        if (!$feedback.length) return;

        if (!email.length) {
            $feedback.text('').removeClass('text-danger text-success');
            return;
        }

        if (email.indexOf('@') === -1) {
            $feedback.text('✗ Email must contain @ symbol').removeClass('text-success').addClass('text-danger');
        } else {
            var atIndex = email.indexOf('@');
            var dotIndex = email.lastIndexOf('.');
            if (dotIndex <= atIndex) {
                $feedback.text('✗ Email must contain a domain (e.g., @example.com)').removeClass('text-success').addClass('text-danger');
            } else if (email.substring(dotIndex + 1).length < 2) {
                $feedback.text('✗ Domain extension must be at least 2 characters').removeClass('text-success').addClass('text-danger');
            } else {
                $feedback.text('✓ Valid email format').removeClass('text-danger').addClass('text-success');
            }
        }
    });

    // ========== PASSWORD STRENGTH INDICATOR ==========
    $('input[name="Password"]').on('keyup input', function () {
        var password = $(this).val();
        var $strength = $('#passwordStrength');
        if (!$strength.length) return;

        if (!password.length) {
            $strength.html('');
            return;
        }

        var score = 0;
        if (password.length >= 8) score++;
        if (/[a-z]/.test(password)) score++;
        if (/[A-Z]/.test(password)) score++;
        if (/\d/.test(password)) score++;
        if (/[@$!%*?&#]/.test(password)) score++;

        var label, barClass, width;
        if (score < 3) { label = 'Weak'; barClass = 'bg-danger'; width = 33; }
        else if (score < 5) { label = 'Medium'; barClass = 'bg-warning'; width = 66; }
        else { label = 'Strong'; barClass = 'bg-success'; width = 100; }

        $strength.html(`
            <small class="text-muted">Password Strength: <span class="fw-bold">${label}</span></small>
            <div class="progress mt-1" style="height:5px;">
                <div class="progress-bar ${barClass}" style="width:${width}%"
                     role="progressbar" aria-valuenow="${width}" aria-valuemin="0" aria-valuemax="100"></div>
            </div>
        `);
    });

    // ========== PREVENT NON-ALPHABETIC IN NAME ==========
    $('input[name="Name"]').on('keypress', function (e) {
        if (!/[a-zA-Z\s]/.test(String.fromCharCode(e.which))) {
            e.preventDefault();
        }
    });

    // ========== BORDER-ONLY VALIDATION FEEDBACK (no layout shift) ==========
    $('input, select, textarea').on('blur', function () {
        if (!$(this).val()) return;
        if ($(this).valid()) {
            $(this).css('outline', '2px solid #198754').css('outline-offset', '-1px');
        } else {
            $(this).css('outline', '2px solid #dc3545').css('outline-offset', '-1px');
        }
    });

    $('input, select, textarea').on('focus', function () {
        $(this).css('outline', '');
    });
});