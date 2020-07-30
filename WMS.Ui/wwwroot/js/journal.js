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
   // TODO

   // score batch button click
   $('body').off('click', 'button[name="scoreBatchButton"]');
   $('body').on('click', 'button[name="scoreBatchButton"]', function () {
      var id = $(this).data('id');
      onScoreClick(id, '1', '2', '3', '4', '5', '6');
      alert(id);
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

