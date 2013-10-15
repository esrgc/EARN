/*
Author: Tu hoang
ESRGC 2013

EARN Maryland Connect

Search application

Dependencies
dx library
*/

dx.application({
    name: 'profileSearch',
    stores: ['Search'],
    models: [],
    views: [],
    controllers: ['Search', 'Map'],
    launch: function () {
        dx.log('Application initialized.');
        var app = dx.getApp();
        //initialize the map
        app.appData.mapViewer = new dx.app.LeafletViewer();
        app.getMapViewer = function () {
            return app.appData.mapViewer;
        };
    }

});


