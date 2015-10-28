(function (ng) {

    var service = function ($http, $q) {

        var getBootstrapBuilder = function () {

            var deferred = $q.defer();

            $http.get('/Home/BootstrapBuilder')
                .then(function success(response) {
                    deferred.resolve(response.data);
                },
                function failure(response) {
                    deferred.reject(response);
                });

            return deferred.promise;
        };

        var triggerBootstrapping = function (model) {

            var deferred = $q.defer();

            $http.post('/Home/Bootstrap', { model: model })
                .then(function success(response) {
                    deferred.resolve(response.data);
                },
                function failure(response) {
                    deferred.reject(response);
                });

            return deferred.promise;
        };

        this.triggerBootstrapping = triggerBootstrapping;
        this.getBootstrapBuilder = getBootstrapBuilder;
    };

    ng.module('nsbBootstrap').service('bootstrapService', ['$http', '$q', service]);

})(angular);