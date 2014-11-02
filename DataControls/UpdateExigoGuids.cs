using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using DataAccess;
using ExigoDesktop.Exigo.WebService;

namespace DataControls
{
    /// <summary>
    /// this class will tie together all CRM accounts to exigo accounts if CRM account has an ID number on it
    /// and that ID number exists in Exigo without a GUID on it
    /// first this pulls all exigo accounts that are in an active status and do not have a GUID
    /// on them, next this pulls all CRM crm contacts with exigo ID numbers on them.
    /// now for ever CRM account with FREZZOR id we check exigo accounts without GUID's for id number if we have a match
    /// we write guid to exigo.
    /// </summary>
    public class UpdateExigoGuids : ExigoGetCustomers
    {
        #region Properties
        private List<ExigoContact> WithoutGuid { get; set; }
        private List<XrmV2.Contact> CrmContactsWithFrezzorIDs { get; set; }

        public int NumberOfContactsUpdated { get; set; }
        public int NumberOfContactsWithoutGuids
        {
            get { return WithoutGuid.Count(); }
        }

        #endregion Properties

        public UpdateExigoGuids() 
        {
             WithoutGuid = base.GetAllAccountsWithoutCrmGUIDs();
             CrmContactsWithFrezzorIDs = new CrmQueries().FREZZORContactsinCRM.ToList();
            
        }


       public void ExecuteUpdate()
        {
           foreach(var crmContact in CrmContactsWithFrezzorIDs)
           {
               if(WithoutGuid.Where(c=>c.ExigoID == crmContact.new_FREZZORID).FirstOrDefault() !=null)
               {
                   var exigoContact = WithoutGuid.Where(c => c.ExigoID == crmContact.new_FREZZORID).FirstOrDefault();
                   exigoContact.CrmGuid = crmContact.Id.ToString();
                    var response = ExigoApiContext.CreateWebServiceContext().UpdateCustomer(new UpdateCustomerRequest()
                       {
                           CustomerID = exigoContact.ExigoID,
                           Field8 = exigoContact.CrmGuid
                       });
               }
           }        
        }


    }
}
