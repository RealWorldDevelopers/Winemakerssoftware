﻿$(document).ready(function () {

   // clear button
   $('body').off('click', '#btnClear');
   $('body').on('click', '#btnClear', function (e) {
      $('#mass input[type="text"]').val('');
      $('#volume input[type="text"]').val('');
      $('#temp input[type="text"]').val('');
   });


   // all inputs keypress events numeric only
   $('body').off('keypress', '#mass input');
   $('body').on('keypress', '#mass input', function (e) {

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

   $('body').off('keypress', '#volume input');
   $('body').on('keypress', '#volume input', function (e) {

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

   $('body').off('keypress', '#temp input');
   $('body').on('keypress', '#temp input', function (e) {

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



   // all temperature inputs paste events numeric only
   $('body').off('paste', '#temp :input');
   $('body').on('paste', '#temp :input', function () {
      var $this = $(this);
      setTimeout(function () {
         if (isNaN(parseFloat($this.val()))) {
            showAlert('Only Numerical Characters allowed !', cssAlert_Warning, true);
            setTimeout(function () {
               $this.val(null);
            }, 2500);
         } else {
            var celsius = $('#Celsius').val();
            var fahrenheit = CelsiusToFahrenheit(celsius);
            $('#Fahrenheit').val(fahrenheit);
         }
      }, 5);
   });

   // calculate Celsius on Fahrenheit Entry
   $('body').off('keyup', '#Fahrenheit');
   $('body').on('keyup', '#Fahrenheit', function () {
      var fahrenheit = parseFloat($(this).val());
      // check for NaN
      if (!isNaN(fahrenheit)) {
         var celsius = FahrenheitToCelsius(fahrenheit);
         $('#Celsius').val(formatForDisplay(celsius));
      }
   });

   // calculate Celsius on Fahrenheit Entry
   $('body').off('keyup', '#Celsius');
   $('body').on('keyup', '#Celsius', function () {
      var celsius = parseFloat($(this).val());
      // check for NaN
      if (!isNaN(celsius)) {
         var fahrenheit = CelsiusToFahrenheit(celsius);
         $('#Fahrenheit').val(formatForDisplay(fahrenheit));
      }
   });



   // all mass inputs paste events numeric only
   $('body').off('paste', '#mass :input');
   $('body').on('paste', '#mass :input', function () {
      var $this = $(this);
      var sender = this.name;
      setTimeout(function () {
         if (isNaN(parseFloat($this.val()))) {
            showAlert('Only Numerical Characters allowed !', cssAlert_Warning, true);
            setTimeout(function () {
               $this.val(null);
            }, 2500);
         } else {
            var mg, g, kg, ozs, lbs;
            switch (sender) {
               case 'Milligrams':
                  mg = $this.val();
                  g = MilligramsToGrams(mg);
                  kg = GramsToKilograms(g);
                  ozs = GramsToOunces(g);
                  lbs = OuncesToPounds(ozs);
                  break;
               case 'Grams':
                  g = $this.val();
                  mg = GramsToMilligrams(g);
                  kg = GramsToKilograms(g);
                  ozs = GramsToOunces(g);
                  lbs = OuncesToPounds(ozs);
                  break;
               case 'Kilograms':
                  kg = $this.val();
                  g = KilogramsToGrams(kg);
                  mg = GramsToMilligrams(g);
                  ozs = GramsToOunces(g);
                  lbs = OuncesToPounds(ozs);
                  break;
               case 'Ounces':
                  ozs = $this.val();
                  lbs = OuncesToPounds(ozs);
                  g = OuncesToGrams(ozs);
                  mg = GramsToMilligrams(g);
                  kg = GramsToKilograms(g);
                  break;
               case 'Pounds':
                  lbs = $this.val();
                  ozs = PoundsToOunces(lbs);
                  kg = PoundsToKilograms(lbs);
                  g = KilogramsToGrams(kg);
                  mg = GramsToMilligrams(g);
                  break;
            }

            $('#Milligrams').val(formatForDisplay(mg));
            $('#Grams').val(formatForDisplay(g));
            $('#Kilograms').val(formatForDisplay(kg));
            $('#Ounces').val(formatForDisplay(ozs));
            $('#Pounds').val(formatForDisplay(lbs));
         }
      }, 5);
   });

   // calculate on Milligrams Entry
   $('body').off('keyup', '#Milligrams');
   $('body').on('keyup', '#Milligrams', function () {
      var mg = parseFloat($(this).val());
      // check for NaN
      if (!isNaN(mg)) {
         var g = MilligramsToGrams(mg);
         var kg = GramsToKilograms(g);
         var ozs = GramsToOunces(g);
         var lbs = OuncesToPounds(ozs);
         $('#Grams').val(formatForDisplay(g));
         $('#Kilograms').val(formatForDisplay(kg));
         $('#Ounces').val(formatForDisplay(ozs));
         $('#Pounds').val(formatForDisplay(lbs));
      }
   });

   // calculate on Grams Entry
   $('body').off('keyup', '#Grams');
   $('body').on('keyup', '#Grams', function () {
      var g = parseFloat($(this).val());
      // check for NaN
      if (!isNaN(g)) {
         var mg = GramsToMilligrams(g);
         var kg = GramsToKilograms(g);
         var ozs = GramsToOunces(g);
         var lbs = OuncesToPounds(ozs);
         $('#Milligrams').val(formatForDisplay(mg));
         $('#Kilograms').val(formatForDisplay(kg));
         $('#Ounces').val(formatForDisplay(ozs));
         $('#Pounds').val(formatForDisplay(lbs));
      }
   });

   // calculate on Kilograms Entry
   $('body').off('keyup', '#Kilograms');
   $('body').on('keyup', '#Kilograms', function () {
      var kg = parseFloat($(this).val());
      // check for NaN
      if (!isNaN(kg)) {
         var g = KilogramsToGrams(kg);
         var mg = GramsToMilligrams(g);
         var ozs = GramsToOunces(g);
         var lbs = OuncesToPounds(ozs);
         $('#Milligrams').val(formatForDisplay(mg));
         $('#Grams').val(formatForDisplay(g));
         $('#Ounces').val(formatForDisplay(ozs));
         $('#Pounds').val(formatForDisplay(lbs));
      }
   });

   // calculate on Ounces Entry
   $('body').off('keyup', '#Ounces');
   $('body').on('keyup', '#Ounces', function () {
      var ozs = parseFloat($(this).val());
      // check for NaN
      if (!isNaN(ozs)) {
         var g = OuncesToGrams(ozs);
         var mg = GramsToMilligrams(g);
         var kg = GramsToKilograms(g);
         var lbs = OuncesToPounds(ozs);
         $('#Kilograms').val(formatForDisplay(kg));
         $('#Milligrams').val(formatForDisplay(mg));
         $('#Grams').val(formatForDisplay(g));
         $('#Pounds').val(formatForDisplay(lbs));
      }
   });

   // calculate on Pounds Entry
   $('body').off('keyup', '#Pounds');
   $('body').on('keyup', '#Pounds', function () {
      var lbs = parseFloat($(this).val());
      // check for NaN
      if (!isNaN(lbs)) {
         var kg = PoundsToKilograms(lbs);
         var g = KilogramsToGrams(kg);
         var mg = GramsToMilligrams(g);
         var ozs = PoundsToOunces(lbs);
         $('#Kilograms').val(formatForDisplay(kg));
         $('#Milligrams').val(formatForDisplay(mg));
         $('#Grams').val(formatForDisplay(g));
         $('#Ounces').val(formatForDisplay(ozs));
      }
   });



   // all volume inputs paste events numeric only
   $('body').off('paste', '#volume :input');
   $('body').on('paste', '#volume :input', function () {
      var $this = $(this);
      var sender = this.name;
      setTimeout(function () {
         if (isNaN(parseFloat($this.val()))) {
            showAlert('Only Numerical Characters allowed !', cssAlert_Warning, true);
            setTimeout(function () {
               $this.val(null);
            }, 2500);
         } else {
            var l, ml, tsp, tbsp, ozs, qrts, gals, cups, pints;
            switch (sender) {
               case 'Milliliters':
                  ml = $this.val();
                  l = MillilitersToLiters(ml);
                  tsp = MillilitersToTeaspoons(ml);
                  tbsp = MillilitersToTablespoons(ml);
                  ozs = MillilitersToFluidOunces(ml);
                  qrts = LitersToQuarts(l);
                  gals = LitersToGallons(l);
                  cups = GallonsToCups(gals);
                  pints = CupsToPints(cups);
                  break;
               case 'Liters':
                  l = $this.val();
                  ml = LitersToMilliliters(l);
                  tsp = MillilitersToTeaspoons(ml);
                  tbsp = MillilitersToTablespoons(ml);
                  ozs = MillilitersToFluidOunces(ml);
                  qrts = LitersToQuarts(l);
                  gals = LitersToGallons(l);
                  cups = GallonsToCups(gals);
                  pints = CupsToPints(cups);
                  break;
               case 'Teaspoons':
                  tsp = $this.val();
                  tbsp = TeaspoonToTablespoon(tsp);
                  ozs = TeaspoonToFluidOunces(tsp);
                  l = FluidOuncesToLiters(ozs);
                  ml = FluidOuncesToMilliliters(ozs);
                  cups = FluidOuncesToCups(ozs);
                  pints = FluidOuncesToPints(ozs);
                  qrts = FluidOuncesToQuarts(ozs);
                  gals = FluidOuncesToGallons(ozs);
                  break;
               case 'Tablespoons':
                  tbsp = $this.val();
                  tsp = TablespoonToTeaspoon(tbsp);
                  ozs = TablespoonToFluidOunces(tbsp);
                  l = FluidOuncesToLiters(ozs);
                  ml = FluidOuncesToMilliliters(ozs);
                  cups = FluidOuncesToCups(ozs);
                  pints = FluidOuncesToPints(ozs);
                  qrts = FluidOuncesToQuarts(ozs);
                  gals = FluidOuncesToGallons(ozs);
                  break;
               case 'Fluid_Ounces':
                  ozs = $this.val();
                  l = FluidOuncesToLiters(ozs);
                  ml = FluidOuncesToMilliliters(ozs);
                  tsp = FluidOuncesToTeaspoons(ozs);
                  tbsp = FluidOuncesToTablespoons(ozs);
                  cups = FluidOuncesToCups(ozs);
                  pints = FluidOuncesToPints(ozs);
                  qrts = FluidOuncesToQuarts(ozs);
                  gals = FluidOuncesToGallons(ozs);
                  break;
               case 'Cups':
                  cups = $this.val();
                  pints = CupsToPints(cups);
                  ozs = CupsToFluidOunces(cups);
                  tsp = FluidOuncesToTeaspoons(ozs);
                  tbsp = FluidOuncesToTablespoons(ozs);
                  qrts = CupsToQuarts(cups);
                  l = QuartsToLiters(qrts);
                  ml = LitersToMilliliters(l);
                  gals = LitersToGallons(l);
                  break;
               case 'Pints':
                  pints = $this.val();
                  qrts = PintsToQuarts(pints);
                  l = QuartsToLiters(qrts);
                  ml = LitersToMilliliters(l);
                  cups = PintsToCups(pints);
                  ozs = CupsToFluidOunces(cups);
                  tsp = FluidOuncesToTeaspoons(ozs);
                  tbsp = FluidOuncesToTablespoons(ozs);
                  gals = QuartsToGallons(qrts);
                  break;
               case 'Quarts':
                  qrts = $this.val();
                  l = QuartsToLiters(qrts);
                  ml = LitersToMilliliters(l);
                  gals = QuartsToGallons(qrts);
                  cups = GallonsToCups(gals);
                  pints = CupsToPints(cups);
                  ozs = CupsToFluidOunces(cups);
                  tsp = FluidOuncesToTeaspoons(ozs);
                  tbsp = FluidOuncesToTablespoons(ozs);
                  break;
               case 'Gallons':
                  gals = $this.val();
                  qrts = GallonsToQuarts(gals);
                  l = QuartsToLiters(qrts);
                  ml = LitersToMilliliters(l);
                  cups = GallonsToCups(gals);
                  pints = CupsToPints(cups);
                  ozs = CupsToFluidOunces(cups);
                  tsp = FluidOuncesToTeaspoons(ozs);
                  tbsp = FluidOuncesToTablespoons(ozs);
                  break;
            }

            $('#Milliliters').val(formatForDisplay(ml));
            $('#Liters').val(formatForDisplay(l));
            $('#Gallons').val(formatForDisplay(gals));
            $('#Fluid_Ounces').val(formatForDisplay(ozs));
            $('#Cups').val(formatForDisplay(cups));
            $('#Pints').val(formatForDisplay(pints));
            $('#Quarts').val(formatForDisplay(qrts));
            $('#Tablespoons').val(formatForDisplay(tbsp));
            $('#Teaspoons').val(formatForDisplay(tsp));
         }
      }, 5);
   });

   // calculate on Milliliters Entry
   $('body').off('keyup', '#Milliliters');
   $('body').on('keyup', '#Milliliters', function () {
      var ml = parseFloat($(this).val());
      // check for NaN
      if (!isNaN(ml)) {
         var l = MillilitersToLiters(ml);
         var tsp = MillilitersToTeaspoons(ml);
         var tbsp = MillilitersToTablespoons(ml);
         var ozs = MillilitersToFluidOunces(ml);
         var qrts = LitersToQuarts(l);
         var gals = LitersToGallons(l);
         var cups = GallonsToCups(gals);
         var pints = CupsToPints(cups);
         $('#Liters').val(formatForDisplay(l));
         $('#Gallons').val(formatForDisplay(gals));
         $('#Fluid_Ounces').val(formatForDisplay(ozs));
         $('#Cups').val(formatForDisplay(cups));
         $('#Pints').val(formatForDisplay(pints));
         $('#Quarts').val(formatForDisplay(qrts));
         $('#Tablespoons').val(formatForDisplay(tbsp));
         $('#Teaspoons').val(formatForDisplay(tsp));
      }
   });

   // calculate on Liters Entry
   $('body').off('keyup', '#Liters');
   $('body').on('keyup', '#Liters', function () {
      var l = parseFloat($(this).val());
      // check for NaN
      if (!isNaN(l)) {
         var ml = LitersToMilliliters(l);
         var tsp = MillilitersToTeaspoons(ml);
         var tbsp = MillilitersToTablespoons(ml);
         var ozs = MillilitersToFluidOunces(ml);
         var qrts = LitersToQuarts(l);
         var gals = LitersToGallons(l);
         var cups = GallonsToCups(gals);
         var pints = CupsToPints(cups);
         $('#Milliliters').val(formatForDisplay(ml));
         $('#Gallons').val(formatForDisplay(gals));
         $('#Fluid_Ounces').val(formatForDisplay(ozs));
         $('#Cups').val(formatForDisplay(cups));
         $('#Pints').val(formatForDisplay(pints));
         $('#Quarts').val(formatForDisplay(qrts));
         $('#Tablespoons').val(formatForDisplay(tbsp));
         $('#Teaspoons').val(formatForDisplay(tsp));
      }
   });

   // calculate on Gallons Entry
   $('body').off('keyup', '#Gallons');
   $('body').on('keyup', '#Gallons', function () {
      var gals = parseFloat($(this).val());
      // check for NaN
      if (!isNaN(gals)) {
         var qrts = GallonsToQuarts(gals);
         var cups = GallonsToCups(gals);
         var pints = CupsToPints(cups);
         var ozs = CupsToFluidOunces(cups);
         var tsp = FluidOuncesToTeaspoons(ozs);
         var tbsp = FluidOuncesToTablespoons(ozs);
         var l = QuartsToLiters(qrts);
         var ml = LitersToMilliliters(l);
         $('#Milliliters').val(formatForDisplay(ml));
         $('#Liters').val(formatForDisplay(l));
         $('#Fluid_Ounces').val(formatForDisplay(ozs));
         $('#Cups').val(formatForDisplay(cups));
         $('#Pints').val(formatForDisplay(pints));
         $('#Quarts').val(formatForDisplay(qrts));
         $('#Tablespoons').val(formatForDisplay(tbsp));
         $('#Teaspoons').val(formatForDisplay(tsp));
      }
   });

   // calculate on Quarts Entry
   $('body').off('keyup', '#Quarts');
   $('body').on('keyup', '#Quarts', function () {
      var qrts = parseFloat($(this).val());
      // check for NaN
      if (!isNaN(qrts)) {
         var gals = QuartsToGallons(qrts);
         var cups = PintsToCups(pints);
         var pints = CupsToPints(cups);
         var ozs = CupsToFluidOunces(cups);
         var tsp = FluidOuncesToTeaspoons(ozs);
         var tbsp = FluidOuncesToTablespoons(ozs);
         var l = QuartsToLiters(qrts);
         var ml = LitersToMilliliters(l);
         $('#Milliliters').val(formatForDisplay(ml));
         $('#Liters').val(formatForDisplay(l));
         $('#Gallons').val(formatForDisplay(gals));
         $('#Fluid_Ounces').val(formatForDisplay(ozs));
         $('#Cups').val(formatForDisplay(cups));
         $('#Pints').val(formatForDisplay(pints));
         $('#Tablespoons').val(formatForDisplay(tbsp));
         $('#Teaspoons').val(formatForDisplay(tsp));
      }
   });

   // calculate on Pints Entry
   $('body').off('keyup', '#Pints');
   $('body').on('keyup', '#Pints', function () {
      var pints = parseFloat($(this).val());
      // check for NaN
      if (!isNaN(pints)) {
         var qrts = PintsToQuarts(pints);
         var gals = QuartsToGallons(qrts);
         var cups = PintsToCups(pints);
         var ozs = CupsToFluidOunces(cups);
         var tsp = FluidOuncesToTeaspoons(ozs);
         var tbsp = FluidOuncesToTablespoons(ozs);
         var l = QuartsToLiters(qrts);
         var ml = LitersToMilliliters(l);
         $('#Milliliters').val(formatForDisplay(ml));
         $('#Liters').val(formatForDisplay(l));
         $('#Gallons').val(formatForDisplay(gals));
         $('#Fluid_Ounces').val(formatForDisplay(ozs));
         $('#Cups').val(formatForDisplay(cups));
         $('#Quarts').val(formatForDisplay(qrts));
         $('#Tablespoons').val(formatForDisplay(tbsp));
         $('#Teaspoons').val(formatForDisplay(tsp));
      }
   });

   // calculate on Cups Entry
   $('body').off('keyup', '#Cups');
   $('body').on('keyup', '#Cups', function () {
      var cups = parseFloat($(this).val());
      // check for NaN
      if (!isNaN(cups)) {
         var qrts = CupsToQuarts(cups);
         var gals = QuartsToGallons(qrts);
         var pints = CupsToPints(cups);
         var ozs = CupsToFluidOunces(cups);
         var tsp = FluidOuncesToTeaspoons(ozs);
         var tbsp = FluidOuncesToTablespoons(ozs);
         var l = QuartsToLiters(qrts);
         var ml = LitersToMilliliters(l);
         $('#Milliliters').val(formatForDisplay(ml));
         $('#Liters').val(formatForDisplay(l));
         $('#Gallons').val(formatForDisplay(gals));
         $('#Fluid_Ounces').val(formatForDisplay(ozs));
         $('#Pints').val(formatForDisplay(pints));
         $('#Quarts').val(formatForDisplay(qrts));
         $('#Tablespoons').val(formatForDisplay(tbsp));
         $('#Teaspoons').val(formatForDisplay(tsp));
      }
   });

   // calculate on Fluid_Ounces Entry
   $('body').off('keyup', '#Fluid_Ounces');
   $('body').on('keyup', '#Fluid_Ounces', function () {
      var ozs = parseFloat($(this).val());
      // check for NaN
      if (!isNaN(ozs)) {
         var cups = FluidOuncesToCups(ozs);
         var pints = FluidOuncesToPints(ozs);
         var qrts = FluidOuncesToQuarts(ozs);
         var gals = FluidOuncesToGallons(ozs);
         var tsp = FluidOuncesToTeaspoons(ozs);
         var tbsp = FluidOuncesToTablespoons(ozs);
         var ml = FluidOuncesToMilliliters(ozs);
         var l = FluidOuncesToLiters(ozs);
         $('#Milliliters').val(formatForDisplay(ml));
         $('#Liters').val(formatForDisplay(l));
         $('#Gallons').val(formatForDisplay(gals));
         $('#Cups').val(formatForDisplay(cups));
         $('#Pints').val(formatForDisplay(pints));
         $('#Quarts').val(formatForDisplay(qrts));
         $('#Tablespoons').val(formatForDisplay(tbsp));
         $('#Teaspoons').val(formatForDisplay(tsp));
      }
   });

   // calculate on Tablespoons Entry
   $('body').off('keyup', '#Tablespoons');
   $('body').on('keyup', '#Tablespoons', function () {
      var tbsp = parseFloat($(this).val());
      // check for NaN
      if (!isNaN(tbsp)) {
         var ozs = TablespoonToFluidOunces(tbsp);
         var tsp = TablespoonToTeaspoon(tbsp);
         var cups = FluidOuncesToCups(ozs);
         var pints = FluidOuncesToPints(ozs);
         var qrts = FluidOuncesToQuarts(ozs);
         var gals = FluidOuncesToGallons(ozs);
         var ml = FluidOuncesToMilliliters(ozs);
         var l = FluidOuncesToLiters(ozs);
         $('#Milliliters').val(formatForDisplay(ml));
         $('#Liters').val(formatForDisplay(l));
         $('#Gallons').val(formatForDisplay(gals));
         $('#Fluid_Ounces').val(formatForDisplay(ozs));
         $('#Cups').val(formatForDisplay(cups));
         $('#Pints').val(formatForDisplay(pints));
         $('#Quarts').val(formatForDisplay(qrts));
         $('#Teaspoons').val(formatForDisplay(tsp));
      }
   });

   // calculate on Teaspoons Entry
   $('body').off('keyup', '#Teaspoons');
   $('body').on('keyup', '#Teaspoons', function () {
      var tsp = parseFloat($(this).val());
      // check for NaN
      if (!isNaN(tsp)) {
         var ozs = TeaspoonToFluidOunces(tsp);
         var tbsp = TeaspoonToTablespoon(tsp);
         var cups = FluidOuncesToCups(ozs);
         var pints = FluidOuncesToPints(ozs);
         var qrts = FluidOuncesToQuarts(ozs);
         var gals = FluidOuncesToGallons(ozs);
         var ml = FluidOuncesToMilliliters(ozs);
         var l = FluidOuncesToLiters(ozs);
         $('#Milliliters').val(formatForDisplay(ml));
         $('#Liters').val(formatForDisplay(l));
         $('#Gallons').val(formatForDisplay(gals));
         $('#Fluid_Ounces').val(formatForDisplay(ozs));
         $('#Cups').val(formatForDisplay(cups));
         $('#Pints').val(formatForDisplay(pints));
         $('#Quarts').val(formatForDisplay(qrts));
         $('#Tablespoons').val(formatForDisplay(tbsp));
      }
   });

});


function CelsiusToFahrenheit(celsius) {
   try {
      var fahrenheit = 9.0 / 5.0 * celsius + 32;
      return fahrenheit;
   } catch (err) {
      console.error(err);
   }
}

function FahrenheitToCelsius(fahrenheit) {
   try {
      var celsius = (fahrenheit - 32) * 5.0 / 9.0;
      return celsius;
   } catch (err) {
      console.error(err);
   }
}



function PoundsToKilograms(lbs) {
   try {
      var kg = lbs * 0.45359237;
      return kg;
   } catch (err) {
      console.error(err);
   }
}

function OuncesToGrams(ozs) {
   try {
      var g = ozs * 28.3495231;
      return g;
   } catch (err) {
      console.error(err);
   }
}

function GramsToOunces(grams) {
   try {
      var ozs = grams * 0.0352739619;
      return ozs;
   } catch (err) {
      console.error(err);
   }
}

function OuncesToPounds(ozs) {
   try {
      var lbs = ozs * 0.0625;
      return lbs;
   } catch (err) {
      console.error(err);
   }
}

function PoundsToOunces(lbs) {
   try {
      var ozs = lbs * 16;
      return ozs;
   } catch (err) {
      console.error(err);
   }
}

function MilligramsToGrams(mg) {
   try {
      var g = mg * 0.001;
      return g;
   } catch (err) {
      console.error(err);
   }
}

function GramsToKilograms(g) {
   try {
      var kg = g * 0.001;
      return kg;
   } catch (err) {
      console.error(err);
   }
}

function GramsToMilligrams(g) {
   try {
      var mg = g * 1000;
      return mg;
   } catch (err) {
      console.error(err);
   }
}

function KilogramsToGrams(kg) {
   try {
      var g = kg * 1000;
      return g;
   } catch (err) {
      console.error(err);
   }
}



function MillilitersToTeaspoons(ml) {
   try {
      var tsp = ml * 0.202884136;
      return tsp;
   } catch (err) {
      console.error(err);
   }
}

function MillilitersToTablespoons(ml) {
   try {
      var tbsp = ml * 0.0676280454;
      return tbsp;
   } catch (err) {
      console.error(err);
   }
}

function MillilitersToFluidOunces(ml) {
   try {
      var ozs = ml * 0.0338140227;
      return ozs;
   } catch (err) {
      console.error(err);
   }
}

function LitersToQuarts(l) {
   try {
      var qrts = l * 1.05668821;
      return qrts;
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

function MillilitersToLiters(ml) {
   try {
      var l = ml * 0.001;
      return l;
   } catch (err) {
      console.error(err);
   }
}

function LitersToMilliliters(l) {
   try {
      var ml = l * 1000;
      return ml;
   } catch (err) {
      console.error(err);
   }
}



function TablespoonToTeaspoon(tbsp) {
   try {
      var tsp = tbsp * 3;
      return tsp;
   } catch (err) {
      console.error(err);
   }
}

function TeaspoonToTablespoon(tsp) {
   try {
      var tbsp = tsp / 3;
      return tbsp;
   } catch (err) {
      console.error(err);
   }
}

function TeaspoonToFluidOunces(tsp) {
   try {
      var ozs = tsp * 0.166666667;
      return ozs;
   } catch (err) {
      console.error(err);
   }
}

function TablespoonToFluidOunces(tbsp) {
   try {
      var ozs = tbsp * 0.5;
      return ozs;
   } catch (err) {
      console.error(err);
   }
}

function FluidOuncesToLiters(ozs) {
   try {
      var l = ozs * 0.0295735296;
      return l;
   } catch (err) {
      console.error(err);
   }
}

function FluidOuncesToMilliliters(ozs) {
   try {
      var ml = ozs * 29.5735296;
      return ml;
   } catch (err) {
      console.error(err);
   }
}

function FluidOuncesToCups(ozs) {
   try {
      var cups = ozs * 0.125;
      return cups;
   } catch (err) {
      console.error(err);
   }
}

function FluidOuncesToPints(ozs) {
   try {
      var pints = ozs * 0.0625;
      return pints;
   } catch (err) {
      console.error(err);
   }
}

function FluidOuncesToQuarts(ozs) {
   try {
      var qrts = ozs * 0.03125;
      return qrts;
   } catch (err) {
      console.error(err);
   }
}

function FluidOuncesToGallons(ozs) {
   try {
      var gals = ozs * 0.0078125;
      return gals;
   } catch (err) {
      console.error(err);
   }
}

function FluidOuncesToTeaspoons(ozs) {
   try {
      var tsp = ozs * 6;
      return tsp;
   } catch (err) {
      console.error(err);
   }
}

function FluidOuncesToTablespoons(ozs) {
   try {
      var tbsp = ozs * 2;
      return tbsp;
   } catch (err) {
      console.error(err);
   }
}

function CupsToQuarts(cups) {
   try {
      var qrts = cups * 0.25;
      return qrts;
   } catch (err) {
      console.error(err);
   }
}

function CupsToPints(cups) {
   try {
      var pints = cups * .5;
      return pints;
   } catch (err) {
      console.error(err);
   }
}

function CupsToFluidOunces(cups) {
   try {
      var ozs = cups * 8;
      return ozs;
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

function QuartsToGallons(qrts) {
   try {
      var gals = qrts * 0.25;
      return gals;
   } catch (err) {
      console.error(err);
   }
}

function PintsToQuarts(pints) {
   try {
      var qrts = pints * 0.5;
      return qrts;
   } catch (err) {
      console.error(err);
   }
}

function PintsToCups(pints) {
   try {
      var cups = pints * 2;
      return cups;
   } catch (err) {
      console.error(err);
   }
}

function GallonsToCups(gals) {
   try {
      var cups = gals * 16;
      return cups;
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

function formatForDisplay(num) {
   try {
      var newNum = Math.round(num * 1000) / 1000;
      return newNum.toLocaleString('en');
   } catch (err) {
      console.error(err);
   }
}

