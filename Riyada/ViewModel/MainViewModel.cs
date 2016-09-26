using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using DAL;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using ModelsLib;
using Riyada.ViewModel.Interaction;

namespace Riyada.ViewModel
{
    public enum SubcriState
    {
        None,
        Payed,
        LastSession,
        Unpayed
    }
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<Client> _clientsSearchResult;
        private ObservableCollection<History> _histories;
        private ObservableCollection<SubcriptionType> _subcriptionTypes;
        private string _mainSearchString;
        private bool _isIdentifiant;
        private Client _selectedClient;
        private DateTime _selectedEndDate;
        private DateTime _selectedStartDate;
        private SubcriptionType _selectedSubcriptionType;
        private bool _canAddSubcribtionForCurrentClient;
        private SubcriState _subcribtionState;
        private int _sessionsLeftForSelectedClient;
        private bool _canSeeDetails;
        private int _selectedClientTabIndex;
        private DateTime _availableBeginingOfEndDate;
        public int SelectedClientTabIndex
        {
            get { return _selectedClientTabIndex; }
            set
            {
                _selectedClientTabIndex = value;
                RaisePropertyChanged();
            }
        }
        public int SessionsLeftForSelectedClient
        {
            get { return _sessionsLeftForSelectedClient; }
            set
            {
                _sessionsLeftForSelectedClient = value;
                RaisePropertyChanged();
            }
        }
        public Main MainInteraction { get; set; }
        public SubcriState SubcribtionState
        {
            get { return _subcribtionState; }
            set
            {
                _subcribtionState = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<History> Histories
        {
            get { return _histories; }
            set
            {
                _histories = new ObservableCollection<History>(value.OrderByDescending(o=>o.Record));
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<Client> ClientsSearchResult
        {
            get { return _clientsSearchResult; }
            set
            {
                _clientsSearchResult = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<SubcriptionType> SubcriptionTypes
        {
            get { return _subcriptionTypes; }
            set
            {
                _subcriptionTypes = value;
                RaisePropertyChanged();
            }
        }
        public SubcriptionType SelectedSubcriptionType
        {
            get { return _selectedSubcriptionType; }
            set
            {
                _selectedSubcriptionType = value;
                RaisePropertyChanged();
                AddSubcriptionCommand.RaiseCanExecuteChanged();
            }
        }
        public DateTime SelectedEndDate
        {
            get { return _selectedEndDate; }
            set
            {
                _selectedEndDate = value;
                RaisePropertyChanged();
                AddSubcriptionCommand.RaiseCanExecuteChanged();
            }
        }
        public DateTime SelectedStartDate
        {
            get { return _selectedStartDate; }
            set
            {
                _selectedStartDate = value;
                RaisePropertyChanged();
                AvailableBeginingOfEndDate= value.AddDays(30);
                if (!MainInteraction.ThisClientIsSubcribed(SelectedClient))
                    SelectedEndDate= AvailableBeginingOfEndDate = value.AddDays(30);
                AddSubcriptionCommand.RaiseCanExecuteChanged();
            }
        }
        public DateTime AvailableBeginingOfEndDate
        {
            get { return _availableBeginingOfEndDate; }
            set
            {
                _availableBeginingOfEndDate = value;
                RaisePropertyChanged();
            }
        }
        public double Amount { get; set; }
        public bool IsIdentifiant
        {
            get { return _isIdentifiant; }
            set
            {
                _isIdentifiant = value;
                RaisePropertyChanged();
            }
        }
        public bool CanSeeDetails
        {
            get { return _canSeeDetails; }
            set
            {
                _canSeeDetails = value;
                RaisePropertyChanged();
            }
        }
        public string MainSearchString
        {
            get { return _mainSearchString; }
            set
            {
                _mainSearchString = value;
                RaisePropertyChanged();
            }
        }
        public Client SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                _selectedClient = value;
                UpdateUiDataOfForTheCurrentClient();
                SelectedClientTabIndex = 0;
                RaisePropertyChanged();
            }
        }         
        public bool CanAddSubcribtionForCurrentClient
        {
            get { return _canAddSubcribtionForCurrentClient; }
            set
            {
                _canAddSubcribtionForCurrentClient = value;
                RaisePropertyChanged();
            }
        }
        public RelayCommand SearchCommand             { get; set; }
        public RelayCommand ClearCommand              { get; set; }
        public RelayCommand SaveClientCommand         { get; set; }
        public RelayCommand AddNewClientCommand       { get; set; }
        public RelayCommand PointingCommand           { get; set; }
        public RelayCommand AddSubcriptionCommand { get; set; }

        public bool CanAddSubcribtion ()
        {
            if (SelectedClient == null || SelectedClient.Id <= 0)
                return false;
            var result = !MainInteraction.ThisClientIsSubcribed(SelectedClient);
            var subcribtions = Main.SubcriptionsRepository.Get(n => n.Client.Id == SelectedClient.Id);
            result = result && subcribtions.Where(n => n.StartDate >= DateTime.Now).ToList().Count <= 0;
            return result;
        }
        public void AddSubcribtion()
        {
            if (SelectedClient != null && SelectedClient.Id > 0 && !MainInteraction.ThisClientIsSubcribed(SelectedClient) && SelectedSubcriptionType !=null)
            {
                var newSubcription= new Subcription { Client = SelectedClient, SubcriptionType = SelectedSubcriptionType, StartDate = SelectedStartDate, EndDate = SelectedEndDate };
                Main.SubcriptionsRepository.Add(newSubcription);
                //var history = new History {Client = SelectedClient, Record = DateTime.Now};
                //var pay=new Payement{Amount = Amount,Subription = newSubcription,History =history,Moment = DateTime.Now};
                var pay=new Payement{Amount = SelectedSubcriptionType.Price,Subription = newSubcription,Moment = DateTime.Now};
                Main.PayementsRepository.Add(pay);
                UpdateUiDataOfForTheCurrentClient();
            }
        }
        private void UpdateUiDataOfForTheCurrentClient()
        {
            CanSeeDetails = false;
            var validClient = SelectedClient != null && SelectedClient.Id > 0;
            
            CanAddSubcribtionForCurrentClient = validClient;
            CanSeeDetails = validClient && !SelectedClient.Name.ToUpper().Contains("INCONNU") && !SelectedClient.LastName.ToUpper().Contains("INCONNU");
            Histories = new ObservableCollection<History>();
            LoadAllHistory();
            SubcribtionState=SubcriState.None;
            if (validClient)
            {
               
                Histories =
                    new ObservableCollection<History>(Main.HistoriesRepository.Get(n => n.Client.Id == SelectedClient.Id));
                var sessionsLeft = MainInteraction.SessionsLeftOnThisMonthForThisClient(SelectedClient);
                SessionsLeftForSelectedClient = sessionsLeft;
                if (MainInteraction.ThisClientIsSubcribed(SelectedClient))
                {
                    var subcribtions = Main.SubcriptionsRepository.Get();
                    var subcribtionOfSelectedClient = subcribtions.FirstOrDefault(n => n.Client.Id == SelectedClient.Id);
          
                    if (sessionsLeft==1)
                        SubcribtionState = SubcriState.LastSession;
                    else
                        SubcribtionState = SubcriState.Payed;
                    if (subcribtionOfSelectedClient != null)
                    {
                        SelectedStartDate =
                            subcribtionOfSelectedClient.StartDate;
                        SelectedEndDate = subcribtionOfSelectedClient.EndDate;
                        SelectedSubcriptionType = subcribtionOfSelectedClient.SubcriptionType;
                        CanAddSubcribtionForCurrentClient = false;
                    }
                }
                else
                {
                    SubcribtionState = SubcriState.Unpayed;
                }
            }
            else
                CanSeeDetails = false;
        }
        private void LoadAllHistory()
        {
            var queryable = Main.ClientsRepository.Get().ToList();
            Histories = new ObservableCollection<History>(Main.HistoriesRepository.Get());
        }
        public void Pointing()
        {
            string sMessageBoxText;
            string sCaption ;
            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Stop;
          
          
            if (SelectedClient!=null && SelectedClient.Id>0)
            {
                
                if (MainInteraction.ThisClientIsSubcribed(SelectedClient))
                MainInteraction.AddSessionForThisClient(SelectedClient);
                else
                {
                    sCaption = "Abonnemet expiré. Client: "+SelectedClient.Id;
                    sMessageBoxText= "L'abonnement de "+SelectedClient.Name + " " +SelectedClient.LastName+" est expiré, voulez vous continuer en pointant une séance ordinaire pour "+Amount+" DA ?";
                    MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                    switch (rsltMessageBox)
                    {
                        case MessageBoxResult.Yes:
                            var history = new History { Client = SelectedClient, Record = DateTime.Now };
                            Main.HistoriesRepository.Add(history);
                            var payement = new Payement { History = history, Amount = Amount, Moment = DateTime.Now };
                            Main.PayementsRepository.Add(payement);
                            break;
                        case MessageBoxResult.No:
                            /* ... */
                            break;
                    }
                    
                }
            }
            else
            {
                sCaption = "Aucun client sélectionné";
                sMessageBoxText = "Voulez vous vraiment pointer une séance pour un client inconnu pour "+Amount+" DA ?";
                 icnMessageBox = MessageBoxImage.Question;
                 var rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                switch (rsltMessageBox)
                {
                    case MessageBoxResult.Yes:
                        var client = MainInteraction.SearchForClients("INCONNU").FirstOrDefault();
                        var history = new History { Client = client, Record = DateTime.Now };
                        Main.HistoriesRepository.Add(history);
                        var payement = new Payement { History = history, Amount = Amount, Moment = DateTime.Now };
                        Main.PayementsRepository.Add(payement);
                        break;
                    case MessageBoxResult.No:
                        /* ... */
                        break;
                }
     
            }
           UpdateUiDataOfForTheCurrentClient();
        }
        public void Search()
        {
            if (IsIdentifiant)
            {
                int id;
                if (!int.TryParse(MainSearchString, out id))
                    return;
                var client = Main.ClientsRepository.Get(id);
                if (client != null)
                {
                    ClientsSearchResult = new ObservableCollection<Client> {client};
                    
                }
            }
            else
            {
                ClientsSearchResult=new ObservableCollection<Client>();
                var searchresult = MainInteraction.SearchForClients(MainSearchString);
                foreach (var client in searchresult)
                {
                    ClientsSearchResult.Add(client);
                }
            }
            if (ClientsSearchResult.Count==1)
                SelectedClient = ClientsSearchResult.FirstOrDefault();
        }
        public void Clear()
        {
            MainSearchString = string.Empty;
            
            ClientsSearchResult=new ObservableCollection<Client>(Main.ClientsRepository.Get());
            //IsIdentifiant = true;
            SelectedClient = null;
            SelectedClientTabIndex = 0;
        }
        public void AddNewClient()
        {
          
            Clear();
            SelectedClient=new Client {DateOfBirth = DateTime.Now.Subtract(new TimeSpan(6588,0,0,0))};
            ClientsSearchResult.Add(SelectedClient);
            CanSeeDetails = true;
            SelectedClientTabIndex = 1;
        }
        public void SaveClient()
        {
            var firstOrDefault = SimpleIoc.Default.GetAllInstances<GlobalContext>().FirstOrDefault();
            if (firstOrDefault != null && SelectedClient.Id > 0)
                firstOrDefault.SaveChangesAsync();
            else
            {
                Main.ClientsRepository.Add(SelectedClient);
          
            }
            RaisePropertyChanged("SelectedClient");
            UpdateUiDataOfForTheCurrentClient();
        }
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                IsIdentifiant = true;
            }
            else
            {
               MainInteraction=new Main();
               // Main.ClientsRepository.Add(new Client {Name ="Riyadh",LastName = "Benmnsour",Phone = "0552395899",DateOfBirth = new DateTime(1990,01,01)});
                IsIdentifiant = true;
                ClientsSearchResult=new ObservableCollection<Client>();
                SearchCommand=new RelayCommand(Search);
                ClearCommand = new RelayCommand(Clear);
                SaveClientCommand = new RelayCommand(SaveClient);
                PointingCommand = new RelayCommand(Pointing);
                AddNewClientCommand=new RelayCommand(AddNewClient);
                AddSubcriptionCommand=new RelayCommand(AddSubcribtion,CanAddSubcribtion);
                SubcriptionTypes = new ObservableCollection<SubcriptionType>(Main.SubcriptionTypesRepository.Get().ToList());
                SelectedStartDate=DateTime.Now;                
                CanAddSubcribtionForCurrentClient = false;
                ClientsSearchResult=new ObservableCollection<ModelsLib.Client>(Main.ClientsRepository.Get());
                Amount = 500;
                LoadAllHistory();
                CanSeeDetails = false;
            }
        }
    }
}