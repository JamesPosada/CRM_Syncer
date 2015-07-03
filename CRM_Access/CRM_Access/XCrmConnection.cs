using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk.Client;


namespace FrezzorXCrm {
    public class XCrmConnection {


        
        public static Microsoft.Xrm.Client.CrmOrganizationServiceContext GetOrganizationService() {
            return new Microsoft.Xrm.Client.CrmOrganizationServiceContext(GetCrmConnection());
        }
        public static XCrmContext GetCrmContext() {
            return new XCrmContext(XCrmConnection.GetCrmConnection());
        }

        public static CrmConnection GetCrmConnection() {
           return new CrmConnection(GetConnectionString());
        }
        public static System.Configuration.ConnectionStringSettings GetConnectionString() {
            return _ConnectionString;

        }
        public static System.Configuration.ConnectionStringSettings _ConnectionString =
            new System.Configuration.ConnectionStringSettings("Xrm", string.Format("Server={0};  Username={1}; Password={2}",
                    Settings.CrmCredentials.Server, Settings.CrmCredentials.UserName,
                    System.Text.ASCIIEncoding.ASCII.GetString(Settings.CrmCredentials.Password)));

    }
}
