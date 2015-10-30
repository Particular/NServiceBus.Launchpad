(function (ng) {

    var controller = function ($scope, bootstrapService, educationService) {

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

        $scope.$watch('selectedPersistence', function () {

            $scope.persistenceDocumentation = null;

            if (!$scope.selectedPersistence || $scope.selectedPersistence.Name === 'None') {
                return;
            }

            educationService
                .getDocumentationMarkup($scope.selectedPersistence.Name + 'Persistence NServiceBus')
                .then(function (documentationMarkup) {
                    $scope.persistenceDocumentation = documentationMarkup;
                });
        });

        $scope.$watch('selectedTransport', function () {

            $scope.transportDocumentation = null;

            if (!$scope.selectedTransport) {
                return;
            }

            educationService
                .getDocumentationMarkup($scope.selectedTransport.Name + 'Transport NServiceBus')
                .then(function (documentationMarkup) {
                    $scope.transportDocumentation = documentationMarkup;
                });
        });

        $scope.$watch('selectedSerializer', function () {

            $scope.serializationDocumentation = null;

            if (!$scope.selectedSerializer) {
                return;
            }

            educationService
                .getDocumentationMarkup($scope.selectedSerializer.Name + 'Serializer NServiceBus')
                .then(function (documentationMarkup) {
                    $scope.serializationDocumentation = documentationMarkup;
                });
        });

        initialize();
    };

    ng.module('nsbBootstrap').controller('bootstrapWizard', ['$scope', 'bootstrapService', 'educationService', controller]);

})(angular);