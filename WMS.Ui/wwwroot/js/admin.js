$(document).ready(function () {

   // show a particular tab on load
   if ($('#TabToShow').length) {
      var idx = $('#TabToShow').val();
      if (idx.length) {
         $('#' + idx).tab('show');
      };
   };

   $('body').off('click', 'button[name="editRecipeButton"]');
   $('body').on('click', 'button[name="editRecipeButton"]', function () {
      var id = $(this).data('id');
      $('#editRecipeId').val(id);
      $('#frmEditRecipe').submit();
   });

   $('body').off('click', 'button[name="deleteRecipeButton"]');
   $('body').on('click', 'button[name="deleteRecipeButton"]', function () {
      if (confirm('Are you sure?')) {
         var id = $(this).data('id');
         $('#deleteRecipeId').val(id);
         $('#frmDeleteRecipe').submit();
      }
   });

   // file selection - show file value after file select 
   $('body').off('change', '.custom-file-input');
   $('body').on('change', '.custom-file-input', function () {
      let fileName = $(this).val().split('\\').pop();
      var label = $('label[for="' + $(this).attr('id') + '"]');
      label.addClass("selected").html(fileName);
   });

   // file selection - reset file value after reset button click 
   $('body').off('click', "button[type = 'reset']");
   $('body').on('click', "button[type = 'reset']", function () {
      $(".custom-file-label").html('Choose file to upload');
   });


   $('body').off('click', 'button[name="deleteRecipeImageButton"]');
   $('body').on('click', 'button[name="deleteRecipeImageButton"]', function () {
      if (confirm('Are you sure?')) {
         var id = $(this).data('id');
         $('#deleteRecipeImageId').val(id);
         $('#frmDeleteRecipeImage').submit();
      }
   });


   $('body').off('click', 'button[name="editYeastPairingButton"]');
   $('body').on('click', 'button[name="editYeastPairingButton"]', function () {
      var id = $(this).data('id');
      $('#editYeastPairingId').val(id);
      $('#frmEditYeastPairing').submit();
   });

   $('body').off('click', 'button[name="deleteYeastPairingButton"]');
   $('body').on('click', 'button[name="deleteYeastPairingButton"]', function () {
      if (confirm('Are you sure?')) {
         var id = $(this).data('id');
         $('#deleteYeastPairingId').val(id);
         $('#frmDeleteYeastPairing').submit();
      }
   });

   $('body').off('click', 'button[name="editYeastButton"]');
   $('body').on('click', 'button[name="editYeastButton"]', function () {
      var id = $(this).data('id');
      $('#editYeastId').val(id);
      $('#frmEditYeast').submit();
   });

   $('body').off('click', 'button[name="editMaloButton"]');
   $('body').on('click', 'button[name="editMaloButton"]', function () {      
      var id = $(this).data('id');
      $('#editMaloId').val(id);
      $('#frmEditMalo').submit();
   });

   $('body').off('click', 'button[name="deleteMaloButton"]');
   $('body').on('click', 'button[name="deleteMaloButton"]', function () {
      if (confirm('Are you sure?')) {
         var id = $(this).data('id');
         $('#deleteMaloId').val(id);
         $('#frmDeleteMalo').submit();
      }
   });

   $('body').off('click', 'button[name="deleteYeastButton"]');
   $('body').on('click', 'button[name="deleteYeastButton"]', function () {
      if (confirm('Are you sure?')) {
         var id = $(this).data('id');
         $('#deleteYeastId').val(id);
         $('#frmDeleteYeast').submit();
      }
   });

   $('body').off('click', 'button[name="editVarietyButton"]');
   $('body').on('click', 'button[name="editVarietyButton"]', function () {
      var id = $(this).data('id');
      $('#editVarietyId').val(id);
      $('#frmEditVariety').submit();
   });

   $('body').off('click', 'button[name="deleteVarietyButton"]');
   $('body').on('click', 'button[name="deleteVarietyButton"]', function () {
      if (confirm('Are you sure?')) {
         var id = $(this).data('id');
         $('#deleteVarietyId').val(id);
         $('#frmDeleteVariety').submit();
      }
   });

   $('body').off('click', 'a[name="editVarietyButton"]');
   $('body').on('click', 'a[name="editVarietyButton"]', function () {
      var id = $(this).data('id');
      $('#editVarietyId').val(id);
      $('#frmEditVariety').submit();
   });

   $('body').off('click', 'button[name="editCategoryButton"]');
   $('body').on('click', 'button[name="editCategoryButton"]', function () {
      var id = $(this).data('id');
      $('#editCategoryId').val(id);
      $('#frmEditCategory').submit();
   });

   $('body').off('click', 'button[name="deleteCategoryButton"]');
   $('body').on('click', 'button[name="deleteCategoryButton"]', function () {
      if (confirm('Are you sure?')) {
         var id = $(this).data('id');
         $('#deleteCategoryId').val(id);
         $('#frmDeleteCategory').submit();
      }
   });

   $('body').off('click', 'button[name="deleteRoleButton"]');
   $('body').on('click', 'button[name="deleteRoleButton"]', function () {
      if (confirm('Are you sure?')) {
         var id = $(this).data('name');
         $('#deleteRoleName').val(id);
         $('#frmDeleteRole').submit();
      }
   });

   $('body').off('click', 'button[name="lockUserButton"]');
   $('body').on('click', 'button[name="lockUserButton"]', function () {
      var id = $(this).data('name');
      $('#lockUserName').val(id);
      $('#frmLockUser').submit();
   });

   $('body').off('click', 'button[name="unlockUserButton"]');
   $('body').on('click', 'button[name="unlockUserButton"]', function () {
      var id = $(this).data('name');
      $('#unlockUserName').val(id);
      $('#frmUnlockUser').submit();
   });

   $('body').off('click', 'button[name="editUserButton"]');
   $('body').on('click', 'button[name="editUserButton"]', function () {
      var id = $(this).data('name');
      $('#editUserName').val(id);
      $('#frmEditUser').submit();
   });

   $('body').off('click', 'button[name="deleteUserButton"]');
   $('body').on('click', 'button[name="deleteUserButton"]', function () {
      if (confirm('Are you sure?')) {
         var id = $(this).data('name');
         $('#deleteUserName').val(id);
         $('#frmDeleteUser').submit();
      }
   });

   $('body').off('click', 'button[name="deleteUserRoleButton"]');
   $('body').on('click', 'button[name="deleteUserRoleButton"]', function () {
      if (confirm('Are you sure?')) {
         var id = $(this).data('name');
         $('#deleteUserRoleName').val(id);
         $('#frmDeleteUserRole').submit();
      }
   });

   //search users bar functions
   $('body').off('keypress', '#userSearch');
   $('body').on('keypress', '#userSearch', function (e) {
      return IsAlphaNumeric(e);
   });

   $('body').off('keyup', '#userSearch');
   $('body').on('keyup', '#userSearch', function (e) {
      filterUsers(e);
   });

   $('body').off('paste', '#userSearch');
   $('body').on('paste', '#userSearch', function (e) {
      var element = this;
      setTimeout(function () {
         var text = $(element).val();
         $(element).val(text.replace(/[^ a-zA-Z0-9]/g, ''));
      }, 100);
   });

   //search recipes bar functions
   $('body').off('keypress', '#recipesSearch');
   $('body').on('keypress', '#recipesSearch', function (e) {
      return IsAlphaNumeric(e);
   });

   $('body').off('keyup', '#recipesSearch');
   $('body').on('keyup', '#recipesSearch', function (e) {
      filterRecipe(e);
   });

   $('body').off('paste', '#recipesSearch');
   $('body').on('paste', '#recipesSearch', function (e) {
      var element = this;
      setTimeout(function () {
         var text = $(element).val();
         $(element).val(text.replace(/[^ a-zA-Z0-9]/g, ''));
      }, 100);
   });


   // alert('admin page opened');
});

function filterUsers() {

   var input, filter, ul, li, li2, a, i;
   input = document.getElementById('userSearch');
   filter = input.value.toUpperCase();

   var partial = '';
   var list = filter.split(' ');

   for (i = 0; i < list.length; i++) {
      if (list[i] != '') {
         var pre = partial;
         var word = '(?=.*\\b'.concat(list[i], '\\w*\\b)');
         partial = pre.concat(word);
      }
   };

   var pattern = partial.concat('.*');
   var regEx = new RegExp(pattern, 'i');

   ul = document.getElementById('ulUsers');
   li = ul.getElementsByTagName('li');
   for (i = 1; i < li.length; i++) {

      var cls = li[i].classList;
      if (cls.contains('d-none')) {
         cls.remove('d-none')
      }

      var testData = li[i].textContent;
      testData = testData.replace(/(\W)/gm, ' x ');
      if (!regEx.test(testData)) {
         cls.add('d-none');
      }
   };
}

function filterRecipe() {

   var input, filter, ul, li, li2, a, i;
   input = document.getElementById('recipesSearch');
   filter = input.value.toUpperCase();

   var partial = '';
   var list = filter.split(' ');

   for (i = 0; i < list.length; i++) {
      if (list[i] != '') {
         var pre = partial;
         var word = '(?=.*\\b'.concat(list[i], '\\w*\\b)');
         partial = pre.concat(word);
      }
   };

   var pattern = partial.concat('.*');
   var regEx = new RegExp(pattern, 'i');

   ul = document.getElementById('ulRecipe');
   li = ul.getElementsByTagName('li');
   for (i = 1; i < li.length; i++) {

      var cls = li[i].classList;
      if (cls.contains('d-none')) {
         cls.remove('d-none')
      }

      var testData = li[i].textContent;
      testData = testData.replace(/(\W)/gm, ' x ');
      if (!regEx.test(testData)) {
         cls.add('d-none');
      }
   };
}





