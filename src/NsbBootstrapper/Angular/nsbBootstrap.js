(function(ng) {
    ng.module('nsbBootstrap', ['hljs'])
        .config(function (hljsServiceProvider) {
            hljsServiceProvider.setOptions({
            // replace tab with 4 spaces
            tabReplace: '    '
        });
    });
})(angular);