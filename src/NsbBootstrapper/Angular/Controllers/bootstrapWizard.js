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

            var setDefault = function (key) {
                $scope.selectedVersion[key].selectedItem =
                    $scope.selectedVersion[key].Items[0];
            };

            setDefault('TransportSection');
            setDefault('SerializerSection');
            setDefault('PersistenceSection');
        };

        $scope.bootstrap = function () {

            var version = $scope.selectedVersion;

            var model = {
                Version: version.NServiceBusVersion,
                EndpointName: $scope.endpointName,
                Serializer: version.SerializerSection.selectedItem,
                Persistence: version.PersistenceSection.selectedItem,
                Transport: version.TransportSection.selectedItem,
            };

            bootstrapService.triggerBootstrapping(model)
                .then(function (bootstrapResult) {
                    $scope.bootstrapResult = bootstrapResult;
                });
        };

        //$scope.$watch('selectedVersion.PersistenceSection.selectedItem', function () {

        //    $scope.persistenceDocumentation = null;

        //    if (!$scope.selectedPersistence || $scope.selectedPersistence.Name === 'None') {
        //        return;
        //    }

        //    educationService
        //        .getDocumentationMarkup($scope.selectedPersistence.Name + 'Persistence NServiceBus')
        //        .then(function (documentationMarkup) {
        //            $scope.persistenceDocumentation = documentationMarkup;
        //        });
        //});

        //$scope.$watch('selectedVersion.TransportSection.selectedItem', function () {

        //    $scope.transportDocumentation = null;

        //    if (!$scope.selectedTransport) {
        //        return;
        //    }

        //    educationService
        //        .getDocumentationMarkup($scope.selectedTransport.Name + 'Transport NServiceBus')
        //        .then(function (documentationMarkup) {
        //            $scope.transportDocumentation = documentationMarkup;
        //        });
        //});

        //$scope.$watch('selectedVersion.SerializerSection.selectedItem', function () {

        //    $scope.serializationDocumentation = null;

        //    if (!$scope.selectedSerializer) {
        //        return;
        //    }

        //    educationService
        //        .getDocumentationMarkup($scope.selectedSerializer.Name + 'Serializer NServiceBus')
        //        .then(function (documentationMarkup) {
        //            $scope.serializationDocumentation = documentationMarkup;
        //        });
        //});

        initialize();
    };

    ng.module('nsbBootstrap').controller('bootstrapWizard', ['$scope', 'bootstrapService', 'educationService', controller]);

})(angular);