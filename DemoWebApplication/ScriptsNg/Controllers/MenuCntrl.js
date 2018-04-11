app.controller('MenuCntrl', ['$scope', '$window', 'GetDataService', 'basePathService',
    function ($scope, $window, GetDataService, basePathService) {

        var baseUrl = basePathService.domainUrl + 'Home/';

        $scope.GetCategorywiseSubCategoryList = function () {
            var apiRoute = baseUrl + 'GetCategorySubCategoryList';
            GetDataService.get(apiRoute)
            .then(function (response) {
                $scope.categorywiseSubCategories = response.data.categorywiseSubCategories;
            }, function (error) {
                alert('Error: ' + JSON.stringify(error));
            });
        };

        $scope.GetCategorywiseSubCategoryList();

    }]);