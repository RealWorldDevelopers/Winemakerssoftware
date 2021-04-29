
$(document).ready(function () {

    $('body').off('show.bs.modal', '#aboutModal');
    $('body').on('show.bs.modal', '#aboutModal', function (e) {

        var button = $(e.relatedTarget);
        var modal = $(this);
        modal.find('.modal-body').load(button.data("remote"));

    });

}); 