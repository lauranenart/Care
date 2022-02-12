using Care.Models;
using Care.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Care.Helpers.Validators.Rules
{
    class IsExistingEmailRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            UserService context = new UserService();
            var check = false;

            if ($"{value}" != null && !$"{value}".Equals(""))
                check = UserModel.Equals(context.FindUserByEmail($"{value}"), null) ? false : true;
            return check;
        }
    }
}
