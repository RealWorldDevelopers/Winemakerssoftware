﻿
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace WMS.Ui.Mvc6.Models.Admin
{
   public class UserViewModel : ApplicationUser
   {
      public UserViewModel()
      {
         MemberRoles = new List<string>();
         AllRoles = new List<SelectListItem>();
      }

      public bool IsAdmin { get; set; }
      public bool IsLockedOut { get; set; }
      public string? LockOutLocalTime
      {
         get
         {
            if (LockoutEnd.HasValue)
               return LockoutEnd.Value.ToLocalTime().ToString("F", CultureInfo.CurrentCulture);
            else
               return string.Empty;
         }
      }

      public List<string> MemberRoles { get; }
      public List<SelectListItem> AllRoles { get; }
      public string? NewRole { get; set; }

   }

}
