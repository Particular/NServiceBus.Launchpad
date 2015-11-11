(function (ng) {

    var service = function ($http, $q) {

        var busConfigurationBuilder = function () {

            var builder = {
                configString: ''

            };

            builder.addLine = function(line) {
                builder.configString = builder.configString + line + ";\r\n";
            };

            builder.addLine('var busConfiguration = new BusConfiguration()');

            var buildValue = function(key, value) {
                builder.configString =
                    builder.configString.replace('{{' + key + '}}', value);
                return builder;
            };

            builder.setEndpointName = function (value) {
                builder.addLine('busConfiguration.EndpointName("{{EndpointName}}")');
                return buildValue('EndpointName', value);
            };

            builder.setPersistence = function (value) {
                builder.addLine('busConfiguration.UsePersistence<{{Persistence}}>()');
                return buildValue('Persistence', value + 'Persistence');
            };

            builder.setSerializer = function (value) {
                builder.addLine('busConfiguration.UseSerialization<{{Serializer}}>()');
                return buildValue('Serializer', value + 'Serializer');
            };

            builder.setTransport = function (value) {
                builder.addLine('busConfiguration.UseTransport<{{Transport}}>()');
                return buildValue('Transport', value + 'Transport');
            };

            builder.build = function () {
                builder.configString = builder.configString + "#if DEBUG\r\n";
                builder.addLine('busConfiguration.EnableInstallers()');
                builder.configString = builder.configString + "#endif\r\n";

                return builder.configString;
            };

            return builder;
        };

        var getBootstrapBuilder = function () {

            var deferred = $q.defer();

            $http.get('/data.json')
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
            
            var busBuilder =
                busConfigurationBuilder()
                    .setEndpointName(model.EndpointName)
                    .setTransport(model.Transport.name)
                    .setSerializer(model.Serializer.name);

            if (model.Persistence.name !== 'None') {
                busBuilder.setPersistence(model.Persistence.name);
            }

            var nugetInstalls = [
                'NServiceBus'
            ];

            nugetInstalls.addDependencies = function(dependencies) {

                if (!dependencies) {
                    return;
                }

                for (var i = 0; i < dependencies.length; i++) {
                    if (nugetInstalls.indexOf(dependencies[i]) === -1) {
                        nugetInstalls.push(dependencies[i]);
                    }
                }
            };

            nugetInstalls.build = function () {

                var result = '';

                for (var i = 0; i < nugetInstalls.length; i++) {
                    result = result + 'Install-Package ' + nugetInstalls[i] + '\r\n';
                }

                return result;
            };

            nugetInstalls.addDependencies(model.Persistence.packages);
            nugetInstalls.addDependencies(model.Serializer.packages);
            nugetInstalls.addDependencies(model.Transport.packages);

            deferred.resolve({
                Configuration: busBuilder.build(),
                NuGetDependencies: nugetInstalls.build()
            });

            return deferred.promise;
        };

        this.triggerBootstrapping = triggerBootstrapping;
        this.getBootstrapBuilder = getBootstrapBuilder;
    };

    ng.module('nsbBootstrap').service('bootstrapService', ['$http', '$q', service]);

})(angular);