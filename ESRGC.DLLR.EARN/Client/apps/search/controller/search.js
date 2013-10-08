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
    tagArray: [],
    refs: {
        tagInput: '#tagInput',
        submitBtn: 'button[type="submit"]',
        searchForm: 'form',
        closeTagBtn: 'button[class="close"]',
        pager: '.pagination',
        currentHiddenInput: 'input[name="tags"]',
        notificationLabel: '#statusText',
        searchResultTags: '#searchResult .tag',
        keywordTags: '#keywords .tag'
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
        var scope = this;
        //call base class' constructor
        dx.app.controller.Search.parent.initialize.apply(this, arguments);
        dx.log('Search controller initialized');

        //get tag input and initialize type ahead
        var input = this.getTagInput();
        input.typeahead({
            name: 'tagSearch',
            prefetch: {
                url: '../profile/tags',
                ttl: 1
            },
            limit: 20
        });
        //modify pagination to small
        this.getPager().addClass('pagination-sm');
        //add current tags to tag array
        this.getCurrentHiddenInput().each(function (i) {
            scope.tagArray.push($(this).val());
        });
        this.highlightMatchedTags();
    },
    onSubmitBtnClick: function (event, object) {
        event.preventDefault();
        var scope = this;
        var inputValue = scope.getTagInput().val().toUpperCase();
        if (scope.tagExists(inputValue)) {
            scope.getNotificationLabel().text('Input tag has already been used for this search!');
            return;
        }
        if (inputValue == '') {
            scope.getNotificationLabel().text('Please enter a tag first!');
            return
        };
        dx.log(inputValue);
        scope.tagArray.push(inputValue);
        //submit form
        var form = scope.getSearchForm();
        form.append('<input type="hidden" value="' + inputValue + '" name="tags"/>');
        form.submit();
    },
    onCloseTagBtnClick: function (event, object) {
        dx.log($(object).html());
    },

    //helpers
    tagExists: function (tag) {
        var tagArray = this.tagArray;
        for (var i = 0; i < tagArray.length; i++) {
            if (tagArray[i] == tag.toUpperCase())
                return true;
        }
        return false;
    },
    highlightMatchedTags: function () {
        //highlight keywords
        this.getKeywordTags().addClass('matched-tag');
        //highlight matched tags in results
        for (var i in this.tagArray) {
            var value = this.tagArray[i];
            this.getSearchResultTags().each(function (index) {
                var tag = $(this).find('span').html();
                if (tag == value) {
                    $(this).addClass('matched-tag')
                }
            });
        }
    }
});


