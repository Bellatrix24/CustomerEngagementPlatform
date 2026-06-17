// Global Javascript logic for CustomerEngagementPlatform

$(document).ready(function () {
    // Prevent double-submission of POST forms (helps prevent 400 Bad Request / CSRF errors during slow DB starts)
    $('form').submit(function () {
        var form = $(this);
        if (form.attr('method') && form.attr('method').toUpperCase() === 'POST') {
            // Find all submit buttons inside the form
            var submitButtons = form.find('button[type="submit"], input[type="submit"]');
            
            // Only disable and mark processing if the form is valid (if using client-side validation)
            if (form.valid && !form.valid()) {
                return; // Let standard validation handle displaying errors
            }

            // Small delay to ensure standard HTML5 form submit handler is registered
            setTimeout(function () {
                submitButtons.prop('disabled', true);
                
                // Show a loading text depending on button type
                submitButtons.each(function () {
                    var btn = $(this);
                    if (btn.is('button')) {
                        btn.data('original-html', btn.html());
                        btn.html('<span class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>Processing...');
                    } else {
                        btn.data('original-val', btn.val());
                        btn.val('Processing...');
                    }
                });
            }, 50);
        }
    });
});
