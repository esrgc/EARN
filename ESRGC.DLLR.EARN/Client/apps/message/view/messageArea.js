/*
Author: Tu Hoang
ESRGC 2014

messageArea.js

controls message area
*/

app.View.MessageArea = app.View.Base.extend({
  name: 'MessageArea',
  el: '#message-area',
  events: {
    'click .send-message-btn': 'sendMessage',
    'keyup textarea#new-message': 'onMessageTextChanged',
    'click input[type="reset"]': 'onClearBtnClick',
    'click button#refreshBtn': 'onRefreshBtnClick'
  },
  initialize: function() {
    var scope = this;
    //refresh every 15 seconds
    //setInterval(function() {
    //  var p = scope.currentConversation;
    //  if (typeof p != 'undefined') {
    //    console.log('refreshing messages...');
    //    scope.fetchMessages(p.id, p.name);
    //  }

    //}, 15000);
  },
  render: function(data, id) {
    var scope = this;
    scope.currentConversation = { id: id };
    var data = scope.processMessages(data);//group messages form the same sender
    
    var template = _.template($('#message-board').html(), data);
    scope.$el.html(template);

    var composer = _.template($('#message-composer').html());
    scope.$('.message-container').append(composer);
    //scroll to bottom
    //scope.$('.message-board').scrollTop(scope.$('.message-board').height());
    var mb = scope.$('.message-board');
    mb.animate({ scrollTop: mb.prop("scrollHeight") - mb.height() }, 0);

  },
  sendMessage: function(ev) {
    var scope = this;
    var p = scope.currentConversation;
    var message = scope.$('textarea#new-message').val();
    console.log('sending message: ')
    console.log(message);
    var message = new app.Model.Message({
      conversationID: p.id,
      message: message.replace(/\r?\n/g, '<br />')
    });
    scope.showLoadingPrompt();
    //post message
    message.save({}, {
      success: function(m, res, options) {
        console.log(res);
        scope.hideLoadingPrompt();
        var convoList = app.getView('ConversationList');
        convoList.refresh(p.id);
      }
    });

  },
  onMessageTextChanged: function(ev) {
    var scope = this;
    console.log('message typing...');
    var message = scope.$('textarea#new-message').val();
    if (message == '')
      scope.$('.send-message-btn').addClass('disabled');
    else
      scope.$('.send-message-btn').removeClass('disabled');
  },
  onClearBtnClick: function(ev) {
    this.$('.send-message-btn').addClass('disabled');
  },
  fetchMessages: function(id) {
    if (typeof id == 'undefined')
      return;
    var scope = this;
    var collection = app.getCollection('Messages');
    if (collection == null) {
      console.log('Error getting "Messages collection"');
      return;
    }
    collection.fetch({
      success: function(data) {
        var newData = data.toJSON()[0];
        //console.log(newData);
        scope.render(newData, id);
        console.log('Messages fetched for convo id ' + id);
      },
      data: {
        conversationID: id
      }
    });
  },
  onRefreshBtnClick: function(ev) {
    var scope = this;
    var p = scope.currentConversation;
    var convoList = app.getView('ConversationList');
    convoList.refresh(p.id);
  },
  ////////////private functions
  processMessages: function(data) {
    var messages = data.messages;
    var currentSenderName = '';
    var currentMsg;
    var removeIndex = [];

    for(var i in messages){
      var msg = messages[i];
      if (currentSenderName != msg.senderName) {
        currentSenderName = msg.senderName;
        currentMsg = msg.message;        
      }
      else {
        delete messages[i - 1];
        var newMessage = currentMsg + '<br/>' + msg.message;
        msg.message = newMessage;
        currentMsg = newMessage;
      }
    }
    
    return data;
  }


});