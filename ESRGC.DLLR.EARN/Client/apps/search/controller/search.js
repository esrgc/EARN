/*
Author: Tu hoang
ESRGC 2013

EARN Maryland Connect

Search controller
search.js

controller for profile searching by tags

Dependencies
dx library
*/

dx.defineController('Search', {
    refs: {
        tagInput: '#tagInput',
        submitBtn: 'button[type="submit"]',
        searchForm: 'form',
        closeTagBtn: 'button[class="close"]'
    },
    control: {
        submitBtn: {
            click: 'onSubmitBtnClick'
        },
        closeTagBtn: {
            click: 'onCloseTagBtnClick'
        }
    },
    initialize: function () {
        //call base class' constructor
        dx.app.controller.Search.parent.initialize.apply(this, arguments);
        dx.log('search constructor');

        //get tag input and initialize type ahead
        var input = this.getTagInput();
        input.typeahead({
            name: 'test',
            prefetch: {
                url: '../profile/tags',
                ttl: 1
            },
            limit: 20
        });
    },
    onSubmitBtnClick: function (event, object) {
        event.preventDefault();
        var scope = this;
        var inputValue = scope.getTagInput().val().toUpperCase();
        dx.log(inputValue);

        var form = scope.getSearchForm();
        form.append('<input type="hidden" value="' + inputValue + '" name="tags"/>');
        form.submit();
    },
    onCloseTagBtnClick: function (event, object) {
        dx.log($(object).html());
    }
});


