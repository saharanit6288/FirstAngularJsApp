app.controller('PrdListCntrl', ['$scope', '$window', 'GetDataService', 'basePathService',
    function ($scope, $window, GetDataService, basePathService) {

        var baseUrl = basePathService.domainUrl + 'Product/';

        var path = window.location.pathname;
        var arr = path.split("/");

        var productType = arr[3];
        var typeID = parseInt(arr[4]);

        $scope.prdPageSizes = [
            { 'ID': 1, 'Title': 'Item on page 9' },
            { 'ID': 2, 'Title': 'Item on page 18' },
            { 'ID': 3, 'Title': 'Item on page 32' },
            { 'ID': 4, 'Title': 'All' }
        ];
        $scope.selectedPrdPageSize = $scope.prdPageSizes[0].ID;

        $scope.prdSortOrders = [
            { 'ID': 1, 'Title': 'Default sorting' },
            { 'ID': 2, 'Title': 'Sort by high to low price' },
            { 'ID': 3, 'Title': 'Sort by low to high price' },
            { 'ID': 4, 'Title': 'Sort by rating' },
            { 'ID': 5, 'Title': 'Sort by discount' }
        ];
        $scope.selectedPrdSortOrder = $scope.prdSortOrders[0].ID;

        $scope.currentPage = 1;

        $scope.products = [];

        $scope.GetAllProductByType = function (pageNo) {
            var apiRoute = baseUrl + 'GetAllDetails/' + productType + '/' + typeID + '/' + $scope.selectedPrdSortOrder + '/' + pageNo + '/' + $scope.selectedPrdPageSize;
            GetDataService.get(apiRoute)
            .then(function (response) {
                $scope.totalItems = response.data.totalCount;
                $scope.products = response.data.products;
                $scope.pageSize = response.data.newPageSize;
                if (productType == 'Category') { $scope.subCategoryTitleShow = false; }
                if (productType == 'SubCategory') { $scope.subCategoryTitleShow = true; }
            }, function (error) {
                alert('Error: ' + JSON.stringify(error));
            });
        };

        $scope.GetAllProductByType($scope.currentPage);

        $scope.ddPagesizeChanged = function () {
            $scope.currentPage = 1;
            $scope.pageSize = $scope.selectedPrdPageSize;
            $scope.products = [];
            $scope.GetAllProductByType($scope.currentPage);
        };

        $scope.ddSortingChanged = function () {
            $scope.currentPage = 1;
            $scope.pageSize = $scope.selectedPrdPageSize;
            $scope.products = [];
            $scope.GetAllProductByType($scope.currentPage);
        };

    }]);