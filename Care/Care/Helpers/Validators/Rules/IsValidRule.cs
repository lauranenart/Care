using System;
using System.Collections.Generic;
using System.Text;

namespace Care.Helpers.Validators.Rules
{
    class IsValidRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            return false;
        }
    }
}
