ts.factory('common', ['$http', function ($http) {
    var comm = {};

    comm.url = '';
    //comm.url = '/clients/***/timeslotting';

    comm.getUserName = function (id, callback) {
        $http.get(window.location.origin + comm.url + '/api/webusers/getusername', {
            params: { id: id }
        }).then(function (response) {
            callback(response.data);
        });
    };

    comm.rebuildJSON = function (json) {
        var refMap = {};
        return JSON.parse(json, function (key, value) {
            if (key === '$id') {
                refMap[value] = this;
                return void (0);
            }

            if (value && value.$ref) { return refMap[value.$ref]; }

            return value;
        });
    };

    return comm;
}]);

ts.filter('formatTime', ['$filter', function ($filter) {
    return function (time) {
        var parts = time.split(':');
        var date = new Date(0, 0, 0, parts[0], parts[1], parts[2]);
        return $filter('date')(date, 'hh:mm a');
    };
}]);