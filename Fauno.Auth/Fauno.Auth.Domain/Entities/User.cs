using Fauno.Auth.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fauno.Auth.Domain.Entities
{
    public class User : EntityBase
    {
        public Email Email { get; set; }
        public Password Password { get; set; }
    }
}
