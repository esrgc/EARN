/*
Author: Tu Hoang
ESRGC 2014

newMessage.js

compose a new message
*/

app.View.NewMessage = app.View.Base.extend({
  name: 'NewMessage',
  el: '#newMessage',
  events: {
    'keyup textarea#new-message': 'onMessageTextChanged'
  },
  initialize: function() {
    var connections = new Bloodhound({
      datumTokenizer: Bloodhound.tokenizers.obj.whitespace('name'),
      queryTokenizer: Bloodhound.tokenizers.whitespace,
      limit: 5,
      prefetch: {
        url: '/message/connections'
        //ttl: 1
        // the json file contains an array of strings, but the Bloodhound
        // suggestion engine expects JavaScript objects so this converts all of
        // those strings
        //filter: function(list) {
        //  return $.map(list, function(i) { return { name: i.name }; });
        //}
      }
    });

    // kicks off the loading/processing of `local` and `prefetch`
    connections.initialize();

    // passing in `null` for the `options` arguments will result in the default
    // options being used
    this.$('input.typeahead').typeahead({
      hint: true,
      highlight: true
    },
    {
      name: 'connections',
      displayKey: 'name',
      // `ttAdapter` wraps the suggestion engine in an adapter that
      // is compatible with the typeahead jQuery plugin
      source: connections.ttAdapter(),
      templates: {
        empty: [
          '<div class="text-center">',
          'Unable to find any result matched',
          '</div>'
        ].join('\n'),
        suggestion: Handlebars.compile([
          '<div style="cursor: pointer;">',
          '<img class="img-thumbnail logo-image pull-left" style="margin-right: 10px;" src="{{logoUrl}}" alt="">',
          '<p><strong>{{name}}</strong></p>',
          '</div>'
        ].join('\n'))
      }
    });
  },
  onMessageTextChanged: function(ev) {
    var scope = this;
    console.log('message typing...');
    var message = scope.$('textarea#new-message').val();
    if (message == '')
      scope.$('.send-message-btn').addClass('disabled');
    else
      scope.$('.send-message-btn').removeClass('disabled');
  }

});