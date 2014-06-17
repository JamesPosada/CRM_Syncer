using Microsoft.Xrm.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XrmV2;
using Microsoft.Xrm.Client.Services;


namespace DataAccess
{
   internal class XrmConnection
   {


       internal static Microsoft.Xrm.Client.CrmOrganizationServiceContext GetOrganizationService()
       {
           return new Microsoft.Xrm.Client.CrmOrganizationServiceContext(GetCrmConnection());
       }
       internal static XrmV2.Context GetCrmContext()
       {
           return new XrmV2.Context(GetCrmConnection());
       }

       internal static CrmConnection GetCrmConnection()
       {
           return new CrmConnection(GetConnectionString());
       }
       internal static System.Configuration.ConnectionStringSettings GetConnectionString()
       {
           return _ConnectionString;
           
       }

       private static System.Configuration.ConnectionStringSettings _ConnectionString =
           new System.Configuration.ConnectionStringSettings("Xrm", string.Format("Server={0};  Username={1}; Password={2}",
                   DataAccess.Settings.CrmCredentials.Server, DataAccess.Settings.CrmCredentials.DomainWithUserName,
                   System.Text.ASCIIEncoding.ASCII.GetString(DataAccess.Settings.CrmCredentials.Password)));

   }
}
