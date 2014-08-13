/*
Author: Tu Hoang
ESRGC 2014

AdminMessage.js

compose a new message
*/

app.View.AdminMessage = app.View.Base.extend({
  name: 'AdminMessage',
  el: '#adminMessage',
  events: {
    'keyup textarea#new-admin-message': 'onMessageTextChanged',
    'click button.send-message-btn': 'onSendMessageBtnClick'
  },
  initialize: function() {
   console.log('Admin new message view initialized.')
  },  
  onMessageTextChanged: function(ev) {
    var scope = this;
    console.log('message typing...');
    var message = scope.$('textarea#new-admin-message').val();
    if (message == '')
      scope.$('.send-message-btn').addClass('disabled');
    else
      scope.$('.send-message-btn').removeClass('disabled');
  },
  onSendMessageBtnClick: function(ev) {
    var scope = this;
    scope.showLoadingPrompt();

    //send the message
    var message = scope.$('textarea#new-admin-message').val();
    console.log('sending admin message: ')
    console.log(message);
    var message = new app.Model.AdminMessage({
      message: message.replace(/\r?\n/g, '<br />')
    });
    //post message
    message.save({}, {
      success: function(m, res, options) {
        console.log(res);
        console.log(m);

        if (res.status == 'success') {
          //reload message area
          scope.$('textarea#new-admin-message').val('');//clear message
          scope.$('.send-message-btn').addClass('disabled');
          scope.hideLoadingPrompt();

          //navigate out of new message
          var router = app.getRouter('Main');
          router.navigate('', { trigger: true, replace: true });
        }
        else {
          scope.hideLoadingPrompt();
          alert('Message sending failed. ' + res.message);
        }
      }
    });
  }

});