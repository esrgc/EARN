/*
Author: Tu Hoang
ESRGC 2014

collection
chart.js

load all Conversations

dependency: backbone.js

*/

app.Collection.Conversations = Backbone.Collection.extend({
  name: 'Conversations',
  url: 'message/Conversations',
  model: BackboneApp.Model.Conversation
  
});