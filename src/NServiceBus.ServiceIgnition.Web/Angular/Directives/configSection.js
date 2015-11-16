(function (ng) {

    ng.module('nsbBootstrap').directive('configSection', function () {

        return {
            restrict: 'E',
            templateUrl: '/Angular/Directives/Templates/configSection.html',
            replace: true,
            scope: {
                model: '=configModel',
                onChange: '&?attrChange'
            },
            link: function (scope, element, attrs) {
                scope.onChange = scope.onChange || function () { };
            }
        };
    });

})(angular);