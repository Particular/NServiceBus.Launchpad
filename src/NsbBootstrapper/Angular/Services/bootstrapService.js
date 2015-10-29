(function (ng) {

    var service = function ($http, $q) {

        var busConfigurationBuilder = function () {

            var builder = {
                configString: ''
            };

            builder.addLine = function(line) {
                builder.configString = builder.configString + line + ";{{newLine}}";
            };

            builder.addLine('var busConfiguration = new BusConfiguration()');
            builder.addLine('busConfiguration.EndpointName("{{EndpointName}}")');
            builder.addLine('busConfiguration.UseSerialization<{{Serializer}}>()');
            builder.addLine('busConfiguration.UsePersistence<{{Persistence}}>()');
            builder.addLine('busConfiguration.UseTransport<{{Transport}}>()');
            builder.addLine('busConfiguration.EnableInstallers()');

            var buildValue = function (key, value) {
                builder.configString =
                    builder.configString.replace('{{' + key + '}}', value);
                return builder;
            }

            builder.setEndpointName = function (value) {
                return buildValue('EndpointName', value);
            };

            builder.setPersistence = function (configItem) {
                return buildValue('Persistence', configItem.Name);
            };

            builder.setSerializer = function (configItem) {
                return buildValue('Serializer', configItem.Name);
            };

            builder.setTransport = function (configItem) {
                return buildValue('Transport', configItem.Name);
            };

            builder.build = function() {
                var lines = builder.configString.split('{{newLine}}');
                lines.splice(-1, 1);
                return lines;
            };

            return builder;
        };

        var nuGetDependencyDictionary = {
            Persistence: {
                NHibernate: ['NHibernate.Client'],
                RavenDb: ['RavenDb.Client'],
                AzureStorage: ['AzureStorage.Adapter']
            },
            Serializer: {},
            Transport: {
                RabbitMQ: ['RabbitMQ.Client'],
                Azure: ['Azure.Client']
            },
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

        var triggerBootstrapping = function (model) {

            var deferred = $q.defer();

            //$http.post('/Home/Bootstrap', { model: model })
            //    .then(function success(response) {
            //        deferred.resolve(response.data);
            //    },
            //    function failure(response) {
            //        deferred.reject(response);
            //    });

            var busConfig =
                busConfigurationBuilder()
                    .setEndpointName(model.EndpointName)
                    .setPersistence(model.Persistence)
                    .setTransport(model.Transport)
                    .setSerializer(model.Serializer)
                    .build();

            var nugetInstalls = [
                'NServiceBus.Host'
            ];

            nugetInstalls.addDependencies = function(dependencies) {

                if (!dependencies)
                    return;

                for (var i = 0; i < dependencies.length; i++) {
                    nugetInstalls.push(dependencies[i]);
                }
            };

            nugetInstalls.addDependencies(nuGetDependencyDictionary.Persistence[model.Persistence.Name]);
            nugetInstalls.addDependencies(nuGetDependencyDictionary.Serializer[model.Serializer.Name]);
            nugetInstalls.addDependencies(nuGetDependencyDictionary.Transport[model.Transport.Name]);

            deferred.resolve({
                Configuration: busConfig,
                NuGetDependencies: nugetInstalls
            });

            return deferred.promise;
        };

        this.triggerBootstrapping = triggerBootstrapping;
        this.getBootstrapBuilder = getBootstrapBuilder;
    };

    ng.module('nsbBootstrap').service('bootstrapService', ['$http', '$q', service]);

})(angular);