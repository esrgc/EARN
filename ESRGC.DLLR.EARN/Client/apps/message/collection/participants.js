/*
Author: Tu Hoang
ESRGC 2014

collection
chart.js

load all participants

dependency: backbone.js

*/

app.Collection.Participants = Backbone.Collection.extend({
  name: 'Participants',
  url: '/message/participants',
  model: BackboneApp.Model.Participant
  
});