var statusChart;

$(document).ready(function () {

   localStorage.removeItem('latestEntryAdded')

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

   //search bar functions
   $('body').off('keypress', '#batchSearch');
   $('body').on('keypress', '#batchSearch', function (e) {
      return IsAlphaNumeric(e);
   });

   $('body').off('keyup', '#batchSearch');
   $('body').on('keyup', '#batchSearch', function (e) {
      filterBatches(e);
   });

   $('body').off('paste', '#batchSearch');
   $('body').on('paste', '#batchSearch', function (e) {
      var element = this;
      setTimeout(function () {
         var text = $(element).val();
         $(element).val(text.replace(/[^ a-zA-Z0-9]/g, ''));
      }, 100);
   });

   // show hide complete cards
   $('body').off('change', '#ShowComplete');
   $('body').on('change', '#ShowComplete', function () {
      var ck = this.checked;
      if (ck) {
         $(".multi-collapse").collapse('show');
      } else {
         $(".multi-collapse").collapse('hide');
      }
   });

   // detailed batch edit
   $('body').off('click', 'button[name="editBatchDetailsButton"]');
   $('body').on('click', 'button[name="editBatchDetailsButton"]', function () {
      var id = $(this).data('id');
      $('#editBatchId').val(id);
      $('#frmEditBatch').submit();
   });

   // update Batch Info
   $('body').off('click', 'button[name="updateBatchButton"]');
   $('body').on('click', 'button[name="updateBatchButton"]', function () {
      var id = $('#updateBatchId').val();
      updateBatch(id);
   });

   // update Target Info
   $('body').off('click', 'button[name="updateTargetButton"]');
   $('body').on('click', 'button[name="updateTargetButton"]', function () {
      var id = $('#updateBatchId').val();
      updateBatchTarget(id);
   });

   // mark batch complete
   $('body').off('change', '#Complete');
   $('body').on('change', '#Complete', function () {
      var id = $('#updateBatchId').val();
      var e1 = $('#batchEntryButton');
      var e2 = $('#divStatusInfo');
      var e3 = $('#divCompleteStatus');
      toggleBatchComplete(id, this, e1, e2, e3);
   });

   // quick edit batch
   $('body').off('click', 'button[name="showBatchEntryQuickButton"]');
   $('body').on('click', 'button[name="showBatchEntryQuickButton"]', function () {
      var id = $(this).data('id');
      // alert(id);
      showAddEntry(id);
   });

   $('body').off('click', 'button[name="showBatchEntryButton"]');
   $('body').on('click', 'button[name="showBatchEntryButton"]', function () {
      var id = $(this).data('id');
      // alert(id);
      showAddEntry(id);

      // Retrieve the new entry from storage and display in list
      $('#batchEntryModal').on('hidden.bs.modal', function () {
         var lastEntry = localStorage.getItem('latestEntryAdded');
         if (lastEntry) {
            var retrievedObject = JSON.parse(lastEntry);
            buildDisplayEntry(retrievedObject);
            addToChart(retrievedObject);
            updateStatus(retrievedObject);
         }
      })
   });

   // add batch entry click
   $('body').off('click', 'button[name="addBatchEntryButton"]');
   $('body').on('click', 'button[name="addBatchEntryButton"]', function () {
      var id = $('#batchEntryModal_id').val();
      addBatchEntry(id);
   });

   // delete batch entry click
   $('body').off('click', 'button[name="delBatchEntryButton"]');
   $('body').on('click', 'button[name="delBatchEntryButton"]', function () {
      if (confirm('Are you sure?')) {
         var id = $(this).data('id');
         var parent = $(this).parent().parent().parent();
         deleteBatchEntry(id, parent);
      }
   });

   // Delete Batch 
   $('body').off('click', 'button[name="deleteBatchButton"]');
   $('body').on('click', 'button[name="deleteBatchButton"]', function () {
      if (confirm('Are you sure?')) {
         var id = $(this).data('id');
         $('#updateBatchId').val(id);
         $('#frmDeleteBatch').submit();
      }
   });

   // load status chart 
   if (window.location.href.lastIndexOf('EditBatch') >= 0) {
      var id = $('#updateBatchId').val();
      getStatusChart(id);
   }

   // resize graph on print
   //window.addEventListener("beforeprint", function (event) {
   //   for (var id in Chart.instances) {
   //      Chart.instances[id].resize()
   //      this.alert('resized');
   //   }
   //});

  

});

function filterBatches() {

   var input, filter, ul, li, li2, a, i;
   input = document.getElementById('batchSearch');
   filter = input.value.toUpperCase();

   var partial = '';
   var list = filter.split(' ');

   for (i = 0; i < list.length; i++) {
      if (list[i] !== '') {
         var pre = partial;
         var word = '(?=.*\\b'.concat(list[i], '\\w*\\b)');
         partial = pre.concat(word);
      }
   };

   var pattern = partial.concat('.*');
   var regEx = new RegExp(pattern, 'i');

   cardsContainer = document.getElementById('batchesContainer');
   cards = cardsContainer.getElementsByClassName('card');
   for (i = 0; i < cards.length; i++) {

      var cls = cards[i].classList;
      if (cls.contains('d-none')) {
         cls.remove('d-none')
      }

      var testData = cards[i].textContent;
      testData = testData.replace(/(\W)/gm, ' x ');
      if (!regEx.test(testData)) {
         cls.add('d-none');
      }
   };
}

function updateBatch(id) {
   const uri = '/api/journal';
   var jwt = $('#BatchJwt').val();

   var form = $("#batchUpdateForm");
   form.validate();

   if (form.valid()) {

      // validate volume
      var vol = parseFloat($('#Volume').val());
      if (!$.isNumeric(vol) || vol < 1 || vol > 999) {
         vol = null;
      };
      var volUom = parseInt($('#VolumeUOM').val());
      if (!Number.isInteger(volUom)) {
         volUom = null;
      };

      // validate variety
      var variety = parseInt($('#VarietyId').val());
      if (!$.isNumeric(variety)) { variety = null; };

      // validate yeast
      var yeast = parseInt($('#YeastId').val());
      if (!$.isNumeric(yeast)) { yeast = null; };

      // validate Culture
      var malo = parseInt($('#MaloCultureId').val());
      if (!$.isNumeric(malo)) { malo = null; };

      // validate vintage
      var vintage = parseInt($('#Vintage').val());
      if (!$.isNumeric(vintage) || vintage < 2015 || vintage > 2040) {
         vintage = null;
      };

      var batch = {
         Title: $('#Title').val(),
         Description: $('#Description').val(),
         Volume: vol,
         VolumeUOM: volUom,
         Vintage: vintage,
         VarietyId: variety,
         YeastId: yeast,
         MaloCultureId: malo
      };

      $.ajax({
         url: rootUri + uri + '/batchUpdate/' + id,
         type: 'PUT',
         headers: { 'Authorization': 'Bearer ' + jwt },
         contentType: 'application/json',
         data: JSON.stringify(batch),
         success: function (result) {
            console.log('successful update batch');

            // show complete
            $('#batchUpdateToast').toast({ delay: 2000 });
            $('#batchUpdateToast').toast('show');

         },
         error: function (xmlHttpRequest, textStatus, errorThrown) {
            alert('something went wrong');
            console.log('Exception: ' + errorThrown);
         }

      });
   }
}

function updateBatchTarget(id) {
   const uri = '/api/journal';
   var jwt = $('#BatchJwt').val();

   var form = $("#targetUpdateForm");
   form.validate();

   if (form.valid()) {

      // validate sugar
      var startSugar = parseFloat($('#Target_StartingSugar').val());
      if (!$.isNumeric(startSugar) || startSugar > 30 || startSugar < .8) {
         startSugar = null;
      };
      var startSugarUom = parseInt($('#Target_StartSugarUOM').val());
      if (!Number.isInteger(startSugarUom)) {
         startSugarUom = null;
      };

      // validate sugar
      var endSugar = parseFloat($('#Target_EndingSugar').val());
      if (!$.isNumeric(endSugar) || endSugar > 30 || endSugar < .8) {
         endSugar = null;
      };
      var endSugarUom = parseInt($('#Target_EndSugarUOM').val());
      if (!Number.isInteger(endSugarUom)) {
         endSugarUom = null;
      };

      // validate temp
      var temp = parseFloat($('#Target_FermentationTemp').val());
      if (!$.isNumeric(temp) || temp > 90 || temp < 0) {
         temp = null;
      };
      var tempUom = parseInt($('#Target_TempUOM').val());
      if (!Number.isInteger(tempUom)) {
         tempUom = null;
      };

      // validate pH
      var ph = parseFloat($('#Target_pH').val());
      if (!$.isNumeric(ph) || ph > 30 || ph < .8) {
         ph = null;
      };

      // validate TA
      var ta = parseFloat($('#Target_TA').val());
      if (!$.isNumeric(ta) || ta > 30 || ta < .8) {
         ta = null;
      };

      var target = {
         StartingSugar: startSugar,
         StartSugarUOM: startSugarUom,
         EndingSugar: endSugar,
         EndSugarUOM: endSugarUom,
         TA: ta,
         pH: ph,
         FermentationTemp: temp,
         TempUOM: tempUom
      };

      $.ajax({
         url: rootUri + uri + '/batchTarget/' + id,
         type: 'PUT',
         headers: { 'Authorization': 'Bearer ' + jwt },
         contentType: 'application/json',
         data: JSON.stringify(target),
         success: function (result) {
            console.log('successful update batch target');

            // show complete
            $('#targetUpdateToast').toast({ delay: 2000 });
            $('#targetUpdateToast').toast('show');

         },
         error: function (xmlHttpRequest, textStatus, errorThrown) {
            alert('something went wrong');
            console.log('Exception: ' + errorThrown);
            checkbox.prop('checked', !value);
         }

      });

   }
}

function toggleBatchComplete(id, checkbox, entryButton, statusInfo, completeStatus) {
   const uri = '/api/journal';
   var jwt = $('#BatchJwt').val();
   var value = checkbox.checked;

   $.ajax({
      url: rootUri + uri + '/batchComplete/' + id,
      type: 'PUT',
      headers: { 'Authorization': 'Bearer ' + jwt },
      contentType: 'application/json',
      data: JSON.stringify(value),
      success: function (result) {
         console.log('successful update batch entry');
         if (value === true) {
            entryButton.addClass('d-none');
            statusInfo.addClass('d-none');
            completeStatus.removeClass('d-none');
         } else {
            entryButton.removeClass('d-none');
            statusInfo.removeClass('d-none');
            completeStatus.addClass('d-none');
         }
      },
      error: function (xmlHttpRequest, textStatus, errorThrown) {
         alert('something went wrong');
         console.log('Exception: ' + errorThrown);
         checkbox.prop('checked', !value);
      }

   });

}

function deleteBatchEntry(id, el) {

   const uri = '/api/journal';
   var jwt = $('#BatchJwt').val();

   $.ajax({
      url: rootUri + uri + '/batchEntry/' + id,
      type: 'POST',
      headers: { 'Authorization': 'Bearer ' + jwt },
      contentType: 'application/json',
      success: function (result) {
         console.log('successful deletion batch entry');

         // hide local
         el.addClass('d-none');
      },
      error: function (xmlHttpRequest, textStatus, errorThrown) {
         alert('something went wrong');
         console.log('Exception: ' + errorThrown);
      }
   });
}

function addBatchEntry(id) {
   // clear former results
   localStorage.removeItem('latestEntryAdded')

   // validate date
   var aDate = new Date(Date.now());
   if ($('#batchEntryModal_actionDate').val()) {
      var tmpDate = new Date($('#batchEntryModal_actionDate').val().replace(/-/g, '/'));

      // in last 30 days
      var targetDate = new Date();
      targetDate.setFullYear(targetDate.getFullYear() - 7);
      if (tmpDate < Date.now() && tmpDate > targetDate) {
         aDate = tmpDate;
      }
   }

   // alert(aDate);

   // validate temp
   var temp = parseFloat($('#batchEntryModal_temp').val());
   if (!$.isNumeric(temp) || temp > 90 || temp < 0) {
      temp = null;
   };
   var tempUom = parseInt($('input[name="optEntryTemp"]:checked').val());
   if (!Number.isInteger(tempUom)) {
      tempUom = null;
   };

   // validate sugar
   var sugar = parseFloat($('#batchEntryModal_sugar').val());
   if (!$.isNumeric(sugar) || sugar > 30 || sugar < .8) {
      sugar = null;
   };
   var sugarUom = parseInt($('input[name="optEntrySugar"]:checked').val());
   if (!Number.isInteger(sugarUom)) {
      sugarUom = null;
   };

   // validate pH
   var ph = parseFloat($('#batchEntryModal_ph').val());
   if (!$.isNumeric(ph) || ph > 30 || ph < .8) {
      ph = null;
   };

   // validate TA
   var ta = parseFloat($('#batchEntryModal_ta').val());
   if (!$.isNumeric(ta) || ta > 30 || ta < .8) {
      ta = null;
   };

   // validate SO2
   var so2 = parseFloat($('#batchEntryModal_so2').val());
   if (!$.isNumeric(so2) || so2 > 30 || so2 < .8) {
      so2 = null;
   };

   const uri = '/api/journal';
   var jwt = $('#BatchJwt').val();

   var batchEntry = {
      //BatchId: id,
      Racked: $('#batchEntryModal_racked').is(':checked'),
      Filtered: $('#batchEntryModal_filtered').is(':checked'),
      Bottled: $('#batchEntryModal_bottled').is(':checked'),
      //EntryDateTime: new Date(Date.now()) ,
      ActionDateTime: aDate,
      Temp: temp,
      TempUomId: tempUom,
      Sugar: sugar,
      SugarUomId: sugarUom,
      pH: ph,
      Ta: ta,
      So2: so2,
      Additions: $('#batchEntryModal_additions').val(),
      Comments: $('#batchEntryModal_comments').val(),
   };

   $.ajax({
      url: rootUri + uri + '/batchEntry/' + id,
      type: 'PUT',
      headers: { 'Authorization': 'Bearer ' + jwt },
      contentType: 'application/json',
      data: JSON.stringify(batchEntry),
      success: function (result) {
         // add updated model to local storage        
         localStorage.setItem('latestEntryAdded', JSON.stringify(result));

         console.log('successful batch entry created');

         // clear text
         clearAddEntry();

         // close modal window
         $('#batchEntryToast').toast({ delay: 2000 });
         $('#batchEntryToast').toast('show');
         $('#batchEntryToast').on('hidden.bs.toast', function () {
            $('#batchEntryModal').modal('hide');
         });
      },
      error: function (xmlHttpRequest, textStatus, errorThrown) {
         alert('something went wrong');
         console.log('Exception: ' + errorThrown);
      }
   });


}

function getStatusChart(id) {


   const uri = '/api/journal/batchChart/';
   var jwt = $('#BatchJwt').val();

   $.ajax({
      url: rootUri + uri + id,
      type: 'GET',
      headers: { "Authorization": 'Bearer ' + jwt },
      contentType: 'application/json',
      success: function (data) {
         //console.log('successful chart data call');

         var timeValues = data.times;
         var tempValues = data.temp;
         var sugarValues = data.sugar;

         var tempLineColor = 'rgba(255,0,0,1)';
         var sugarLineColor = 'rgba(0,0,255,1)';

         var config = {
            type: 'line',
            data: {
               labels: timeValues,
               datasets: [{
                  label: 'Sugar',
                  backgroundColor: sugarLineColor,
                  borderColor: sugarLineColor,
                  data: sugarValues,
                  yAxisID: 'y-axis-1',
                  fill: false,
               }, {
                  label: 'Temp',
                  fill: false,
                  backgroundColor: tempLineColor,
                  borderColor: tempLineColor,
                  yAxisID: 'y-axis-2',
                  data: tempValues,
               }]
            },
            options: {
               responsive: true,
               title: {
                  display: true,
                  text: 'Fermentation'
               },
               tooltips: {
                  mode: 'index',
                  intersect: false,
               },
               hover: {
                  mode: 'nearest',
                  intersect: true
               },
               scales: {
                  xAxes: [{
                     display: true,
                     scaleLabel: {
                        display: true,
                        labelString: 'Date'
                     }
                  }],
                  yAxes: [{
                     type: 'linear',
                     display: true,
                     position: 'left',
                     id: 'y-axis-1',
                     scaleLabel: {
                        display: true,
                        labelString: 'Sugar'
                     }
                  }, {
                     type: 'linear',
                     display: true,
                     position: 'right',
                     id: 'y-axis-2',
                     scaleLabel: {
                        display: true,
                        labelString: 'Temp'
                     }
                  }]
               }
            }
         };

         var ctx = $('#statusChart');
         statusChart = new Chart(ctx, config);

      },
      error: function (xmlHttpRequest, textStatus, errorThrown) {
         console.log('Chart Data Exception: ' + errorThrown);
      }
   });

}

function showAddEntry(id) {
   //alert(id);
   $('#batchEntryModal_id').val(id);
   $('#batchEntryModal').modal();
}

function clearAddEntry() {
   $('#batchEntryModal input[type="text"]').val('');
   $('#batchEntryModal textarea').val('');
}

function addToChart(batchEntry) {

   if (batchEntry) {

      if (statusChart) {

         var aDate = void (0);
         if (batchEntry.actionDateTime) {
            aDate = batchEntry.actionDateTime;
         }

         if (aDate) {
            var sugar = void (0);
            // make sugar always SG 
            if (batchEntry.sugarUom) {
               if (batchEntry.sugarUom === 'Brix') {
                  sugar = ConvertBrixToSG(batchEntry.sugar)
               } else {
                  sugar = batchEntry.sugar;
               }
            }
            if (sugar) {
               var temp = void (0);
               // make Temp always F
               if (batchEntry.tempUom) {
                  if (batchEntry.tempUom === 'C') {
                     temp = CelsiusToFahrenheit(batchEntry.temp)
                  } else {
                     temp = batchEntry.temp;
                  }
               }
               if (temp) {
                  //alert('yes');
                  statusChart.data.labels.push(formatDate(aDate));
                  statusChart.data.datasets[0].data.push(sugar);
                  statusChart.data.datasets[1].data.push(temp);
                  statusChart.update();
               } else {
                  //alert('missing temp data');
                  console.log('Chart Update Missing Temp Data');
               }
            } else {
               //alert('missing sugar data');
               console.log('Chart Update Missing Sugar Data');
            }
         } else {
            //alert('missing activity date');
            console.log('Chart Update Missing Activity Date');
         }

      } else {
         //alert('no chart to update');
         console.log('Chart Update No Chart Created');
      }

   } else {
      //alert('no batch entry data for chart');
      console.log('Chart Update No Chart Data');
   }
}

function updateStatus(batchEntry) {

   if (batchEntry.racked === true) {
      $('#summaryRacked').removeClass('d-none');
      $('#summaryRackedOnDate').html(formatDate(batchEntry.actionDateTime));
   }

   if (batchEntry.filtered) {
      $('#summaryFiltered').removeClass('d-none');
      $('#summaryFilteredOnDate').html(formatDate(batchEntry.actionDateTime));
   }

   if (batchEntry.bottled) {
      $('#summaryBotted').removeClass('d-none');
      $('#summaryBottedOnDate').html(formatDate(batchEntry.actionDateTime));
   }

   if (batchEntry.sugarUom) {
      $('#summarySugar').removeClass('d-none');
      $('#summarySugarOnValue').html(batchEntry.sugar);
      $('#summarySugarOnUom').html(batchEntry.sugarUom);
      $('#summarySugarOnDate').html(formatDate(batchEntry.actionDateTime));
   }

   if (batchEntry.tempUom) {
      $('#summaryTemp').removeClass('d-none');
      $('#summaryTempOnValue').html(batchEntry.temp);
      $('#summaryTempOnUom').html(batchEntry.tempUom);
      $('#summaryTempOnDate').html(formatDate(batchEntry.actionDateTime));
   }

   if (batchEntry.so2) {
      $('#summarySo2').removeClass('d-none');
      $('#summarySo2OnValue').html(batchEntry.so2);
      $('#summarySo2OnDate').html(formatDate(batchEntry.actionDateTime));
   }

   if (batchEntry.pH) {
      $('#summarypH').removeClass('d-none');
      $('#summarypHOnValue').html(batchEntry.pH);
      $('#summarypHOnDate').html(formatDate(batchEntry.actionDateTime));
   }

   if (batchEntry.ta) {
      $('#summaryTa').removeClass('d-none');
      $('#summaryTaOnValue').html(batchEntry.ta);
      $('#summaryTaOnDate').html(formatDate(batchEntry.actionDateTime));
   }

   if (batchEntry.comments) {
      $('#summaryComments').removeClass('d-none');
      $('#summaryCommentsOnValue').html(batchEntry.comments);
      $('#summaryCommentsOnDate').html(formatDate(batchEntry.actionDateTime));
   }

}

function buildDisplayEntry(entry) {

   if (entry) {

      const card = document.createElement('div');
      card.className = 'card flex-grow-1';

      const cardBody = document.createElement('div');
      cardBody.className = 'card-body';

      // date line
      var aDate = new Date(entry.entryDateTime);
      if (entry.actionDateTime) {
         aDate = new Date(entry.actionDateTime);
      }
      const cardTitle = document.createElement('div');
      cardTitle.className = 'd-flex card-title h5';
      cardTitle.innerHTML = formatDateTime(aDate) + '&nbsp;';
      const delIcon = document.createElement('i');
      delIcon.className = 'fa fa-trash-o';
      const delEntryButton = document.createElement('button');
      delEntryButton.className = 'btn btn-outline-secondary btn-sm';
      delEntryButton.setAttribute('name', 'delBatchEntryButton');
      delEntryButton.setAttribute('data-id', entry.id);
      delEntryButton.appendChild(delIcon);
      cardTitle.appendChild(delEntryButton);
      cardBody.appendChild(cardTitle);

      // measurements line
      const cardMeasurements = document.createElement('div');
      cardMeasurements.className = 'd-flex ml-3';

      if (entry.sugar && entry.sugarUom) {
         const cardSugar = document.createElement('div');
         cardSugar.className = 'p-2';
         cardSugar.innerHTML = '<strong>' + entry.sugarUom + '</strong>: ' + entry.sugar;
         cardMeasurements.appendChild(cardSugar);
      }

      if (entry.pH) {
         const cardPh = document.createElement('div');
         cardPh.className = 'p-2';
         cardPh.innerHTML = '<strong>pH</strong>: ' + entry.pH;
         cardMeasurements.appendChild(cardPh);
      }

      if (entry.ta) {
         const cardTa = document.createElement('div');
         cardTa.className = 'p-2';
         cardTa.innerHTML = '<strong>TA</strong>: ' + entry.ta + ' g/L';
         cardMeasurements.appendChild(cardTa);
      }

      if (entry.so2) {
         const cardSo2 = document.createElement('div');
         cardSo2.className = 'p-2';
         cardSo2.innerHTML = '<strong>Sulfite</strong>: ' + entry.so2 + ' mg/L';
         cardMeasurements.appendChild(cardSo2);
      }

      if (entry.temp && entry.tempUom) {
         const cardTemp = document.createElement('div');
         cardTemp.className = 'p-2';
         cardTemp.innerHTML = '<strong>Temp</strong>: ' + entry.temp + ' &deg;' + entry.tempUom;
         cardMeasurements.appendChild(cardTemp);
      }

      cardBody.appendChild(cardMeasurements);

      // racked line
      const cardRacking = document.createElement('div');
      cardRacking.className = 'd-flex ml-3';

      if (entry.racked === true) {
         const cardRacked = document.createElement('div');
         cardRacked.className = 'p-2';
         cardRacked.innerHTML = '<strong>Racked</strong>';
         cardRacking.appendChild(cardRacked);
      }
      if (entry.filtered === true) {
         const cardFiltered = document.createElement('div');
         cardFiltered.className = 'p-2';
         cardFiltered.innerHTML = '<strong>Filtered</strong>';
         cardRacking.appendChild(cardFiltered);
      }
      if (entry.bottled === true) {
         const cardBottled = document.createElement('div');
         cardBottled.className = 'p-2';
         cardBottled.innerHTML = '<strong>Bottled</strong>';
         cardRacking.appendChild(cardBottled);
      }

      cardBody.appendChild(cardRacking);

      // additions line
      if (entry.additions) {
         const cardAdditions = document.createElement('div');
         cardAdditions.className = 'd-flex ml-3';
         cardAdditions.innerHTML = '<strong class="pl-2">Additions</strong>: ';
         const cardAdditionText = document.createElement('textarea');
         cardAdditionText.className = 'form-control border-0 pl-2 pt-0 bg-transparent';
         cardAdditionText.setAttribute('rows', 3);
         cardAdditionText.setAttribute('disabled', '');
         cardAdditionText.innerHTML = entry.additions;
         cardAdditions.appendChild(cardAdditionText);
         cardBody.appendChild(cardAdditions);
      }

      // comments line
      if (entry.comments) {
         const cardComments = document.createElement('div');
         cardComments.className = 'd-flex ml-3';
         cardComments.innerHTML = '<strong class="pl-2">Comments</strong>: ';
         const cardCommentText = document.createElement('textarea');
         cardCommentText.className = 'form-control border-0 pl-2 pt-0 bg-transparent';
         cardCommentText.setAttribute('rows', 3);
         cardCommentText.setAttribute('disabled', '');
         cardCommentText.innerHTML = entry.comments;
         cardComments.appendChild(cardCommentText);
         cardBody.appendChild(cardComments);
      }

      card.appendChild(cardBody);

      document.getElementById('batchEntries').prepend(card);

   };

   // clear out storage 
   localStorage.removeItem('latestEntryAdded')
}

