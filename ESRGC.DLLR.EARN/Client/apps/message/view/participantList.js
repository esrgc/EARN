/*
Author: Tu Hoang
ESRGC 2014

index.js

start up function
*/

app.View.ParticipantList = app.View.Base.extend({
  name: 'ParticipantList',
  el: '#participant-list',
  events: {

  },
  initialize: function() {
  },
  render: function(data, currentname, currentId) {
    var scope = this;
    scope.currentParticipant = { name: name, id: currentId };
    console.log('Rendering participants...')
    console.log(data.toJSON());
    scope.$el.html('');
    _.each(data.models, function(m) {
      var data = m.toJSON();
      var template = _.template($('#participant').html(), { model: data, name: currentname, id: currentId });
      scope.$el.append(template);
    });
  },
  refresh: function(name, id) {
    var scope = this;
    var collection = app.getCollection('Participants');
    collection.fetch({
      success: function(data) {
        scope.$el.html('');
        _.each(data.models, function(m) {
          var data = m.toJSON();
          var template = _.template($('#participant').html(), { model: data, name: name, id: id });
          scope.$el.append(template);
        });
      }
    });
  }
});