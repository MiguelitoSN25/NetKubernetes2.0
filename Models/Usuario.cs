using Microsoft.AspNetCore.Identity;

namespace NetKubernetes2._0.Models
{
    public class Usuario : IdentityUser 
    {

        public string? Name { get; set; }

        public string? LastName { get; set; }

        public string? Phone { get; set; }
    }
}
