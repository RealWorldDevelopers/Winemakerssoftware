
(function ($) {

   // not equal to
   $.validator.addMethod('notequalto',
      function (value, element, params) {
         if (this.optional(element)) return true;
         var props = params[0].split(',');
         for (var i in props) {

            var el = $('#' + props[i]);
            var testVal = el.val();

            // if both numeric
            if (!isNaN(testVal) && !isNaN(value)) {
               if (parseFloat(testVal) === parseFloat(value)) return false;
            } else {
               if (testVal === value) return false;
            }

         }
         return true;
      });
   $.validator.unobtrusive.adapters.add('notequalto', ['dependentproperty'],
      function (options) {
         options.rules['notequalto'] = [options.params['dependentproperty']];
         options.messages['notequalto'] = options.message;
      });

   // require or
   jQuery.validator.addMethod('requiredor',
      function (value, element, params) {
         if (value === '') {
            var $other = $('#' + params);
            return $other.val() !== '';
         }
         return true;
      });
   $.validator.unobtrusive.adapters.add('requiredor', ['dependentproperty'],
      function (options) {
         options.rules['requiredor'] = [options.params['dependentproperty']];
         options.messages['requiredor'] = options.message;
      });

   // require if
   jQuery.validator.addMethod('requiredif',
      function (value, element, params) {
         if (value !== '') return true;
         var prop = $('#' + params[0]);
         var otherVal = prop.attr('type').toUpperCase() === 'CHECKBOX' ? prop.attr('checked') ? 'true' : 'false' : prop.val();
         var testVal = params[2];

         // if both numeric
         if (!isNaN(testVal) && !isNaN(otherVal)) {
            return params[1] === 'isequalto' ? parseFloat(otherVal) !== parseFloat(testVal) : parseFloat(otherVal) === parseFloat(testVal);
         } else {
            return params[1] === 'isequalto' ? otherVal !== testVal : otherVal === testVal;
         }

      });
   $.validator.unobtrusive.adapters.add('requiredif', ['dependentproperty', 'comparison', 'dependentvalue'],
      function (options) {
         options.rules['requiredif'] = [options.params['dependentproperty'], options.params['comparison'], options.params['dependentvalue']];
         options.messages['requiredif'] = options.message;
      });

   // Range if 
   jQuery.validator.addMethod('rangeif',
      function (value, element, params) {
         if (this.optional(element)) return true;

         var required;
         var prop = $('#' + params[0]);
         var otherVal = prop.attr('type').toUpperCase() === 'CHECKBOX' ? prop.attr('checked') ? 'true' : 'false' : prop.val();
         var testVal = params[2];
         var min = params[3];
         var max = params[4];

         // if both numeric
         if (!isNaN(testVal) && !isNaN(otherVal)) {
            required = params[1] === 'isequalto' ? parseFloat(otherVal) === parseFloat(testVal) : parseFloat(otherVal) !== parseFloat(testVal);
         } else {
            required = params[1] === 'isequalto' ? otherVal === testVal : otherVal !== testVal;
         }

         if (required) {
            // test for Range
            return parseFloat(value) >= parseFloat(min) && parseFloat(value) <= parseFloat(max);
         }

         return true;
      });
   $.validator.unobtrusive.adapters.add('rangeif', ['dependentproperty', 'comparison', 'dependentvalue', 'min', 'max'],
      function (options) {
         options.rules['rangeif'] = [options.params['dependentproperty'], options.params['comparison'], options.params['dependentvalue'], options.params['min'], options.params['max']];
         options.messages['rangeif'] = options.message;
      });

}(jQuery));