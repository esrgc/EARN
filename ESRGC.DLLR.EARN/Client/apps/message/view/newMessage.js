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
    'keyup textarea#new-message': 'onMessageTextChanged',
    'click button.send-message-btn': 'onSendMessageBtnClick'
  },
  initialize: function() {
    var connections = new Bloodhound({
      datumTokenizer: Bloodhound.tokenizers.obj.whitespace('name'),
      queryTokenizer: Bloodhound.tokenizers.whitespace,
      limit: 3,
      prefetch: {
        url: '/message/connections',
        ttl: 1
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
          '<div style="cursor: pointer; display: block;">',
          '<img class="img-thumbnail logo-image pull-left" style="margin-right: 10px;" src="{{logoUrl}}" alt="">',
          '<p><strong>{{name}}</strong></p>',
          '<div class="clear"></div>',
          '</div>'
        ].join('\n'))
      }
    });
  },
  setName: function(name) {
    this.$('#msg-participant').val(name);
  },
  onMessageTextChanged: function(ev) {
    var scope = this;
    console.log('message typing...');
    var message = scope.$('textarea#new-message').val();
    if (message == '')
      scope.$('.send-message-btn').addClass('disabled');
    else
      scope.$('.send-message-btn').removeClass('disabled');
  },
  onSendMessageBtnClick: function(ev) {
    var scope = this;
    var recipient = scope.$('#msg-participant').val().trim();
    if (recipient == '')
      return;
    console.log(recipient);
    scope.showLoadingPrompt();

    //send the message
    var message = scope.$('textarea#new-message').val();
    console.log('sending message: ')
    console.log(message);
    var message = new app.Model.Message({
      name: recipient,
      message: message.replace(/\r?\n/g, '<br />')
    });
    //post message
    message.save({}, {
      success: function(m, res, options) {
        console.log(res);

        if (res.status == 'success') {
          //reload message area
          scope.$('#msg-participant').val('');
          scope.$('textarea#new-message').val('');//clear message
          scope.$('.send-message-btn').addClass('disabled');
          scope.hideLoadingPrompt();

          var name = res.name;
          var id = res.id;

          //navigate out of new message
          var router = app.getRouter('Main');
          router.navigate('for/' + name + '/' + id, { trigger: true, replace: true });
        }
        else {
          scope.hideLoadingPrompt();
          alert('Message sending failed. ' + res.message);
        }
      }
    });
  }

});