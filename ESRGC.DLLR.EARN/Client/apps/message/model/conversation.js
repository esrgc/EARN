/*
Author: Tu Hoang
ESRGC 2014

Model
Conversation.js

Represent Conversation

dependency: backbone.js

*/

app.Model.Conversation = Backbone.Model.extend({
  name: 'Conversation',
  url: 'message',
  remove: function(options) {
    this.url = 'message/removeFromConvo/' + this.id;
    this.destroy(options);
  }
 

});