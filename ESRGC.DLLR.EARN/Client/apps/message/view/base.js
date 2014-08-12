/*
Author: Tu Hoang
ESRGC 2014

Base.js

*/

app.View.Base = Backbone.View.extend({
  name: 'Base',
  events: {

  },
  initialize: function() {
  },
  render: function() {

  },
  show: function() {
    this.$el.addClass('active');
  },
  hide: function() {
    this.$el.removeClass('active');
  },
  showLoadingPrompt: function() {
    this.$('.loading-prompt').addClass('active');
  },
  hideLoadingPrompt: function() {
    this.$('.loading-prompt').removeClass('active');
  }

});