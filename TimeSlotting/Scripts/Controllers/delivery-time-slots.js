ts.controller('deliveryTimeslotsController', ['NgTableParams', 'ngTableEventsChannel', '$timeout', '$compile', '$scope', 'common', '$http', function (NgTableParams, ngTableEventsChannel, $timeout, $compile, $scope, common, $http) {
    $scope.common = common;

    $scope.day = moment().format('DD-MMM-YYYY');

    getUserRole();
    getStatusTypes();

    $scope.getTimeSlot = function (id) {
        $scope.modalError = "";
        $scope.form.$setPristine();
        $scope.form.$setUntouched();

        $http.get(window.location.origin + $scope.common.url + '/api/deliverytimeslots/gettimeslot', {
            params: { id: id }
        }).then(function (response) {            
            $scope.timeslot = response.data;
            $scope.getVehicles($scope.timeslot.DriverId);

            $scope.common.getUserName($scope.timeslot.CreatedBy, function (response) {
                $scope.createdBy = response;
            });

            $scope.common.getUserName($scope.timeslot.ModifiedBy, function (response) {
                $scope.modifiedBy = response;
            });

            $scope.modalHeader = "Edit Delivery";

            if ($scope.role == "Administrator" || $scope.role == "CustomerAdmin" || $scope.role == "CustomerUser" || ($scope.role == "Driver" && $scope.uid == $scope.timeslot.DriverId && $scope.timeslot.StatusTypeId == 2)) {
                $('#modalTimeSlot').modal('show');
            }
        });
    };

    $scope.addTimeSlot = function (tid) {
        $scope.modalError = "";
        $scope.form.$setPristine();
        $scope.form.$setUntouched();

        $scope.timeslot = {
            TimeSlotId: tid,
            CustomerId: $scope.cid,
            SiteId: $scope.site.Id,
            DriverId: '',
            VehicleId: '',
            StatusTypeId: 2
        };

        if ($scope.role === 'Driver') {
            $scope.timeslot.DriverId = $scope.uid;
        }

        $scope.createdBy = '';
        $scope.modifiedBy = '';

        $scope.modalHeader = "Add Delivery";

        $('#modalTimeSlot').modal('show');
    };

    $scope.saveTimeSlot = function () {
        $scope.modalError = "";
        if ($scope.form.$valid) {
            $scope.timeslot.TimeSlot = null;
            $scope.timeslot.Customer = null;
            $scope.timeslot.Site = null;
            $scope.timeslot.WebUser = null;
            $scope.timeslot.Vehicle = null;
            $scope.timeslot.StatusType = null;
            $scope.timeslot.DeliveryDate = $scope.day;

            $http.put(window.location.origin + $scope.common.url + '/api/deliverytimeslots/puttimeslot', {
                timeslot: $scope.timeslot
            }).then(function (response) {
                if (response.data === "OK") {
                    getData($scope.site.Id);

                    $("#modalTimeSlot").modal('hide');
                }
                else {
                    $scope.modalError = "Problem saving delivery - " + response.data;
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

    $scope.deleteTimeSlot = function () {
        $http.delete(window.location.origin + $scope.common.url + '/api/deliverytimeslots/deletetimeslot', {
            params: { id: $scope.deleteId }
        }).then(function (response) {
            if (response.data === "OK") {
                PopOverClose('.timeslot-delete');
                getData($scope.site.Id);
            }
            else {
                $scope.modalError = "Problem deleting delivery - " + response.data;
            }
        });
    };

    $scope.getTimeSlotData = function (id) {
        getData(id);
    };

    function getData(id) {
        if (id !== null && id !== undefined && id !== '') {
            $http.get(window.location.origin + $scope.common.url + '/api/deliverytimeslots/gettimeslotdata', {
                params: {
                    sid: id, day: moment($scope.day, 'DD-MMM-YYYY').format('YYYY-MM-DD')
                }
            }).then(function (response) {
                $scope.cid = response.data.cid;

                var data = $scope.common.rebuildJSON(JSON.stringify(response.data.data));
                $scope.tableParams = new NgTableParams({}, { dataset: data });

                $timeout(function () {
                    AddPopovers();
                });
            });
        }       
    }

    function getUserRole() {
        $http.get(window.location.origin + $scope.common.url + '/api/webusers/getuserrole').then(function (response) {
            var data = $scope.common.rebuildJSON(JSON.stringify(response.data));
            $scope.role = data.role;
            $scope.uid = data.uid;

            if ($scope.role != "Administrator") {
                $scope.cid = data.cid;
                $scope.getSites(data.cid);

                if ($scope.role != "Driver") {
                    $scope.getDrivers(data.cid);
                }
                else
                {
                    $scope.getVehicles(data.uid);
                }
            }
            else
            {
                getCustomers();
            }
        });
    }

    function getStatusTypes() {
        $http.get(window.location.origin + $scope.common.url + '/api/statustypes/getstatustypelist').then(function (response) {
            var data = $scope.common.rebuildJSON(JSON.stringify(response.data));
            $scope.statusTypes = data;
        });
    }

    function getCustomers() {
        $http.get(window.location.origin + $scope.common.url + '/api/customers/getcustomers').then(function (response) {
            var data = $scope.common.rebuildJSON(JSON.stringify(response.data));
            $scope.customers = data;
        });
    }

    $scope.getSites = function (id) {
        if (id !== null && id !== undefined && id !== '') {
            $http.get(window.location.origin + $scope.common.url + '/api/sites/getsites?id=' + id).then(function (response) {
                var data = $scope.common.rebuildJSON(JSON.stringify(response.data));
                $scope.sites = data;
            });
        }
    };

    $scope.getDrivers = function (id) {
        if (id !== null && id !== undefined && id !== '') {
            $http.get(window.location.origin + $scope.common.url + '/api/webusers/getdrivers?cid=' + id).then(function (response) {
                var data = $scope.common.rebuildJSON(JSON.stringify(response.data));
                $scope.drivers = data;
            });
        }
    };

    $scope.getVehicles = function (id) {
        if (id !== null && id !== undefined && id !== '') {
            $http.get(window.location.origin + $scope.common.url + '/api/vehicles/getdrivervehicles?uid=' + id).then(function (response) {
                var data = $scope.common.rebuildJSON(JSON.stringify(response.data));
                $scope.vehicles = data;
            });
        }
    };

    function AddPopovers() {
        if ($('#popover-delete').length > 0) {
            $('.timeslot-delete').popover({
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
