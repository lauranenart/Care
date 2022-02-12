﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Care.Helpers.Validators.Rules
{
    class IsValidLongDescrLength<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (!string.IsNullOrWhiteSpace($"{value}"))
            {
                if ($"{value}".Length < 50)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
