app.controller('SubCategoryCntrl', ['$scope', '$window', 'CRUDService', 'basePathService',
    function ($scope, $window, CRUDService, basePathService) {
        
        var baseUrl = basePathService.domainUrl + 'SubCategory/';
        $scope.btnText = 'Submit';
        //$scope.catID = 0;
        $scope.TxtMsg = '';
        $scope.errors = [];
        $scope.searchTerm = '';
        $scope.checkedAll = false;
        $scope.selectedIds = [];

        $scope.pageSizes = [5, 10, 25, 50, 100];
        $scope.selectedPageSize = $scope.pageSizes[0];

        $scope.currentPage = 1;
        $scope.pageSize = $scope.selectedPageSize;


        $scope.subCategories = [];

        //$scope.Clear = function () {
        //    $scope.catID = 0;
        //    $scope.title = "";
        //    $scope.sequence = "";
        //};

        //$scope.Clear();

        
        
        $scope.categories = null;

        $scope.getAllCategories = function () {
            $scope.errorMsg = false;
            var apiRoute = basePathService.domainUrl + 'Category/GetAllCategories';
            CRUDService.getAll(apiRoute)
            .then(function (response) {
                $scope.categories = response.data.categories;
            }, function (error) {
                alert('Error: ' + JSON.stringify(error));
            });
        };

        $scope.getAllCategories();

        $scope.subCategory = {};
        $scope.subCategory.CategoryID = "";

        //Save Or Update a Data
        $scope.SaveUpdate = function () {

            if ($scope.subCategory.CategoryID == "") {
                $scope.errorMsg = true;
                return false;
            }

            if ($scope.frmSubCat.$invalid) {
                $scope.errorMsg = true;
                return false;
            }
            
            if ($scope.btnText.toLowerCase() == 'Submit'.toLowerCase()) {
                var apiRoute = baseUrl + 'SaveSubCategory';
                debugger;
                CRUDService.post(apiRoute, $scope.subCategory)
                .then(function (response) {
                    if (response.data.status == 1) {
                        $scope.TxtMsg = 'Data Save Successfully';
                        $scope.errors = [];
                        $scope.subCategory = {};
                        $scope.subCategory.CategoryID = "";
                        $scope.selectedIds = [];
                        $scope.checkedAll = false;
                        $scope.GetSubCategories($scope.currentPage);
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
                var apiRoute = baseUrl + 'UpdateSubCategory';

                CRUDService.post(apiRoute, $scope.subCategory)
                .then(function (response) {
                    if (response.data.status == 1) {
                        $scope.TxtMsg = 'Data Update Successfully';
                        $scope.errors = [];
                        $scope.subCategory = {};
                        $scope.subCategory.CategoryID = "";
                        $scope.selectedIds = [];
                        $scope.checkedAll = false;
                        $scope.GetSubCategories($scope.currentPage);
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
        $scope.DeleteSubCategory = function (id) {
            var apiRoute = baseUrl + 'DeleteSubCategory/' + id;
            var deleteUser = $window.confirm('Are you sure you want to delete?');

            if (deleteUser) {

                CRUDService.delete(apiRoute)
                .then(function (response) {
                    if (response.data.status == 1) {
                        $scope.TxtMsg = 'Data Delete Successfully';
                        $scope.subCategory = {};
                        $scope.subCategory.CategoryID = "";
                        $scope.GetSubCategories($scope.currentPage);
                        $scope.btnText = 'Submit';
                        $scope.selectedIds = [];
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
        $scope.GetSubCategories = function (pageNo) {
            $scope.errorMsg = false;
            $scope.selectedIds = [];
            $scope.checkedAll = false;
            var apiRoute = baseUrl + 'GetSubCategories?term=' + $scope.searchTerm + '&page=' + pageNo + '&pageSize=' + $scope.pageSize;
            CRUDService.getAll(apiRoute)
            .then(function (response) {
                $scope.totalItems = response.data.totalCount;
                $scope.subCategories = response.data.subCategories;
            }, function (error) {
                alert('Error: ' + JSON.stringify(error));
            });
        };

        $scope.GetSubCategories($scope.currentPage);

        //Get a Data
        $scope.GetSubCategoryByID = function (id) {
            var apiRoute = baseUrl + 'GetSubCategoryByID';
            $scope.subCategory.CategoryID = "";
            CRUDService.getById(apiRoute, id)
            .then(function (response) {
                //debugger;
                $scope.subCategory = response.data.subCategory;
                $scope.subCategory.CategoryID = response.data.subCategory.CategoryID;
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

            $scope.GetSubCategories($scope.currentPage);
        };

        $scope.dropDownChanged = function () {
            $scope.currentPage = 1;
            $scope.pageSize = $scope.selectedPageSize;
            $scope.subCategories = [];
            $scope.selectedIds = [];
            $scope.checkedAll = false;

            $scope.GetSubCategories($scope.currentPage);
        };

        /*---Toggle All chkbox select/deselect---*/
        $scope.toggleCheckAll = function () {
            if ($scope.checkedAll) {
                angular.forEach($scope.subCategories, function (data) {
                    data.checked = true;
                    $scope.checkedAll = true;
                    $scope.cbChecked(data);
                });
            }
            else {
                angular.forEach($scope.subCategories, function (data) {
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
            angular.forEach($scope.subCategories, function (data) {
                if (!data.checked) {
                    $scope.checkedAll = false;
                }
            });
        }


        //Delete Multiple Data
        $scope.deleteSelectedData = function (Ids) {
            var apiRoute = baseUrl + 'DeleteSelectedSubCategories/';
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
                        $scope.subCategory = {};
                        $scope.subCategory.CategoryID = "";
                        $scope.subCategories = [];
                        $scope.selectedIds = [];
                        $scope.checkedAll = false;
                        $scope.GetSubCategories($scope.currentPage);
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