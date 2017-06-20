$(document).ready(InitPage);

function InitPage() {
    var viewModel = {
        shortLink: function() {
            var input = $("#text-box-input").val();
            $.ajax({
                url: "Home/CreateShortLink",
                type: "post",
                data: { url: input },
                success: function() {
                    
                }
            });
        },

        returnToMainLink: function() {
            window.location.href('~/');
        }
    }

    ko.applyBindings(viewModel);
}