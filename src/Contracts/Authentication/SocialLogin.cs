using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Contracts.Authentication
{
    public record SocialLogin
    {
        [Required]
        public required string IdToken { get; set; }
    }
}
