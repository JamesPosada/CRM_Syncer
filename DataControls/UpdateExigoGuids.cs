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
    public class UpdateExigoGuids
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
             WithoutGuid = new ExigoGetCustomers().GetAllAccountsWithoutCrmGUIDs();
             CrmContactsWithFrezzorIDs = new CrmQueries().FREZZORContactsinCRM.ToList();
             NumberOfContactsUpdated = 0;
        }


       public void ExecuteUpdate()
        {
           

           foreach(var crmContact in CrmContactsWithFrezzorIDs)
           {
               if(WithoutGuid.Where(c=>c.ExigoID == crmContact.new_FREZZORID).FirstOrDefault() !=null)
               {
                   var exContact = WithoutGuid.Where(c => c.ExigoID == crmContact.new_FREZZORID).FirstOrDefault();
                   exContact.CrmGuid = crmContact.Id.ToString();
                    var response = ExigoApiContext.CreateWebServiceContext().UpdateCustomer(new UpdateCustomerRequest()
                       {
                           CustomerID = exContact.ExigoID,
                           Field8 = exContact.CrmGuid
                       });
                   if(response.Result.Status == ResultStatus.Success)
                   { 
                       ++NumberOfContactsUpdated;
                   }
               }
           }        
        }


    }
}
