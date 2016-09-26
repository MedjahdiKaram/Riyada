using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using ModelsLib;
using Riyada.ViewModel.Interaction;

namespace Riyada.ViewModel
{
    public class ManagementViewModel:ViewModelBase
    {
        private ObservableCollection<Payement> _payements;
        private ObservableCollection<History> _histories;
        private ObservableCollection<Subcription> _subcriptions;
        private double _sumPayments;
        public Main MainInteraction => SimpleIoc.Default.GetInstance<MainViewModel>().MainInteraction;

        public ObservableCollection<Payement> Payements
        {
            get { return _payements; }
            set
            {
                _payements = value;
                RaisePropertyChanged();
                CalcSumPayments();
            }
        }

        public ObservableCollection<History> Histories
        {
            get { return _histories; }
            set
            {
                _histories = value; 
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Subcription> Subcriptions
        {
            get { return _subcriptions; }
            set
            {
                _subcriptions = value;
                RaisePropertyChanged();
            }
        }

        public DateTime PayementsStartDate   { get; set; }
        public DateTime HistoriesStartDate   { get; set; }
        public DateTime SubcriptionsStartDate{ get; set; }
        public DateTime PayementsEndDate     { get; set; }
        public DateTime HistoriesEndDate     { get; set; }  
        public DateTime SubcriptionsEndDate { get; set; }

        public double SumPayments
        {
            get { return _sumPayments; }
            set
            {
                _sumPayments = value;
                RaisePropertyChanged();
            }
        }

        public void CalcSumPayments()
        {
            SumPayments= Payements.Sum(n => n.Amount);
        }
        public RelayCommand GoSubcribtions       { get; set; }
        public RelayCommand GoHistories          { get; set; }
        public RelayCommand GoPayments           { get; set; }
        public RelayCommand ResetSubcribtions    { get; set; }
        public RelayCommand ResetHistories       { get; set; }
        public RelayCommand ResetPayments { get; set; }
        public void FilterSubcribtions()
        {
            ReloadSubcriptions();
            Subcriptions = new ObservableCollection<Subcription>(Subcriptions.Where(n => n.EndDate.Date >= SubcriptionsStartDate.Date && n.EndDate.Date <= SubcriptionsEndDate.Date));
     
        }
        public void FilterHistories()
        {
            ReloadHistories();
            Histories = new ObservableCollection<History>(Histories.Where(n => n.Record.Date >= HistoriesStartDate.Date && n.Record.Date <= HistoriesEndDate.Date));
        }
        public void FilterPayments()
        {
            ReloadPayments();
            Payements = new ObservableCollection<Payement>(Payements.Where(n => n.Moment.Date >= PayementsStartDate.Date && n.Moment.Date <= PayementsEndDate.Date));
        
        }
        public void ReloadSubcriptions()
        {

            ReloadAllDatesFilter();
            Subcriptions = new ObservableCollection<Subcription>(Main.SubcriptionsRepository.Get());
        }
        public void ReloadHistories()
        {
            ReloadAllDatesFilter();
            Histories = new ObservableCollection<History>(Main.HistoriesRepository.Get());
        }
        public void ReloadPayments()
        {
            ReloadAllDatesFilter();
            Payements = new ObservableCollection<Payement>(Main.PayementsRepository.Get());

        }

        public void ReloadAllDatesFilter()
        {
            var today = DateTime.Now;
            var amonthBefore = DateTime.Now.AddDays(-30);
            HistoriesStartDate = SubcriptionsStartDate = PayementsStartDate = amonthBefore;
            HistoriesEndDate = SubcriptionsEndDate = PayementsEndDate = today;
        }
        
        public ManagementViewModel()
        {
            ReloadSubcriptions();
            ReloadHistories();
            ReloadPayments();
            GoSubcribtions  =new RelayCommand(FilterSubcribtions);
            GoHistories     =new RelayCommand(FilterHistories);
            GoPayments = new RelayCommand(FilterPayments);
            ResetSubcribtions  =new RelayCommand(ReloadSubcriptions);
            ResetHistories     =new RelayCommand(ReloadHistories);
            ResetPayments = new RelayCommand(ReloadPayments);
        }
    }
}
