ts.controller('timeslotsController', ['NgTableParams', 'ngTableEventsChannel', '$timeout', '$compile', '$scope', 'common', '$http', function (NgTableParams, ngTableEventsChannel, $timeout, $compile, $scope, common, $http) {
    $scope.common = common;

    getData();

    $scope.getTimeSlot = function (id) {
        $scope.modalError = "";
        $scope.form.$setPristine();
        $scope.form.$setUntouched();

        $http.get(window.location.origin + $scope.common.url + '/api/timeslots/gettimeslot', {
            params: { id: id }
        }).then(function (response) {            
            $scope.timeslot = response.data;

            $scope.common.getUserName($scope.timeslot.CreatedBy, function (response) {
                $scope.createdBy = response;
            });

            $scope.common.getUserName($scope.timeslot.ModifiedBy, function (response) {
                $scope.modifiedBy = response;
            });
        });

        $scope.modalHeader = "Edit Time Slot";
    };

    $scope.addTimeSlot = function () {
        $scope.modalError = "";
        $scope.form.$setPristine();
        $scope.form.$setUntouched();

        $scope.timeslot = {
            StartTime: '',
            EndTime: '',
            IsEnabled: true
        };

        $scope.createdBy = '';
        $scope.modifiedBy = '';

        $scope.modalHeader = "Add TimeSlot";
    };

    $scope.saveTimeSlot = function () {
        $scope.modalError = "";
        if ($scope.form.$valid) {
            $scope.timeslot.StartTime = ConvertTo24($scope.timeslot.StartTime);
            $scope.timeslot.EndTime = ConvertTo24($scope.timeslot.EndTime);

            $http.put(window.location.origin + $scope.common.url + '/api/timeslots/puttimeslot', {
                timeslot: $scope.timeslot
            }).then(function (response) {
                if (response.data === "OK") {
                    getData();

                    $("#modalTimeSlot").modal('hide');
                }
                else {
                    $scope.modalError = "Problem saving time slot - " + response.data;
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
        $http.delete(window.location.origin + $scope.common.url + '/api/timeslots/deletetimeslot', {
            params: { id: $scope.deleteId }
        }).then(function (response) {
            if (response.data === "OK") {
                PopOverClose('.list-delete');
                getData();
            }
            else {
                $scope.modalError = "Problem deleting time slot - " + response.data;
            }
        });
    };

    function getData() {
        $http.get(window.location.origin + $scope.common.url + '/api/timeslots/gettimeslots').then(function (response) {
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

    function ConvertTo24(time) {
        var d = new Date("1/1/2013 " + time);
        return d.getHours() + ':' + d.getMinutes();
    }
}]);
