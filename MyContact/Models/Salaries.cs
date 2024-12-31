using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyContact.Models
{
    public class Salaries
    {
        public int Id { get; set; }
        public required string Nom { get; set; }
        public string? Prenom { get; set; }
        public string? TelephoneFixe { get; set; }
        public string? TelephonePortable { get; set; }
        public string? Email { get; set; }

        // Foreign Key
        public int ServiceId { get; set; }
        public Services? Service { get; set; }

        public int SiteId { get; set; }
        public Sites? Site { get; set; }

        public string? ServiceNom => Service?.Nom;
        public string? SiteVille => Site?.Ville;
    }

}
