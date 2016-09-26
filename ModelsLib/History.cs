using InterfacesLib.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLib
{
    public class History:IEntity
    {
        public int Id          { get; set; }
        public Client Client   { get; set; }
        public DateTime Record { get; set; }
    }
}
