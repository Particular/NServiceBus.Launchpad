(function (ng) {

    var controller = function ($scope, bootstrapService) {

        var initialize = function () {

            bootstrapService.getBootstrapBuilder()
                .then(function (builderModel) {
                    $scope.builderModel = builderModel;
                });

            $scope.model = { endpointName: 'MyEndpointName' };

        };

        $scope.bootstrap = function () {

            var m = $scope.model;

            var model = {
                Version: $scope.builderModel.version,
                EndpointName: m.endpointName,
                Serializer: m.serializer,
                Persistence: m.persistence,
                Transport: m.transport
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