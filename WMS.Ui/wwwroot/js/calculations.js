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
      var $form = $('#frmChaptalization');
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

   // calculate alcohol button
   $('body').off('click', '#btnCalcAlcohol');
   $('body').on('click', '#btnCalcAlcohol', function (e) {

      event.preventDefault();
      var $form = $('#frmAlcoholABV');
      if ($form.valid()) {
         var useBrix = false;
         if ($('input[name=optUomAlcohol]:checked', '#frmAlcoholABV').val() === 'brix') {
            useBrix = true;
         }

         var start = $('#AlcoholCalculator_SugarStart').val();
         var end = $('#AlcoholCalculator_SugarEnd').val();

         var abv;
         if (useBrix) {
            // brix
            var init = start * 0.55 - 0.63;
            var final = end * 0.55 - 0.63;
            abv = init - final;
         } else {
            //SG
            abv = Math.round(((start - end) * 131.25) * 10) / 10;
         }

         $('#AlcoholCalculator_Abv').val(formatForDisplay(abv));

         $('#AlcoholCalculator_SugarStart').val(formatForDisplay(start));
         $('#AlcoholCalculator_SugarEnd').val(formatForDisplay(end));

      }

   });

   // calculate fortify button
   $('body').off('click', '#btnCalcFortify');
   $('body').on('click', '#btnCalcFortify', function (e) {

      event.preventDefault();
      var $form = $('#frmFortify');
      if ($form.valid()) {
         var metricTransform = 3.7854;
         if ($('input[name=optUomFortify]:checked', '#frmFortify').val() === 'metric') {
            metricTransform = 1;
         }

         var volume = $('#FortifyCalculator_Volume').val();
         var spirit_alchohol = $('#FortifyCalculator_SpiritReading').val() / 100;
         var wine_alchohol = $('#FortifyCalculator_CurrentReading').val() / 100;
         var target_alchohol = $('#FortifyCalculator_Goal').val() / 100;

         var tmp4 = volume * metricTransform * (target_alchohol - wine_alchohol) / (spirit_alchohol - target_alchohol);
         var needed = tmp4 / metricTransform;

         $('#FortifyCalculator_Spirit').val(formatForDisplay(needed));

         $('#FortifyCalculator_Volume').val(formatForDisplay(volume));
         $('#FortifyCalculator_SpiritReading').val(formatForDisplay(spirit_alchohol * 100));
         $('#FortifyCalculator_CurrentReading').val(formatForDisplay(wine_alchohol * 100));
         $('#FortifyCalculator_Goal').val(formatForDisplay(target_alchohol * 100));

      }

   });

   // calculate gravity temp button
   $('body').off('click', '#btnCalcGravityTemp');
   $('body').on('click', '#btnCalcGravityTemp', function (e) {

      event.preventDefault();
      var $form = $('#frmGravityTemp');
      if ($form.valid()) {

         var measured_gravity = $('#GravityTempCalculator_MeasuredGravity').val();
         var temperature_reading = $('#GravityTempCalculator_TempReading').val();
         var calibration_temperature = $('#GravityTempCalculator_TempCalibrate').val();

         mg = measured_gravity; // needs to be SG
         if ($('input[name=optUomSugar]:checked', '#frmChaptalization').val() === 'brix') {
            mg = (measured_gravity / (258.6 - ((measured_gravity / 258.2) * 227.1)) + 1);
         }

         tr = temperature_reading; // needs to be Fahrenheit
         tc = calibration_temperature; // needs to be Fahrenheit
         if ($('input[name=optUomVolume]:checked', '#frmChaptalization').val() === 'metric') {
            tr = 9.0 / 5.0 * temperature_reading + 32;
            tc = 9.0 / 5.0 * calibration_temperature + 32;
         }

         var corrected_gravity = mg * ((1.00130346 - 0.000134722124 * tr + 0.00000204052596 * tr * tr - 0.00000000232820948 * tr * tr * tr) /
            (1.00130346 - 0.000134722124 * tc + 0.00000204052596 * tc * tc - 0.00000000232820948 * tc * tc * tc));

         var cg = corrected_gravity;
         if ($('input[name=optUomSugar]:checked', '#frmChaptalization').val() === 'brix') {
            cg = ((corrected_gravity - 1) * 220) + 1.6;
         }

         $('#GravityTempCalculator_CorrectedValue').val(formatForDisplay(cg));

         $('#GravityTempCalculator_MeasuredGravity').val(formatForDisplay(measured_gravity));
         $('#GravityTempCalculator_TempReading').val(formatForDisplay(temperature_reading));
         $('#GravityTempCalculator_TempCalibrate').val(formatForDisplay(calibration_temperature));

      }

   });








   // adjust labels by user choice Chaptalization
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

   // adjust labels by user choice AlcoholABV
   $('#frmAlcoholABV').off('click', 'input[type=radio]');
   $('#frmAlcoholABV').on('click', 'input[type=radio]', function (e) {
      if ($('input[name=optUomAlcohol]:checked', '#frmAlcoholABV').val() === 'brix') {
         $('label[name=lblUomABV]').text('Brix');
      } else {
         $('label[name=lblUomABV]').text('SG');
      }

   });

   // adjust labels by user choice Fortify
   $('#frmFortify').off('click', 'input[type=radio]');
   $('#frmFortify').on('click', 'input[type=radio]', function (e) {
      if ($('input[name=optUomFortify]:checked', '#frmFortify').val() === 'metric') {
         $('label[name=lblUomFortify]').text('Liters');
      } else {
         $('label[name=lblUomFortify]').text('Gallons');
      }

   });

   // adjust labels by user choice Gravity Temp
   $('#frmGravityTemp').off('click', 'input[type=radio]');
   $('#frmGravityTemp').on('click', 'input[type=radio]', function (e) {
      if ($('input[name=optUomGravityTemp]:checked', '#frmGravityTemp').val() === 'brix') {
         $('label[name=lblUomGravitySugar]').text('Brix');
      } else {
         $('label[name=lblUomGravitySugar]').text('SG');
      }

      if ($('input[name=optGravityMetric]:checked', '#frmGravityTemp').val() === 'metric') {
         $('label[name=lblUomGravityTemp]').text('°C');
      } else {
         $('label[name=lblUomGravityTemp]').text('°F');
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



