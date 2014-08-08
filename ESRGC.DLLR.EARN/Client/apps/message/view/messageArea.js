/*
Author: Tu Hoang
ESRGC 2014

messageArea.js

start up function
*/

app.View.MessageArea = Backbone.View.extend({
  name: 'MessageArea',
  el: '#message-area',
  events: {

  },
  initialize: function() {
  },
  render: function(data, name) {
    var scope = this;
    console.log(this.name + ' init for ' + name);
    var template = _.template($('#message-board').html(), { name: name, messages: data });
    scope.$el.html(template);
  }
  

});