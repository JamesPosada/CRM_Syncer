using DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XrmV2;

namespace DataControls
{
    public class RemoveTerminatedAccounts :CrmQueries
    {
        protected List<ExigoContact> TermFromExigo
        {
            get
            {
                if(_termFromExigo == null)
                {
                    _termFromExigo = new List<ExigoContact>();
                }
                return _termFromExigo; 
            }
        }
        private List<ExigoContact> _termFromExigo;

        private XrmV2.Context crmContext { get; set; }

        public RemoveTerminatedAccounts()
        {
            TermFromExigo.AddRange(new ExigoGetCustomers().GetTerminatedAccounts());
            crmContext = DataAccess.Utilities.GetCRMContext();
        }

        public void RemoveTermsNow()
        {
          foreach( var a in TermFromExigo)
            {
                LocateTermCrmAccount(a);
            }
        }

        private void LocateTermCrmAccount(ExigoContact eContact)
        {
            var crmAccountList = SearchForContact(eContact.GetGUID(), eContact.FirstName, eContact.LastName);
            if (crmAccountList.Count() > 0)
            {
                var crmAccount = crmAccountList.FirstOrDefault();
                if (crmAccount.Id == eContact.GetGUID() && crmAccount.FirstName == eContact.FirstName && crmAccount.LastName == eContact.LastName)
                {
                    crmContext.Update(new Contact()
                    {
                        Id = eContact.GetGUID(),
                        New_Status = (int)eContact.Status
                    });

                    
                }               
            }
            ExigoApiContext.CreateWebServiceContext().UpdateCustomer(new ExigoDesktop.Exigo.WebService.UpdateCustomerRequest()
            {
                CustomerID = eContact.ExigoID,
                Field8 = string.Empty
            });
        }

        

    }
}
