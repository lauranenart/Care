using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using Care.Droid;
using Care.Helpers.Behaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(JustifiedLabel), typeof(JustifiedLabelRenderer))]
namespace Care.Droid
{
    public class JustifiedLabelRenderer : LabelRenderer
    {
        public JustifiedLabelRenderer(Context context): base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if(Control != null)
            {
                Control.JustificationMode = JustificationMode.InterWord;
            }
        }
    }
}