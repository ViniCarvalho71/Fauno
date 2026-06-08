using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Fauno.Auth.Domain.ValueObjects
{
    public class Password
    {
        public string Value { get; set; }

        public Password(string value) { 
            if (value.Length < 8)
            {
                throw new Exception("A senha deve ter no mínimo 8 caracteres");
            }

            if(!Regex.IsMatch(value, "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^A-Za-z\\d]).+$"))
            {
                throw new Exception("Senha inválida");
            }

            Value = value;
        }
    }
}
