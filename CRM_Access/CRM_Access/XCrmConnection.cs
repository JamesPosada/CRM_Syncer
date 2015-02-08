using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;


namespace FrezzorXCrm {
    internal class XCrmConnection {


        internal static Microsoft.Xrm.Client.CrmOrganizationServiceContext GetOrganizationService() {
            return new Microsoft.Xrm.Client.CrmOrganizationServiceContext(GetCrmConnection());
        }
        internal static XCrmContext GetCrmContext() {
            return new XCrmContext(XCrmConnection.GetCrmConnection());
        }

        internal static CrmConnection GetCrmConnection() {
            return new CrmConnection(GetConnectionString());
        }
        internal static System.Configuration.ConnectionStringSettings GetConnectionString() {
            return _ConnectionString;

        }

        private static System.Configuration.ConnectionStringSettings _ConnectionString =
            new System.Configuration.ConnectionStringSettings("Xrm", string.Format("Server={0};  Username={1}; Password={2}",
                    Settings.CrmCredentials.Server, Settings.CrmCredentials.DomainWithUserName,
                    System.Text.ASCIIEncoding.ASCII.GetString(Settings.CrmCredentials.Password)));

    }
}
