using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Models;
using XrmV2;

namespace DataControls
{
   public class NewAccountsControl
    {
        const string Contact = "contact";
        #region Properties
        protected List<ExigoContact> NewExigoAccounts 
        {
            get
            {
                if (_newExgioAccounts == null)
                {
                    _newExgioAccounts = new List<ExigoContact>();                    
                }
                return _newExgioAccounts;
            }
        }
        private List<ExigoContact> _newExgioAccounts;

        protected ExigoGetCustomers ExigoCustomersApi
        {
            get
            {
                if (_exigoCustomers == null)
                {
                    _exigoCustomers = new ExigoGetCustomers();
                }
                return _exigoCustomers;
            }
        }
        private ExigoGetCustomers _exigoCustomers;

        protected Dictionary<ExigoContact, Contact> AccountsFoundInCrm
        {
            get
            {
                if (_accountsFoundInCrm == null)
                {
                    _accountsFoundInCrm = new Dictionary<ExigoContact, Contact>();
                }
                return _accountsFoundInCrm;
            }
        }
        private Dictionary<ExigoContact, Contact> _accountsFoundInCrm;        

        protected CrmQueries CrmQ
        {
            get
            {
                if (_crmQueries == null)
                {
                    _crmQueries = new CrmQueries();
                }
                return _crmQueries;
            }
        }
        private CrmQueries _crmQueries;

        private List<Contact> _newCrmAccountsToSubmit { get; set; }
        private Updater updater;
        private XrmV2.Context _crmContext;
        #endregion

       #region Constructors
       public NewAccountsControl()
        {
            updater = new Updater();
            GetNewAccounts();
        }        
        

        public void CreateNewAccounts()
        {
            if(NewExigoAccounts.Count >0)
            {
                NewExigoAccounts.ForEach(CreateNewCrmAccount);
                _newCrmAccountsToSubmit = updater.RetreiveAccounts();
                _crmContext = DataAccess.Utilities.GetCRMContext();
                _newCrmAccountsToSubmit.ForEach(SubmitToCrm);
                AccountsFoundInCrm.AsParallel().ForAll(SendKeyValueToExigo);
                
            }
        }
        #endregion
        
        private void GetNewAccounts()
        {
            var contacts = ExigoCustomersApi.GetAccountsGreaterThanID(CrmQ.GetLastIDNumber()).Where(c=>string.IsNullOrEmpty(c.CrmGuid)).ToList();                      
            contacts.ForEach(FindAccountInCRM);
            _newCrmAccountsToSubmit = new List<Contact>();
           
        }
        private void FindAccountInCRM(ExigoContact contact)
        {
            if(CrmQ.FREZZORContactsinCRM.Where(c=>c.new_FREZZORID == contact.ExigoID).Count() >=1)
            {
                return;
            }
            var results=  CrmQ.SearchForContact(contact.FirstName, contact.LastName, contact.Email);
            if(results.Count() >0  )
            {
                contact.CrmGuid = results.FirstOrDefault().Id.ToString();
                AccountsFoundInCrm.Add(contact, results.FirstOrDefault());
            }
            else
            {
                NewExigoAccounts.Add(contact);                
            }
        
        }

        private void CreateNewCrmAccount(ExigoContact eContact)
        {          
          Contact cContact = new Contact();
          updater.CheckForUpdate(eContact, cContact);
        }

        private void SubmitToCrm(Contact cContact)
        {
           var guid = _crmContext.Create(cContact);
           UpdateExigo((int)cContact.new_FREZZORID, guid);            
        }

        private void SendKeyValueToExigo(KeyValuePair<ExigoContact,Contact> entry)
        {
            UpdateExigo(entry.Key.ExigoID, entry.Value.Id);
            updater.CheckForUpdate(entry.Key, entry.Value);
        }
        
        private void UpdateExigo(int ExigoID, Guid CrmID)
        {
            ExigoApiContext.CreateWebServiceContext().UpdateCustomer(new ExigoDesktop.Exigo.WebService.UpdateCustomerRequest()
            {
                CustomerID = ExigoID,
                Field8 = CrmID.ToString()
            });
        }


    }
}
