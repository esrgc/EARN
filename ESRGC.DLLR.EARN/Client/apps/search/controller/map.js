/*
Author: Tu hoang
ESRGC 2013

EARN MD CONNECT

Map controller
map.js

controller for profile searching by tags

Dependencies
dx library
*/

dx.defineController('Map', {
    name: 'Map',
    refs: {
        searchResults: '#searchResult .profile-section',
        ownProfile: '#searchResult #ownProfile'
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
                //scope.zoomToOwnProfile();
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
            var id = $(result).attr('id');
            if (geom != "") {
                var feature = mapViewer.createFeature(geom);
                if (id != 'ownProfile') {
                    feature.bindPopup('<a href="' + url + '">' + orgName + '</a>')
                }
                else {
                    //change icon
                    var icon = L.AwesomeMarkers.icon({
                        icon: 'user',
                        color: 'red'
                    });
                    feature.setIcon(icon);
                    feature.bindPopup('<a href="' + url + '">Your organization</a>')
                }
                mapViewer.addFeatureToFeatureGroup(feature);
            }
        });
    },
    zoomToOwnProfile: function () {
        var scope = dx.getController('Map');
        var mapViewer = dx.getApp().getMapViewer();
        var ownProfile = scope.getOwnProfile();
        var geom = ownProfile.attr('data-location');
        var feature = mapViewer.createFeature(geom);
        //dx.log(feature);
        mapViewer.zoomToPoint(feature.getLatLng());
    },
    scrollToResult: function goToByScroll(id) {
        // Scroll
        $('html,body').animate({
            scrollTop: $("#" + id).offset().top
        }, 'slow');
    }
});