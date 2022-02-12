using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Care.Helpers.Validators.Rules
{
    public class IsValidPasswordRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }
        public bool Check(T value)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");

            return hasNumber.IsMatch($"{value}") && hasUpperChar.IsMatch($"{value}") && hasMinimum8Chars.IsMatch($"{value}");
        }
    }
}
