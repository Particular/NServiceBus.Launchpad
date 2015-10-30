(function (ng) {

    ng.module('nsbBootstrap').directive('documentationLinks', function () {

        return {
            restrict: 'E',
            replace: true,
            templateUrl: '/Angular/Directives/Templates/documentationLinks.html',
            scope: {
                title: '=attrTitle',
                documentationItems: '=attrItems'
            }
        };
    });

})(angular);