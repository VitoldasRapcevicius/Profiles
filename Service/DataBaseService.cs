using Dapper;
using Profiles.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Profiles.Service
{
    public class DataBaseService
    {
        //String with local DB name and project name
        string connectionString = "Data Source=DESKTOP-GT1OEJO\\SQLEXPRESS;Initial Catalog=Profiles;Integrated Security=True";

        public List<Project> GetProjects()
        {
            //SQL Query to get information from DB
            string sqlQuery =
                @"SELECT Id, InternalId, Name 
                FROM Projects";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                return conn.Query<Project>(sqlQuery).ToList();
            }
        }

        public List<PropertyType> GetPropertyTypesByProjectID(int? projectID)
        {
            //SQL Query to get information from DB
            string sqlQuery =
                @"SELECT Id, ProjectId, Name, Coating 
                FROM PropertyTypes
                WHERE ProjectId=@ProjectId";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //Declared and mapped parameter projectID with query parameter 
                return conn.Query<PropertyType>(sqlQuery, new { ProjectId = projectID }).ToList();
            }
        }

        public List<Propertie> GetPropertiesByProjectIDPropertyTypeID(int? projectID, int? propertyTypeID)
        {
            //SQL Query to get information from DB
            string sqlQuery =
                @"SELECT Id, ProjectId, PropertyTypeId, Name, Prefix 
                FROM Properties 
                WHERE ProjectId=@ProjectId AND PropertyTypeId=@PropertyTypeId";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //Declared and mapped parameter projectID with query parameter
                return conn.Query<Propertie>(sqlQuery, new { ProjectId = projectID, PropertyTypeId = propertyTypeID }).ToList();
            }
        }

        public List<SubProperty> GetSubPropertiesByPropertyID(int? propertyID)
        {
            //SQL Query to get information from DB
            string sqlQuery =
                @"SELECT Id, PropertyId, Name, Prefix 
                FROM SubProperties 
                WHERE PropertyId=@PropertyId";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //Declared and mapped parameter projectID with query parameter
                return conn.Query<SubProperty>(sqlQuery, new { PropertyID = propertyID }).ToList();
            }
        }

        public void CreateProject(Project project)
        {
            string sqlQuery =
                 @"INSERT 
                INTO Projects
                VALUES (@InternalId, @Name)";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Query(sqlQuery, project);
            }
        }

        public void CreatePropertyType(PropertyType propertyType, int projectId)
        {
            PropertyType parameters = new()
            {
                Name = propertyType.Name,
                Coating = propertyType.Coating,
                ProjectID = projectId
            };

            string sqlQuery =
                @"INSERT 
                INTO PropertyTypes (ProjectId, Name, Coating)
                VALUES (@ProjectId, @Name, @Coating)";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Query(sqlQuery, parameters);
            }
        }

        public void CreateProperty(Propertie property, int projectId, int propertyTypeId)
        {
            Propertie parameters = new()
            {
                Name = property.Name,
                Prefix = property.Prefix,
                ProjectId = projectId,
                PropertyTypeId = propertyTypeId
            };

            string sqlQuery =
                @"INSERT 
                INTO Properties (ProjectId,PropertyTypeId, Name, Prefix)
                VALUES (@ProjectId,@PropertyTypeId, @Name, @Prefix)";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Query(sqlQuery, parameters);
            }
        }

        public void CreateSubProperty(SubProperty subProperty, int propertyId)
        {
            SubProperty parameters = new()
            {
                Name = subProperty.Name,
                Prefix = subProperty.Prefix,
                PropertyId = propertyId
            };

            string sqlQuery =
                @"INSERT 
                INTO SubProperties (PropertyId, Name, Prefix)
                VALUES (@PropertyId, @Name, @Prefix)";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Query(sqlQuery, parameters);
            }
        }

        public void DeleteProjectByID(int projectId)
        {
            string sqlQuery =
                @"DELETE Projects
                WHERE Id=@ProjectId";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Query(sqlQuery, new { ProjectId = projectId });
            }
        }

        public void DeletePropertyTypeByID(int propertyTypeId)
        {
            string sqlQuery =
                @"DELETE PropertyTypes
                WHERE Id=@PropertyTypeId";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Query(sqlQuery, new { PropertyTypeId = propertyTypeId });
            }
        }

        public void DeletePropertyByID(int propertyId)
        {
            string sqlQuery =
                @"DELETE Properties
                WHERE Id=@PropertyId";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Query(sqlQuery, new { PropertyId = propertyId });
            }
        }

        public void DeleteSubPropertyByID(int subPropertyId)
        {
            string sqlQuery =
                @"DELETE SubProperties
                WHERE Id=@SubPropertyId";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Query(sqlQuery, new { SubPropertyId = subPropertyId });
            }
        }

        public void UpdateProjectByID(Project project, int projectId)
        {
            Project parameters = new()
            {
                Id = projectId,
                InternalId = project.InternalId,
                Name = project.Name
            };

            string sqlQuery =
            @"UPDATE Projects 
            SET InternalId=@InternalId, Name=@Name
            WHERE Id=@Id";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Query(sqlQuery, parameters);
            }
        }

        public void UpdatePropertyTypeByID(PropertyType propertyType, int propertyTypeId, int projectId)
        {
            PropertyType parameters = new()
            {
                Id = propertyTypeId,
                ProjectID = projectId,
                Name = propertyType.Name,
                Coating = propertyType.Coating
            };

            string sqlQuery =
                @"UPDATE PropertyTypes
                SET ProjectId=@ProjectId, Name=@Name, Coating=@Coating
                WHERE Id=@Id";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Query(sqlQuery, parameters);
            }
        }

        public void UpdatePropertyByID(Propertie property, int propertyTypeId, int projectId, int propertyId)
        {
            Propertie parameters = new()
            {
                Id = propertyId,
                ProjectId = projectId,
                PropertyTypeId = propertyTypeId,
                Name = property.Name,
                Prefix = property.Prefix
            };

            string sqlQuery =
                @"UPDATE Properties
                SET ProjectId=@ProjectId, PropertyTypeId=@PropertyTypeId, Name=@Name, Prefix=@Prefix
                WHERE Id=@Id";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Query(sqlQuery, parameters);
            }
        }

        public void UpdateSubPropertyByID(SubProperty subProperty, int propertyId, int subPropertyId)
        {
            SubProperty parameters = new()
            {
                Id = subPropertyId,
                PropertyId = propertyId,
                Name = subProperty.Name,
                Prefix = subProperty.Prefix
            };

            string sqlQuery =
                @"UPDATE SubProperties
                SET PropertyId=@PropertyId, Name=@Name, Prefix=@Prefix
                WHERE Id=@Id";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Query(sqlQuery, parameters);
            }
        }
    }
}