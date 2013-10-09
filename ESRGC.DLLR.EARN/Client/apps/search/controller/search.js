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
        closeTagBtn: '.tag a',
        pager: '.pagination',
        currentHiddenInput: 'input[name="tags"]',
        notificationLabel: '#statusText',
        searchResultTags: '#searchResult .tag',
        keywordTags: '#keywords .tag',
        pageContent: '#basicSearchPage'
    },
    control: {
        searchForm: {
            submit: 'onSearchSubmit'
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
                url: 'profile/tags',
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
        //store event handler
        var store = dx.getStore('Search');
        if (typeof store != 'undefined') {
            store.on('load', scope.onSearchStoreLoad);
        }
    },
    //intercept submit event to use ajax to load content
    onSearchSubmit: function (event, object) {
        var scope = this;
        event.preventDefault();
        var params = scope.getFormData($(object));
        dx.log(params)
        var store = dx.getStore('Search');
        if (typeof store != 'undefined') {
            store.setParams(params);
            
            dx.log(store.constructParams());
            store.loadContent();
        }
    },
    //////////////////////////////////////////////
    //store event handlers
    //////////////////////////////////////////////
    onSearchStoreLoad: function(store, data){
        var scope = dx.getController('Search');
        var pageContainer = scope.getPageContent();//get page container
        //replace content of page container with new content
        pageContainer.replaceWith($(scope.getRef('pageContent'), data));
    },
    //helpers
    tagExists: function (tag) {
        var tagArray = this.tagArray;
        for (var i = 0; i < tagArray.length; i++) {
            if (tagArray[i] == tag.toUpperCase())
                return true;
        }
        return false;
    }
    
});


