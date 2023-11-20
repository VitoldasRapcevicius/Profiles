namespace Profiles.Models
{
    public class SubProperty
    {
        //Variables name according to the DB table column name
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public string Name { get; set; }
        public string Prefix { get; set; }
    }
}