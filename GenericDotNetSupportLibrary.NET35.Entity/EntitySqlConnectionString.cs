using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using System.Text;

namespace GenericDotNetSupportLibrary
{
  public class EntitySqlConnectionString : SqlConnectionString
  {
    public EntitySqlConnectionString() : base() { }
    public EntitySqlConnectionString(SqlConnectionString connectionstring) : base(connectionstring) { }

    public string ToEDMXString(string metadataPath)
    {
      EntityConnectionStringBuilder builder = new EntityConnectionStringBuilder();

      builder.Provider = "System.Data.SqlClient";
      builder.ProviderConnectionString = this.ToString();

      // Set the Metadata location.
      builder.Metadata = "res://*/" + metadataPath + ".csdl|res://*/" +
          metadataPath + ".ssdl|res://*/" + metadataPath + ".msl";

      return builder.ToString();
    }
  }
}
