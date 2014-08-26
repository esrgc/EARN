/*
Author: Tu Hoang
ESRGC 2014

collection
chart.js

load all Organizations

dependency: backbone.js

*/

app.Collection.Organizations = Backbone.Collection.extend({
  name: 'Organizations',
  url: 'message/Organizations'
 
});