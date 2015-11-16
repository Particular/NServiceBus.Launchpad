(function (ng) {

    ng.module('nsbBootstrap').directive('documentationLinks', function () {

        return {
            restrict: 'E',
            templateUrl: '/Angular/Directives/Templates/documentationLinks.html',
            scope: {
                title: '=attrTitle',
                documentationItems: '=attrItems'
            },
            link: function(scope) {
                scope.hideList = false;
            }
        };
    });

})(angular);