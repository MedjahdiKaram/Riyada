using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using Riyada.ViewModel.Interaction;

namespace Riyada.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class PrintViewModel : ViewModelBase
    {
        private ObservableCollection<ClientView> _clientViews;

        public ObservableCollection<ClientView> ClientViews
        {
            get { return _clientViews; }
            set
            {
                _clientViews = value;
                RaisePropertyChanged();
            }
        }

        public PrintViewModel()
        {
            ClientViews=new ObservableCollection<ClientView>();
            var clients=Main.ClientsRepository.Get().ToList();
            foreach (var client in clients)
            {
                ClientView cl =  new ClientView(client);
                ClientViews.Add(cl);
            }
        }
    }
}