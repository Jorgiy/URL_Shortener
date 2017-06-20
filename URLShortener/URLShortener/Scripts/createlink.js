$(document).ready(InitPage);

function InitPage() {

    var self = this;

    self.newError = function (err) {
        return "<div class=\"row main-page-row\"><div class=\"col-md-6 col-lg-6 col-sm-6 error-row\" >" +
            err +
            "</div></div>";
    };

    self.setCookies = function(token) {
        var date = new Date();
        date.setYear(date.getFullYear() + 1);
        date = date.toUTCString();
        var cookie = "token=" + token + "; expires=" + date;
        document.cookie = cookie;
    }

    var viewModel = {
        shortLink: function() {
            var input = $("#text-box-input").val();

            $("#shorten-button").attr("disabled", "disabled");

            $.ajax({
                url: "Home/CreateShortLink",
                type: "post",
                data: { url: input },
                success: function (res) {
                    self.setCookies(res.Token);
                },
                error : function() {
                    if ($("div").is(".error-row")) {
                        $(".error-row").text("Произошла неизвестная ошибка :(");
                    } else {
                        $("div.container").prepend(self.newError("Произошла неизвестная ошибка :("));
                    }

                    $("#shorten-button").removeAttr("disabled", "disabled");
                }
            });
        },

        returnToMainLink: function() {
            window.location.href ="";
        }
    }

    ko.applyBindings(viewModel);
}