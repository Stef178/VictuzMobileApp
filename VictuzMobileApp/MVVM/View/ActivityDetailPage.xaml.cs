namespace VictuzMobileApp.MVVM.View;
using VictuzMobileApp.MVVM.Model;


public partial class ActivityDetailPage : ContentPage
    {
        public ActivityDetailPage(Activity activity)
        {
            InitializeComponent();
            BindingContext = activity;  // Zet de activiteit als de bindingcontext
        }
    }
