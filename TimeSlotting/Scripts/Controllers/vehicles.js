ts.controller('vehiclesController', ['NgTableParams', 'ngTableEventsChannel', '$timeout', '$compile', '$scope', 'common', '$http', function (NgTableParams, ngTableEventsChannel, $timeout, $compile, $scope, common, $http) {
    $scope.common = common;

    getData();
    getCustomers();

    $scope.getVehicle = function (id) {
        $scope.modalError = "";
        $scope.form.$setPristine();
        $scope.form.$setUntouched();

        $http.get(window.location.origin + $scope.common.url + '/api/vehicles/getvehicle', {
            params: { id: id }
        }).then(function (response) {
            $scope.vehicle = response.data;

            $scope.getFleets($scope.vehicle.CustomerId);

            $scope.common.getUserName($scope.vehicle.CreatedBy, function (response) {
                $scope.createdBy = response;
            });

            $scope.common.getUserName($scope.vehicle.ModifiedBy, function (response) {
                $scope.modifiedBy = response;
            });
        });

        $scope.modalHeader = "Edit Vehicle";
    };

    $scope.addVehicle = function () {
        $scope.modalError = "";

        $scope.vehicle = {
            Rego: '',
            CustomerId: $scope.cid,
            FleetId: '',
            IsEnabled: true
        };

        $scope.createdBy = '';
        $scope.modifiedBy = '';

        $scope.modalHeader = "Add Vehicle";
    };

    $scope.saveVehicle = function () {
        $scope.modalError = "";
        if ($scope.form.$valid) {
            $scope.vehicle.Customer = null;
            $scope.vehicle.Fleet = null;

            $http.put(window.location.origin + $scope.common.url + '/api/vehicles/putvehicle', {
                vehicle: $scope.vehicle
            }).then(function (response) {
                if (response.data === "OK") {
                    getData();

                    $("#modalVehicle").modal('hide');
                }
                else {
                    $scope.modalError = "Problem saving vehicle - " + response.data;
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

    $scope.deleteVehicle = function () {
        $http.delete(window.location.origin + $scope.common.url + '/api/vehicles/deletevehicle', {
            params: { id: $scope.deleteId }
        }).then(function (response) {
            if (response.data === "OK") {
                PopOverClose('.list-delete');
                getData();
            }
            else {
                $scope.modalError = "Problem deleting vehicle - " + response.data;
            }
        });
    };

    function getData() {
        $http.get(window.location.origin + $scope.common.url + '/api/vehicles/getvehicles').then(function (response) {
            $scope.admin = response.data.admin;
            $scope.cid = response.data.cid;

            if (!$scope.admin) {
                $scope.getFleets($scope.cid);
            }

            var data = $scope.common.rebuildJSON(JSON.stringify(response.data.data));
            $scope.tableParams = new NgTableParams({}, { dataset: data });

            $timeout(function () {
                AddPopovers();
            });
        });
    }

    function getCustomers() {
        $http.get(window.location.origin + $scope.common.url + '/api/customers/getcustomers').then(function (response) {
            var data = $scope.common.rebuildJSON(JSON.stringify(response.data));
            $scope.customers = data;
        });
    }

    $scope.getFleets = function (id) {
        if (id !== null && id !== undefined && id !== '') {
            $http.get(window.location.origin + $scope.common.url + '/api/fleets/getfleets?id=' + id).then(function (response) {
                var data = $scope.common.rebuildJSON(JSON.stringify(response.data));
                $scope.fleets = data;
            });
        }
    };

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
