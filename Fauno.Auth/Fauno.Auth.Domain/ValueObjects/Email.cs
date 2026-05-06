using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Fauno.Auth.Domain.ValueObjects
{
    public class Email
    {
        public string Value { get; set; }

        public Email(string value) { 

            if (Regex.IsMatch(value, "^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,}$"))
            {
                throw new Exception("Email inválido");
            }

            Value = value;
        }
    }
}
