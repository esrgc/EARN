/*
Author: Tu Hoang
ESRGC 2014

index.js

start up function
*/

app.View.ParticipantList = Backbone.View.extend({
  name: 'ParticipantList',
  el: '#participant-list',
  events: {},
  initialize: function() {
    this.render();
  },
  render: function() {
    var scope = this;
    var collection = app.getCollection('Participants');
    collection.fetch({
      success: function(data) {
        console.log('Fetched participants...')
        console.log(data.toJSON());
        scope.$el.html('');
        _.each(data.models, function(m) {
          var data = m.toJSON();
          var template = _.template($('#participant').html(), data);
          scope.$el.append(template);
        });
      }
    });

  }

});