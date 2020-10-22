
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace PIPDataViewStudio.ViewModel
{

    public class ViewModelLocator
    {

        public ViewModelLocator()
        {

            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return SimpleIoc.Default.GetInstance<MainViewModel>();
            }
        }
        
        public static void Cleanup()
        {
            
        }
    }
}