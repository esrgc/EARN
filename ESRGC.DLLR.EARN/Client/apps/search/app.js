﻿/*
Author: Tu hoang
ESRGC 2013

EARN MD CONNECT

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
        var scope = dx.getController('Map');
        //add markers after the map is initialized
        scope.addMarkers();
        //scope.zoomToOwnProfile();
        dx.log('Application initialized.');

    }

});


