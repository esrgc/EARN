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
    var currentMsg = '';
    var currentDate = '';
    var removeIndex = [];

    //loop through messages and group messages from the same
    //sender
    for (var i in messages) {
      var msg = messages[i];
      if (currentSenderName != msg.senderName) {
        currentSenderName = msg.senderName;
        currentMsg = msg.message;
        currentDate = msg.date;
      }
      else {
        if (i != 0) {
          if (msg.date == currentDate) {
            //group messages
            delete messages[i - 1];
            var newMessage = [currentMsg, msg.message].join('<br/>');
            msg.message = newMessage;
            currentMsg = newMessage;
          }
          else
            currentDate = msg.date;//store current message date for the next message processing
        }
      }
    }



    return model.toJSON();
  }

});