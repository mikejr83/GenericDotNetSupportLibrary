using System;
using GenericDotNetSupportLibrary;
using Xunit;

namespace SupportLibraryTests
{
  public class SqlConnectionStringTests
  {
    [Fact(DisplayName = "Test SQL 2012 Standard Security Connection String Parsing")]
    public void TestSQL2012StandardParse()
    {
      SqlConnectionString connectionString = new SqlConnectionString(@"Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

      Record.Exception(new Assert.ThrowsDelegate(() => { string server = connectionString.Server; }));
      Record.Exception(new Assert.ThrowsDelegate(() => { string Database = connectionString.Database; }));
      Record.Exception(new Assert.ThrowsDelegate(() => { string UserName = connectionString.UserName; }));
      Record.Exception(new Assert.ThrowsDelegate(() => { string Password = connectionString.Password; }));
      
      Assert.Equal("myServerAddress", connectionString.Server);
      Assert.Equal("myDataBase", connectionString.Database);
      Assert.Equal("myUsername", connectionString.UserName);
      Assert.Equal("myPassword", connectionString.Password);
    }

    [Fact(DisplayName = "Test SQL 2012 Instance Connection String Parsing")]
    public void TestSQL2012InstanceParse()
    {
      SqlConnectionString connectionString = new SqlConnectionString(@"Server=myServerName\myInstanceName;Database=myDataBase;User Id=myUsername;Password=myPassword;");

      Record.Exception(new Assert.ThrowsDelegate(() => { string server = connectionString.Server; }));
      Record.Exception(new Assert.ThrowsDelegate(() => { string Database = connectionString.Database; }));
      Record.Exception(new Assert.ThrowsDelegate(() => { string UserName = connectionString.UserName; }));
      Record.Exception(new Assert.ThrowsDelegate(() => { string Password = connectionString.Password; }));
      
      Assert.Equal(@"myServerName\myInstanceName", connectionString.Server);
      Assert.Equal("myDataBase", connectionString.Database);
      Assert.Equal("myUsername", connectionString.UserName);
      Assert.Equal("myPassword", connectionString.Password);
    }

    [Fact(DisplayName = "Test SQL 2008 Standard Security Connection String Parsing")]
    public void TestSQL2008StandardParse()
    {
      SqlConnectionString connectionString = new SqlConnectionString(@"Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

      Record.Exception(new Assert.ThrowsDelegate(() => { string server = connectionString.Server; }));
      Record.Exception(new Assert.ThrowsDelegate(() => { string Database = connectionString.Database; }));
      Record.Exception(new Assert.ThrowsDelegate(() => { string UserName = connectionString.UserName; }));
      Record.Exception(new Assert.ThrowsDelegate(() => { string Password = connectionString.Password; }));
      
      Assert.Equal("myServerAddress", connectionString.Server);
      Assert.Equal("myDataBase", connectionString.Database);
      Assert.Equal("myUsername", connectionString.UserName);
      Assert.Equal("myPassword", connectionString.Password);
    }

    [Fact(DisplayName = "Test SQL 2008 Instance Connection String Parsing")]
    public void TestSQL2008InstanceParse()
    {
      SqlConnectionString connectionString = new SqlConnectionString(@"Server=myServerName\myInstanceName;Database=myDataBase;User Id=myUsername;Password=myPassword;");

      Record.Exception(new Assert.ThrowsDelegate(() => { string server = connectionString.Server; }));
      Record.Exception(new Assert.ThrowsDelegate(() => { string Database = connectionString.Database; }));
      Record.Exception(new Assert.ThrowsDelegate(() => { string UserName = connectionString.UserName; }));
      Record.Exception(new Assert.ThrowsDelegate(() => { string Password = connectionString.Password; }));
      
      Assert.Equal(@"myServerName\myInstanceName", connectionString.Server);
      Assert.Equal("myDataBase", connectionString.Database);
      Assert.Equal("myUsername", connectionString.UserName);
      Assert.Equal("myPassword", connectionString.Password);
    }

    [Fact(DisplayName = "Test Generation of Conn String from Object")]
    public void TestConnectionStringGeneration()
    {
      SqlConnectionString connectionString = new SqlConnectionString();
      connectionString.Server = "myServerAddress";
      connectionString.Database = "myDataBase";
      connectionString.UserName = "myUsername";
      connectionString.Password = "myPassword";

      Assert.Equal(@"Data Source=myServerAddress;Initial Catalog=myDataBase;Integrated Security=False;User ID=myUsername;Password=myPassword;Pooling=True;Connect Timeout=0", connectionString);
    }

    [Fact(DisplayName = "Test Equality From Cloning")]
    public void TestEquality()
    {
      SqlConnectionString connectionString = new SqlConnectionString();
      connectionString.Server = "myServerAddress";
      connectionString.Database = "myDataBase";
      connectionString.UserName = "myUsername";
      connectionString.Password = "myPassword";

      SqlConnectionString connectionStringClone = new SqlConnectionString(connectionString);

      Assert.Equal(connectionString.ToString(), connectionStringClone.ToString());
    }

    [Fact(DisplayName = "Test Cloning and Then Changing a Catalog Value")]
    public void TestCatalogChange()
    {
      SqlConnectionString connectionString = new SqlConnectionString();
      connectionString.Server = "myServerAddress";
      connectionString.Database = "myDataBase";
      connectionString.UserName = "myUsername";
      connectionString.Password = "myPassword";

      SqlConnectionString connectionStringClone = connectionString.WithDatabase("myOtherDataBase");

      Assert.NotEqual(connectionString.ToString(), connectionStringClone.ToString());
      Assert.Equal("myOtherDataBase", connectionStringClone.Database);
    }
  }
}
