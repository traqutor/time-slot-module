ts.controller('commoditiesController', ['NgTableParams', 'ngTableEventsChannel', '$timeout', '$compile', '$scope', 'common', '$http', function (NgTableParams, ngTableEventsChannel, $timeout, $compile, $scope, common, $http) {
    $scope.common = common;

    getData();

    $scope.getCommodity = function (id) {
        $scope.modalError = "";
        $scope.form.$setPristine();
        $scope.form.$setUntouched();

        $http.get(window.location.origin + $scope.common.url + '/api/commodities/getcommodity', {
            params: { id: id }
        }).then(function (response) {
            $scope.commodity = response.data;

            $scope.common.getUserName($scope.commodity.CreatedBy, function (response) {
                $scope.createdBy = response;
            });

            $scope.common.getUserName($scope.commodity.ModifiedBy, function (response) {
                $scope.modifiedBy = response;
            });
        });

        $scope.modalHeader = "Edit Commodity";
    };

    $scope.addCommodity = function () {
        $scope.modalError = "";
        $scope.form.$setPristine();
        $scope.form.$setUntouched();

        $scope.commodity = {
            Name: '',
            IsEnabled: true
        };

        $scope.createdBy = '';
        $scope.modifiedBy = '';

        $scope.modalHeader = "Add Commodity";
    };

    $scope.saveCommodity = function () {
        $scope.modalError = "";
        if ($scope.form.$valid) {
            $http.put(window.location.origin + $scope.common.url + '/api/commodities/putcommodity', {
                commodity: $scope.commodity
            }).then(function (response) {
                if (response.data === "OK") {
                    getData();

                    $("#modalCommodity").modal('hide');
                }
                else {
                    $scope.modalError = "Problem saving commodity - " + response.data;
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

    $scope.deleteCommodity = function () {
        $http.delete(window.location.origin + $scope.common.url + '/api/commodities/deletecommodity', {
            params: { id: $scope.deleteId }
        }).then(function (response) {
            if (response.data === "OK") {
                PopOverClose('.list-delete');
                getData();
            }
            else {
                $scope.modalError = "Problem deleting commodity - " + response.data;
            }
        });
    };

    function getData() {
        $http.get(window.location.origin + $scope.common.url + '/api/commodities/getcommodities').then(function (response) {
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
