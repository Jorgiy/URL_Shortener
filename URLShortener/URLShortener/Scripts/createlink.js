$(document).ready(InitPage);

function InitPage() {
    var viewModel = {
        shortLink: function() {
            var input = $("#text-box-input").val();
            $(".shorten-button").css("disabled","disabled");
            $.ajax({
                url: "Home/CreateShortLink",
                type: "post",
                data: { url: input },
                success: function(data) {
                    var res = data.response;
                },
                error : function() {
                    if ($("div").is(".error-row")) {
                        $(".error-row").text("Произошла неизвестная ошибка :(");
                    } else {
                        $("div.container").prepend("<div class=\"row main-page-row\"><div class=\"col-md-6 col-lg-6 col-sm-6 error-row\" >Произошла неизвестная ошибка :(</div></div>");
                    }
                }
            });
        },

        returnToMainLink: function() {
            window.location.href ="";
        }
    }

    ko.applyBindings(viewModel);
}