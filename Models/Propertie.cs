namespace Profiles.Models
{
    public class Propertie
    {
        //Variables name according to the DB table column name
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int PropertyTypeId { get; set; }
        public string Name { get; set; }
        public string Prefix { get; set; }
    }
}
