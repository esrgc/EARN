﻿/*
Author: Tu Hoang
ESRGC 2014

Router
main.js

provides routes for messsage center

dependency: backbone.js

*/



app.Router.Main = Backbone.Router.extend({
  fetchedParticipants: false,
  newMessage: false,
  routes: {
    '': "init",//page index route,
    'for/:name/:id': 'renderMessageArea',
    'new': 'newMessage'
  },
  ///////////////////route functions/////////////////////////////
  init: function() {
    this.newMessage = false;//allow selecting the first message thread
    this.refresh();
  },
  renderMessageArea: function(name, id) {
    this.newMessage = false;//allow selecting the first message thread
    var newMessageView = app.getView('NewMessage');
    var messageArea = app.getView('MessageArea');
    newMessageView.hide();
    messageArea.show();
    this.refresh(name, id);//maintain current and hi-light active participant
  },
  newMessage: function() {
    this.newMessage = true;
    this.refresh();
    //render new message view
    var newMessageView = app.getView('NewMessage');
    var messageArea = app.getView('MessageArea');
    newMessageView.show();
    messageArea.hide();

  },
  /////////////helpers and private call backs///////////////
  refresh: function(currentName, currentId) {
    var scope = this;
    //only fetch once at start up
    if (!scope.fetchedParticipants) {
      var collection = app.getCollection('Participants');
      var participantList = app.getView('ParticipantList');

      collection.fetch({
        success: function(data) {
          participantList.render(data, currentName, currentId);
          scope.fetchedParticipants = true;
          //fetch messags
          if (scope.newMessage)
            return;
          else {
            if (typeof currentId == 'undefined') {
              //select 1st convo
              var url = $('#participant-list .list-group-item:first-child').attr('href');
              if (typeof url != 'undefined')
                scope.navigate(url.replace('#', ''), { trigger: true, replace: true }) //reload with the first conversation messages
            }
            else
              scope.fetchMessages(currentId, currentName);
          }
        }
      });
    }
    else {
      //just hi-light the selected participant
      $('#participant-list .list-group-item').removeClass('active');
      $('#participant-list .list-group-item[href="#for/' + currentName + '/' + currentId + '"]').addClass('active');
      //fetch messages
      scope.fetchMessages(currentId, currentName);
    }
  },
  fetchMessages: function(id, name) {
    var view = app.getView('MessageArea');
    if (typeof view == 'undefined')
      return;
    view.fetchMessages(id, name);
  }
  

});