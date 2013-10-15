/*
Author: Tu hoang
ESRGC
Provides base (prototype) functions for mapviewer
Note: this class is defined using dx library

This class implement leaflet API
*/

dx.app.MapViewer = dx.define({
    name: 'MapViewer',
    extend: 'dx.Component',
    _className: 'dx.app.MapViewer',
    initialize: function (options) {
        dx.Component.prototype.initialize.apply(this, arguments);
    },
    zoomToExtent: function (extent) {
        this.map.fitBounds(new L.LatLngBounds(new L.LatLng(extent.xmin, extent.ymin),
         new L.LatLng(extent.xmax, extent.ymax)));
    },
    zoomToFullExtent: function () {
    },
    //zoom to point with 2 zoom levels below max level
    zoomToPoint: function (x, y, level) {
        this.map.setView(new L.LatLng(y, x), this.map.getMaxZoom() - 2);
    },
    zoomIn: function () {
        this.map.zoomIn();
    },
    zoomOut: function () {
        this.map.zoomOut();
    },
    zoomToDataExtent: function (layer) {
    },
    panTo: function (x, y) {
        this.map.panTo(new L.LatLng(y, x));
    },
    locate: function () {
        this.map.locateAndSetView(this.map.getMaxZoom() - 2);
    }

});

