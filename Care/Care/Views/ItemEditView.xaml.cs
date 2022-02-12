using Care.Helpers.Validators;
using Care.Services;
using Care.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Care.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemEditView : Rg.Plugins.Popup.Pages.PopupPage
    {
        public ItemEditView(int id)
        {
            InitializeComponent();
            BindingContext = new ItemViewModel(id);
        }
        private void Close_Popup(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync(true);
        }
    }
}