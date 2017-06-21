$(document).ready(InitPage);

function InitPage() {

    var self = this;

    self.newErrorElement = function (err) {
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

    self.enableShortingButton = function() {
        $(".main-button").removeAttr("disabled", "disabled");
    }

    self.disableShortingButton = function () {
        $(".main-button").attr("disabled", "disabled");
    }

    self.addError = function(errortext) {
        if ($("div").is(".error-row")) {
            $(".error-row").text(errortext);
        } else {
            $("div.container").prepend(self.newErrorElement(errortext));
        }
    }

    self.setTitle = function (oldurl) {
        $("#title").text("Ссылка " + oldurl + " была укорочена до:");
    }

    self.setInputBox = function(newUrl) {
        $("#text-box-input").val(newUrl);
        $("#text-box-input").attr("readonly", "readonly");
    }

    self.setCopyButtonEnabled = function() {
        $("#copy-button").css("visibility", "visible");
    }

    self.changeMainButtonFunc = function() {
        $(".main-button").attr("id", "return-button");
        $(".main-button").text("На главную");
    }
    
    var viewModel = {
        shortLink: function () {
            if ($(".main-button").is("#shorten-button")) {
                var input = $("#text-box-input").val();

                self.disableShortingButton();

                $.ajax({
                    url: "Home/CreateShortLink",
                    type: "post",
                    data: { url: input },
                    success: function(res) {
                        if (res.ErrorMessage != null) self.addError(res.ErrorMessage);

                        if (res.Success) {
                            self.setTitle(res.Url);
                            self.setInputBox(res.Host + res.ShortUrl);
                            self.setCopyButtonEnabled();
                            self.changeMainButtonFunc();

                            if (res.TokenCreated) self.setCookies(res.Token);
                        }
                        self.enableShortingButton();
                    },
                    error: function() {
                        self.addError("Произошла неизвестная ошибка :(");
                        self.enableShortingButton();
                    }
                });
            } else {
                window.location.href = "";
            }
        }
    }

    ko.applyBindings(viewModel);
}