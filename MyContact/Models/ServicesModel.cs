namespace MyContact.Models
{
    public class ServicesModel
    {
        public int Id { get; set; }
        //public required string Nom { get; set; }
        public string? Nom { get; set; }
        public string? ServiceNom { get; internal set; }
    }
}
