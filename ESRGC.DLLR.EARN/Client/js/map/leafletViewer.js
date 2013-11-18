/*
Author: Tu hoang
ESRGC
Provides base (prototype) functions for mapviewer
Note: this class is defined using dx library

implements leaflet API 
operates foodshed application
*/

dx.app.LeafletViewer = dx.define({
    name: 'LeafletViewer',
    extend: dx.app.MapViewer,
    _className: 'LeafletViewer',
    initialize: function (options) {
        dx.app.MapViewer.prototype.initialize.apply(this, arguments);
        //cloudmade layer
        var cloudMadeUrl = 'http://{s}.tile.cloudmade.com/c0925b1494384159af7cd710aadbda8d/997/256/{z}/{x}/{y}.png';
        var cloudMadeAttribution = 'Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, <a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="http://cloudmade.com">CloudMade</a>';
        var cmLayer = new L.TileLayer(cloudMadeUrl, {
            attribution: cloudMadeAttribution,
            maxZoom: 18
        });
        //osmlayer
        var osmUrl = 'http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png';
        var osmAttrib = 'Map data © OpenStreetMap contributors';
        var osm = new L.TileLayer(osmUrl, {
            minZoom: 1,
            maxZoom: 18,
            attribution: osmAttrib
        });
        this.features = new L.FeatureGroup([
            //new L.Marker([39.0, -76.70]).bindPopup('Some organization'),
            //new L.Marker([39.0, -76.20]).bindPopup('Abc company'),
            //new L.Marker([38.9, -76.0]).bindPopup('Eastern shore company'),
            //new L.Marker([38.36, -75.59]).bindPopup('Salisbury University')
        ]);
        this.map = L.map('map', {
            layers: [osm, this.features],
            center: new L.LatLng(39.0, -76.70),
            zoom: 7,
            measureControl: true
        });
        //set up layer control
        //var baseMaps = {
        //    'CloudMade': cmLayer,
        //    'OpenStreetMap': osm
        //};

        //var overlayMaps = {
        //    //other overlay layers go here
        //    //feature layer
        //    'Features': this.features
        //};
        //L.control.layers(baseMaps, overlayMaps).addTo(this.map);
        L.control.scale().addTo(this.map);
    },
    addFeatureToFeatureGroup: function (feature) {
        var features = this.features;
        if (typeof features == 'undefined') {
            dx.log('No feature group found');
            return;
        }
        if (feature != null)
            features.addLayer(feature);
    },
    clearFeatures: function () {
        var features = this.features;
        if (typeof features == 'undefined') {
            dx.log('No feature group found');
            return;
        }
        features.clearLayers();
    },
    createFeature: function (obj) {
        var wkt = new Wkt.Wkt();
        wkt.read(obj);
        var f = wkt.toObject();
        return f;
    },
    getFeaturesBound: function () {
        var features = this.features;
        if (typeof features == 'undefined') {
            dx.log('No feature group found');
            return;
        }
        return features.getBounds();
    },
    zoomToFeatures: function () {
        var bounds = this.getFeaturesBound();
        this.map.fitBounds(bounds);
    },
    zoomToPoint: function (point, zoom) {
        var z = zoom || 10;//default zoom
        if (typeof point.x != 'undefined' && typeof point.y != 'undefined') {
            var latlng = new L.LatLng(point.x, point.y);
            this.map.setView(latlng, z);
        }
        else {
            this.map.setView(point, z);
        }
    }
});