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
            maxZoom: 12,
            attribution: osmAttrib
        });
        this.features = new L.FeatureGroup([
            new L.Marker([39.0, -76.70]),
            new L.Marker([39.0, -76.20]),
            new L.Marker([38.9, -76.0]),
            new L.Marker([38.36, -75.59]).bindPopup('Salisbury University')
        ]);
        this.map = L.map('map', {
            layers: [osm, this.features],
            center: new L.LatLng(39.0, -76.70),
            zoom: 7,
            measureControl: true
        });
        //set up layer control
        var baseMaps = {
            'CloudMade': cmLayer,
            'OpenStreetMap': osm
        };

        var overlayMaps = {
            //other overlay layers go here
            //feature layer
            'Features': this.features
        };
        L.control.layers(baseMaps, overlayMaps).addTo(this.map);
        L.control.scale().addTo(this.map);
    },
    addPolygonToFeatureGroup: function (polygon) {
        var features = this.features;
        if (typeof features == 'undefined') {
            dx.log('No feature group found');
            return;
        }
        features.addLayer(polygon);
    },
    clearFeatures: function () {
        var features = this.features;
        if (typeof features == 'undefined') {
            dx.log('No feature group found');
            return;
        }
        features.clearLayers();
    },
    createPolygon: function (obj) {
        var wkt = new Wkt.Wkt();
        wkt.read(obj.g);
        var polygon = wkt.toObject();        
        return polygon;
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
    }
});