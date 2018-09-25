ts.controller('suppliersController', ['NgTableParams', 'ngTableEventsChannel', '$timeout', '$compile', '$scope', 'common', '$http', function (NgTableParams, ngTableEventsChannel, $timeout, $compile, $scope, common, $http) {
    $scope.common = common;

    getData();

    $scope.getSupplier = function (id) {
        $scope.modalError = "";
        $scope.form.$setPristine();
        $scope.form.$setUntouched();

        $http.get(window.location.origin + $scope.common.url + '/api/suppliers/getsupplier', {
            params: { id: id }
        }).then(function (response) {
            $scope.supplier = response.data;

            $scope.common.getUserName($scope.supplier.CreatedBy, function (response) {
                $scope.createdBy = response;
            });

            $scope.common.getUserName($scope.supplier.ModifiedBy, function (response) {
                $scope.modifiedBy = response;
            });
        });

        $scope.modalHeader = "Edit Supplier";
    };

    $scope.addSupplier = function () {
        $scope.modalError = "";
        $scope.form.$setPristine();
        $scope.form.$setUntouched();

        $scope.supplier = {
            Name: '',
            IsEnabled: true
        };

        $scope.createdBy = '';
        $scope.modifiedBy = '';

        $scope.modalHeader = "Add Supplier";
    };

    $scope.saveSupplier = function () {
        $scope.modalError = "";
        if ($scope.form.$valid) {
            $http.put(window.location.origin + $scope.common.url + '/api/suppliers/putsupplier', {
                supplier: $scope.supplier
            }).then(function (response) {
                if (response.data === "OK") {
                    getData();

                    $("#modalSupplier").modal('hide');
                }
                else {
                    $scope.modalError = "Problem saving supplier - " + response.data;
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

    $scope.deleteSupplier = function () {
        $http.delete(window.location.origin + $scope.common.url + '/api/suppliers/deletesupplier', {
            params: { id: $scope.deleteId }
        }).then(function (response) {
            if (response.data === "OK") {
                PopOverClose('.list-delete');
                getData();
            }
            else {
                $scope.modalError = "Problem deleting supplier - " + response.data;
            }
        });
    };

    function getData() {
        $http.get(window.location.origin + $scope.common.url + '/api/suppliers/getsuppliers').then(function (response) {
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
