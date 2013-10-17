/*
Author: Tu hoang
ESRGC 2013

EARN Maryland Connect

Map controller
map.js

controller for profile searching by tags

Dependencies
dx library
*/

dx.defineController('Map', {
    name: 'Map',
    refs: {
        searchResults: '#searchResult section'
    },
    control: {

    },
    initialize: function () {
        dx.app.controller.Map.parent.initialize.apply(this, arguments);

        var scope = this;

        //initialize map
        var app = dx.getApp();
        //initialize the map
        app.appData.mapViewer = new dx.app.LeafletViewer();
        app.getMapViewer = function () {
            return app.appData.mapViewer;
        };

        var store = dx.getStore('Search');
        if (typeof store != 'undefined') {
            //after the store data is loaded this handler is called
            store.on('searchStoreLoaded', function (store, data) {
                scope.addMarkers();
            });
        }
    },
    ////
    ///private helper functions
    ///
    addMarkers: function () {
        var scope = dx.getController('Map');
        var mapViewer = dx.getApp().getMapViewer();
        var searchResult = scope.getSearchResults();
        mapViewer.clearFeatures();
        searchResult.each(function (i, result) {
            var geom = $(result).attr('data-location');
            var orgName = $(result).attr('data-organization');
            var url = $(result).attr('data-profileUrl');
            if (geom != "") {
                var feature = mapViewer.createFeature(geom);
                feature.bindPopup('<a href="' + url + '">' + orgName + '</a>')
                mapViewer.addFeatureToFeatureGroup(feature);
            }
        });
        //mapViewer.zoomToFeatures();
    },
    scrollToResult: function goToByScroll(id) {
        // Scroll
        $('html,body').animate({
            scrollTop: $("#" + id).offset().top
        }, 'slow');
    }
});