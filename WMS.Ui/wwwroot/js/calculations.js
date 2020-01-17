$(document).ready(function () {

   // all inputs keypress events numeric only
   $('body').off('keypress', '.card-body input');
   $('body').on('keypress', '.card-body input', function (e) {

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

   // filter calculators   
   $('body').off('click', 'input[name=chkCalcGroup]');
   $('body').on('click', 'input[name=chkCalcGroup]', function () {
      var checkboxes = $('input[name=chkCalcGroup]');
      checkboxes.each(function () {
         var card = '.card[name="calc' + $(this).val() + '"]';
         if (this.checked) {
            $(card).removeClass('d-none');
         } else {
            $(card).addClass('d-none');
         }
      });
   });

   // calculate sugar button
   $('body').off('click', '#btnCalcSugar');
   $('body').on('click', '#btnCalcSugar', function (e) {

      event.preventDefault();
      var $form = $('form');
      if ($form.valid()) {        
         var useMetric = false;
         var useBrix = false;
         if ($('input[name=optUomSugar]:checked', '#frmChaptalization').val() === 'brix') {
            useBrix = true;
         }
         if ($('input[name=optUomVolume]:checked', '#frmChaptalization').val() === 'metric') {
            useMetric = true;
         }
         var start = $('#ChaptalizationCalculator_CurrentReading').val();
         var end = $('#ChaptalizationCalculator_Goal').val();
         var vol = $('#ChaptalizationCalculator_Volume').val();
         
         var gal = vol;
         if (useMetric) {
            gal = LitersToGallons(vol);
         }

         var sugar = CalcSugar(start, end, gal, useBrix);
         $('#ChaptalizationCalculator_Sugar').val(formatForDisplay(sugar));

         $('#ChaptalizationCalculator_CurrentReading').val(formatForDisplay(start));
         $('#ChaptalizationCalculator_Goal').val(formatForDisplay(end));
         $('#ChaptalizationCalculator_Volume').val(formatForDisplay(vol));
      }

   });

   // adjust labels by user choice
   $('#frmChaptalization').off('click', 'input[type=radio]');
   $('#frmChaptalization').on('click', 'input[type=radio]', function (e) {
      if ($('input[name=optUomSugar]:checked', '#frmChaptalization').val() === 'brix') {
         $('label[name=lblUomSugar]').text('Brix');
      } else {
         $('label[name=lblUomSugar]').text('SG');
      }

      if ($('input[name=optUomVolume]:checked', '#frmChaptalization').val() === 'metric') {
         $('label[name=lblUomVolume]').text('Liters');
         $('label[name=lblUomWeight]').text('KiloGrams');
      } else {
         $('label[name=lblUomVolume]').text('Gallons');
         $('label[name=lblUomWeight]').text('Pounds');
      }

   });

});



function CalcSugar(starting, ending, volume, useBrix) {
   try {
      var startingSugar = GetSugarContent(starting, useBrix) * volume;
      var endingingSugar = GetSugarContent(ending, useBrix) * volume;
      var result = endingingSugar - startingSugar;

      return result;
   } catch (err) {
      console.error(err);
   }
}

function GetSugarContent(reading, useBrix) {
   var ounces = 37;
   try {
      if (useBrix) {
         if (reading < 25) { ounces = 35; }
         if (reading < 24) { ounces = 33; }
         if (reading < 23) { ounces = 32; }
         if (reading < 22) { ounces = 30; }
         if (reading < 21) { ounces = 27; }
         if (reading < 20) { ounces = 26; }
         if (reading < 18.5) { ounces = 24; }
         if (reading < 17.5) { ounces = 23; }
         if (reading < 16.5) { ounces = 21; }
         if (reading < 15) { ounces = 19; }
         if (reading < 14) { ounces = 18; }
         if (reading < 12.5) { ounces = 16; }
         if (reading < 11.5) { ounces = 14; }
         if (reading < 10) { ounces = 12; }
      } else {
         if (reading < 1.105) { ounces = 35; }
         if (reading < 1.1) { ounces = 33; }
         if (reading < 1.095) { ounces = 32; }
         if (reading < 1.09) { ounces = 30; }
         if (reading < 1.085) { ounces = 27; }
         if (reading < 1.08) { ounces = 26; }
         if (reading < 1.075) { ounces = 24; }
         if (reading < 1.07) { ounces = 23; }
         if (reading < 1.065) { ounces = 21; }
         if (reading < 1.06) { ounces = 19; }
         if (reading < 1.055) { ounces = 18; }
         if (reading < 1.055) { ounces = 16; }
         if (reading < 1.045) { ounces = 14; }
         if (reading < 1.04) { ounces = 12; }
      }

      var result = ounces / 16;
      return result;

   } catch (err) {
      console.error(err);
   }
}

function formatForDisplay(num) {
   try {
      var newNum = Math.round(num * 1000) / 1000;
      return newNum.toLocaleString('en');
   } catch (err) {
      console.error(err);
   }
}

function LitersToGallons(l) {
   try {
      var gals = l * 0.264172052;
      return gals;
   } catch (err) {
      console.error(err);
   }
}



