/*
Author: Tu Hoang
ESRGC 2014

Router
main.js

provides routes for messsage center

dependency: backbone.js

*/



app.Router.Main = Backbone.Router.extend({
  name: 'Main',
  fetchedParticipants: false,
  newMessage: false,
  routes: {
    '': "init",//page index route,
    'for/:name/:id': 'renderMessageArea',
    'new': 'newMessage',
    'new/:name': 'newMessageByName',
    'newAdmin': 'newAdminMessage'
  },
  ///////////////////route functions/////////////////////////////
  init: function() {
    this.showActiveView('MessageArea');
    this.newMessage = false;//allow selecting the first message thread
    this.refresh();
  },
  renderMessageArea: function(name, id) {
    this.newMessage = false;//allow selecting the first message thread
    this.showActiveView('MessageArea');
    this.refresh(name, id);//maintain current and hi-light active participant
  },
  newMessage: function() {
    this.newMessage = true;
    this.refresh();
    //render new message view
    this.showActiveView('NewMessage');
  },
  newMessageByName: function(name) {
    this.newMessage = true;
    this.refresh();
    //render new message view
    this.showActiveView('NewMessage');
    var messageArea = app.getView('NewMessage');
    newMessageView.setName(name);
  },
  newAdminMessage: function() {
    this.newMessage = true;
    this.refresh();
    //render new message view
    this.showActiveView('adminMessage')
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
                scope.navigate(url.replace('#', ''), { trigger: true, replace: false }) //reload with the first conversation messages
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
  },
  showActiveView: function(viewName) {
    var views = app.getViews();
    _.each(views, function(v) {
      if (v.name != 'ParticipantList') {
        v.hide();
      }      
    });
    var activeView = app.getView(viewName);
    if (activeView)
      activeView.show();
  }
  

});