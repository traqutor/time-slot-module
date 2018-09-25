ts.controller('vendorsController', ['NgTableParams', 'ngTableEventsChannel', '$timeout', '$compile', '$scope', 'common', '$http', function (NgTableParams, ngTableEventsChannel, $timeout, $compile, $scope, common, $http) {
    $scope.common = common;

    getData();

    $scope.getVendor = function (id) {
        $scope.modalError = "";
        $scope.form.$setPristine();
        $scope.form.$setUntouched();

        $http.get(window.location.origin + $scope.common.url + '/api/vendors/getvendor', {
            params: { id: id }
        }).then(function (response) {
            $scope.vendor = response.data;

            $scope.common.getUserName($scope.vendor.CreatedBy, function (response) {
                $scope.createdBy = response;
            });

            $scope.common.getUserName($scope.vendor.ModifiedBy, function (response) {
                $scope.modifiedBy = response;
            });
        });

        $scope.modalHeader = "Edit Vendor";
    };

    $scope.addVendor = function () {
        $scope.modalError = "";
        $scope.form.$setPristine();
        $scope.form.$setUntouched();

        $scope.vendor = {
            Name: '',
            IsEnabled: true
        };

        $scope.createdBy = '';
        $scope.modifiedBy = '';

        $scope.modalHeader = "Add Vendor";
    };

    $scope.saveVendor = function () {
        $scope.modalError = "";
        if ($scope.form.$valid) {
            $http.put(window.location.origin + $scope.common.url + '/api/vendors/putvendor', {
                vendor: $scope.vendor
            }).then(function (response) {
                if (response.data === "OK") {
                    getData();

                    $("#modalVendor").modal('hide');
                }
                else {
                    $scope.modalError = "Problem saving vendor - " + response.data;
                }
            });
        }
        else {
            $scope.invalid = true;
        }
    };

    $scope.prepareDelete = function (id) {
        $scope.modalError = "";

        $scope.deleteId = id;
    };

    $scope.deleteVendor = function () {
        $http.delete(window.location.origin + $scope.common.url + '/api/vendors/deletevendor', {
            params: { id: $scope.deleteId }
        }).then(function (response) {
            if (response.data === "OK") {
                PopOverClose('.list-delete');
                getData();
            }
            else {
                $scope.modalError = "Problem deleting vendor - " + response.data;
            }
        });
    };

    function getData() {
        $http.get(window.location.origin + $scope.common.url + '/api/vendors/getvendors').then(function (response) {
            var data = $scope.common.rebuildJSON(JSON.stringify(response.data));
            $scope.tableParams = new NgTableParams({}, { dataset: data });

            $timeout(function () {
                AddPopovers();
            });
        });
    }

    function AddPopovers() {
        if ($('#popover-delete').length > 0) {
            $('.list-delete').popover({
                trigger: 'click',
                placement: "bottom",
                html: true,
                template: $('#popover-delete').html().replace(/>\s+</g, '><'),
                content: function () {
                    var html = $compile($('#popover-delete-body').html().replace(/\n/g, '').replace(/>\s+</g, '><').trim())($scope);
                    return html;
                }
            });
        }
    }
}]);
