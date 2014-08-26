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
  mode: '',
  url: function() {
    if (this.mode == 'new-conversation') {
      return 'message/newMessage';
    }
    else
      return 'message/send';
  },
  //for new conversation thread
  startNew: function(options) {
    this.mode = 'new-conversation';
    this.save({}, options);
  },
  //for sending message in current convo
  send: function(options) {
    this.mode = 'send-msg';
    this.save({}, options);
  }
});