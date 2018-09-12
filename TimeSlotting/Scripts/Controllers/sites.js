ts.controller('sitesController', ['NgTableParams', 'ngTableEventsChannel', '$timeout', '$compile', '$scope', 'common', '$http', function (NgTableParams, ngTableEventsChannel, $timeout, $compile, $scope, common, $http) {
    $scope.common = common;

    getData();
    getCustomers();

    $scope.getSite = function (id) {
        $scope.modalError = "";
        $scope.form.$setPristine();
        $scope.form.$setUntouched();

        $http.get(window.location.origin + $scope.common.url + '/api/sites/getsite', {
            params: { id: id }
        }).then(function (response) {

            $scope.site = response.data;

            $scope.common.getUserName($scope.site.CreatedBy, function (response) {
                $scope.createdBy = response;
            });

            $scope.common.getUserName($scope.site.ModifiedBy, function (response) {
                $scope.modifiedBy = response;
            });
        });

        $scope.modalHeader = "Edit Site";
    };

    $scope.addSite = function () {
        $scope.modalError = "";

        $scope.site = {
            Name: '',
            CustomerId: $scope.cid,
            IsEnabled: true
        };

        $scope.createdBy = '';
        $scope.modifiedBy = '';

        $scope.modalHeader = "Add Site";
    };

    $scope.saveSite = function () {
        $scope.modalError = "";
        if ($scope.form.$valid) {
            $scope.site.Customer = null;

            $http.put(window.location.origin + $scope.common.url + '/api/sites/putsite', {
                site: $scope.site
            }).then(function (response) {
                if (response.data === "OK") {
                    getData();

                    $("#modalSite").modal('hide');
                }
                else {
                    $scope.modalError = "Problem saving site - " + response.data;
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

    $scope.deleteSite = function () {
        $http.delete(window.location.origin + $scope.common.url + '/api/sites/deletesite', {
            params: { id: $scope.deleteId }
        }).then(function (response) {
            if (response.data === "OK") {
                PopOverClose('.list-delete');
                getData();
            }
            else {
                $scope.modalError = "Problem deleting site - " + response.data;
            }
        });
    };

    function getData() {
        $http.get(window.location.origin + $scope.common.url + '/api/sites/getsites').then(function (response) {
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
