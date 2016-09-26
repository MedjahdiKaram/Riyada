using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using InterfacesLib.Repository;
using ModelsLib;

namespace Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var ctx=new GlobalContext();
            var repo = new Repository<Client>(ctx);
            repo.Add(new Client {Name = "Hmida", Sex = 'H', DateOfBirth = DateTime.Now});
            ctx.Dispose();
        }
    }
}
