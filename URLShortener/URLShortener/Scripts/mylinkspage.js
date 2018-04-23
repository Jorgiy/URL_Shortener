$(document).ready(InitPage);

function InitPage() {

    var self = this;

    self.makeLinksFromRows = function () {
        $(".grid-cell-link").each(function (inex, element) {
            var link = $(element).text();
            $(element).text("");
            $(element).append("<a href=\"" + link + "\">" + link + "</a>");
        });
    };
    self.addArrowsOnGrid = function () {
        $(".sort_asc").find("a").append("<span class=\"glyphicon glyphicon-chevron-up\"></span>");
        $(".sort_desc").find("a").append("<span class=\"glyphicon glyphicon-chevron-down\"></span>");
    };

    self.addFrameToPagination = function () {
        $(".paginationLeft").parent().parent().attr("style", "border: double; border-color: #ebebff");
    };

    self.addArrowsOnGrid();
    self.makeLinksFromRows();
    self.addFrameToPagination();
}