$(document).ready(function () {

    var pathname = window.location.pathname; // Returns path only no domain
    //alert(pathname + ' ready'); 

    // call hit counter api for recipe details pages
    // example for api call / recipes / recipe / 8
    if (pathname.match(/^\/recipes\/recipe\/(?:\d+)$/i)) {
        var match = pathname.match(/\d+/);
        UpdateHitCount(match, '', '');
    }

    // rating change
    $('body').off('change', "input[name = 'rating']");
    $('body').on('change', "input[name = 'rating']", function () {
        // alert('rating clicked');
        var userRating = $(this).val();
        var idx = $(this).parent().attr('id');
        UpdateStarValue(idx, userRating);
    });


    // file selection - show file value after file select 
    $('body').off('change', '.custom-file-input');
    $('body').on('change', '.custom-file-input', function () {
        let fileName = $(this).val().split('\\').pop();
        var label = $('label[for="' + $(this).attr('id') + '"]');
        label.addClass("selected").html(fileName);
    });

    // file selection - reset file value after reset button click 
    $('body').off('click', "button[type = 'reset']");
    $('body').on('click', "button[type = 'reset']", function () {
        $(".custom-file-label").html('Choose file to upload');
    });

    //search bar functions
    $('body').off('keypress', '#recipeSearch');
    $('body').on('keypress', '#recipeSearch', function (e) {
        return IsAlphaNumeric(e);
    });

    $('body').off('keyup', '#recipeSearch');
    $('body').on('keyup', '#recipeSearch', function (e) {
        filterRecipes(e);
    });

    $('body').off('paste', '#recipeSearch');
    $('body').on('paste', '#recipeSearch', function (e) {
        var element = this;
        setTimeout(function () {
            var text = $(element).val();
            $(element).val(text.replace(/[^ a-zA-Z0-9]/g, ''));
        }, 100);
    });


});


function filterRecipes() {
    var input, filter, ul, li, li2, a, i;
    input = document.getElementById('recipeSearch');
    filter = input.value.toUpperCase();

    var partial = '';
    var list = filter.split(' ');

    for (i = 0; i < list.length; i++) {
        if (list[i] != '') {
            var pre = partial;
            var word = '(?=.*\\b'.concat(list[i], '\\w*\\b)');
            partial = pre.concat(word);
        }
    };

    var pattern = partial.concat('.*');
    var regEx = new RegExp(pattern, 'i');

    ul = document.getElementById('ulRecipes');
    li = ul.getElementsByTagName('li');
    for (i = 1; i < li.length; i++) {

        var cls = li[i].classList;
        if (cls.contains('d-none')) {
            cls.remove('d-none')
        }

        var testData = li[i].textContent;
        testData = testData.replace(/(\W)/gm, ' x ');
        if (!regEx.test(testData)) {
            cls.add('d-none');
        }
    };
}


function UpdateHitCount(id, tokenName, tokenValue) {
    //alert('update hits value happened'); 
    const uri = '/api/recipes';
    var jwt = $('#HitCounterJwt').val();
    $.ajax({
        url: rootUri + uri + '/hits/' + id,
        type: 'PUT',
        headers: { "Authorization": 'Bearer ' + jwt },
        contentType: 'application/json',
        success: function (result) { console.log('successful hits call'); },
        error: function (xmlHttpRequest, textStatus, errorThrown) { console.log('Hits Exception: ' + errorThrown); }
    });
}


function UpdateStarValue(id, starValue) {
    // alert('update star value happened');
    const uri = '/api/recipes';
    var jwt = $('#RatingJwt').val();
    $.ajax({
        url: rootUri + uri + '/rating/' + id,
        type: 'PUT',
        headers: { "Authorization": 'Bearer ' + jwt },
        contentType: 'application/json',
        data: JSON.stringify(starValue),
        success: function (result) {
            console.log('successful rating update call');
        },
        error: function (xmlHttpRequest, textStatus, errorThrown) {
            console.log('Exception: ' + errorThrown);
        }

    });

}

