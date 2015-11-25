(function (ng) {

    var service = function ($http, $q, $window) {

        var triggerBootstrapping = function (model) {

            var deferred = $q.defer();

            $http.post('/Home/Bootstrap', { configuration: model })
                .then(function success(guid) {

                    window.open('/Home/SolutionZip?guid=' + guid.data, '_blank');
                    //deferred.resolve(response.data);
                },
                function failure(response) {
                    deferred.reject(response);
                });

            return deferred.promise;
        };

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

        this.triggerBootstrapping = triggerBootstrapping;
        this.getBootstrapBuilder = getBootstrapBuilder;
    };

    ng.module('nsbBootstrap').service('bootstrapService', ['$http', '$q', service]);

})(angular);