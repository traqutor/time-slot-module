ts.controller('fleetsController', ['NgTableParams', 'ngTableEventsChannel', '$timeout', '$compile', '$scope', 'common', '$http', function (NgTableParams, ngTableEventsChannel, $timeout, $compile, $scope, common, $http) {
    $scope.common = common;

    getData();
    getCustomers();

    $scope.getFleet = function (id) {
        $scope.modalError = "";
        $scope.form.$setPristine();
        $scope.form.$setUntouched();

        $http.get(window.location.origin + $scope.common.url + '/api/fleets/getfleet', {
            params: { id: id }
        }).then(function (response) {

            $scope.fleet = response.data;

            $scope.common.getUserName($scope.fleet.CreatedBy, function (response) {
                $scope.createdBy = response;
            });

            $scope.common.getUserName($scope.fleet.ModifiedBy, function (response) {
                $scope.modifiedBy = response;
            });
        });

        $scope.modalHeader = "Edit Fleet";
    };

    $scope.addFleet = function () {
        $scope.modalError = "";
        $scope.form.$setPristine();
        $scope.form.$setUntouched();

        $scope.fleet = {
            Name: '',
            CustomerId: $scope.cid,
            IsEnabled: true
        };

        $scope.createdBy = '';
        $scope.modifiedBy = '';

        $scope.modalHeader = "Add Fleet";
    };

    $scope.saveFleet = function () {
        $scope.modalError = "";
        if ($scope.form.$valid) {
            $scope.fleet.Customer = null;

            $http.put(window.location.origin + $scope.common.url + '/api/fleets/putfleet', {
                fleet: $scope.fleet
            }).then(function (response) {
                if (response.data === "OK") {
                    getData();

                    $("#modalFleet").modal('hide');
                }
                else {
                    $scope.modalError = "Problem saving fleet - " + response.data;
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

    $scope.deleteFleet = function () {
        $http.delete(window.location.origin + $scope.common.url + '/api/fleets/deletefleet', {
            params: { id: $scope.deleteId }
        }).then(function (response) {
            if (response.data === "OK") {
                PopOverClose('.list-delete');
                getData();
            }
            else {
                $scope.modalError = "Problem deleting fleet - " + response.data;
            }
        });
    };

    function getData() {
        $http.get(window.location.origin + $scope.common.url + '/api/fleets/getfleets').then(function (response) {
            $scope.admin = response.data.admin;
            $scope.cid = response.data.cid;

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
