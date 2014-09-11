/*
Author: Tu Hoang
ESRGC 2014

newMessage.js

compose a new message
*/

app.View.NewMessage = app.View.Base.extend({
  name: 'NewMessage',
  el: '#newMessage',
  recipients: [],
  events: {
    'keyup textarea#new-message': 'onMessageTextChanged',
    'click button.send-message-btn': 'onSendMessageBtnClick',
    'click button#addProfile': 'onAddProfileBtnClick',
    'click .recipient-container .recipient > a': 'onRemoveRecipientClick'
  },
  initialize: function() {
    var scope = this;
    var collection = app.getCollection('Organizations');
    collection.fetch({
      success: function(data) {
        scope.organizations = data.toJSON();

        var engine = new Bloodhound({
          name: 'organizations',
          local: data.toJSON(),
          remote: {
            url: 'message/Organizations'
          },
          datumTokenizer: function(d) {
            return Bloodhound.tokenizers.whitespace(d.name);
          },
          queryTokenizer: Bloodhound.tokenizers.whitespace,
          limit: 3
          //prefetch: {
          //  url: 'message/engine',
          //  ttl: 1
          //  // the json file contains an array of strings, but the Bloodhound
          //  // suggestion engine expects JavaScript objects so this converts all of
          //  // those strings
          //  //filter: function(list) {
          //  //  return $.map(list, function(i) { return { name: i.name }; });
          //  //}
          //}
        });

        // kicks off the loading/processing of `local` and `prefetch`
        engine.initialize();

        // passing in `null` for the `options` arguments will result in the default
        // options being used
        this.$('input.typeahead').typeahead({
          hint: true,
          highlight: true
        },
        {
          name: 'Organizations',
          displayKey: 'name',
          // `ttAdapter` wraps the suggestion engine in an adapter that
          // is compatible with the typeahead jQuery plugin
          source: engine.ttAdapter(),
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
        })
        .on('typeahead:selected', function(ev, obj, dataset) {
          var name = obj.name;
          if (typeof name != 'undefined')
            scope.addRecipient(name);
          //reset text for the name box
          scope.$('#msg-participant').val('');
        });
        //add recipients by id
        if (typeof scope.ids != 'undefined') {
          _.each(scope.ids, function(id) {
            console.log(id);
            //add recipient
            scope.addRecipientById(id);
          });
          return;//skip adding names if this step is done
        }
        //if there was names for recipient add them
        if (typeof scope.names != 'undefined') {
          _.each(scope.names, function(name) {
            console.log(name);
            //add recipient
            scope.addRecipient(name.trim());
          });
        }
      }
    });


  },
  setName: function(name) {
    this.names = name.split(',');
    //this.$('#msg-participant').val(name);
  },
  setIds: function(ids) {
    this.ids = ids.split(',');
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
  onAddProfileBtnClick: function() {
    var scope = this;
    var recipient = scope.$('#msg-participant').val().trim();
    if (recipient == '')
      return;
    console.log(recipient);

    //console.log(scope.organizations);
    scope.addRecipient(recipient);

    //reset text for the name box
    scope.$('#msg-participant').val('');

  },
  onRemoveRecipientClick: function(ev) {
    var target = ev.currentTarget;
    var name = $(target).attr('data-name');
    this.removeRecipient(name);//remove from the reipient array
    $(target).parent().remove();//remove from the dom
  },
  onSendMessageBtnClick: function(ev) {
    var scope = this;
    if (scope.recipients.length == 0) {
      scope.updateValidationMessage('Please add an organization!');
      return;
    }
    scope.showLoadingPrompt();
    //send the message
    var message = scope.$('textarea#new-message').val();
    console.log('sending message: ')
    console.log(message);
    //parse id
    var IDs = [];
    _.each(scope.recipients, function(r) {
      IDs.push(r.id);
    });

    var message = new app.Model.Message({
      profileIDs: IDs,
      message: message.replace(/\r?\n/g, '<br />')
    });
    //post message
    message.startNew({
      success: function(m, res, options) {
        console.log(res);

        if (res.status == 'success') {
          //reload message area
          scope.$('#msg-participant').val('');
          scope.$('textarea#new-message').val('');//clear message
          scope.$('.send-message-btn').addClass('disabled');
          scope.hideLoadingPrompt();

          //clear info
          scope.recipients = [];
          scope.$('.recipient-container').html('');

          console.log('Message sent..!');
          //var name = res.name;
          var id = res.id;

          //navigate out of new message
          var router = app.getRouter('Main');
          router.navigate('for/' + id, { trigger: true, replace: true });
        }
        else {
          scope.hideLoadingPrompt();
          alert('Message sending failed. ' + res.message);
        }
      }
    });
  },
  addRecipient: function(name) {
    var scope = this;
    scope.updateValidationMessage('');
    if (typeof scope.recipients == 'undefined')
      scope.recipients = [];
    var organizations = scope.organizations;
    if (typeof organizations == 'undefined') {
      console.log('No recipient loaded from server');
      return;
    }
    var valid = false, exist = false;
    _.each(scope.recipients, function(r) {
      if (name == r.name) {
        exist = true;
        return;
      }
    });
    if (exist)
      return;//already exists so does nothing
    _.each(organizations, function(o) {
      if (o.name == name) {
        scope.recipients.push(o);
        //console.log(o);
        //draw the list in html
        var template = _.template($('#recipient').html(), { recipients: scope.recipients });
        scope.$('.recipient-container').html(template);
        valid = true; //indidates the recipient is found
      }
    });
    //prompt that the recipient is invalid
    if (!valid)
      scope.updateValidationMessage('Invalid recipient name: ' + name);
    else
      scope.updateValidationMessage('');

    //console.log(scope.recipients);

  },
  addRecipientById: function(id) {
    var scope = this;
    scope.updateValidationMessage('');
    if (typeof scope.recipients == 'undefined')
      scope.recipients = [];
    var organizations = scope.organizations;
    if (typeof organizations == 'undefined') {
      console.log('No recipient loaded from server');
      return;
    }
    var valid = false, exist = false;
    _.each(scope.recipients, function(r) {
      if (id == r.id) {
        exist = true;
        return;
      }
    });
    if (exist)
      return;//already exists so does nothing
    _.each(organizations, function(o) {
      if (o.id == id) {
        scope.recipients.push(o);
        //console.log(o);
        //draw the list in html
        var template = _.template($('#recipient').html(), { recipients: scope.recipients });
        scope.$('.recipient-container').html(template);
        valid = true; //indidates the recipient is found
      }
    });
    //prompt that the recipient is invalid
    if (!valid)
      scope.updateValidationMessage('Invalid recipient id: ' + id);
    else
      scope.updateValidationMessage('');

    //console.log(scope.recipients);

  },
  removeRecipient: function(name) {
    for (var i = 0; i < this.recipients.length; i++) {
      var r = this.recipients[i];
      if (name == r.name) {
        this.recipients.splice(i, 1);
      }
    }
    console.log(this.recipients);
  },
  updateValidationMessage: function(msg) {
    this.$('#validation-msg').text(msg);
  }


});