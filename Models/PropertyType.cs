namespace Profiles.Models
{
    public class PropertyType
    {
        //Variables name according to the DB table column name
        public int Id { get; set; }
        public int ProjectID { get; set; }
        public string Name { get; set; }
        public bool Coating { get; set; }
    }
}