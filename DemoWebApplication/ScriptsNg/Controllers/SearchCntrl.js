app.controller('SearchCntrl', ['$scope', '$window', 'GetDataService', 'basePathService',
    function ($scope, $window, GetDataService, basePathService) {

        $scope.selectedItem = function (selected) {
            if (selected) {
                document.location.href = selected.originalObject.PageUrl;
            }
        };

    }]);