(function (ng) {

    var controller = function ($scope, bootstrapService) {

        var initialize = function () {

            bootstrapService.getBootstrapBuilder()
                .then(function (builderModel) {
                    $scope.builderModel = builderModel;
                });
        };

        $scope.bootstrap = function () {

            var model = {
                Version: $scope.selectedVersion.NServiceBusVersion,
                Serializer: $scope.selectedSerializer,
                Persistence: $scope.selectedPersistence,
                Transport: $scope.selectedTransport
            };

            bootstrapService.triggerBootstrapping(model)
                .then(function (configuration) {
                    $scope.configuration = configuration;
                });

        };

        initialize();
    };

    ng.module('nsbBootstrap').controller('bootstrapWizard', ['$scope', 'bootstrapService', controller]);

})(angular);