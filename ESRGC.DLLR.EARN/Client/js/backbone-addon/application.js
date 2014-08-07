/*
Tu Hoang
ESRGC 2014


app.js

application namespace
dependency: Backbone.js
*/

var BackboneApp;
if (typeof BackboneApp == 'undefined')
  BackboneApp = {
    //prototypes
    View: {},
    Collection: {},
    Model: {},
    Router: {},
    Map: {},
    Util: {},
    //getters
    getViews: function() {
      if (typeof this.appInstance == 'undefined')
        return null;
      return this.appInstance._views;
    },
    getCollections: function() {
      if (typeof this.appInstance == 'undefined')
        return null;
      return this.appInstance._collections;
    },
    getModels: function() {
      if (typeof this.appInstance == 'undefined')
        return null;
      return this.appInstance._models;
    },
    getRouters: function() {
      if (typeof this.appInstance == 'undefined')
        return null;
      return this.appInstance._routers;
    },
    getView: function(name) {
      if (typeof this.appInstance == 'undefined')
        return null;
      for (var i in this.appInstance._views) {
        var o = this.appInstance._views[i];
        var oname = (o.name || '').toLowerCase();
        if (oname == name.toLowerCase())
          return o;
      }
    },
    getViewByID: function(id) {
      id = '#' + id;
      if (typeof this.appInstance == 'undefined')
        return null;
      for (var i in this.appInstance._views) {
        var o = this.appInstance._views[i];
        var vId = o.id;
        if (id == vId)
          return o;
      }
    },
    getViewByType: function(type) {
      if (typeof this.appInstance == 'undefined')
        return null;
      for (var i in this.appInstance._views) {
        var o = this.appInstance._views[i];
        var otype = (o.type || '').toLowerCase();
        if (otype == type.toLowerCase())
          return o;
      }
    },
    getCollection: function(name) {
      if (typeof this.appInstance == 'undefined')
        return null;
      for (var i in this.appInstance._collections) {
        var o = this.appInstance._collections[i];
        var oname = (o.name || '').toLowerCase();
        if (oname == name.toLowerCase())
          return o;
      }
    },
    getModel: function(name) {
      if (typeof this.appInstance == 'undefined')
        return null;
      for (var i in this.appInstance._models) {
        var o = this.appInstance._models[i];
        var oname = (o.name || '').toLowerCase();
        if (oname == name.toLowerCase())
          return o;
      }
    },
    getRouter: function(name) {
      if (typeof this.appInstance == 'undefined')
        return null;
      for (var i in this.appInstance._routers) {
        var o = this.appInstance._routers[i];
        var oname = (o.name || '').toLowerCase();
        if (oname == name.toLowerCase())
          return o;
      }
    },
    //application initialization
    application: function(options) {
      var scope = this;
      this.appInstance = {
        name: options.name || 'BackboneApp',
        //instances
        _views: [],
        _collections: [],
        _models: [],
        _routers: []
      };
      //create getter for app instance
      this.getApp = function() { return this.appInstance; };
      //initialize components
      var models = options.models || [];
      var collections = options.collections || [];
      var views = options.views || [];
      var routers = options.routers || [];
      //instantiate components
      for (var i in models) {
        var model = models[i];
        this.appInstance._models.push(new app.Model[model]());
      }
      for (var i in collections) {
        var collection = collections[i];
        this.appInstance._collections.push(new app.Collection[collection]());
      }
      for (var i in views) {
        var view = views[i];
        this.appInstance._views.push(new app.View[view]());
      }
      for (var i in routers) {
        var router = routers[i];
        this.appInstance._routers.push(new app.Router[router]());
      }


      //wire window resize event
      var TO = false;
      //wire window resize event
      $(window).on('resize', function(e) {
        if (TO !== false) clearTimeout(TO);
        TO = setTimeout(function() {
          var views = app.getViews();
          _.each(views, function(v) {
            if (typeof v.resize == 'function')
              v.resize.call(v);
          });
        }, 300);
      });

      //run launch function
      if (typeof options.launch == 'function')
        options.launch();
      //finally starts backbone history;
      Backbone.history.start();
    }

  };

var app = BackboneApp;

