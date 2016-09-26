using InterfacesLib.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLib
{
    public class Payement : IEntity
    {
        public int Id                   { get; set; }
        public DateTime Moment          { get; set; }
        public double Amount            { get; set; }
        public Subcription Subription   { get; set; }
        public History History { get; set; }

    }
}
