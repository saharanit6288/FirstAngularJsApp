app.controller('PrdDtlsCntrl', ['$scope', '$window', 'GetDataService', 'basePathService', 
    function ($scope, $window, GetDataService, basePathService) {

        var baseUrl = basePathService.domainUrl + 'Product/';

        var path = window.location.pathname;
        var arr = path.split("/");

        var productID = parseInt(arr[3]);

        $scope.product = {};

        $scope.GetProductDetails = function () {
            var apiRoute = baseUrl + 'GetSingleDetails/' + productID;
            GetDataService.get(apiRoute)
            .then(function (response) {
                $scope.product = response.data.product;
            }, function (error) {
                alert('Error: ' + JSON.stringify(error));
            });
        };

        $scope.GetProductDetails();

        $scope.GetNewProducts = function () {
            var apiRoute = basePathService.domainUrl + 'Home/GetNewProducts/4';
            GetDataService.get(apiRoute)
            .then(function (response) {
                $scope.newProducts = response.data.products;
            }, function (error) {
                alert('Error: ' + JSON.stringify(error));
            });
        };

        $scope.GetNewProducts();

    }]);