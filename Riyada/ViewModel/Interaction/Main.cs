using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using GalaSoft.MvvmLight.Ioc;
using ModelsLib;
using Riyada.Helpers;

namespace Riyada.ViewModel.Interaction
{
    public class Main
    {

        public static Repository<Login> LoginsRepository { get; set; }
        public static Repository<Client> ClientsRepository                    { get; set; }
        public static Repository<History> HistoriesRepository                 { get; set; }
        public static Repository<Subcription> SubcriptionsRepository          { get; set; }
        public static Repository<SubcriptionType> SubcriptionTypesRepository  { get; set; }
        public static Repository<Payement> PayementsRepository                { get; set; }

        public ICollection<Client> SearchForClients(string searchTerm)
        {
            var result= new List<Client>();            
            var idOrNum = 0;
            DateTime dateTime;
            var isInt = int.TryParse(searchTerm,out idOrNum);
            if (isInt)
            {
           
                var client = ClientsRepository.Get(idOrNum);
                if (client!=null)
                    result.Add(client);
                result.AddRange(ClientsRepository.Get(cl=>cl.Phone== searchTerm));
                result.AddRange(ClientsRepository.Get(cl=>cl.TutorPhone== searchTerm));
                result.AddRange(ClientsRepository.Get(cl=>cl.IdentityId==idOrNum));
                result = result.Distinct(new ClientComparator()).ToList();
                return result;
            }
            if (searchTerm.Length == 1 && (searchTerm.ToUpper() == "H" || searchTerm.ToUpper() == "F"))
            {
                result.AddRange(ClientsRepository.Get(cl => string.Equals(cl.Sex.ToString(), searchTerm, StringComparison.CurrentCultureIgnoreCase)));
                result = result.Distinct(new ClientComparator()).ToList();
                return result;
            }

            var isDateTime = DateTime.TryParse(searchTerm, out dateTime);
            if (isDateTime)
            {
       
                result.AddRange(ClientsRepository.Get(cl => cl.DateOfBirth.ToShortDateString() == dateTime.ToShortDateString()));
                result = result.Distinct(new ClientComparator()).ToList();
                return result;
            }

            
            result.AddRange(ClientsRepository.Get(n => (n.LastName.ToUpper() + " " + n.Name.ToUpper()).Contains(searchTerm.ToUpper())));
            result.AddRange(ClientsRepository.Get(n => (n.Name.ToUpper() + " " + n.LastName.ToUpper()).Contains(searchTerm.ToUpper())));
            result.AddRange(ClientsRepository.Get(n => n.TutorFullName.ToUpper().Contains(searchTerm.ToUpper())));
            result = result.Distinct(new ClientComparator()).ToList();
            return result;
        }

        public bool ThisClientIsSubcribed(Client client)
        {

            if (client == null)
                return false;
            var subcribtions= SubcriptionsRepository.Get(e => e.Client.Id == client.Id).ToList();
            subcribtions=new List<Subcription>(subcribtions.OrderByDescending(n=>n.StartDate));
            var subcribtion= subcribtions.FirstOrDefault(e => (e.EndDate - DateTime.Now).TotalDays >= 0);
            var sessions = SessiosOfThisMonthForThisClient(client);   
                           
            if (subcribtion != null && sessions!=null)
                return sessions.Count < subcribtion.SubcriptionType.MaxSessions;
            return false;
        }

        public ICollection<History> SessiosOfThisMonthForThisClient(Client client)
        {
            var subcribtions = SubcriptionsRepository.Get(e => e.Client.Id == client.Id).ToList();                          
            var subcribtion = subcribtions.FirstOrDefault(e=>(e.EndDate - DateTime.Now).TotalDays > 0);  
            var sessionsofthisClient = HistoriesRepository.Get(n => n.Client.Id == client.Id).ToList();
            var sessions =sessionsofthisClient.Where(n => subcribtion != null && n.Record >= subcribtion.StartDate && n.Record <= subcribtion.EndDate)
                    .ToList();
            return sessions;
        }
        
        public int SessionsLeftOnThisMonthForThisClient(Client client)
        {
            if (!ThisClientIsSubcribed(client)) return 0;
            var subcribtions = SubcriptionsRepository.Get(e => e.Client.Id == client.Id).ToList();
            var subcribtion = new List<Subcription>(subcribtions.OrderByDescending(e=>e.EndDate)).FirstOrDefault(e => (e.EndDate - DateTime.Now).TotalDays >= 0);
            if (subcribtion != null)
                return subcribtion.SubcriptionType.MaxSessions - SessiosOfThisMonthForThisClient(client).Count;
            return 0;
        }

        public ICollection<History> GetHistory(DateTime startDate, DateTime endDate)
        {
            return HistoriesRepository.Get(n => n.Record >= startDate && n.Record <= endDate).ToList();
        }


        public void AddSessionForThisClient(Client client)
        {
            var history = new History { Client = client, Record = DateTime.Now };
            HistoriesRepository.AddAsync(history);
           
           
               
           
        }

        public Main()
        {
            var dataContext = SimpleIoc.Default.GetAllInstances<GlobalContext>().FirstOrDefault();
            if (LoginsRepository == null)
                LoginsRepository =
                    new Repository<Login>(dataContext);

            if (ClientsRepository == null)
                ClientsRepository =
                    new Repository<Client>(dataContext);

            if (HistoriesRepository == null)
                HistoriesRepository =
                    new Repository<History>(dataContext);

            if (SubcriptionsRepository == null)
                SubcriptionsRepository =
                    new Repository<Subcription>(dataContext);

            if (SubcriptionTypesRepository == null)
                SubcriptionTypesRepository =
                    new Repository<SubcriptionType>(dataContext);
            if (PayementsRepository == null)
                PayementsRepository =
                    new Repository<Payement>(dataContext);

        }


    }
}
