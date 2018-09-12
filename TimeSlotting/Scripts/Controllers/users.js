ts.controller('usersController', ['NgTableParams', 'ngTableEventsChannel', '$timeout', '$compile', '$scope', 'common', '$http', function (NgTableParams, ngTableEventsChannel, $timeout, $compile, $scope, common, $http) {
    //ngTableEventsChannel.onDatasetChanged(AddPopovers, $scope);

    $scope.common = common;

    getData();
    getCustomers();

    $scope.getUser = function (id) {
        $scope.modalError = "";
        $scope.form.$setPristine();
        $scope.form.$setUntouched();

        $http.get(window.location.origin + $scope.common.url + '/api/webusers/getuser', {
            params: { id: id }
        }).then(function (response) {
            $scope.user = response.data.user;
            $scope.email = response.data.email;
            $scope.password = '';
            $scope.role = response.data.role;
            $scope.vehicleSelect = response.data.vehicles;

            if ($scope.role === 'Driver') {
                $scope.getFleets($scope.user.CustomerId);
                $scope.getVehicles($scope.user.FleetId);
            }

            if ($scope.role === 'SiteUser') {
                $scope.getSites($scope.user.CustomerId);
            }

            $scope.common.getUserName($scope.user.CreatedBy, function (response) {
                $scope.createdBy = response;
            });

            $scope.common.getUserName($scope.user.ModifiedBy, function (response) {
                $scope.modifiedBy = response;
            });
        });

        $scope.new = 'false';
        $scope.modalHeader = "Edit User";
    };

    $scope.addUser = function () {
        $scope.modalError = "";
        $scope.form.$setPristine();
        $scope.form.$setUntouched();

        $scope.user = {
            FirstName: '',
            LastName: '',
            CustomerId: $scope.cid,
            SiteId: '',
            FleetId: '',
            IsEnabled: true
        };

        $scope.email = '';
        $scope.password = '';
        $scope.role = '';
        $scope.vehicleSelect = '';
        $scope.createdBy = '';
        $scope.modifiedBy = '';

        $scope.new = 'true';
        $scope.modalHeader = "Add User";
    };

    $scope.saveUser = function () {
        $scope.modalError = "";
        if ($scope.form.$valid) {
            $scope.user.Customer = null;
            $scope.user.Site = null;
            $scope.user.Fleet = null;

            if ($scope.role !== "CustomerUser" && $scope.role !== "CustomerAdmin" && $scope.role !== "SiteUser" && $scope.role !== "Driver") {
                $scope.user.CustomerId = null;
            }

            if ($scope.role !== "SiteUser") {
                $scope.user.SiteId = null;
            }

            if ($scope.role !== "Driver") {
                $scope.user.FleetId = null;                
            }

            $http.put(window.location.origin + $scope.common.url + '/api/webusers/putuser', {
                user: $scope.user,
                email: $scope.email,
                password: $scope.password,
                role: $scope.role,
                vehicles: $scope.vehicleSelect
            }).then(function (response) {
                if (response.data === "OK") {
                    getData();

                    $("#modalUser").modal('hide');
                }
                else {
                    $scope.modalError = "Problem saving user - " + response.data;
                }
            });
        }
        else
        {
            $scope.invalid = true;
        }
    };

    $scope.prepareDelete = function (id) {
        $scope.modalError = "";

        $scope.deleteId = id;
    };

    $scope.deleteUser = function () {
        $http.delete(window.location.origin + $scope.common.url + '/api/webusers/deleteuser', {
            params: { id: $scope.deleteId }
        }).then(function (response) {
            if (response.data === "OK") {
                PopOverClose('.list-delete');
                getData();
            }
            else {
                $scope.modalError = "Problem deleting user - " + response.data;
            }
        });
    };

    function getData() {
        $http.get(window.location.origin + $scope.common.url + '/api/webusers/getusers').then(function (response) {
            $scope.admin = response.data.admin;
            $scope.cid = response.data.cid;

            var data = $scope.common.rebuildJSON(JSON.stringify(response.data.data));
            $scope.tableParams = new NgTableParams({}, { dataset: data, filterOptions: { filterLayout: "horizontal" } });

            $scope.nameFilterDef = {
                FirstName: {
                    id: "text",
                    placeholder: ""
                },
                LastName: {
                    id: "text",
                    placeholder: ""
                }
            };

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

    $scope.getSites = function (id) {
        if ($scope.admin !== true) {
            id = $scope.cid;
        }
        if (id !== null && id !== undefined && id !== '') {
            $http.get(window.location.origin + $scope.common.url + '/api/sites/getsites?id=' + id).then(function (response) {
                var data = $scope.common.rebuildJSON(JSON.stringify(response.data));
                $scope.sites = data;
            });
        }
    };

    $scope.getFleets = function (id) {
        if ($scope.admin !== true) {
            id = $scope.cid;
        }
        if (id !== null && id !== undefined && id !== '') {
            $http.get(window.location.origin + $scope.common.url + '/api/fleets/getfleets?id=' + id).then(function (response) {
                var data = $scope.common.rebuildJSON(JSON.stringify(response.data));
                $scope.fleets = data;
            });
        }
    };

    $scope.getVehicles = function (id) {
        if (id !== null && id !== undefined && id !== '') {
            $http.get(window.location.origin + $scope.common.url + '/api/vehicles/getvehicles?id=' + id).then(function (response) {
                var data = $scope.common.rebuildJSON(JSON.stringify(response.data));
                $scope.vehicles = data;
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
