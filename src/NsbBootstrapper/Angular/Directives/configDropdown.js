(function (ng) {

    ng.module('nsbBootstrap').directive('configDropdown', function () {

        return {
            restrict: 'E',
            templateUrl: '/Angular/Directives/Templates/configDropdown.html',
            scope: {
                label: '@attrLabel',
                selectedItem: '=attrModel',
                configurationItems: '=attrItems'
            },
            link: function(scope, element, attrs) {
            }
        };
    });

})(angular);