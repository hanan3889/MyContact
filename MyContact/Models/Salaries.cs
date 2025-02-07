using System.Text.Json.Serialization;

namespace MyContact.Models
{
    public class Salaries
    {
        [JsonPropertyName("nom")]
        public required string Nom { get; set; }

        [JsonPropertyName("prenom")]
        public string? Prenom { get; set; }

        [JsonPropertyName("telephoneFixe")]
        public string? TelephoneFixe { get; set; }

        [JsonPropertyName("telephonePortable")]
        public string? TelephonePortable { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("siteVille")]
        public string? SiteVille { get; set; }

        [JsonPropertyName("serviceNom")]
        public string? ServiceNom { get; set; }

    }
}
