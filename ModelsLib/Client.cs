using InterfacesLib.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLib
{
    public class Client : IEntity
    {
        [Key]
        public int Id                { get; set; }
        public string Name           { get; set; }
        public string LastName       { get; set; }
        public char Sex              { get; set; }
        public DateTime DateOfBirth  { get; set; }
        public int IdentityId        { get; set; }
        public string Phone          { get; set; }
        public string TutorFullName  { get; set; }
        public string TutorPhone     { get; set; }
        public string QrCodeImagePath { get; set; }

    }
}
