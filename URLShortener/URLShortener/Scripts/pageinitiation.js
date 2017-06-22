$(document).ready(InitPage);

function InitPage() {

    var self = this;

    /* обработчики страницы с ссылками пользователя */

    self.makeLinksFromRows = function () {
        $(".grid-cell-link").each(function(inex, element) {
            var link = $(element).text();
            $(element).text("");
            $(element).append("<a href=\"" + link + "\">" + link + "</a>");
        });
    };
    self.addArrowsOnGrid = function() {
        $(".sort_asc").find("a").append("<span class=\"glyphicon glyphicon-chevron-up\"></span>");
        $(".sort_desc").find("a").append("<span class=\"glyphicon glyphicon-chevron-down\"></span>");
    };

    self.addFrameToPagination = function () {
        $(".paginationLeft").parent().parent().attr("style","border: double; border-color: #ebebff");
    }

    /* методы для изсенения страницы после запроса на создание ссылки */

    self.newErrorElement = function (err) {
        return "<div class=\"row main-page-row error-header\" style=\"display: none\"><div class=\"col-md-6 col-lg-6 col-sm-6 error-row\" >" +
            err +
            "</div></div>";
    };

    self.setCookies = function(token) {
        var date = new Date();
        date.setYear(date.getFullYear() + 1);
        date = date.toUTCString();
        var cookie = "token=" + token + "; expires=" + date;
        document.cookie = cookie;
    };
    self.enableShortingButton = function() {
        $(".main-button").removeAttr("disabled", "disabled");
        $("#main-button-container").find("img").remove();
    };
    self.disableShortingButton = function () {
        $(".main-button").attr("disabled", "disabled");
        $("#main-button-container").append("<img src=\"Content/loading.gif\" alt=\"load-gif\"></img>");
    };
    self.addError = function(errortext) {
        if ($("div").is(".error-row")) {
            $("div.error-header").remove();
        }
        $(self.newErrorElement(errortext)).prependTo("div.container").show("slow");
    };
    self.deleteError = function() {
        $("div.error-header").remove();
    }
    self.setTitle = function (oldurl) {
        $("#title").text("Ссылка " + oldurl + " была укорочена до:");
    };
    self.setInputBox = function(newUrl) {
        $("#text-box-input").val(newUrl);
        $("#text-box-input").attr("readonly", "readonly");
    };
    self.setCopyButtonEnabled = function() {
        $("#copy-button").css("visibility", "visible");
    };
    self.changeMainButtonFunc = function() {
        $(".main-button").attr("id", "return-button");
        $(".main-button").text("На главную");
    };

    /* биндинги для нокаута по обработке запроса на создание ссылки */

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
                            self.deleteError();
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
    };
    ko.applyBindings(viewModel);

    // для таблицы ссылок пользователей
    self.addArrowsOnGrid();
    self.makeLinksFromRows();
    self.addFrameToPagination();
}