using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacesLib.Shared;
using ModelsLib;

namespace Riyada.Helpers
{
    class ClientComparator : IEqualityComparer<Client>
    {
        

        public bool Equals(Client x, Client y)
        {
           return x.Id==y.Id;
        }

        public int GetHashCode(Client obj)
        {
            var hashCode = obj.GetHashCode();
            return hashCode;
        }
    }
}
