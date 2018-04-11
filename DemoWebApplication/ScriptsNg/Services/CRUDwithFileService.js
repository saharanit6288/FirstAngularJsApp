app.service('CRUDwithFileService', function ($http) {

    this.post = function (apiRoute, model) {
        return $http.post(apiRoute, model, {
            headers: { 'Content-Type': undefined },
            transformRequest: angular.identity
        });
    };

    this.put = function (apiRoute, model) {
        return $http.put(apiRoute, model, {
            headers: { 'Content-Type': undefined },
            transformRequest: angular.identity
        });
    };

    this.delete = function (apiRoute) {
        return $http.delete(apiRoute);
    };

    this.getAll = function (apiRoute) {
        return $http.get(apiRoute);
    };

    this.getById = function (apiRoute, Id) {
        return $http.get(apiRoute + '/' + Id);
    };

    this.deleteAll = function (apiRoute, model) {
        return $http.post(apiRoute, model);
    };
});