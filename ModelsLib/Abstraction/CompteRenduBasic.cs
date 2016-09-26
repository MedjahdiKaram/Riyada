using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfacesLib.Shared;


namespace ModelsLib.Abstraction
{
    [Serializable]
    public class CompteRenduBasic:IEntity
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public List<string> ValeursPossible { get; set; }
    }
}
