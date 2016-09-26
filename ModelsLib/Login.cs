using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacesLib.Shared;
using System.ComponentModel.DataAnnotations;

namespace ModelsLib
{
    public class Login:IEntity
    {
        [Key]
        public int Id              { get; set; }
        public string Name         { get; set; }
                            
        public string Serial       { get; set; }
                           
        public string Hash { get; set; }
    }
}
