app.controller('HomeBannerCntrl', ['$scope', '$window', 'CRUDwithFileService', 'basePathService',
    function ($scope, $window, CRUDwithFileService, basePathService) {

        var baseUrl = basePathService.domainUrl + 'HomeBanner/';
        $scope.btnText = 'Submit';
        $scope.bannerID = 0;
        $scope.TxtMsg = '';
        $scope.errors = [];
        $scope.searchTerm = '';
        $scope.checkedAll = false;
        $scope.selectedIds = [];

        $scope.bannerTypes = ["Slider", "Carousel"];
        $scope.selectedBannerType = $scope.bannerTypes[0];

        $scope.pageSizes = [5, 10, 25, 50, 100];
        $scope.selectedPageSize = $scope.pageSizes[0];

        $scope.currentPage = 1;
        $scope.pageSize = $scope.selectedPageSize;


        $scope.banners = [];

        $scope.Clear = function () {
            $scope.bannerID = 0;
            $scope.title = "";
            $scope.description = "";
            $scope.url = "";
            $scope.sequence = "";
            $('#idImage').val('');
            $scope.selectedBannerType = $scope.bannerTypes[0];
        };

        $scope.Clear();

        //Only upload Images
        $scope.fnValidateExtention = function () {
            //Upload Image Extension Validation
            var ext = $('#idImage').val().split('.').pop().toLowerCase();
            if ($.inArray(ext, ['gif', 'png', 'jpg', 'jpeg']) == -1) {
                alert('Invalid extension!');
                $('#idImage').val('');
            }
            else {
                $scope.errorMsg = false;
            }
        };

        //Save Or Update a Data
        $scope.SaveUpdate = function () {

            if ($scope.frmHomeBanner.$invalid) {
                $scope.errorMsg = true;
                return false;
            }

            var fileInput = document.getElementById('idImage');

            if (fileInput.files.length === 0 && $scope.btnText.toLowerCase() == 'Submit'.toLowerCase()) {
                //alert("Please Upload a File");
                $scope.errorMsg = true;
                return false;
            }

            var file = fileInput.files[0];
            var payload = new FormData();
            payload.append("ID", $scope.bannerID);
            payload.append("Title", $scope.title);
            payload.append("Description", $scope.description);
            payload.append("Url", $scope.url);
            payload.append("Sequence", $scope.sequence);
            payload.append("BannerType", $scope.selectedBannerType);
            payload.append("ImagePath", $('#idImage').val());
            payload.append("file", file);

            if ($scope.btnText.toLowerCase() == 'Submit'.toLowerCase()) {
                var apiRoute = baseUrl + 'SaveBanner';
                
                CRUDwithFileService.post(apiRoute, payload)
                .then(function (response) {
                    if (response.data.status == 1) {
                        $scope.TxtMsg = 'Data Save Successfully';
                        $scope.errors = [];
                        $scope.Clear();
                        $scope.selectedIds = [];
                        $scope.checkedAll = false;
                        $scope.GetBanners($scope.currentPage);
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
                var apiRoute = baseUrl + 'UpdateBanner';
                CRUDwithFileService.post(apiRoute, payload)
                .then(function (response) {
                    if (response.data.status == 1) {
                        $scope.TxtMsg = 'Data Update Successfully';
                        $scope.errors = [];
                        $scope.Clear();
                        $scope.selectedIds = [];
                        $scope.checkedAll = false;
                        $scope.GetBanners($scope.currentPage);
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
        $scope.DeleteBanner = function (id) {
            var apiRoute = baseUrl + 'DeleteBanner/' + id;
            var deleteUser = $window.confirm('Are you sure you want to delete?');

            if (deleteUser) {

                CRUDwithFileService.delete(apiRoute)
                .then(function (response) {
                    if (response.data.status == 1) {
                        $scope.TxtMsg = 'Data Delete Successfully';
                        $scope.Clear();
                        $scope.selectedIds = [];
                        $scope.checkedAll = false;
                        $scope.GetBanners($scope.currentPage);
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


        //Get all Data
        $scope.GetBanners = function (pageNo) {
            $scope.errorMsg = false;
            $scope.selectedIds = [];
            $scope.checkedAll = false;
            var apiRoute = baseUrl + 'GetBanners?term=' + $scope.searchTerm + '&page=' + pageNo + '&pageSize=' + $scope.pageSize;
            CRUDwithFileService.getAll(apiRoute)
            .then(function (response) {
                $scope.totalItems = response.data.totalCount;
                $scope.banners = response.data.banners;
            }, function (error) {
                alert('Error: ' + JSON.stringify(error));
            });
        };

        $scope.GetBanners($scope.currentPage);

        //Get a Data
        $scope.GetBannerByID = function (id) {
            var apiRoute = baseUrl + 'GetBannerByID';
            CRUDwithFileService.getById(apiRoute, id)
            .then(function (response) {
                $scope.bannerID = response.data.banner.ID;
                $scope.title = response.data.banner.Title;
                $scope.sequence = response.data.banner.Sequence;
                $scope.url = response.data.banner.Url;
                $scope.description = response.data.banner.Description;
                $scope.selectedBannerType = response.data.banner.BannerType;
                $scope.btnText = 'Update';
                $scope.TxtMsg = '';
                $scope.selectedIds = [];
                $scope.checkedAll = false;
            }, function (error) {
                alert('Error: ' + JSON.stringify(error));
            });
        };


        $scope.SearchItem = function () {
            $scope.selectedIds = [];
            $scope.checkedAll = false;

            $scope.GetBanners($scope.currentPage);
        };

        $scope.dropDownChanged = function () {
            $scope.currentPage = 1;
            $scope.pageSize = $scope.selectedPageSize;
            $scope.banners = [];
            $scope.selectedIds = [];
            $scope.checkedAll = false;

            $scope.GetBanners($scope.currentPage);
        };

        /*---Toggle All chkbox select/deselect---*/
        $scope.toggleCheckAll = function () {
            if ($scope.checkedAll) {
                angular.forEach($scope.banners, function (data) {
                    data.checked = true;
                    $scope.checkedAll = true;
                    $scope.cbChecked(data);
                });
            }
            else {
                angular.forEach($scope.banners, function (data) {
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
            angular.forEach($scope.banners, function (data) {
                if (!data.checked) {
                    $scope.checkedAll = false;
                }
            });
        }


        //Delete Multiple Data
        $scope.deleteSelectedData = function (Ids) {
            var apiRoute = baseUrl + 'DeleteSelectedHomeBanners/';
            var deleteConfirm = $window.confirm('Are you sure you want to delete?');

            if (deleteConfirm) {
                if (Ids.length <= 0) {
                    $window.alert('Please Select Atleast One');
                    return;
                }

                CRUDwithFileService.deleteAll(apiRoute, Ids)
                .then(function (response) {
                    if (response.data.status == 1) {
                        $scope.TxtMsg = 'Data Delete Successfully';
                        $scope.Clear();
                        $scope.banners = [];
                        $scope.selectedIds = [];
                        $scope.checkedAll = false;
                        $scope.GetBanners($scope.currentPage);
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