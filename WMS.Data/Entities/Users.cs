using System;
using System.Collections.Generic;

namespace WMS.Data.Entities
{
    public partial class Users
    {
        public Users()
        {
            Recipes = new HashSet<Recipes>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool? IsAdmin { get; set; }

        public ICollection<Recipes> Recipes { get; set; }
    }
}
