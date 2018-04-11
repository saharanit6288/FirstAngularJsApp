app.service('CRUDService', function ($http) {

    this.post = function (apiRoute, model) {
        return $http.post(apiRoute, model);
    };

    this.put = function (apiRoute, model) {
        return $http.put(apiRoute, model);
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
});