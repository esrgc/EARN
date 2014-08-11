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
    //  var p = scope.currentParticipant;
    //  if (typeof p != 'undefined') {
    //    console.log('refreshing messages...');
    //    scope.fetchMessages(p.id, p.name);
    //  }

    //}, 15000);
  },
  render: function(data, name, id) {
    var scope = this;
    scope.currentParticipant = { id: id, name: name };
    console.log(this.name + ' init for ' + name);
    var template = _.template($('#message-board').html(), { id: id, name: name, messages: data });
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
    var p = scope.currentParticipant;
    var message = scope.$('textarea#new-message').val();
    console.log('sending message: ')
    console.log(message);
    var message = new app.Model.Message({
      participantID: p.id,
      message: message.replace(/\r?\n/g, '<br />')
    });
    //post message
    message.save({}, {
      success: function(m, res, options) {
        console.log(res);
        //reload message area
        scope.fetchMessages(p.id, p.name);        
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
  fetchMessages: function(id, name) {
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
        //console.log(data.toJSON());
        scope.render(data.toJSON(), name, id);
        console.log('Messages fetched for ' + name);
        //refresh participant list
        var participantListView = app.getView('ParticipantList');
        participantListView.refresh(name, id);
      },
      data: {
        participantID: id
      }
    });
  },
  onRefreshBtnClick: function(ev) {
    var scope = this;
    var p = scope.currentParticipant;
    if (typeof p != 'undefined') {
      console.log('refreshing messages...');
      scope.fetchMessages(p.id, p.name);
    }
  }


});