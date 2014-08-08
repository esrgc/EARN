/*
Author: Tu Hoang
ESRGC 2014

Model
participant.js

Represent participant

dependency: backbone.js

*/

app.Model.Participant = Backbone.Model.extend({
  name: 'Participant',
  url: function() {
    return 'fetch'
  }
 

});