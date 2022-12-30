using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Data.CosmosDB.Interfaces
{
   public interface IIdentityService
   {
      Guid GetUserIdentity();
      string GetUserEmail();

   }
}
