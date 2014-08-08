/*
Author: Tu Hoang
ESRGC 2014

Router
main.js

provides routes for messsage center

dependency: backbone.js

*/



app.Router.Main = Backbone.Router.extend({
  fetchedParticipants: false,
  routes: {
    '': "init",//page index route,
    'for/:name/:id': 'renderMessageArea'
  },
  init: function() {
    this.refreshMessages();

  },
  renderMessageArea: function(name, id) {
    this.refreshMessages(name, id);//maintain current and hi-light active participant
  },
  refreshMessages: function(currentName, currentId) {
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
          scope.fetchMessages(currentId, currentName);
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
    if (typeof id == 'undefined')
      return;
    var scope = this;
    var collection = app.getCollection('Messages');
    if (collection == null) {
      console.log('Error getting "Messages collection"');
      return;
    }
    var view = app.getView('MessageArea');
    if (typeof id == 'undefined')
      return;
    collection.fetch({
      success: function(data) {
        console.log(data.toJSON());
        view.render(data.toJSON(), name);
      },
      data: {
        participantID: id
      }
    });
   
  }

});