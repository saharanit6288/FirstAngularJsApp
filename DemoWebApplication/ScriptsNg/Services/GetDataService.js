app.service('GetDataService', function ($http) {
    
    this.get = function (apiRoute) {
        return $http.get(apiRoute);
    };

});