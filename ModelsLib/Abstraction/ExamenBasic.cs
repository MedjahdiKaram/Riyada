using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using InterfacesLib.Shared;


namespace ModelsLib.Abstraction
{
    [Serializable]
 
    public class ExamenBasic : IEntity
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("Nom")]
        public string Nom { get; set; }
         [XmlElement("ComptesRendusPresents")]
        public List<CompteRenduBasic> ComptesRendusPresents { get; set; }
    }
}
