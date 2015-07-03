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
    /// <summary>
    /// This class is used to preform incremntal updates to crm.
    /// Here is a short breakdown of how we update
    /// during instantization we get the date of last modifed in exigo date from crm 
    /// then we subtract 1 day (done just to be inclusive of all possible changes) then we pull from exigo all acoounts modifed after
    /// this date.... finally we compare accounts in CRM.... and update where needed.
    ///  ***** important to run Remove Terminated Accounts, and UpdatedExigoGUIDs before running.
    /// 
    /// </summary>
    public class UpdateModifiedAccounts : CrmQueries
    {

        protected DateTime LastCrmModDate;


        protected List<ExigoContact> AccountsToUpdate
        {
            get
            {
                if(_accountsToUpdate == null)
                {
                    _accountsToUpdate = exigoContext.GetAccountsGreaterThanModfiedOn(LastCrmModDate).Where(e => !string.IsNullOrEmpty(e.CrmGuid)).ToList();
                }
                return _accountsToUpdate;
            }
        }
        private List<ExigoContact> _accountsToUpdate;

        protected List<ExigoContact> ExigoRemoveGUIDs
        {
            get
            {
                if (_exigoRemoveGUIDs == null)
                {
                    _exigoRemoveGUIDs = new List<ExigoContact>();
                }
                return _exigoRemoveGUIDs;
            }
        }
        private List<ExigoContact> _exigoRemoveGUIDs;

        private ExigoGetCustomers exigoContext;
        private Updater updater;

        protected List<Contact> crmAccounts;
        
       

        public UpdateModifiedAccounts()
        {
            exigoContext = new ExigoGetCustomers();
            updater = new Updater();
            crmAccounts = new List<Contact>();
            LastCrmModDate = base.GetLastExigoModifedDate().Subtract(TimeSpan.FromDays(1));
            _accountsToUpdate = exigoContext.GetAccountsGreaterThanModfiedOn(LastCrmModDate).Where(e => !string.IsNullOrEmpty(e.CrmGuid)).ToList();
            /// uncomment line below and comment out line above to update all accounts
            //_accountsToUpdate = exigoContext.GetAccountsGreaterThanModfiedOn(new DateTime(2010,01,01)).Where(e => !string.IsNullOrEmpty(e.CrmGuid)).ToList();
            
        }



        public void IncrementalUpdate()
        {
            crmAccounts.Clear();
            AccountsToUpdate.ForEach(PopulateCrmAccounts);
         
            ExigoRemoveGUIDs.ForEach(a => AccountsToUpdate.Remove(a));
            RemoveGuids();
            foreach (var c in crmAccounts)
            {
                RunThroughUpdater(c);
            }
            updater.UpdateAllContacts();

        }


        public void UpdateFrom(DateTime Date)
        {
            crmAccounts.Clear();
            List<ExigoContact> accounts = exigoContext.GetAccountsGreaterThanModfiedOn(Date);
            accounts.AsParallel().ForAll(PopulateCrmAccounts);
            ExigoRemoveGUIDs.ForEach(a => AccountsToUpdate.Remove(a));
            foreach (var c in crmAccounts)
            {
                RunThroughUpdater(c);
            }            
            updater.UpdateAllContacts();


            
            
        }
        /// <summary>
        /// Here we are looking for an account in CRM using the GUID in Exigo if we find the Account
        /// by GUID we are adding to crmAccounts list if we cannot find the GUID we add the account to the
        /// ExigoRemoveGUIDs List.
        /// </summary>
        /// <param name="Econtact"></param>
        private void PopulateCrmAccounts(ExigoContact Econtact)
        {
            try
            {
              crmAccounts.Add(base.SearchForContact(Econtact.GetGUID()));
              
            }
            catch
            {
                ExigoRemoveGUIDs.Add(Econtact);
            }

            
        }

        private void RunThroughUpdater(Contact crmContact)
        {
            var t = AccountsToUpdate.FirstOrDefault(c=>c.CrmGuid == crmContact.Id.ToString());
            if(t !=null)
            {
                updater.CheckForUpdate(t, crmContact);
            }
        }
        
        private void RemoveGuids()
        {
            ExigoDesktop.Exigo.WebService.UpdateCustomerRequest req = new ExigoDesktop.Exigo.WebService.UpdateCustomerRequest(){Field8 = string.Empty};
            
            foreach(var c in ExigoRemoveGUIDs)
            {
                req.CustomerID = c.ExigoID;
                exigoContext.UpdateSingleCustomer(req);
            }
        }


    }
}
