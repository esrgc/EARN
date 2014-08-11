/*
Author: Tu Hoang
ESRGC 2014

newMessage.js

compose a new message
*/

app.View.NewMessage = app.View.Base.extend({
  name: 'NewMessage',
  el: '#newMessage',
  events: {
    'keyup textarea#new-message': 'onMessageTextChanged'
  },
  initialize: function() {
  },
  onMessageTextChanged: function(ev) {
    var scope = this;
    console.log('message typing...');
    var message = scope.$('textarea#new-message').val();
    if (message == '')
      scope.$('.send-message-btn').addClass('disabled');
    else
      scope.$('.send-message-btn').removeClass('disabled');
  }
  
});