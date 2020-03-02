
importScripts('/lib/sw-toolbox/sw-toolbox.js');

const spCaches = {
   'static': 'wms-static-v3',
   'dynamic': 'wms-dynamic-v3'
};

var OFFLINEFILE = 'fallBack.html';
const staticMaxAge = 60 * 60 * 24 * 30; // seconds x minutes x hours x days = age // this is one month
const dynamicTimeoutSeconds = 4;
const dynamicMaxEntries = 50;

var precacheFiles = [
   OFFLINEFILE
];


// Install stage sets up the cache-array to configure precache content
self.addEventListener('install', function (evt) {
   console.log('The service worker is being installed');
   evt.waitUntil(precache().then(function () {
      console.log('Skip waiting on install');
      return self.skipWaiting();
   }));
});

// permanent cache
function precache() {
   console.log('filling precache');
   return caches.open(spCaches.static).then(function (cache) {
      return cache.addAll(precacheFiles);
   });
}

self.addEventListener('activate', function (event) {
   console.log('The service worker is being activated at');
   event.waitUntil(
      caches.keys()
         .then(function (keys) {
            return Promise.all(keys.filter(function (key) {
               //return key !== CACHE;
               return !Object.values(spCaches).includes(key);
            }).map(function (key) {
               return caches.delete(key);
            }));
         }));
});


// no cache calls
toolbox.router.any(/\/api\/.+/i, toolbox.networkOnly);
toolbox.router.post(/\/account\/.+/i, toolbox.networkOnly);
toolbox.router.post(/\/recipes\/.+/i, toolbox.networkOnly);
toolbox.router.any(/\/admin.*/i, toolbox.networkOnly);

//cache only calls
toolbox.router.any(/fallBack.html/i, toolbox.cacheOnly);


// long term cache
toolbox.router.get(/.+\.js/i, toolbox.cacheFirst, {
   cache: {
      name: spCaches.static,
      maxAgeSeconds: staticMaxAge
   }
});

toolbox.router.get(/.+\.css/i, toolbox.cacheFirst, {
   cache: {
      name: spCaches.static,
      maxAgeSeconds: staticMaxAge
   }
});

toolbox.router.get(/.+\.(png|jpg|bmp|svg)/i, toolbox.cacheFirst, {
   cache: {
      name: spCaches.static,
      maxAgeSeconds: staticMaxAge
   }
});

// streamed images
toolbox.router.get(/.+\/ViewImage\/\d+/i, toolbox.cacheFirst, {
   cache: {
      name: spCaches.static,
      maxAgeSeconds: staticMaxAge
   }
});

toolbox.router.get(/.+\/ViewThumbnail\/\d+/i, toolbox.cacheFirst, {
   cache: {
      name: spCaches.static,
      maxAgeSeconds: staticMaxAge
   }
});

// short term cache
toolbox.router.get(/^https:\/\/.*winemakerssoftware\.com$/i, function (request, values, options) {
   return toolbox.networkFirst(request, values, options)
      .catch(function (err) {
         console.log(err);
         return caches.match(OFFLINEFILE);
      });
}, {
   networkTimeoutSeconds: dynamicTimeoutSeconds,
   cache: {
      name: spCaches.dynamic,
      maxEntries: dynamicMaxEntries
   }
}
);


toolbox.router.get(/^https:\/\/.+\/Recipes(\/Recipe\/\d+)?$/i, function (request, values, options) {
   return toolbox.networkFirst(request, values, options)
      .catch(function (err) {
         console.log(err);
         return caches.match(OFFLINEFILE);
      });
}, {
   networkTimeoutSeconds: dynamicTimeoutSeconds,
   cache: {
      name: spCaches.dynamic,
      maxEntries: dynamicMaxEntries
   }
}
);

toolbox.router.get(/^https:\/\/.+\/YeastPicker$/i, function (request, values, options) {
   return toolbox.networkFirst(request, values, options)
      .catch(function (err) {
         console.log(err);
         return caches.match(OFFLINEFILE);
      });
}, {
   networkTimeoutSeconds: dynamicTimeoutSeconds,
   cache: {
      name: spCaches.dynamic,
      maxEntries: dynamicMaxEntries
   }
}
);

toolbox.router.get(/^https:\/\/.+\/Conversions$/i, function (request, values, options) {
   return toolbox.networkFirst(request, values, options)
      .catch(function (err) {
         console.log(err);
         return caches.match(OFFLINEFILE);
      });
}, {
   networkTimeoutSeconds: dynamicTimeoutSeconds,
   cache: {
      name: spCaches.dynamic,
      maxEntries: dynamicMaxEntries
   }
}
);

toolbox.router.get(/^https:\/\/.+\/Calculations$/i, function (request, values, options) {
   return toolbox.networkFirst(request, values, options)
      .catch(function (err) {
         console.log(err);
         return caches.match(OFFLINEFILE);
      });
}, {
   networkTimeoutSeconds: dynamicTimeoutSeconds,
   cache: {
      name: spCaches.dynamic,
      maxEntries: dynamicMaxEntries
   }
}
);

toolbox.router.get(/^https:\/\/.+\/About$/i, function (request, values, options) {
   return toolbox.networkFirst(request, values, options)
      .catch(function (err) {
         console.log(err);
         return caches.match(OFFLINEFILE);
      });
}, {
   networkTimeoutSeconds: dynamicTimeoutSeconds,
   cache: {
      name: spCaches.dynamic,
      maxEntries: dynamicMaxEntries
   }
}
);

// TODO add journal when complete

toolbox.router.default = toolbox.networkOnly
