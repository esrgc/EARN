/*
Author: Tu hoang
ESRGC 2013

EARN MD CONNECT

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
        goBtn: 'button[type="submit"]',
        searchForm: 'form#search',
        orgTypeDropdown: '#orgType',
        pageLinks: '.tag a, a#clearSearchLink, .pagination a',
        pager: '.pagination',
        currentHiddenInput: 'input[type="hidden"][name="tags"]',
        hiddenInputHolder: '#hiddenInputHolder',
        notificationLabel: '#statusText',
        searchResult: '#searchResult',
        keywordTags: '#keywords .tag',
        pageContent: '#basicSearchPage',
        queryTagHolder: '.tag-list-holder',
        typeahead: '.tt-hint'
    },
    control: {
        searchForm: {
            submit: 'onSearchSubmit'
        },
        pageLinks: {
            click: 'onPageLinksClick'
        },
        orgTypeDropdown: {
            change: 'onOrgTypeChange'
        }
    },
    initialize: function () {
        var scope = this;
        //call base class' constructor
        dx.app.controller.Search.parent.initialize.apply(this, arguments);

        //get tag input and initialize type ahead
        var input = this.getTagInput();
        input.typeahead({
            name: 'tagSearch',
            prefetch: {
                url: '../tag/tags',
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
            store.on('beforeLoad', function () { scope.preloadState(); });
            store.on('load', scope.onSearchStoreLoad);
        }
    },
    onPageLinksClick: function (event, object) {
        event.preventDefault();
        var url = $(object).attr('href');
        if (typeof url == 'undefined')
            return;
        var store = dx.getStore('Search');
        if (typeof store != 'undefined') {
            //dx.log(url);
            store.loadContentUrl(url);
        }
    },
    //intercept submit event to use ajax to load content
    onSearchSubmit: function (event, object) {
        event.preventDefault();

        var scope = this;
        scope.updateStatus('');
        //get form parameters
        var params = scope.getFormData($(object));
        var input = scope.getTagInput().val().toUpperCase();
        if (input != '') {
            //scope.updateStatus('Please enter a tag first!');
            //return;
            if (scope.tagExists(input)) {
                scope.updateStatus('This tag has already been used for this search');
                return;
            }
            else
                scope.tagArray.push(input);
        }
        else {
            dx.log(params);
            if (typeof params != 'undefined')
                scope.removeEmptyTags(params);

        }

        //dx.log(params)
        var store = dx.getStore('Search');
        if (typeof store != 'undefined') {
            store.setParams(params);

            //dx.log(store.constructParams());
            store.loadContent();
        }
    },
    onOrgTypeChange: function (event, object) {
        var scope = this;
        var searchForm = scope.getSearchForm();
        var tagInput = scope.getTagInput();
        if (tagInput.val() != '')
            tagInput.val('');
        if (typeof searchForm != 'undefined')
            searchForm.submit();
    },
    //////////////////////////////////////////////
    //store event handlers
    //////////////////////////////////////////////
    onSearchStoreLoad: function (store, data) {
        var scope = dx.getController('Search');
        //var pageContainer = scope.getPageContent();//get page container
        //replace content of page container with new content
        //pageContainer.replaceWith($(scope.getRef('pageContent'), data));
        var searchResult = scope.getSearchResult();
        var queryTagHolder = scope.getQueryTagHolder();
        var hiddenInputHolder = scope.getHiddenInputHolder();
        var orgTypeDropdown = scope.getOrgTypeDropdown();

        //replace with new content
        searchResult.replaceWith(scope.getSearchResult(data));
        queryTagHolder.replaceWith(scope.getQueryTagHolder(data));
        hiddenInputHolder.replaceWith(scope.getHiddenInputHolder(data));
        orgTypeDropdown.replaceWith(scope.getOrgTypeDropdown(data));
        //empty input box
        //scope.getTagInput().focus();
        scope.getTagInput().val('')
        scope.getTypeahead().val('');
        //set state to loaded
        scope.loadedState();
        scope.updateTagList();

        if (typeof this.events.searchStoreLoaded == 'function')
            this.events.searchStoreLoaded.apply(store, arguments);//call in store scope
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
    removeEmptyTags: function (params) {
        var tags = params.tags;
        if (tags == '') {
            delete params.tags;
            return;
        }
        for (var i in tags) {
            if (tags[i] == '')
                delete tags[i];
        }
    },
    updateStatus: function (msg) {
        this.getNotificationLabel().text(msg);
    },
    preloadState: function () {
        //dx.log('pre-load');
        this.getGoBtn().button('loading');
    },
    loadedState: function () {
        //dx.log('loaded');
        this.getGoBtn().button('reset');
    },
    updateTagList: function () {
        var currentTags = this.getCurrentHiddenInput();
        this.tagArray = [];
        var tagArray = this.tagArray;
        currentTags.each(function (i, tag) {
            var tagName = $(tag).val().toUpperCase();
            dx.log(tagName);
            tagArray.push(tagName);
        });
    }

});


