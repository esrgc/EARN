/*
Author: Tu Hoang
ESRGC 2014

Model
Message.js

Represent Message

dependency: backbone.js

*/

app.Model.Message = Backbone.Model.extend({
  name: 'Message',
  url: '/message/send'//for posting new message

});