function CalcAcidAdditiveNeeded(starting_ta, ending_ta) {
   try {
      var taDifference = 0;
      taDifference = ending_ta - starting_ta;
      result = Math.abs(taDifference);
      return result;
   } catch (err) {
      console.error(err);
   }
}

function CalcPpmAcid(mL_Wine, N_NaOH, mL_NaOH) {
   try {
      var ppm_Ta = 0;
      if (mL_Wine > 0) {
         ppm_Ta = 75 * mL_NaOH * N_NaOH / mL_Wine;
      }
      return ppm_Ta;
   } catch (err) {
      console.error(err);
   }
}

function CalcNofNaOH(mL_KaPh, N_KaPh, mL_NaOH) {
   try {
      var n_NaOH = 0;
      if (mL_NaOH > 0) {
         n_NaOH = mL_KaPh * N_KaPh / mL_NaOH;
      }
      return n_NaOH;
   } catch (err) {
      console.error(err);
   }
}

function DiluteSolution(strengthOfConcentrate, finalSolutionStrength, finalSolutionVolume) {
   try {
      var volumeOfConcentrateNeeded = finalSolutionStrength * finalSolutionVolume / strengthOfConcentrate;
      return volumeOfConcentrateNeeded;
   } catch (err) {
      console.error(err);
   }
}

function TitrateSO2(mL_Wine, N_NaOH, mL_NaOH) {
   try {
      var ppm_SO2;
      if (mL_Wine !== 0) {
         ppm_SO2 = 32000 * mL_NaOH * N_NaOH / mL_Wine;
      }
      return ppm_SO2;

   } catch (err) {
      console.error(err);
   }
}

function CalcFortifyAddition(volume, targetAlchohol, wineAlchohol, spiritAlchohol) {
   try {
      var tmp = volume * (targetAlchohol - wineAlchohol) / (spiritAlchohol - targetAlchohol);
      var needed = tmp;
      return needed;
   } catch (err) {
      console.error(err);
   }
}

function AdjustSpecificGravityForTemp(readingSG, calabratedTemp, fahrenheitAtReading) {
   try {
      var adjustedSG = 0;
      if (Math.abs(calabratedTemp - fahrenheitAtReading) < 5.01) {
         adjustedSG = readingSG;
      } else {
         adjustedSG = readingSG * ((1.00130346 - (0.000134722124 * fahrenheitAtReading) + (0.00000204052596 * fahrenheitAtReading * fahrenheitAtReading) -
            (0.00000000232820948 * fahrenheitAtReading * fahrenheitAtReading * fahrenheitAtReading)) / (1.00130346 - (0.000134722124 * calabratedTemp) +
               (0.00000204052596 * calabratedTemp * calabratedTemp) - (0.00000000232820948 * calabratedTemp * calabratedTemp * calabratedTemp)));
      }

      return adjustedSG;

   } catch (err) {
      console.error(err);
   }
}

function AdjustBrixForTemp(readingBrix, calabratedTemp, fahrenheitAtReading) {
   try {

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

   } catch (err) {
      console.error(err);
   }
}

function CalcAlcohol(start, end, useBrix) {
   try {
      var abv;
      if (useBrix) {
         // using Brix
         var init = start * 0.55 - 0.63;
         var final = end * 0.55 - 0.63;
         abv = init - final;
      } else {
         // using SG
         abv = Math.round((start - end) * 131.25) * 10 / 10;
      }
      return abv;

   } catch (err) {
      console.error(err);
   }
}

function CalcTargetSO2(pH, red) {
   try {

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

   } catch (err) {
      console.error(err);
   }
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
   // 4 oz in one gallon = +0.010 SG change 
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
         if (reading < 7.5) { ounces = 8; }
         if (reading < 5) { ounces = 4; }
         if (reading < 3.5) { ounces = 2; }
         if (reading < 1) { ounces = 0; }
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
         if (reading < 1.03) { ounces = 8; }
         if (reading < 1.02) { ounces = 4; }
         if (reading < 1.015) { ounces = 2; }
         if (reading < 1.01) { ounces = 0; }
      }

      var result = ounces / 16;
      return result;

   } catch (err) {
      console.error(err);
   }
}
