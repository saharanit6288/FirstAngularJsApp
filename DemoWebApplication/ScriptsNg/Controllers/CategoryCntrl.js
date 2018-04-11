app.controller('CategoryCntrl', ['$scope', '$window', 'CRUDService', 'basePathService',
    function ($scope, $window, CRUDService, basePathService) {

        var baseUrl = basePathService.domainUrl + 'Category/';
        $scope.btnText = 'Submit';
        //$scope.catID = 0;
        $scope.TxtMsg = '';
        $scope.errors = [];
        $scope.searchTerm = '';
        $scope.checkedAll = false;
        $scope.selectedIds = [];

        $scope.pageSizes = [5,10, 25, 50, 100];
        $scope.selectedPageSize = $scope.pageSizes[0];

        $scope.currentPage = 1;
        $scope.pageSize = $scope.selectedPageSize;


        $scope.categories = [];

        
        //$scope.Clear = function () {
        //    $scope.catID = 0;
        //    $scope.title = "";
        //    $scope.sequence = "";
        //};

        //$scope.Clear();

        $scope.category = {};

        //Save Or Update a Data
        $scope.SaveUpdate = function () {

            if ($scope.frmCat.$invalid) {
                $scope.errorMsg = true;
                return false;
            }

            if ($scope.btnText.toLowerCase() == 'Submit'.toLowerCase()) {
                var apiRoute = baseUrl + 'SaveCategory';

                CRUDService.post(apiRoute, $scope.category)
                .then(function (response) {
                    if (response.data.status == 1) {
                        $scope.TxtMsg = 'Data Save Successfully';
                        $scope.errors = [];
                        $scope.category = {};
                        $scope.selectedIds = [];
                        $scope.checkedAll = false;
                        $scope.GetCategories($scope.currentPage);
                    }
                    else if (response.data.errCode == 401) {
                        $scope.TxtMsg = '';
                        $scope.errors = response.data.status;
                    }
                    else {
                        $scope.TxtMsg = response.data.status;
                    }
                }, function (error) {
                    alert('Error: ' + JSON.stringify(error));
                });
            }
            else {
                var apiRoute = baseUrl + 'UpdateCategory';

                CRUDService.post(apiRoute, $scope.category)
                .then(function (response) {
                    if (response.data.status == 1) {
                        $scope.TxtMsg = 'Data Update Successfully';
                        $scope.errors = [];
                        $scope.category = {};
                        $scope.selectedIds = [];
                        $scope.checkedAll = false;
                        $scope.GetCategories($scope.currentPage);
                    }
                    else if (response.data.errCode == 401) {
                        $scope.TxtMsg = '';
                        $scope.errors = response.data.status;
                    }
                    else {
                        $scope.TxtMsg = response.data.status;
                    }
                }, function (error) {
                    alert('Error: ' + JSON.stringify(error));
                });
            }
            $scope.btnText = 'Submit';
            $scope.InvalidExt = '';
        };

        //Delete a Data
        $scope.DeleteCategory = function (id) {
            var apiRoute = baseUrl + 'DeleteCategory/' + id;
            var deleteUser = $window.confirm('Are you sure you want to delete?');

            if (deleteUser) {

                CRUDService.delete(apiRoute)
                .then(function (response) {
                    if (response.data.status == 1) {
                        $scope.TxtMsg = 'Data Delete Successfully';
                        //$scope.Clear();
                        $scope.category = {};
                        $scope.selectedIds = [];
                        $scope.GetCategories($scope.currentPage);
                        $scope.btnText = 'Submit';
                        $scope.checkedAll = false;
                    }
                    else {
                        $scope.TxtMsg = response.data.status;
                    }
                }, function (error) {
                    alert('Error: ' + JSON.stringify(error));
                });
            }
        };


        //Get all Data
        $scope.GetCategories = function (pageNo) {
            $scope.errorMsg = false;
            $scope.selectedIds = [];
            $scope.checkedAll = false;
            var apiRoute = baseUrl + 'GetCategories?term=' + $scope.searchTerm + '&page=' + pageNo + '&pageSize=' + $scope.pageSize;
            CRUDService.getAll(apiRoute)
            .then(function (response) {
                $scope.totalItems = response.data.totalCount;
                $scope.categories = response.data.categories;
            }, function (error) {
                alert('Error: ' + JSON.stringify(error));
            });
        };

        $scope.GetCategories($scope.currentPage);

        //Get a Data
        $scope.GetCategoryByID = function (id) {
            var apiRoute = baseUrl + 'GetCategoryByID';
            CRUDService.getById(apiRoute, id)
            .then(function (response) {
                $scope.category = response.data.category;
                $scope.selectedIds = [];
                $scope.checkedAll = false;
                $scope.btnText = 'Update';
                $scope.TxtMsg = '';
            }, function (error) {
                alert('Error: ' + JSON.stringify(error));
            });
        };

        $scope.SearchItem = function () {
            $scope.selectedIds = [];
            $scope.checkedAll = false;

            $scope.GetCategories($scope.currentPage);
        };

        $scope.dropDownChanged = function () {
            $scope.currentPage = 1;
            $scope.pageSize = $scope.selectedPageSize;
            $scope.categories = [];
            $scope.selectedIds = [];
            $scope.checkedAll = false;

            $scope.GetCategories($scope.currentPage);
        };

        /*---Toggle All chkbox select/deselect---*/
        $scope.toggleCheckAll = function () {
            if ($scope.checkedAll) {
                angular.forEach($scope.categories, function (data) {
                    data.checked = true;
                    $scope.checkedAll = true;
                    $scope.cbChecked(data);
                });
            }
            else {
                angular.forEach($scope.categories, function (data) {
                    data.checked = false;
                    $scope.checkedAll = false;
                    $scope.cbChecked(data);
                });
            }

        }

        $scope.cbChecked = function (data) {
            if (data.checked && $scope.selectedIds.indexOf(data.ID) == -1) {
                $scope.selectedIds.push(data.ID);
            }
            else if (!data.checked) {
                $scope.selectedIds.splice($scope.selectedIds.indexOf(data.ID), 1);
            }
            $scope.checkedAll = true;
            angular.forEach($scope.categories, function (data) {
                if (!data.checked) {
                    $scope.checkedAll = false;
                }
            });
        }


        //Delete Multiple Data
        $scope.deleteSelectedData = function (Ids) {
            var apiRoute = baseUrl + 'DeleteSelectedCategories/';
            var deleteConfirm = $window.confirm('Are you sure you want to delete?');

            if (deleteConfirm) {
                if (Ids.length <= 0) {
                    $window.alert('Please Select Atleast One');
                    return;
                }

                CRUDService.post(apiRoute, Ids)
                .then(function (response) {
                    if (response.data.status == 1) {
                        $scope.TxtMsg = 'Data Delete Successfully';
                        //$scope.Clear();
                        $scope.category = {};
                        $scope.categories = [];
                        $scope.selectedIds = [];
                        $scope.checkedAll = false;
                        $scope.GetCategories($scope.currentPage);
                        $scope.btnText = 'Submit';
                    }
                    else {
                        $scope.TxtMsg = response.data.status;
                    }
                }, function (error) {
                    alert('Error: ' + JSON.stringify(error));
                });
            }
        };

    }]);