/*
Author: Tu Hoang
ESRGC 2014

collection
messages.js

load all message for selected participant

dependency: backbone.js

*/

app.Collection.Messages = Backbone.Collection.extend({
  name: 'Messages',
  url: '/message/fetch',
  model: BackboneApp.Model.Message

});