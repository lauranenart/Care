using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Care.Helpers.Behaviours
{
    class PickerLineValidationBehaviour : BehaviorBase<Picker>
    {
        #region StaticFields
        public static readonly BindableProperty IsValidProperty = BindableProperty.Create(nameof(IsValid), typeof(bool), typeof(PickerLineValidationBehaviour), true, BindingMode.Default, null, (bindable, oldValue, newValue) => OnIsValidChanged(bindable, newValue));
        #endregion
        #region Properties
        public bool IsValid
        {
            get
            {
                return (bool)GetValue(IsValidProperty);
            }
            set
            {
                SetValue(IsValidProperty, value);
            }
        }
        #endregion
        #region StaticMethods
        private static void OnIsValidChanged(BindableObject bindable, object newValue)
        {
            if (bindable is PickerLineValidationBehaviour IsValidBehavior &&
                 newValue is bool IsValid)
            {
                IsValidBehavior.AssociatedObject.TitleColor = IsValid ? Color.Default : Color.Red;
            }
        }

        #endregion
    }
}
