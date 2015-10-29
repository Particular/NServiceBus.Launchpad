(function (ng) {

    var controller = function ($scope, bootstrapService) {

        var initialize = function () {

            bootstrapService.getBootstrapBuilder()
                .then(function (builderModel) {
                    $scope.builderModel = builderModel;
                });

            $scope.endpointName = 'Sample.Bootstrapper.Endpoint';
        };

        $scope.setVersionDefaults = function () {

            if (!$scope.selectedVersion)
                return;

            $scope.selectedSerializer =
                $scope.selectedVersion.AvailableSerializers[0];
            $scope.selectedPersistence =
                $scope.selectedVersion.AvailablePersistence[0];
            $scope.selectedTransport =
                $scope.selectedVersion.AvailableTransports[0];
        };

        $scope.bootstrap = function () {

            var model = {
                Version: $scope.selectedVersion.NServiceBusVersion,
                EndpointName: $scope.endpointName,
                Serializer: $scope.selectedSerializer,
                Persistence: $scope.selectedPersistence,
                Transport: $scope.selectedTransport
            };

            bootstrapService.triggerBootstrapping(model)
                .then(function (bootstrapResult) {
                    $scope.bootstrapResult = bootstrapResult;
                });

        };

        initialize();
    };

    ng.module('nsbBootstrap').controller('bootstrapWizard', ['$scope', 'bootstrapService', controller]);

})(angular);