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
    organizationTypes: {},
    markerColors: [
        'darkred', 'orange', 'green', 'darkgreen',
        'blue', 'purple', 'darkpuple', 'cadetblue'
    ],
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
            var orgType = $(result).attr('data-userGroupName');
            var orgTypeID = $(result).attr('data-userGroupID');
            var url = $(result).attr('data-profileUrl');
            var id = $(result).attr('id');
            if (geom != "") {
                var feature = mapViewer.createFeature(geom);
                if (id != 'ownProfile') {

                    if (typeof orgType != 'undefined' && typeof orgTypeID != 'undefined') {
                        var typeExists = false;
                        for (var i in scope.organizationTypes) {
                            var type = scope.organizationTypes[i].name;
                            if (type == orgType) {
                                typeExists = true;
                                break;
                            }
                        }
                        var iconConfig = null;
                        if (!typeExists) {
                            var color = scope.markerColors[0];
                            scope.markerColors.splice(0, 1);
                            var orgTypeObj = {
                                name: orgType,
                                id: orgTypeID,
                                markerColor: color,
                                prefix: 'fa',
                                icon: 'building-o'
                            };
                            scope.organizationTypes[orgTypeID] = orgTypeObj;
                            iconConfig = orgTypeObj;
                        }
                        else
                            iconConfig = scope.organizationTypes[orgTypeID];

                        var icon = L.AwesomeMarkers.icon(iconConfig);
                        feature.setIcon(icon);
                        feature.bindPopup([
                            '<a href="' + url + '">' + orgName + '</a>',
                            '<br/>',
                            '<small>',
                            orgType,
                            '</small>'
                        ].join(''));
                    }
                }
                else {
                    //change icon
                    var icon = L.AwesomeMarkers.icon({
                        icon: 'user',
                        markerColor: 'red'
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