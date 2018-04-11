app.controller('HomeCntrl', ['$scope', '$window', 'GetDataService', 'basePathService', 
    function ($scope, $window, GetDataService, basePathService) {

        var baseUrl = basePathService.domainUrl + 'Home/';

        $scope.myInterval = 3000;

        $scope.GetSliderBanners = function () {
            var apiRoute = baseUrl + 'GetBannerByType/Slider';
            GetDataService.get(apiRoute)
            .then(function (response) {
                $scope.silderBanners = response.data.banners;
            }, function (error) {
                alert('Error: ' + JSON.stringify(error));
            });
        };

        $scope.GetSliderBanners();

        //$scope.silderBanners = [
        //    { 'ImagePath': '/Content/images/11.jpg', 'Title': 'a', 'Description': 'aaa' },
        //    { 'ImagePath': '/Content/images/22.jpg', 'Title': 'b', 'Description': 'bbb' },
        //    { 'ImagePath': '/Content/images/44.jpg', 'Title': 'c', 'Description': 'ccc' }
        //];

        $scope.GetCarouselBanners = function () {
            var apiRoute = baseUrl + 'GetBannerByType/Carousel';
            GetDataService.get(apiRoute)
            .then(function (response) {
                $scope.carouselBanners = response.data.banners;
            }, function (error) {
                alert('Error: ' + JSON.stringify(error));
            });
        };

        $scope.GetCarouselBanners();

        $scope.GetNewProducts = function () {
            var apiRoute = baseUrl + 'GetNewProducts/4';
            GetDataService.get(apiRoute)
            .then(function (response) {
                $scope.newProducts = response.data.products;
            }, function (error) {
                alert('Error: ' + JSON.stringify(error));
            });
        };

        $scope.GetNewProducts();

        $scope.GetWeeklyTopProducts = function () {
            var apiRoute = baseUrl + 'GetWeeklyTopProducts/6';
            GetDataService.get(apiRoute)
            .then(function (response) {
                $scope.weeklyTopProducts = response.data.products;
            }, function (error) {
                alert('Error: ' + JSON.stringify(error));
            });
        };

        $scope.GetWeeklyTopProducts();

        $scope.GetTopDiscountedProducts = function () {
            var apiRoute = baseUrl + 'GetTopDiscountedProducts/6';
            GetDataService.get(apiRoute)
            .then(function (response) {
                $scope.topDiscountedProducts = response.data.products;
            }, function (error) {
                alert('Error: ' + JSON.stringify(error));
            });
        };

        $scope.GetTopDiscountedProducts();

    }]);