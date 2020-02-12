var BrixDigitsDisplayed = 1;
var SGDigitsDisplayed = 3;

var ImperialVolumeDisplayed = 3;
var MetricVolumeDisplayed = 3;

var ImperialMassDisplayed = 2;
var MetricMassDisplayed = 3;

var FahrenheitDigitDisplayed = 1;
var CelsiusDigitDisplayed = 1;

var NormalityDisplayed = 4;

var PpmDigitDisplayed = 0;

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

         var sugarDigitsDisplay = SGDigitsDisplayed;
         var volDigitsDisplay = ImperialVolumeDisplayed;
         var massDigitDisplay = ImperialMassDisplayed;
         var useMetric = false;
         var useBrix = false;
         if ($('input[name=optUomSugar]:checked', '#frmChaptalization').val() === 'brix') {
            useBrix = true;
            sugarDigitsDisplay = BrixDigitsDisplayed;
         }
         if ($('input[name=optUomVolume]:checked', '#frmChaptalization').val() === 'metric') {
            useMetric = true;
            volDigitsDisplay = MetricVolumeDisplayed;
         }
         var start = $('#CurrentReading').val();
         var end = $('#Goal').val();
         var vol = $('#Volume').val();

         var gal = vol;
         if (useMetric) {
            gal = LitersToGallons(vol);
         }

         var sugar = CalcSugar(start, end, gal, useBrix);

         $('#Sugar').val(formatNumericForDisplay(sugar, massDigitDisplay, false));

         $('#CurrentReading').val(formatNumericForDisplay(start, sugarDigitsDisplay, true));
         $('#Goal').val(formatNumericForDisplay(end, sugarDigitsDisplay, true));
         $('#Volume').val(formatNumericForDisplay(vol, volDigitsDisplay, false));

      }

   });

   // calculate alcohol button
   $('body').off('click', '#btnCalcAlcohol');
   $('body').on('click', '#btnCalcAlcohol', function (e) {

      event.preventDefault();
      var $form = $('#frmAlcoholABV');
      if ($form.valid()) {
         var sugarDigitsDisplay = SGDigitsDisplayed;
         var useBrix = false;
         if ($('input[name=optUomAlcohol]:checked', '#frmAlcoholABV').val() === 'brix') {
            useBrix = true;
            sugarDigitsDisplay = BrixDigitsDisplayed;
         }

         var start = $('#AlcoholCalculator_SugarStart').val();
         var end = $('#AlcoholCalculator_SugarEnd').val();

         var abv = CalcAlcohol(start, end, useBrix);

         $('#AlcoholCalculator_Abv').val(formatNumericForDisplay(abv, 2, false));

         $('#AlcoholCalculator_SugarStart').val(formatNumericForDisplay(start, sugarDigitsDisplay, true));
         $('#AlcoholCalculator_SugarEnd').val(formatNumericForDisplay(end, sugarDigitsDisplay, true));

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

         $('#FortifyCalculator_Spirit').val(formatNumericForDisplay(needed, 2, false));

         $('#FortifyCalculator_Volume').val(formatNumericForDisplay(volume, 2, false));
         $('#FortifyCalculator_SpiritReading').val(formatNumericForDisplay(spirit_alchohol * 100, 2, false));
         $('#FortifyCalculator_CurrentReading').val(formatNumericForDisplay(wine_alchohol * 100, 2, false));
         $('#FortifyCalculator_Goal').val(formatNumericForDisplay(target_alchohol * 100, 2, false));

      }

   });

   // calculate gravity temp button
   $('body').off('click', '#btnCalcGravityTemp');
   $('body').on('click', '#btnCalcGravityTemp', function (e) {

      event.preventDefault();
      var $form = $('#frmGravityTemp');
      if ($form.valid()) {

         var sugarDigitsDisplay = SGDigitsDisplayed;
         var tempDigitsDisplay = FahrenheitDigitDisplayed;

         var measured_gravity = $('#GravityTempCalculator_MeasuredGravity').val();
         var temperature_reading = $('#GravityTempCalculator_TempReading').val();
         var calibration_temperature = $('#GravityTempCalculator_TempCalibrate').val();

         mg = measured_gravity;

         tr = temperature_reading; // needs to be Fahrenheit
         tc = calibration_temperature; // needs to be Fahrenheit
         if ($('input[name=optGravityMetric]:checked', '#frmGravityTemp').val() === 'metric') {
            tr = 9.0 / 5.0 * temperature_reading + 32;
            tc = 9.0 / 5.0 * calibration_temperature + 32;
            tempDigitsDisplay = CelsiusDigitDisplayed;
         }

         var corrected_gravity;
         if ($('input[name=optUomGravityTemp]:checked', '#frmGravityTemp').val() === 'brix') {
            corrected_gravity = AdjustBrixForTemp(mg, tc, tr);
            sugarDigitsDisplay = BrixDigitsDisplayed;
         } else {
            corrected_gravity = AdjustSpecificGravityForTemp(mg, tc, tr);
         }


         $('#GravityTempCalculator_CorrectedValue').val(formatNumericForDisplay(corrected_gravity, sugarDigitsDisplay, true));

         $('#GravityTempCalculator_MeasuredGravity').val(formatNumericForDisplay(measured_gravity, sugarDigitsDisplay, true));
         $('#GravityTempCalculator_TempReading').val(formatNumericForDisplay(temperature_reading, tempDigitsDisplay, false));
         $('#GravityTempCalculator_TempCalibrate').val(formatNumericForDisplay(calibration_temperature, tempDigitsDisplay, false));

      }

   });

   // calculate SO2 Titrate button
   $('body').off('click', '#btnTitrateSO2');
   $('body').on('click', '#btnTitrateSO2', function (e) {

      event.preventDefault();
      var $form = $('#frmSO2Titrate');
      if ($form.valid()) {

         var normal = $('#TitrateSO2_Normal').val();
         var volNaOH = $('#TitrateSO2_VolumeNaOH').val();
         var volWine = $('#TitrateSO2_TestSize').val();

         var ppm = TitrateSO2(volWine, normal, volNaOH);

         $('#TitrateSO2_FreeSO2').val(formatNumericForDisplay(ppm, PpmDigitDisplayed, false));

         $('#TitrateSO2_Normal').val(formatNumericForDisplay(normal, NormalityDisplayed, false));
         $('#TitrateSO2_VolumeNaOH').val(formatNumericForDisplay(volNaOH, MetricVolumeDisplayed, false));
         $('#TitrateSO2_TestSize').val(formatNumericForDisplay(volWine, MetricVolumeDisplayed, false));
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
               $('#DoseSO2Calculator_DoseRate').val(formatNumericForDisplay(gPerLiter, MetricMassDisplayed, false));
               $('#DoseSO2Calculator_DoseAmount').val(formatNumericForDisplay(gDoseForLiters, MetricMassDisplayed, false));

               $('#DoseSO2Calculator_Volume').val(formatNumericForDisplay(liters, MetricVolumeDisplayed, false));

            } else {
               $('#DoseSO2Calculator_DoseRate').val(formatNumericForDisplay(gPerGallon, MetricMassDisplayed, false));
               $('#DoseSO2Calculator_DoseAmount').val(formatNumericForDisplay(gDoseForGallons, MetricMassDisplayed, false));

               $('#DoseSO2Calculator_Volume').val(formatNumericForDisplay(gallons, ImperialVolumeDisplayed, false));
            }

         } else {
            var mlPerLiter = (endingSO2 - startingSO2) * 0.0175;
            var mlPerGallon = mlPerLiter * 3.378541;

            var mlDoseForLiters = mlPerLiter * liters;
            var mlDoseForGallons = mlPerGallon * gallons;

            if ($('input[name=optUomSO2DoseVolume]:checked', '#frmSO2Dose').val() === 'metric') {
               $('#DoseSO2Calculator_DoseRate').val(formatNumericForDisplay(mlPerLiter, MetricVolumeDisplayed, false));
               $('#DoseSO2Calculator_DoseAmount').val(formatNumericForDisplay(mlDoseForLiters, MetricVolumeDisplayed, false));

               $('#DoseSO2Calculator_Volume').val(formatNumericForDisplay(liters, MetricVolumeDisplayed, false));

            } else {
               $('#DoseSO2Calculator_DoseRate').val(formatNumericForDisplay(mlPerGallon, MetricVolumeDisplayed, false));
               $('#DoseSO2Calculator_DoseAmount').val(formatNumericForDisplay(mlDoseForGallons, MetricVolumeDisplayed, false));

               $('#DoseSO2Calculator_Volume').val(formatNumericForDisplay(gallons, ImperialVolumeDisplayed, false));
            }

         }

         $('#DoseSO2Calculator_CurrentReading').val(formatNumericForDisplay(startingSO2, PpmDigitDisplayed, true));
         $('#DoseSO2Calculator_Goal').val(formatNumericForDisplay(endingSO2, PpmDigitDisplayed, true));

      }

   });


   // calculate Dilute Solution button
   $('body').off('click', '#btnCalcDilute');
   $('body').on('click', '#btnCalcDilute', function (e) {

      event.preventDefault();
      var $form = $('#frmDilute');
      if ($form.valid()) {
         var concetration = $('#DiluteSolution_StrengthOfConcentrate').val();
         var finalStrength = $('#DiluteSolution_FinalSolutionStrength').val();
         var finalVolume = $('#DiluteSolution_FinalSolutionVolume').val();

         var needed = DiluteSolution(concetration, finalStrength, finalVolume);

         $('#DiluteSolution_VolumeOfConcentrateNeeded').val(formatNumericForDisplay(needed, 4, false));

         $('#DiluteSolution_StrengthOfConcentrate').val(formatNumericForDisplay(concetration, NormalityDisplayed, false));
         $('#DiluteSolution_FinalSolutionStrength').val(formatNumericForDisplay(finalStrength, NormalityDisplayed, false));
         $('#DiluteSolution_FinalSolutionVolume').val(formatNumericForDisplay(finalVolume, 4, false));

      }
   });

   // calculate Titrate NaOH button
   $('body').off('click', '#btnTitrateNaOH');
   $('body').on('click', '#btnTitrateNaOH', function (e) {

      event.preventDefault();
      var $form = $('#frmTitrateNaOH');
      if ($form.valid()) {
         var KaPhVolume = $('#TitrateNaOH_KaPhVolume').val();
         var KaPhNormal = $('#TitrateNaOH_KaPhNormal').val();
         var NaOHVolume = $('#TitrateNaOH_NaOHVolume').val();

         var n = CalcNofNaOH(KaPhVolume, KaPhNormal, NaOHVolume);

         $('#TitrateNaOH_NaOHNormal').val(formatNumericForDisplay(n, NormalityDisplayed, false));

         $('#TitrateNaOH_KaPhVolume').val(formatNumericForDisplay(KaPhVolume, MetricVolumeDisplayed, false));
         $('#TitrateNaOH_KaPhNormal').val(formatNumericForDisplay(KaPhNormal, NormalityDisplayed, false));
         $('#TitrateNaOH_NaOHVolume').val(formatNumericForDisplay(NaOHVolume, MetricVolumeDisplayed, false));
      }
   });

   // calculate Titrate Acid button
   $('body').off('click', '#btnTitrateAcid');
   $('body').on('click', '#btnTitrateAcid', function (e) {

      event.preventDefault();
      var $form = $('#frmTitrateAcid');
      if ($form.valid()) {
         var mustVolume = $('#TitrateAcid_MustVolume').val();
         var NaOHVolume = $('#TitrateAcid_NaOHVolume').val();
         var NaOHNormal = $('#TitrateAcid_NaOHNormal').val();

         var ppm = CalcPpmAcid(mustVolume, NaOHNormal, NaOHVolume);

         $('#TitrateAcid_TotalAcid').val(formatNumericForDisplay(ppm, PpmDigitDisplayed, true));

         $('#TitrateAcid_MustVolume').val(formatNumericForDisplay(mustVolume, MetricVolumeDisplayed, false));
         $('#TitrateAcid_NaOHVolume').val(formatNumericForDisplay(NaOHVolume, MetricVolumeDisplayed, false));
         $('#TitrateAcid_NaOHNormal').val(formatNumericForDisplay(NaOHNormal, NormalityDisplayed, false));
      }
   });

   // calculate Adjust Acid button
   $('body').off('click', '#btnAdjustAcid');
   $('body').on('click', '#btnAdjustAcid', function (e) {

      event.preventDefault();
      var $form = $('#frmAdjustAcid');
      if ($form.valid()) {
         var currentTA = $('#AdjustAcid_CurrentTa').val();
         var goalTA = $('#AdjustAcid_GoalTa').val();
         var gramPerLiter = CalcAcidAdditiveNeeded(currentTA, goalTA);
         var gramPerGallon = gramPerLiter * 3.78541;
         var volume = $('#AdjustAcid_Volume').val();

         if (goalTA < currentTA) {
            $('#AdjustAcid_Additive').val('Potassium Bicarbonate');
         } else {
            $('#AdjustAcid_Additive').val('Tartaric Acid');
         }

         var rate = gramPerGallon;
         if ($('input[name=optUomAdjustAcid]:checked', '#frmAdjustAcid').val() === 'metric') {
            rate = gramPerLiter;
         }

         var totalAdditive = rate * volume;

         $('#AdjustAcid_DoseRate').val(formatNumericForDisplay(rate, MetricMassDisplayed, false));
         $('#AdjustAcid_TotalAdditive').val(formatNumericForDisplay(totalAdditive, MetricMassDisplayed, false));

         $('#AdjustAcid_CurrentTa').val(formatNumericForDisplay(currentTA, MetricMassDisplayed, false));
         $('#AdjustAcid_GoalTa').val(formatNumericForDisplay(goalTA, MetricMassDisplayed, false));
         $('#AdjustAcid_Volume').val(formatNumericForDisplay(volume, MetricVolumeDisplayed, false));

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


   // adjust labels for Adjust Acid
   $('#frmAdjustAcid').off('click', 'input[type=radio]');
   $('#frmAdjustAcid').on('click', 'input[type=radio]', function (e) {
      if ($('input[name=optUomAdjustAcid]:checked', '#frmAdjustAcid').val() === 'metric') {
         $('#lblUomAdjustAcid').text('Liters');
         $('#lblUomAdjustAcidRate').text('Grams/Liter');
      } else {
         $('#lblUomAdjustAcid').text('Gallons');
         $('#lblUomAdjustAcidRate').text('Grams/Gallon');
      }

   });

});













