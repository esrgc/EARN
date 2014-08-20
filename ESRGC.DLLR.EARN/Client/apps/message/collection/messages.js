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
  url: 'message/fetch',
  model: BackboneApp.Model.Message,
  parseData: function() {
    var scope = this;
    var model = scope.at(0);//always the first
    var messages = model.get('messages');
    var currentSenderName = '';
    var currentMsg;
    var removeIndex = [];

    for (var i in messages) {
      var msg = messages[i];
      if (currentSenderName != msg.senderName && !msg.isAdminMessage) {
        currentSenderName = msg.senderName;
        currentMsg = msg.message;
      }
      else {
        if (i != 0) {
          delete messages[i - 1];
          var newMessage = currentMsg + '<br/>' + msg.message;
          msg.message = newMessage;
          currentMsg = newMessage;
        }
      }
    }

    return model.toJSON();
  }

});