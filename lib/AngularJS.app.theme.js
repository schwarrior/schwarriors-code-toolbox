(function() {
    "use strict";

    angular
        .module("ngJsApp")
        .config(theme);

    function theme($mdThemingProvider) {
        
        var indigoExt = $mdThemingProvider.extendPalette('indigo', {
            '300': '039be5'
        });

        $mdThemingProvider.definePalette('indigoExt', indigoExt);

        $mdThemingProvider.theme('default')
            .primaryPalette('indigoExt')
            .accentPalette('orange');

        //// Extend the red theme with a few different colors
        //var neonRedMap = $mdThemingProvider.extendPalette('red', {
        //    '500': 'ff0000'
        //});
        //// Register the new color palette map with the name <code>neonRed</code>
        //$mdThemingProvider.definePalette('neonRed', neonRedMap);
        //// Use that theme for the primary intentions
        //$mdThemingProvider.theme('default')
        //    .primaryPalette('neonRed');

    };

})();

