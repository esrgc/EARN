/*
Author: Tu Hoang
ESRGC 2014

index.js

start up function
*/

app.View.ParticipantList = Backbone.View.extend({
  name: 'ParticipantList',
  el: '#participant-list',
  events: {

  },
  initialize: function() {
  },
  render: function(data, currentname, currentId) {
    var scope = this;
    console.log('Rendering participants...')
    console.log(data.toJSON());
    scope.$el.html('');
    _.each(data.models, function(m) {
      var data = m.toJSON();
      var template = _.template($('#participant').html(), { model: data, name: currentname, id: currentId });
      scope.$el.append(template);
    });
  },
  
});