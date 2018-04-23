var app = angular.module('App', []);
app.controller('mainpageController', function($scope, $http) {
    var self = this;

    var defaultTitile = 'Make your links shorter';
    var defaultButtonText = 'Shorten link!';   
    
    var setDefaults = function () {
        self.linkCreated = false;
        self.creationStarted = false;
        self.errorOccured = false;        
        self.urlForShortening = '';
        self.titleText = defaultTitile;
        self.buttonText = defaultButtonText;
    };

    var setCookies = function (token) {
        var date = new Date();
        date.setYear(date.getFullYear() + 1);
        date = date.toUTCString();
        document.cookie = 'token=' + token + '; expires=' + date;
    };
    
    setDefaults();
    
    self.createLink = function () {
        if (!self.linkCreated) {

            self.creationStarted = true;

            $http({
                method: 'POST',
                url: "Home/CreateShortLink",
                data: {
                    url: self.urlForShortening,
                    timeOffset: new Date().getTimezoneOffset() / 60
                }
            }).then(function onSuccess(response) {
                self.errorOccured = false;
                var responseData = response.data;

                if (responseData.ErrorMessage) {
                    self.errorText = responseData.ErrorMessage;
                    self.errorOccured = true;
                }
                
                if (responseData.Success) {
                    self.errorOccured = false;
                    self.titleText = 'Link ' + responseData.Url + ' was shorten to:';
                    self.urlForShortening = responseData.Host + responseData.ShortUrl;
                    self.linkCreated = true;
                    self.buttonText = 'To main page';

                    if (responseData.TokenCreated) {
                        setCookies(res.Token)
                    }
                }
                
                self.creationStarted = false;
                
            }, function onError(response) {
                self.errorText = 'Unknown error occured :(';
                self.errorOccured = true;
                self.creationStarted = false;
            });       
            
        } else {
            setDefaults();
        }
    };      
});