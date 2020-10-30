$(document).ready(function () {

  

   // all inputs numeric only
   $('body').off('keypress', '#batchInfo input');
   $('body').on('keypress', '#batchInfo input', function (e) {

      var currentValue = $(this).val();
      var charCode = e.which ? e.which : e.keyCode;

      // valid numeric
      if (charCode > 31 && (charCode === 47 || charCode < 45 || charCode > 57))
         return false;

      // only one decimal
      var decimalCount = 1 + currentValue.indexOf('.');
      if (charCode === 46 && decimalCount > 0)
         return false;

   });

   // all inputs paste events numeric only
   $('body').off('paste', '#batchInfo :input');
   $('body').on('paste', '#batchInfo :input', function () {
      var $this = $(this);
      setTimeout(function () {
         if (isNaN(parseFloat($this.val()))) {
            showAlert('Only Numerical Characters allowed !', cssAlert_Warning, true);
            setTimeout(function () {
               $this.val(null);
            }, 2500);
         }
      }, 5);
   });

   // all inputs numeric only
   $('body').off('keypress', '#batchTargets input');
   $('body').on('keypress', '#batchTargets input', function (e) {

      var currentValue = $(this).val();
      var charCode = e.which ? e.which : e.keyCode;

      // valid numeric
      if (charCode > 31 && (charCode === 47 || charCode < 45 || charCode > 57))
         return false;

      // only one decimal
      var decimalCount = 1 + currentValue.indexOf('.');
      if (charCode === 46 && decimalCount > 0)
         return false;

   });

   // all inputs paste events numeric only
   $('body').off('paste', '#batchTargets :input');
   $('body').on('paste', '#batchTargets :input', function () {
      var $this = $(this);
      setTimeout(function () {
         if (isNaN(parseFloat($this.val()))) {
            showAlert('Only Numerical Characters allowed !', cssAlert_Warning, true);
            setTimeout(function () {
               $this.val(null);
            }, 2500);
         }
      }, 5);
   });

   // detailed batch edit
   $('body').off('click', 'button[name="editBatchDetailsButton"]');
   $('body').on('click', 'button[name="editBatchDetailsButton"]', function () {
      var id = $(this).data('id');
      $('#editBatchId').val(id);
      $('#frmEditBatch').submit();
   });

   // quick edit batch
   $('body').off('click', 'button[name="editBatchQuickButton"]');
   $('body').on('click', 'button[name="editBatchQuickButton"]', function () {
      var id = $(this).data('id');
      showAddEntry(id);   
   });

   // score batch button click
   $('body').off('click', 'button[name="scoreBatchButton"]');
   $('body').on('click', 'button[name="scoreBatchButton"]', function () {
      var id = $(this).data('id');
      onScoreClick(id, '1', '2', '3', '4', '5', '6');     
   });

});

function onScoreClick(id, title, style, tempMin, tempMax, alcohol, note) {
   // alert('it works' + id);
   document.getElementById("scoreBatchModal_title").innerHTML = title;
   document.getElementById("scoreBatchModal_style").innerHTML = style;
   document.getElementById("scoreBatchModal_tempMin").innerHTML = tempMin;
   document.getElementById("scoreBatchModal_tempMax").innerHTML = tempMax;
   document.getElementById("scoreBatchModal_alcohol").innerHTML = alcohol;
   document.getElementById("scoreBatchModal_note").innerHTML = note;
   $("#scoreBatchModal").modal();
}

function showAddEntry(id) {
   $("#batchEntryModal_id").val(id);
   $("#batchEntryModal").modal();
}

function addBatchEntry(id, starValue, tokenName, tokenValue) {
   // alert('update star value happened');
   const uri = '/api/recipes';
   var jwt = $('#RatingJwt').val();
   $.ajax({
      url: rootUri + uri + '/rating/' + id,
      type: 'PUT',
      headers: { "Authorization": 'Bearer ' + jwt },
      contentType: 'application/json',
      data: JSON.stringify({ 'starValue': starValue }),
      success: function (result) { console.log('successful rating update call'); },
      error: function (xmlHttpRequest, textStatus, errorThrown) { console.log('Exception: ' + errorThrown); }

   });

}
