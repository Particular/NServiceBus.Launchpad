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

            var buildValue = function (key, value) {
                builder.configString =
                    builder.configString.replace('{{' + key + '}}', value);
                return builder;
            }

            builder.setEndpointName = function (value) {
                builder.addLine('busConfiguration.EndpointName("{{EndpointName}}")');
                return buildValue('EndpointName', value);
            };

            builder.setPersistence = function (configItem) {
                builder.addLine('busConfiguration.UsePersistence<{{Persistence}}>()');
                return buildValue('Persistence', configItem.Name + 'Persistence');
            };

            builder.setSerializer = function (configItem) {
                builder.addLine('busConfiguration.UseSerialization<{{Serializer}}>()');
                return buildValue('Serializer', configItem.Name + 'Serializer');
            };

            builder.setTransport = function (configItem) {
                builder.addLine('busConfiguration.UseTransport<{{Transport}}>()');
                return buildValue('Transport', configItem.Name + 'Transport');
            };

            builder.build = function () {
                builder.configString = builder.configString + "#if DEBUG\r\n";
                builder.addLine('busConfiguration.EnableInstallers()');
                builder.configString = builder.configString + "#endif\r\n";

                return builder.configString;
            };

            return builder;
        };

        var nuGetDependencyDictionary = {
            Persistence: {
                NHibernate: ['NServiceBus.NHibernate'],
                RavenDB: ['NServiceBus.RavenDB'],
                AzureStorage: ['NServiceBus.Azure']
            },
            Serializer: {},
            Transport: {
                RabbitMQ: ['NServiceBus.RabbitMQ'],
                AzureServiceBus: ['NServiceBus.Azure.Transports.WindowsAzureServiceBus']
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

            var busBuilder =
                busConfigurationBuilder()
                    .setEndpointName(model.EndpointName)
                    .setTransport(model.Transport)
                    .setSerializer(model.Serializer);

            if (model.Persistence.Name !== 'None') {
                busBuilder.setPersistence(model.Persistence);
            }

            var nugetInstalls = [
                'NServiceBus',
                'NServiceBus.Host'
            ];

            nugetInstalls.addDependencies = function(dependencies) {

                if (!dependencies)
                    return;

                for (var i = 0; i < dependencies.length; i++) {
                    nugetInstalls.push(dependencies[i]);
                }
            };

            nugetInstalls.build = function () {

                var result = '';

                for (var i = 0; i < nugetInstalls.length; i++) {
                    result = result + 'Install-Package ' + nugetInstalls[i] + '\r\n';
                }

                return result;
            };

            nugetInstalls.addDependencies(nuGetDependencyDictionary.Persistence[model.Persistence.Name]);
            nugetInstalls.addDependencies(nuGetDependencyDictionary.Serializer[model.Serializer.Name]);
            nugetInstalls.addDependencies(nuGetDependencyDictionary.Transport[model.Transport.Name]);

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