ts.controller('contractsController', ['NgTableParams', 'ngTableEventsChannel', '$timeout', '$compile', '$scope', 'common', '$http', function (NgTableParams, ngTableEventsChannel, $timeout, $compile, $scope, common, $http) {
    $scope.common = common;

    getData();

    $scope.getContract = function (id) {
        $scope.modalError = "";
        $scope.form.$setPristine();
        $scope.form.$setUntouched();

        $http.get(window.location.origin + $scope.common.url + '/api/contracts/getcontract', {
            params: { id: id }
        }).then(function (response) {
            $scope.contract = response.data;

            $scope.common.getUserName($scope.contract.CreatedBy, function (response) {
                $scope.createdBy = response;
            });

            $scope.common.getUserName($scope.contract.ModifiedBy, function (response) {
                $scope.modifiedBy = response;
            });
        });

        $scope.modalHeader = "Edit Contract";
    };

    $scope.addContract = function () {
        $scope.modalError = "";
        $scope.form.$setPristine();
        $scope.form.$setUntouched();

        $scope.contract = {
            Name: '',
            IsEnabled: true
        };

        $scope.createdBy = '';
        $scope.modifiedBy = '';

        $scope.modalHeader = "Add Contract";
    };

    $scope.saveContract = function () {
        $scope.modalError = "";
        if ($scope.form.$valid) {
            $http.put(window.location.origin + $scope.common.url + '/api/contracts/putcontract', {
                contract: $scope.contract
            }).then(function (response) {
                if (response.data === "OK") {
                    getData();

                    $("#modalContract").modal('hide');
                }
                else {
                    $scope.modalError = "Problem saving contract - " + response.data;
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

    $scope.deleteContract = function () {
        $http.delete(window.location.origin + $scope.common.url + '/api/contracts/deletecontract', {
            params: { id: $scope.deleteId }
        }).then(function (response) {
            if (response.data === "OK") {
                PopOverClose('.list-delete');
                getData();
            }
            else {
                $scope.modalError = "Problem deleting contract - " + response.data;
            }
        });
    };

    function getData() {
        $http.get(window.location.origin + $scope.common.url + '/api/contracts/getcontracts').then(function (response) {
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
