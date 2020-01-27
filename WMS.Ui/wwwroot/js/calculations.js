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

         var abv = CalcAlcohol(start, end, useBrix);

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

         var volume = $('#FortifyCalculator_Volume').val();
         var spirit_alchohol = $('#FortifyCalculator_SpiritReading').val() / 100;
         var wine_alchohol = $('#FortifyCalculator_CurrentReading').val() / 100;
         var target_alchohol = $('#FortifyCalculator_Goal').val() / 100;

         var needed = CalcFortifyAddition(volume, target_alchohol, wine_alchohol, spirit_alchohol);

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

         mg = measured_gravity;

         tr = temperature_reading; // needs to be Fahrenheit
         tc = calibration_temperature; // needs to be Fahrenheit
         if ($('input[name=optUomVolume]:checked', '#frmChaptalization').val() === 'metric') {
            tr = 9.0 / 5.0 * temperature_reading + 32;
            tc = 9.0 / 5.0 * calibration_temperature + 32;
         }

         var corrected_gravity;
         if ($('input[name=optUomSugar]:checked', '#frmChaptalization').val() === 'brix') {
            corrected_gravity = AdjustBrixForTemp(mg, tc, tr);
         } else {
            corrected_gravity = AdjustSpecificGravityForTemp(mg, tc, tr);
         }

         $('#GravityTempCalculator_CorrectedValue').val(formatForDisplay(corrected_gravity));

         $('#GravityTempCalculator_MeasuredGravity').val(formatForDisplay(measured_gravity));
         $('#GravityTempCalculator_TempReading').val(formatForDisplay(temperature_reading));
         $('#GravityTempCalculator_TempCalibrate').val(formatForDisplay(calibration_temperature));

      }

   });

   // calculate SO2 Dose button
   $('body').off('click', '#btnCalcSO2Dose');
   $('body').on('click', '#btnCalcSO2Dose', function (e) {

      event.preventDefault();
      var $form = $('#frmSO2Dose');
      if ($form.valid()) {

         var startingSO2 = $('#DoseSO2Calculator_CurrentReading').val();
         var endingSO2 = $('#DoseSO2Calculator_Goal').val();
         if (endingSO2 > 50) {
            endingSO2 = 50;
         }

         var liters;
         var gallons;
         if ($('input[name=optUomSO2DoseVolume]:checked', '#frmSO2Dose').val() === 'metric') {
            liters = $('#DoseSO2Calculator_Volume').val();
            gallons = LitersToGallons(liters);

         } else {
            gallons = $('#DoseSO2Calculator_Volume').val();
            var qrts = GallonsToQuarts(gallons);
            liters = QuartsToLiters(qrts);
         }

         if ($('input[name=optUomSO2DoseSolution]:checked', '#frmSO2Dose').val() === 'powder') {

            var gPerGallon = (endingSO2 - startingSO2) * 0.00657;
            var gPerLiter = gPerGallon * 0.2642;

            var gDoseForLiters = gPerLiter * liters;
            var gDoseForGallons = gPerGallon * gallons;

            if ($('input[name=optUomSO2DoseVolume]:checked', '#frmSO2Dose').val() === 'metric') {
               $('#DoseSO2Calculator_DoseRate').val(formatForDisplay(gPerLiter));
               $('#DoseSO2Calculator_DoseAmount').val(formatForDisplay(gDoseForLiters));
            } else {
               $('#DoseSO2Calculator_DoseRate').val(formatForDisplay(gPerGallon));
               $('#DoseSO2Calculator_DoseAmount').val(formatForDisplay(gDoseForGallons));
            }

         } else {
            var mlPerLiter = (endingSO2 - startingSO2) * 0.0175;
            var mlPerGallon = mlPerLiter * 3.378541;

            var mlDoseForLiters = mlPerLiter * liters;
            var mlDoseForGallons = mlPerGallon * gallons;

            if ($('input[name=optUomSO2DoseVolume]:checked', '#frmSO2Dose').val() !== 'metric') {
               $('#DoseSO2Calculator_DoseRate').val(formatForDisplay(mlPerLiter));
               $('#DoseSO2Calculator_DoseAmount').val(formatForDisplay(mlDoseForLiters));
            } else {
               $('#DoseSO2Calculator_DoseRate').val(formatForDisplay(mlPerGallon));
               $('#DoseSO2Calculator_DoseAmount').val(formatForDisplay(mlDoseForGallons));
            }

         }

      }

   });

   // calculate SO2 Dose button
   $('body').off('click', '#btnTitrateSO2');
   $('body').on('click', '#btnTitrateSO2', function (e) {

      event.preventDefault();
      var $form = $('#frmSO2Titrate');
      if ($form.valid()) {

         var normal = $('#TitrateSO2_Normal').val();
         var volNaOH = $('#TitrateSO2_VolumeNaOH').val();
         var volWine = $('#TitrateSO2_TestSize').val();

         var ppm = TitrateSO2(volWine, normal, volNaOH);

         $('#TitrateSO2_FreeSO2').val(ppm);

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

   // adjust labels by user choice SO2 Dose
   $('#frmSO2Dose').off('click', 'input[type=radio]');
   $('#frmSO2Dose').on('click', 'input[type=radio]', function (e) {

      if ($('input[name=optUomSO2DoseVolume]:checked', '#frmSO2Dose').val() === 'metric') {
         $('label[name=lblUomSO2DoseVolume]').text('Liters');

         if ($('input[name=optUomSO2DoseSolution]:checked', '#frmSO2Dose').val() === 'powder') {
            $('label[name=lblUomSO2DoseAmount]').text('Grams');
            $('label[name=lblUomSO2DoseRate]').text('Grams/Liter');
         } else {
            $('label[name=lblUomSO2DoseAmount]').text('ML');
            $('label[name=lblUomSO2DoseRate]').text('ML/Liter');
         }
      } else {
         $('label[name=lblUomSO2DoseVolume]').text('Gallons');

         if ($('input[name=optUomSO2DoseSolution]:checked', '#frmSO2Dose').val() === 'powder') {
            $('label[name=lblUomSO2DoseAmount]').text('Grams');
            $('label[name=lblUomSO2DoseRate]').text('Grams/Gallon');
         } else {
            $('label[name=lblUomSO2DoseAmount]').text('ML');
            $('label[name=lblUomSO2DoseRate]').text('ML/Gallon');
         }
      }

   });

   $('#frmSO2Dose').off('keyup', '#DoseSO2Calculator_pH');
   $('#frmSO2Dose').on('keyup', '#DoseSO2Calculator_pH', function (e) {
      var red = true;
      if ($('input[name=optSO2DoseVariety]:checked', '#frmSO2Dose').val() === 'white') {
         red = false;
      }

      var pH = $('#DoseSO2Calculator_pH').val();
      var target = CalcTargetSO2(pH, red);

      $('#DoseSO2Calculator_Goal').val(target);

   });


});

function DiluteSolution(strengthOfConcentrate, finalSolutionStrength, finalSolutionVolume) {
   var volumeOfConcentrateNeeded = (finalSolutionStrength * finalSolutionVolume) / strengthOfConcentrate;
   return volumeOfConcentrateNeeded;
}

function TitrateSO2(mL_Wine, N_NaOH, mL_NaOH) {
   var ppm_SO2;
   if (mL_Wine !== 0) {
      ppm_SO2 = (32000 * mL_NaOH * N_NaOH) / mL_Wine;
   }
   return ppm_SO2;
}

function CalcFortifyAddition(volume, targetAlchohol, wineAlchohol, spiritAlchohol) {
   var tmp = volume * (targetAlchohol - wineAlchohol) / (spiritAlchohol - targetAlchohol);
   var needed = tmp;
   return needed;
}

function AdjustSpecificGravityForTemp(readingSG, calabratedTemp, fahrenheitAtReading) {
   var adjustedSG = 0;
   if (Math.abs(calabratedTemp - fahrenheitAtReading) < 5.01) {
      adjustedSG = readingSG;
   } else {
      adjustedSG = readingSG * ((1.00130346 - (0.000134722124 * fahrenheitAtReading) + (0.00000204052596 * fahrenheitAtReading * fahrenheitAtReading) -
         (0.00000000232820948 * fahrenheitAtReading * fahrenheitAtReading * fahrenheitAtReading)) / (1.00130346 - (0.000134722124 * calabratedTemp) +
            (0.00000204052596 * calabratedTemp * calabratedTemp) - (0.00000000232820948 * calabratedTemp * calabratedTemp * calabratedTemp)));
   }

   return adjustedSG;
   // var newNum = Math.round(adjustedSG * 1000) / 1000;
   // return newNum.toLocaleString('en');
}

function AdjustBrixForTemp(readingBrix, calabratedTemp, fahrenheitAtReading) {
   var adjustedBrix = 0;

   if (Math.abs(calabratedTemp - fahrenheitAtReading) < 5.01) {
      adjustedBrix = readingBrix;
   } else {
      var degreesCorrection = 0;
      var tempDiffernece = fahrenheitAtReading - calabratedTemp;
      var originalBrix = readingBrix;

      degreesCorrection = 0.0000006907947565 * tempDiffernece * tempDiffernece * tempDiffernece * tempDiffernece +
         0.0000008650898228 * tempDiffernece * tempDiffernece * tempDiffernece * originalBrix + 0.0000002111610273 * tempDiffernece * tempDiffernece * originalBrix * originalBrix -
         0.000000420289855 * tempDiffernece * originalBrix * originalBrix * originalBrix + 3.388131789E-19 * tempDiffernece * tempDiffernece * tempDiffernece * tempDiffernece -
         0.00002646880494 * tempDiffernece * tempDiffernece * tempDiffernece - 0.00003812273795 * tempDiffernece * tempDiffernece * originalBrix +
         0.00002132555958 * tempDiffernece * originalBrix * originalBrix + 0.0000003140096619 * originalBrix * originalBrix * originalBrix +
         0.001470413886 * tempDiffernece * tempDiffernece + 0.0003854292164 * tempDiffernece * originalBrix -
         0.00001254869767 * originalBrix * originalBrix + 0.04799327348 * tempDiffernece + 0.0002013056055 * originalBrix - 0.002157758291;

      adjustedBrix = readingBrix + degreesCorrection;
   }

   return adjustedBrix;
   //var newNum = Math.round(adjustedBrix * 10) / 10;
   //return newNum.toLocaleString('en');

}

function CalcAlcohol(start, end, useBrix) {
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
   return 0;
}

function CalcTargetSO2(pH, red) {
   var target = 40;
   var whiteAdjustment = 10;
   if (red === true) {
      target = 35;
      whiteAdjustment = 0;
   }

   if (pH > 3) {
      target = (pH - 3) * 100;
      target = target + whiteAdjustment;
   }
   if (target > 50) {
      target = 50;
   }
   return target;
}

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

function GallonsToQuarts(gals) {
   try {
      var qrts = gals * 4;
      return qrts;
   } catch (err) {
      console.error(err);
   }
}

function QuartsToLiters(qrts) {
   try {
      var l = qrts * 0.946352946;
      return l;
   } catch (err) {
      console.error(err);
   }
}




