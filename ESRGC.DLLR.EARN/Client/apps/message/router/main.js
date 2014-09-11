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
  fetchConversations: false,
  newMessage: false,
  routes: {
    '': "init",//page index route,
    'for/:id': 'renderMessageArea',
    'new': 'newMessage',
    'new/:name': 'newMessageByName',
    'newById/:id': 'newMessageById',
    'newAdmin': 'newAdminMessage'
  },
  ///////////////////route functions/////////////////////////////
  init: function() {
    this.showActiveView('MessageArea');
    var convoList = app.getView('ConversationList');
    convoList.refresh();
  },
  renderMessageArea: function(id) {
    this.showActiveView('MessageArea');
    var convoList = app.getView('ConversationList');
    convoList.refresh(id);
  },
  newMessage: function() {
    this.newMessage = true;
    //render new message view
    this.showActiveView('NewMessage');
    var convoList = app.getView('ConversationList');
    convoList.refresh();

  },
  newMessageByName: function(name) {
    this.newMessage = true;
    //render new message view
    this.showActiveView('NewMessage');
    var newMessageView = app.getView('NewMessage');
    newMessageView.setName(name);
    var convoList = app.getView('ConversationList');
    convoList.refresh();
  },
  newMessageById: function(id){
    this.newMessage = true;
    //render new message view
    this.showActiveView('NewMessage');
    var newMessageView = app.getView('NewMessage');
    newMessageView.setIds(id);
    var convoList = app.getView('ConversationList');
    convoList.refresh();
  },
  newAdminMessage: function() {
    this.newMessage = true;
    //render new message view
    this.showActiveView('adminMessage')
  },
  /////////////helpers and private call backs/////////////// 
  showActiveView: function(viewName) {
    var views = app.getViews();
    _.each(views, function(v) {
      if (v.name != 'conversationList') {
        v.hide();
      }      
    });
    var activeView = app.getView(viewName);
    if (activeView)
      activeView.show();
  }
  

});