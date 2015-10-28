(function (ng) {

    var directive = {
        template: 'todo',
        link: function(scope, element) {
            
        }
    };

    ng.module('nsbBootstrap').directive('configItemDropdown', [function() {
        return directive;
    }]);

})(angular);