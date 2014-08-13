/*
Author: Tu Hoang
ESRGC 2014

Model
AdminMessage.js

Represent AdminMessage

dependency: backbone.js

*/

app.Model.AdminMessage = Backbone.Model.extend({
  name: 'AdminMessage',
  url: 'Message/adminSend'//for posting new AdminMessage
});