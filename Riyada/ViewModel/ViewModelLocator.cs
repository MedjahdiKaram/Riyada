/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Riyada"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using System.Linq;
using DAL;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using InterfacesLib.Repository;
using Microsoft.Practices.ServiceLocation;
using ModelsLib;

namespace Riyada.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ManagementViewModel>();
            SimpleIoc.Default.Register<GlobalContext>();
            SimpleIoc.Default.Register<PrintViewModel>();

        }
        private GlobalContext DataContext
        {
            get
            {
                return ServiceLocator.Current.GetInstance<GlobalContext>();
            }
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        public ManagementViewModel Management
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ManagementViewModel>();
            }
        }
        public PrintViewModel Print
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PrintViewModel>();
            }
        }
        
        public static void Cleanup()
        {
            var globalContext = SimpleIoc.Default.GetAllInstances<GlobalContext>().FirstOrDefault();
            if (globalContext != null)
                globalContext.Dispose();
            ;
        }
    }
}