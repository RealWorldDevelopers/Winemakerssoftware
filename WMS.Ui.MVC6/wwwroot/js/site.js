// TODO
//// register the service worker first
//if ('serviceWorker' in navigator) {
//   navigator.serviceWorker
//      .register('/sw.js', { scope: '/' })
//      .catch(function (err) {
//         console.log('Service Worker did not register', err)
//      });
//} else { console.log('Active service worker found, no need to register') }


// configure the add to home screen functions
window.addEventListener('appinstalled', function () {
   console.log('Application installed.');
});

window.addEventListener('beforeinstallprompt', function (evt) {
   evt.preventDefault();
   promptEvt = evt;
   return false;
});

// capture rootUri for later use
if (!window.location.origin) {
   window.location.origin = window.location.protocol + '//'
      + window.location.hostname
      + (window.location.port ? ':' + window.location.port : '');
}
var rootUri = window.location.origin;
if (rootUri.substr(-1) === '/') {
   rootUri = rootUri.substr(0, rootUri.length - 1);
}

var cssAlert_Error = 'alert alert-danger';
var cssAlert_Success = 'alert alert-success';
var cssAlert_Info = 'alert alert-info';
var cssAlert_Warning = 'alert alert-warning';

var specialKeys = new Array();
specialKeys.push(8); //Backspace
specialKeys.push(9); //Tab
specialKeys.push(46); //Delete
specialKeys.push(36); //Home
specialKeys.push(35); //End
specialKeys.push(37); //Left
specialKeys.push(39); //Right
specialKeys.push(32); //(Space)


$(document).ready(function () {

   $(window).bind('beforeunload', function () {
      showLoader();
   });

   // CSP: disable automatic style injection on chartJS objects
   Chart.platform.disableCSSInjection = true;

   // hover effect images
   $('body').off('click', 'a.info');
   $('body').on('click', 'a.info', function () {
      showPage();
      event.preventDefault();
      var el = $(this);
      onImageClick(this.id, el.data('src'), el.data('alt'), el.data('title'), el.data('caption'));
   });

   // click event of print icon
   $('body').off('click', 'a.printer');
   $('body').on('click', 'a.printer', function () {
      showPage();
      window.print();
   });

   // initialize tooltips
   $('[data-toggle="tooltip"]').tooltip();

   // initialize popovers
   $('[data-toggle="popover"]').popover();

   // Prompt user to install app
   if (typeof promptEvt !== 'undefined') {
      promptEvt.prompt();
   }

});

function showLoader() {
   $('#loader').removeClass('d-none');
   $('#bodyContent').addClass('d-none');
}

function showPage() {
   $('#loader').addClass('d-none');
   $('#bodyContent').removeClass('d-none');
}

function onImageClick(id, src, alt, title, caption) {
   //alert('it works' + id);
   document.getElementById('modalImageDisplay_img').src = src;
   document.getElementById('modalImageDisplay_img').alt = alt;
   document.getElementById('modalImageDisplay_img').title = title;
   var captionText = document.getElementById('caption');
   captionText.innerHTML = caption;
   $('#modalImageDisplay').modal();
}

function showAlert(msg, css, dimissable) {
   var dismiss = '<button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>';
   if (!dimissable) dismiss = '';
   var display = '<div class="' + css + '">' + dismiss + msg + '</div>';
   $('#alertSection').append(display);
}

function IsAlphaNumeric(e) {
   var keyCode = e.keyCode === 0 ? e.charCode : e.keyCode;
   var ret = ((keyCode === 32) ||             // (Space)
      (keyCode >= 48 && keyCode <= 57) ||   // numeric (0-9)
      (keyCode >= 65 && keyCode <= 90) ||   // upper alpha (A-Z)
      (keyCode >= 97 && keyCode <= 122) ||  // lower alpha (a-z)
      (specialKeys.indexOf(e.keyCode) !== -1 && e.charCode !== e.keyCode));
   return ret;
}

function formatNumericForDisplay(num, places, fixed) {
   try {
      var factor = Math.pow(10, places);
      var newNum = Math.round(num * factor) / factor;
      if (fixed) {
         newNum = newNum.toFixed(places);
      }
      return newNum.toLocaleString('en');
   } catch (err) {
      console.error(err);
      return num;
   }
}

function clearValidation(formId) {
   // get the form inside we are working - change selector to your form as needed
   var form = $('#' + formId);

   // get validator object
   var validator = form.validate();

   // get errors that were created using jQuery.validate.unobtrusive
   var errors = form.find('.field-validation-error span');

   // trick unobtrusive to think the elements were successfully validated
   // this removes the validation messages
   errors.each(function () { validator.settings.success($(this)); });

   // clear errors from validation
   validator.resetForm();
}

function formatDate(date) {
   //alert(date);
   var dt = new Date(date);
   var mon = dt.getMonth() + 1;
   var day = dt.getDate();
   var yr = dt.getFullYear();
   var ds = mon + '/' + day + '/' + yr;
   return ds;
}

var formatDateTime = function (date) {
   //alert(date);
   var dt = new Date(date);
   var mon = dt.getMonth() + 1;
   var day = dt.getDate();
   var yr = dt.getFullYear();
   var hr = dt.getHours();
   var min = dt.getMinutes();
   var tz = (hr < 12 ? 'AM' : 'PM');
   hr = (hr > 12 ? (hr - 12) : hr);

   ds = mon + '/' + day + '/' + yr + ' ' + ('0' + hr).slice(-2) + ':' + ('0' + min).slice(-2) + " " + tz;
   return ds;
}

// TODO use to grey scale objects not in the cache when offline (SW Toolbox video) like available menu choices
//function() {
//    if (!navigator.onLine) {
//        var list = document.querySelector('.pod-list');
//        list.classList.add('offline');

//        caches.open('dynamic-v5')
//            .then(c => c.keys())
//            .then(keys => keys.map(key => {
//                var urlParser = document.createElement('a');
//                urlParser.href = key.url;
//                return urlParser.pathname;
//            }))
//            .then(urls => urls.forEach(url => {
//                var element = document.querySelector('.pod-list a[href=' + { url } + '] .sprite');
//                if (element)
//                    element.style.filter = 'grayscale(0)';
//            }));
//    }
//}

// TODO use to background sync submits or contact emails
//document.getElementById('form-now').addEventListener('submit', function (evt) {
//    evt.preventDefault();
//    disableButton();
//    checkAuth(evt);

//    indexedDB.open('sodapooped', 1).onsuccess = evt => {
//        var reviews = evt.target.result.transaction(['reviews'], 'readwrite').objectStore('reviews'),
//            image = picture.files ? picture.files[0] : null,
//            val = {
//                key: key,
//                drindId: drinkId,
//                drinkSlug: drink,
//                rating: Number.parseInt(rating.value),
//                comments: comments.value,
//                picture: image,
//                isSynced: false
//            },
//            operation = reviews.put(val);

//        operation.onsuccess = () => {
//            if ('serviceWorker' in navigator && 'SyncManager' in window) {
//                navigator.serviceWorker.ready.then(sw => {
//                    return sw.sync.register('sync-reviews').then(updateUi(val));
//                });
//            } else {
//                // send data to server via ajax
//            }
//        };
//    };
//});





