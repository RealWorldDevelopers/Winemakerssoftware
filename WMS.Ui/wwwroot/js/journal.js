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



});

