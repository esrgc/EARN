/*
Author: Tu Hoang
ESRGC 2014

index.js

start up function
*/
var startup = app.startup = function() {
  console.log('Initilizing message box...');

  //start application
  app.application({
    name: 'EARNConnectMessage',
    views: [
      'ParticipantList'
    ],
    collections: [
     'Participants'
    ],
    routers: [
      'Main'
    ],
    launch: function() {

      //for underscore template custom dilimiters
      _.templateSettings = {
        evaluate: /\{\[([\s\S]+?)\]\}/g,
        interpolate: /\{\{([\s\S]+?)\}\}/g
      };

      console.log("Message box initialized.");
    }
  });
};