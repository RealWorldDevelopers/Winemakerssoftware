
var yeastPairs = getYeastPairs();

$(document).ready(function () {

    // yeast selected for modal
    $('body').off('click', 'li[name=yeastListItem]');
    $('body').on('click', 'li[name=yeastListItem]', function () {
        event.preventDefault();
        var el = $(this);

        var note = el.data('pnote');
        if (!note) {
            note = el.data('note');
        };
        onYeastClick(el.data('id'), el.data('title'), el.data('style'), el.data('tempmin'), el.data('tempmax'), el.data('alcohol'), note);
    })

    // filter on brand selected
    $('body').off('click', 'input[name=chkYeastBand]');
    $('body').on('click', 'input[name=chkYeastBand]', function () {
        filterYeast();
    })

    $('body').off('change', '#varietySelector');
    $('body').on('change', '#varietySelector', function () {
        filterYeast();
    });

});

function onYeastClick(id, title, style, tempMin, tempMax, alcohol, note) {
    // alert('it works' + id);
    document.getElementById("yeastModal_title").innerHTML = title;
    document.getElementById("yeastModal_style").innerHTML = style;
    document.getElementById("yeastModal_tempMin").innerHTML = tempMin;
    document.getElementById("yeastModal_tempMax").innerHTML = tempMax;
    document.getElementById("yeastModal_alcohol").innerHTML = alcohol;
    document.getElementById("yeastModal_note").innerHTML = note;
    $("#yeastModal").modal()
}

function getYeastPairs() {

    const uri = '/api/yeasts';

    $.ajax({
        url: rootUri + uri + '/pairs/',
        type: 'GET',
        contentType: 'application/json',
        success: function (data) {
            yeastPairs = data;
            //console.log('successful pairs call');
        },
        error: function (xmlHttpRequest, textStatus, errorThrown) { console.log('Yeast Pair Exception: ' + errorThrown); }
    });

}


function filterYeast() {

    // filter brands
    var checkboxes = $('input[name=chkYeastBand]');
    checkboxes.each(function () {
        var card = '#groupId-' + $(this).val();
        if (this.checked) {
            $(card).removeClass('d-none');
        } else {
            $(card).addClass('d-none');
        };
    });

    // get cat/var id to filter for
    varietyPickedId = parseInt($('#varietySelector').val());

    // if variety picked filter out variety
    if (varietyPickedId > 0) {

        //// get list of yeast to show
        var found_pairs = $.grep(yeastPairs, function (v) {
            return v.category === varietyPickedId || v.variety === varietyPickedId;
        });

        // hide all but on list to show;
        var yeastListed = $('li[name=yeastListItem]');
        yeastListed.each(function () {
            var yeastId = $(this).data('id');

            var found_yeast = $.grep(found_pairs, function (v) {
                return v.yeast === yeastId;
            });
            if (found_yeast.length > 0) {
                // show
                $(this).removeClass('d-none');

                // update note to pair note
                var newNote = found_yeast[0].display;
                if (!newNote) {
                    newNote = $(this).data('display');
                };
                $(this).data('pnote', found_yeast[0].note);
                $(this).find('p').text(newNote);

            } else {
                // hide
                $(this).addClass('d-none');

                // update note to default
                $(this).data('pnote', '');
                $(this).find('p').text($(this).data('display'));
            };

        });


    };

}