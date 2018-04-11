app.controller('ProductCntrl', ['$scope', '$window', 'CRUDwithFileService', 'basePathService',
    function ($scope, $window, CRUDwithFileService, basePathService) {

        var baseUrl = basePathService.domainUrl + 'Product/';
        $scope.btnText = 'Submit';
        $scope.productID = 0;
        $scope.TxtMsg = '';
        $scope.errors = [];
        $scope.searchTerm = '';
        $scope.checkedAll = false;
        $scope.selectedIds = [];

        $scope.pageSizes = [5, 10, 25, 50, 100];
        $scope.selectedPageSize = $scope.pageSizes[0];

        $scope.currentPage = 1;
        $scope.pageSize = $scope.selectedPageSize;


        $scope.products = [];

        $scope.categories = null;
        $scope.subCategories = null;
        $scope.selectedCategory = "";
        $scope.selectedSubCategory = "";

        $scope.getAllCategories = function () {
            $scope.errorMsg = false;
            var apiRoute = basePathService.domainUrl + 'Category/GetAllCategories';
            CRUDwithFileService.getAll(apiRoute)
            .then(function (response) {
                $scope.categories = response.data.categories;
            }, function (error) {
                alert('Error: ' + JSON.stringify(error));
            });
        };

        $scope.getAllCategories();

        $scope.onCategoryChange = function () {
            $scope.errorMsg = false;
            var apiRoute = basePathService.domainUrl + 'SubCategory/GetAllSubCategoriesByCategory?catId=' + $scope.selectedCategory;
            CRUDwithFileService.getAll(apiRoute)
            .then(function (response) {
                //$scope.selectedSubCategory = "";
                $scope.subCategories = response.data.subCategories;
            }, function (error) {
                alert('Error: ' + JSON.stringify(error));
            });
        };
        
        $scope.Clear = function () {
            $scope.productID = 0;
            $scope.title = "";
            $scope.description = "";
            $scope.originalPrice = "";
            $scope.offerPrice = "";
            $scope.quantity = "";
            $scope.rating = "";
            $scope.discount = "";
            $scope.isOfferable = false;
            $scope.selectedCategory = "";
            $scope.selectedSubCategory = "";
            $scope.sequence = "";
            $('#idImage').val('');
            $scope.errorMsg = false;
            $scope.errorMsgCat = false;
            $scope.errorMsgSubCat = false;
            $scope.errorMsgImage = false;
            $scope.onCategoryChange();
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
            if ($scope.selectedCategory == "") {
                $scope.errorMsg = false;
                $scope.errorMsgCat = true;
                $scope.errorMsgSubCat = false;
                $scope.errorMsgImage = false;
                return false;
            }

            if ($scope.selectedSubCategory == "") {
                $scope.errorMsg = false;
                $scope.errorMsgCat = false;
                $scope.errorMsgSubCat = true;
                $scope.errorMsgImage = false;
                return false;
            }

            if ($scope.frmProd.$invalid) {
                $scope.errorMsg = true;
                $scope.errorMsgCat = false;
                $scope.errorMsgSubCat = false;
                $scope.errorMsgImage = false;
                return false;
            }

            var fileInput = document.getElementById('idImage');

            if (fileInput.files.length === 0 && $scope.btnText.toLowerCase() == 'Submit'.toLowerCase()) {
                //alert("Please Upload a File");
                $scope.errorMsg = false;
                $scope.errorMsgCat = false;
                $scope.errorMsgSubCat = false;
                $scope.errorMsgImage = true;
                return false;
            }


            $scope.discount = !$scope.discount ? 0 : $scope.discount;
            $scope.offerPrice = $scope.originalPrice - (($scope.originalPrice * $scope.discount) / 100);

            var file = fileInput.files[0];
            var payload = new FormData();
            payload.append("ID", $scope.productID);
            payload.append("Title", $scope.title);
            payload.append("Description", $scope.description);
            payload.append("IsOfferable", $scope.isOfferable);
            payload.append("OriginalPrice", $scope.originalPrice);
            payload.append("Discount", $scope.discount);
            payload.append("OfferPrice", $scope.offerPrice);
            payload.append("Quantity", $scope.quantity);
            payload.append("Rating", $scope.rating);
            payload.append("CategoryId", $scope.selectedCategory);
            payload.append("SubCategoryId", $scope.selectedSubCategory);
            payload.append("Sequence", $scope.sequence);
            payload.append("ImagePath", $('#idImage').val());
            payload.append("file", file);

            if ($scope.btnText.toLowerCase() == 'Submit'.toLowerCase()) {
                var apiRoute = baseUrl + 'SaveProduct';

                CRUDwithFileService.post(apiRoute, payload)
                .then(function (response) {
                    if (response.data.status == 1) {
                        $scope.TxtMsg = 'Data Save Successfully';
                        $scope.errors = [];
                        $scope.Clear();
                        $scope.currentPage = 1;
                        $scope.newPageNumber = 1;
                        $scope.pageSize = $scope.selectedPageSize;
                        $scope.products = [];
                        $scope.selectedIds = [];
                        $scope.checkedAll = false;
                        $scope.GetProducts($scope.newPageNumber);
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
                var apiRoute = baseUrl + 'UpdateProduct';
                CRUDwithFileService.post(apiRoute, payload)
                .then(function (response) {
                    if (response.data.status == 1) {
                        $scope.TxtMsg = 'Data Update Successfully';
                        $scope.errors = [];
                        $scope.Clear();
                        $scope.currentPage = 1;
                        $scope.newPageNumber = 1;
                        $scope.pageSize = $scope.selectedPageSize;
                        $scope.products = [];
                        $scope.selectedIds = [];
                        $scope.checkedAll = false;
                        $scope.GetProducts($scope.newPageNumber);
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
        $scope.DeleteProduct = function (id) {
            var apiRoute = baseUrl + 'DeleteProduct/' + id;
            var deleteItem = $window.confirm('Are you sure you want to delete?');

            if (deleteItem) {

                CRUDwithFileService.delete(apiRoute)
                .then(function (response) {
                    if (response.data.status == 1) {
                        $scope.TxtMsg = 'Data Delete Successfully';
                        $scope.Clear();
                        $scope.currentPage = 1;
                        $scope.pageSize = $scope.selectedPageSize;
                        $scope.products = [];
                        $scope.selectedIds = [];
                        $scope.checkedAll = false;
                        $scope.GetProducts($scope.currentPage);
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
        $scope.GetProducts = function (pageNo) {
            $scope.errorMsg = false;
            $scope.selectedIds = [];
            $scope.checkedAll = false;
            var apiRoute = baseUrl + 'GetProducts?term=' + $scope.searchTerm + '&page=' + pageNo + '&pageSize=' + $scope.pageSize;
            CRUDwithFileService.getAll(apiRoute)
            .then(function (response) {
                $scope.totalItems = response.data.totalCount;
                $scope.products = response.data.products;
            }, function (error) {
                alert('Error: ' + JSON.stringify(error));
            });
        };

        $scope.GetProducts($scope.currentPage);

        //Get a Data
        $scope.GetProductByID = function (id) {
            var apiRoute = baseUrl + 'GetProductByID';
            CRUDwithFileService.getById(apiRoute, id)
            .then(function (response) {
                $scope.productID = response.data.product.ID;
                $scope.title = response.data.product.Title;
                $scope.description = response.data.product.Description;
                $scope.originalPrice = response.data.product.OriginalPrice;
                $scope.discount = response.data.product.Discount;
                $scope.offerPrice = response.data.product.OfferPrice;
                $scope.isOfferable = response.data.product.IsOfferable;
                $scope.quantity = response.data.product.Quantity;
                $scope.rating = response.data.product.Rating;
                $scope.selectedCategory = response.data.product.CategoryId;
                $scope.onCategoryChange();
                $scope.selectedSubCategory = response.data.product.SubCategoryId;
                $scope.sequence = response.data.product.Sequence;
                $scope.btnText = 'Update';
                $scope.TxtMsg = '';
                $scope.selectedIds = [];
                $scope.checkedAll = false;
            }, function (error) {
                alert('Error: ' + JSON.stringify(error));
            });
        };

        $scope.SearchItem = function () {
            $scope.currentPage = 1;
            $scope.pageSize = $scope.selectedPageSize;
            $scope.products = [];
            $scope.selectedIds = [];
            $scope.checkedAll = false;
            $scope.GetProducts($scope.currentPage);
        };

        $scope.dropDownChanged = function () {
            $scope.currentPage = 1;
            $scope.pageSize = $scope.selectedPageSize;
            $scope.products = [];
            $scope.selectedIds = [];
            $scope.checkedAll = false;
            $scope.GetProducts($scope.currentPage);
        };

        /*---Toggle All chkbox select/deselect---*/
        $scope.toggleCheckAll = function () {
            if ($scope.checkedAll) {
                angular.forEach($scope.products, function (data) {
                    data.checked = true;
                    $scope.checkedAll = true;
                    $scope.cbChecked(data);
                });
            }
            else {
                angular.forEach($scope.products, function (data) {
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
            angular.forEach($scope.products, function (data) {
                if (!data.checked) {
                    $scope.checkedAll = false;
                }
            });
        }


        //Delete Multiple Data
        $scope.deleteSelectedData = function (Ids) {
            var apiRoute = baseUrl + 'DeleteSelectedProducts/';
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
                        $scope.products = [];
                        $scope.selectedIds = [];
                        $scope.checkedAll = false;
                        $scope.GetProducts($scope.currentPage);
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