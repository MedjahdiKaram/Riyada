using InterfacesLib.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLib
{
    public class Subcription : IEntity
    {
        [Key]
        public int Id                          { get; set; }
        public Client Client                   { get; set; }
        public SubcriptionType SubcriptionType { get; set; } 
        public DateTime StartDate              { get; set; }
        public DateTime EndDate { get; set; }
    }
}
