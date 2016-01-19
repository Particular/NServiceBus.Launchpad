(function (ng) {

    var controller = function ($scope, bootstrapService, educationService) {

        var initialize = function () {

            bootstrapService.getBootstrapBuilder()
                .then(function (builderModel) {
                    $scope.builderModel = builderModel;
                });
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
            var transport = version.TransportSection.selectedItem;
            var serializer = version.SerializerSection.selectedItem;
            var persistence = version.PersistenceSection.selectedItem;
            var endpoints = $scope.endpointList;

            for (var i = 0; i < endpoints.length; i++) {
                endpoints[i].NServiceBusVersion = version.NServiceBusVersion;
                endpoints[i].Transport = transport.Value;
                endpoints[i].Serializer = serializer.Value;
                endpoints[i].Persistence = persistence.Value;
            }

            var model = {
                NServiceBusVersion: version.NServiceBusVersion,
                Transport: transport.Value,
                Serializer: serializer.Value,
                EndpointConfigurations: endpoints,
                InCodeSubscriptions: version.InCodeSubscriptions
            };

            bootstrapService.triggerBootstrapping(model)
                .then(function (bootstrapDownloadLink) {
                    $scope.downloadLink = bootstrapDownloadLink;
                    $scope.createdDate = new Date().toLocaleTimeString();
                });
        };

        $scope.endpointList = [];
        var defaultEndpointNameTemplate = "Sample.Endpoint";

        $scope.messageDefinitions = {
        };

        $scope.sharedEvents = [];

        var refreshEvents = function () {
            var newList = [];
            for (var key in $scope.messageDefinitions) {

                var messageExists = $scope.messageDefinitions.hasOwnProperty(key);

                if (messageExists && $scope.messageDefinitions[key].isEvent) {
                    newList.push(key);
                }
            }
            $scope.sharedEvents = newList;
        };

        $scope.addNewEndpoint = function () {

            var endpoint = {
                EndpointName: defaultEndpointNameTemplate + ($scope.endpointList.length + 1),
                MessageHandlers: []
            };

            var messageIsDuplicate = function (messageName) {

                for (var i = 0; i < endpoint.MessageHandlers.length; i++) {
                    var message = endpoint.MessageHandlers[i];

                    var nameMatches = message.MessageTypeName === messageName;

                    if (nameMatches) {
                        return true;
                    }
                }

                return false;
            };

            var isExistingNonEvent = function(messageName) {
                var messageExists = $scope.messageDefinitions.hasOwnProperty(messageName);
                return messageExists && !$scope.messageDefinitions[messageName].isEvent;
            }

            var addMessage = function (messageName, isEvent) {

                if (!messageName
                    || messageName.length === 0
                    || messageIsDuplicate(messageName)
                    || isExistingNonEvent(messageName)) {
                    return;
                }

                endpoint.MessageHandlers.push({ MessageTypeName: messageName, IsEvent: isEvent || false });

                if (!$scope.messageDefinitions[messageName]) {
                    $scope.messageDefinitions[messageName] = { isEvent: isEvent || false, count: 0 };
                }

                $scope.messageDefinitions[messageName].count++;
                refreshEvents();
            };

            endpoint.addMessage = function (messageName) {
                addMessage(messageName, false);
                endpoint.currentMessageName = '';
            };

            endpoint.addEvent = function (messageName) {
                addMessage(messageName, true);
                endpoint.currentEventName = '';
            };

            endpoint.removeMessage = function (message) {

                var messageName = message.MessageTypeName;

                if (!messageIsDuplicate(messageName)) {
                    return;
                }

                var newMessageList = [];

                for (var i = 0; i < endpoint.MessageHandlers.length; i++) {
                    if (endpoint.MessageHandlers[i].MessageTypeName !== messageName) {
                        newMessageList.push(endpoint.MessageHandlers[i]);
                    }
                }

                endpoint.MessageHandlers = newMessageList;

                if (--($scope.messageDefinitions[messageName].count) < 1) {
                    delete $scope.messageDefinitions[messageName];
                }

                refreshEvents();
            };

            $scope.endpointList.push(endpoint);

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