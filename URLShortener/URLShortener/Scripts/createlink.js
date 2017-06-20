$(document).ready(InitPage);

function InitPage() {
    var viewModel = {
        shortLink: function() {
            var input = $("#text-box-input").val();
            alert(input);
        },

        returnToMainLink: function() {
            
        },

        copyTextToBuffer : function() {
            
        }
    }

    ko.applyBindings(viewModel);
}