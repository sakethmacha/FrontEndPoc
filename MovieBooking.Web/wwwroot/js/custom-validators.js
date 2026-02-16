$(document).ready(function () {

    // ========== STRICT EMAIL VALIDATOR ==========
    $.validator.addMethod("emailvalidate", function (value, element, params) {
        if (!value) {
            return true; // Let [Required] handle empty values
        }

        // Trim whitespace
        value = value.trim();

        // Must contain @
        if (value.indexOf('@') === -1) {
            return false;
        }

        // Must contain . after @
        var atIndex = value.indexOf('@');
        var dotIndex = value.lastIndexOf('.');

        if (dotIndex <= atIndex) {
            return false;
        }

        // Check TLD (domain extension) is at least 2 chars
        var tld = value.substring(dotIndex + 1);
        if (tld.length < 2) {
            return false;
        }

        // Validate with regex pattern
        var pattern = new RegExp(params.pattern);
        return pattern.test(value);
    });

    $.validator.unobtrusive.adapters.add("emailvalidate", ["pattern"], function (options) {
        options.rules["emailvalidate"] = {
            pattern: options.params.pattern
        };
        options.messages["emailvalidate"] = options.message;
    });

    // ========== PASSWORD VALIDATOR ==========
    $.validator.addMethod("passwordvalidate", function (value, element, params) {
        if (!value) {
            return true;
        }
        var pattern = new RegExp(params.pattern);
        return pattern.test(value);
    });

    $.validator.unobtrusive.adapters.add("passwordvalidate", ["pattern"], function (options) {
        options.rules["passwordvalidate"] = {
            pattern: options.params.pattern
        };
        options.messages["passwordvalidate"] = options.message;
    });

    // ========== NAME VALIDATOR ==========
    $.validator.addMethod("namevalidate", function (value, element, params) {
        if (!value) {
            return true;
        }
        var pattern = new RegExp(params.pattern);
        return pattern.test(value);
    });

    $.validator.unobtrusive.adapters.add("namevalidate", ["pattern"], function (options) {
        options.rules["namevalidate"] = {
            pattern: options.params.pattern
        };
        options.messages["namevalidate"] = options.message;
    });

    // ========== REAL-TIME EMAIL VALIDATION FEEDBACK ==========
    $('input[name="Email"]').on('blur keyup', function () {
        var email = $(this).val().trim();
        var $feedbackDiv = $('#emailFeedback');

        // Remove existing feedback
        $feedbackDiv.remove();

        if (email.length > 0) {
            var feedback = '';
            var feedbackClass = '';

            if (email.indexOf('@') === -1) {
                feedback = '✗ Email must contain @ symbol';
                feedbackClass = 'text-danger';
            } else {
                var atIndex = email.indexOf('@');
                var dotIndex = email.lastIndexOf('.');

                if (dotIndex <= atIndex) {
                    feedback = '✗ Email must contain a domain (e.g., @example.com)';
                    feedbackClass = 'text-danger';
                } else {
                    var tld = email.substring(dotIndex + 1);
                    if (tld.length < 2) {
                        feedback = '✗ Domain extension must be at least 2 characters';
                        feedbackClass = 'text-danger';
                    } else if ($(this).valid()) {
                        feedback = '✓ Valid email format';
                        feedbackClass = 'text-success';
                    }
                }
            }

            if (feedback) {
                $(this).after('<small id="emailFeedback" class="form-text ' + feedbackClass + '">' + feedback + '</small>');
            }
        }
    });

    // ========== PREVENT NON-ALPHABETIC IN NAME ==========
    $('input[name="Name"]').on('keypress', function (e) {
        var char = String.fromCharCode(e.which);
        if (!/[a-zA-Z\s]/.test(char)) {
            e.preventDefault();
            return false;
        }
    });

    // ========== PASSWORD STRENGTH INDICATOR ==========
    $('input[name="Password"]').on('keyup', function () {
        var password = $(this).val();
        var strength = 0;

        if (password.length >= 8) strength++;
        if (/[a-z]/.test(password)) strength++;
        if (/[A-Z]/.test(password)) strength++;
        if (/\d/.test(password)) strength++;
        if (/[@$!%*?&#]/.test(password)) strength++;

        $('#passwordStrength').remove();

        if (password.length > 0) {
            var strengthText = "";
            var strengthClass = "";
            var progressWidth = 0;

            if (strength < 3) {
                strengthText = "Weak";
                strengthClass = "bg-danger";
                progressWidth = 33;
            } else if (strength < 5) {
                strengthText = "Medium";
                strengthClass = "bg-warning";
                progressWidth = 66;
            } else {
                strengthText = "Strong";
                strengthClass = "bg-success";
                progressWidth = 100;
            }

            var strengthHtml = `
                <div id="passwordStrength" class="mt-1">
                    <small class="text-muted">Password Strength: <span class="fw-bold">${strengthText}</span></small>
                    <div class="progress" style="height: 5px;">
                        <div class="progress-bar ${strengthClass}" role="progressbar" 
                             style="width: ${progressWidth}%" aria-valuenow="${progressWidth}" 
                             aria-valuemin="0" aria-valuemax="100"></div>
                    </div>
                </div>
            `;
            $(this).parent().append(strengthHtml);
        }
    });

    // ========== BOOTSTRAP VALIDATION CLASSES ==========
    $('input, select, textarea').on('blur', function () {
        if ($(this).val()) {
            if ($(this).valid()) {
                $(this).removeClass('is-invalid').addClass('is-valid');
            } else {
                $(this).removeClass('is-valid').addClass('is-invalid');
            }
        }
    });

    $('input, select, textarea').on('focus', function () {
        $(this).removeClass('is-invalid is-valid');
    });
});