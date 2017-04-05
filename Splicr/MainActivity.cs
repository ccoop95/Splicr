using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;

namespace Splicr
{
    [Activity(Label = "Splicr", MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.NoTitleBar")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
        }
    }
}

