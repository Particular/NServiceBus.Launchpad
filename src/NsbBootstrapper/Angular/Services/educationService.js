(function (ng) {

    var service = function ($http, $q, $window) {

        var searchCache = {};

        var scrapeDocumentationLinks = function (htmlString) {

            htmlString = htmlString.substring(htmlString.indexOf("<body>") + 6);
            htmlString = htmlString.substring(0, htmlString.indexOf("<footer>"));
            htmlString = htmlString.replace(/a href="\//g, 'a href="http://docs.particular.net/');
            htmlString = htmlString.replace(/img/g, 'span');
            var htmlDocument = $window.document.createElement('html');;

            htmlDocument.innerHTML = htmlString;

            var searchResultList = htmlDocument.querySelector('.contentArea ul');

            var listItems = searchResultList.getElementsByTagName('li');

            var items = [];

            for (var i = 0; i < listItems.length; i++) {

                var anchor = listItems[i].querySelector('a');
                var pTags = listItems[i].getElementsByTagName('p');

                items.push({
                    url: anchor.getAttribute('href'),
                    title: anchor.innerHTML,
                    grouping: pTags[0].innerHTML,
                    description: pTags[1].innerHTML
                });
            }

            return items;
        };

        var getDocumentationMarkup = function (searchTerms) {

            if (searchCache[searchTerms]) {
                return searchCache[searchTerms].promise;
            }

            var deferred = $q.defer();
            var encodedSearchTerms = $window.encodeURIComponent(searchTerms);

            deferred.reject();
            return deferred.promise;

            $http.get('/Home/Documentation?q=' + encodedSearchTerms)
                .then(function success(response) {
                    var documentationItems = scrapeDocumentationLinks(response.data);

                    if (!documentationItems || documentationItems.length === 0) {
                        deferred.reject('Documentation items are not available at the moment.');
                        return;
                    }

                    deferred.resolve(documentationItems);
                    searchCache[searchTerms] = deferred;
                },
                function failure(response) {
                    deferred.reject(response);
                });

            return deferred.promise;
        };

        this.getDocumentationMarkup = getDocumentationMarkup;
    };

    ng.module('nsbBootstrap').service('educationService', ['$http', '$q', '$window', service]);

})(angular);