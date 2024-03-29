﻿/*
Author: Tu Hoang
ESRGC 2014

index.js

start up function
*/

app.View.ConversationList = app.View.Base.extend({
  name: 'ConversationList',
  el: '#conversation-list',
  events: {
    'click small.btn-delete-convo': 'onDeleteConvoClick'
  },
  initialize: function() {
  },
  render: function(data, currentId) {
    var scope = this;
    scope.currentConversation = { id: currentId };
    console.log('Rendering Conversations...')
    //console.log(data);
    scope.$el.html('');
    if (data.length == 0) {
      scope.$el.html('<p class="text-center">No conversation found.</p>');
    }
    else {
      if (typeof currentId == 'undefined')
        currentId = data[0].toJSON().id;
      _.each(data, function(m) {
        var newData = m.toJSON();
        var template = _.template($('#conversation').html(), { model: newData, id: currentId });
        scope.$el.append(template);
      });
      scope.$el.animate({ scrollTop: 0 }, 0);//auto scrolls to the top

      //fetch messages
      var messageArea = app.getView('MessageArea');
      messageArea.fetchMessages(currentId);
    }
  },
  refresh: function(id) {
    var scope = this;
    var collection = app.getCollection('Conversations');
    collection.fetch({
      success: function(collection, res) {
        scope.render(collection.models, id);
        scope.$el.animate({ scrollTop: 0 }, 0);//auto scrolls to the top
        
      }
    });
  },
  onDeleteConvoClick: function(ev) {
    var scope = this;
    var target = ev.currentTarget;
    var convoId = $(target).attr('data-id');
    var collection = app.getCollection('Conversations');
    var convo = collection.get(convoId);
    console.log(convo);
    convo.remove({
      success: function(m, res) {
        if (res.status == 'success') {
          console.log('conversation removed!');
          scope.refresh();
        }
      }
    });
  }
});