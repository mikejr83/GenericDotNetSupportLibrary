using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace GenericDotNetSupportLibrary
{
  public partial class SqlConnectionString : INotifyPropertyChanged, IEditableObject
  {
    struct ConnectionData
    {
      internal string BaseConnectionString;
      internal string Server;
      internal string Database;
      internal string UserName;
      internal bool Pooling;
      internal int ConnectionTimeout;
      internal string Password;
      internal bool IntegratedSecurity;
    }

    public static implicit operator string(SqlConnectionString connectionString)
    {
      return connectionString.ToString();
    }

    private ConnectionData _ConnectionData;
    private ConnectionData _BackupConnectionData;
    private bool _InTxn = false;

    #region Properties
    public string Server
    {
      get
      {
        return this._ConnectionData.Server;
      }
      set
      {
        if (this._ConnectionData.Server == value) return;
        this._ConnectionData.Server = value;
        OnPropertyChanged("Server");
        OnPropertyChanged("IsValid");
      }
    }

    public string Database
    {
      get
      {
        return this._ConnectionData.Database;
      }
      set
      {
        if (this._ConnectionData.Database == value) return;
        this._ConnectionData.Database = value;
        OnPropertyChanged("Database");
        OnPropertyChanged("IsValid");
      }
    }

    public string UserName
    {
      get
      {
        return this._ConnectionData.UserName;
      }
      set
      {
        if (this._ConnectionData.UserName == value) return;
        this._ConnectionData.UserName = value;
        OnPropertyChanged("UserName");
        OnPropertyChanged("IsValid");
      }
    }

    public bool Pooling
    {
      get
      {
        return this._ConnectionData.Pooling;
      }
      set
      {
        if (this._ConnectionData.Pooling == value) return;
        this._ConnectionData.Pooling = value;
        OnPropertyChanged("Pooling");
        OnPropertyChanged("IsValid");
      }
    }

    public int ConnectionTimeout
    {
      get
      {
        return this._ConnectionData.ConnectionTimeout;
      }
      set
      {
        if (this._ConnectionData.ConnectionTimeout == value) return;
        this._ConnectionData.ConnectionTimeout = value;
        OnPropertyChanged("ConnectionTimeout");
        OnPropertyChanged("IsValid");
      }
    }

    public string Password
    {
      get
      {
        return this._ConnectionData.Password;
      }
      set
      {
        if (this._ConnectionData.Password == value) return;
        this._ConnectionData.Password = value;
        OnPropertyChanged("Password");
        OnPropertyChanged("IsValid");
      }
    }

    public bool IntegratedSecurity
    {
      get
      {
        return this._ConnectionData.IntegratedSecurity;
      }
      set
      {
        if (this._ConnectionData.IntegratedSecurity == value) return;

        this._ConnectionData.IntegratedSecurity = value;

        this.OnPropertyChanged("IntegratedSecurity");
        this.OnPropertyChanged("IsValid");
      }
    }

    public bool IsValid
    {
      get
      {
        return
            (!string.IsNullOrEmpty(Server) && Server.EndsWith(".sdf", System.StringComparison.CurrentCultureIgnoreCase)) ||
            (!string.IsNullOrEmpty(Server) &&
             !string.IsNullOrEmpty(Database) &&
             (IntegratedSecurity || (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))));
      }
    }

    public string BaseConnectionString
    {
      get
      {
        return this._ConnectionData.BaseConnectionString;
      }
      set
      {
        this._ConnectionData.BaseConnectionString = value;
        this.OnPropertyChanged("BaseConnectionString");
      }
    }
    #endregion

    public event PropertyChangedEventHandler PropertyChanged;

    #region Constructors
    public SqlConnectionString()
    {
      this._ConnectionData = new ConnectionData();
      this._ConnectionData.Pooling = true;
      this._ConnectionData.IntegratedSecurity = false;
      this._ConnectionData.Server = string.Empty;
      this._ConnectionData.BaseConnectionString = string.Empty;
      this._ConnectionData.Database = string.Empty;
      this._ConnectionData.UserName = string.Empty;
      this._ConnectionData.ConnectionTimeout = default(int);
      this._ConnectionData.Password = string.Empty;
      //new System.Data.SqlClient.SqlConnectionStringBuilder
      //{
      //    Pooling = false,
      //    IntegratedSecurity = true
      //};
    }

    public SqlConnectionString(string connectionString)
      : this()
    {
      StringBuilder cleansedString = new StringBuilder();
      foreach (string componentStr in connectionString.Split(new char[] { ';' }))
      {
        string component = componentStr.Trim();
        if (string.IsNullOrEmpty(component) || string.IsNullOrEmpty(component.Trim()) || component.ToUpperInvariant().StartsWith("PROVIDER=", System.StringComparison.CurrentCultureIgnoreCase))
          continue;

        cleansedString.Append(component);
        cleansedString.Append(";");
      }
      this._ConnectionData.BaseConnectionString = connectionString;

      System.Data.SqlClient.SqlConnectionStringBuilder tempBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder(cleansedString.ToString());

      this.Server = tempBuilder.DataSource;
      this.Database = tempBuilder.InitialCatalog;
      this.UserName = tempBuilder.UserID;
      this.ConnectionTimeout = tempBuilder.ConnectTimeout;
      this.Pooling = tempBuilder.Pooling;
      this.Password = tempBuilder.Password;
      this.IntegratedSecurity = tempBuilder.IntegratedSecurity;
    }
    #endregion

    /// <summary>
    /// Creates a copy of this connection string with the specified database instead of the current
    /// </summary>
    /// <param name="databaseName">Name of the database.</param>
    /// <returns></returns>
    public SqlConnectionString WithDatabase(string databaseName)
    {
      return new SqlConnectionString
      {
        Server = Server,
        Database = databaseName,
        IntegratedSecurity = IntegratedSecurity,
        UserName = UserName,
        Password = Password,
        Pooling = Pooling
      };
    }

    private void OnPropertyChanged(string propertyName)
    {
      if (this.PropertyChanged == null) return;

      this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }

    public override string ToString()
    {
      if (Server.EndsWith(".sdf", System.StringComparison.CurrentCultureIgnoreCase))
        if (string.IsNullOrEmpty(Password))
          return new System.Data.SqlClient.SqlConnectionStringBuilder { DataSource = Server }.ConnectionString;
        else
          return new System.Data.SqlClient.SqlConnectionStringBuilder { DataSource = Server, Password = Password }.
              ConnectionString;

      return new System.Data.SqlClient.SqlConnectionStringBuilder()
      {
        DataSource = this.Server,
        InitialCatalog = this.Database,
        UserID = this.UserName,
        ConnectTimeout = this.ConnectionTimeout,
        Pooling = this.Pooling,
        Password = this.Password,
        IntegratedSecurity = this.IntegratedSecurity
      }.ConnectionString;
    }

    public string ToSSISSafeString()
    {
      string connString = new System.Data.SqlClient.SqlConnectionStringBuilder()
      {
        DataSource = this.Server,
        InitialCatalog = this.Database,
        UserID = this.UserName,
        ConnectTimeout = this.ConnectionTimeout,
        Password = this.Password
      }.ConnectionString;

      connString += ";Provider=SQLNCLI10.1";

      return connString.Replace(";", "; ");
    }

    public string ToDataSourceConnectionString()
    {
      return string.Format(System.Globalization.CultureInfo.InvariantCulture, "^User ID:^?Password:;LOCAL;OL;DBInfo_Type=MS;Provider=SQLOLEDB;User ID=%s;Password=%s;Data Source={0};Provider_String=Initial Catalog={1};@COLSEQ=",
          this.Server, this.Database);
    }

    #region IEditableObject Members

    public void BeginEdit()
    {
      if (!this._InTxn)
      {
        this._BackupConnectionData = this._ConnectionData;
        this._InTxn = true;
      }
    }

    public void CancelEdit()
    {
      if (this._InTxn)
      {
        this._ConnectionData = _BackupConnectionData;
        this._InTxn = false;
      }
    }

    public void EndEdit()
    {
      if (this._InTxn)
      {
        this._BackupConnectionData = new ConnectionData();
        this._BackupConnectionData.Pooling = true;
        this._BackupConnectionData.IntegratedSecurity = true;
        this._BackupConnectionData.Server = string.Empty;
        this._BackupConnectionData.BaseConnectionString = string.Empty;
        this._BackupConnectionData.Database = string.Empty;
        this._BackupConnectionData.UserName = string.Empty;
        this._BackupConnectionData.ConnectionTimeout = default(int);
        this._BackupConnectionData.Password = string.Empty;
        this._InTxn = false;
      }
    }

    #endregion
  }
}
