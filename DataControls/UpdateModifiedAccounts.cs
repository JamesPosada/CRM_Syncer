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

        private ExigoGetCustomers exigoContext;
        private Updater updater;

        protected List<Contact> crmAccounts;
       

        public UpdateModifiedAccounts()
        {
            exigoContext = new ExigoGetCustomers();
            updater = new Updater();
            crmAccounts = new List<Contact>();
            LastCrmModDate = base.GetLastExigoModifedDate().Subtract(TimeSpan.FromDays(2));
            _accountsToUpdate = exigoContext.GetAccountsGreaterThanModfiedOn(LastCrmModDate).Where(e => !string.IsNullOrEmpty(e.CrmGuid)).ToList();
            
        }



        public void IncrementalUpdate()
        {
            crmAccounts.Clear();
            AccountsToUpdate.AsParallel().ForAll(PopulateCrmAccounts);
            Parallel.ForEach<Contact>(crmAccounts, RunThroughUpdater);
        }


        public void UpdateFrom(DateTime Date)
        {
            crmAccounts.Clear();
            List<ExigoContact> accounts = exigoContext.GetAccountsGreaterThanModfiedOn(Date);
            accounts.AsParallel().ForAll(PopulateCrmAccounts);
            Parallel.ForEach<Contact>(crmAccounts, RunThroughUpdater);
        }

        private void PopulateCrmAccounts(ExigoContact Econtact)
        {
            crmAccounts.Add(base.SearchForContact(Econtact.GetGUID()));
        }

        private void RunThroughUpdater(Contact crmContact)
        {
            var t = AccountsToUpdate.First(c=>c.ExigoID == crmContact.new_FREZZORID & c.CrmGuid == crmContact.Id.ToString());
            if(t !=null)
            {
                updater.CheckForUpdate(t, crmContact);
            }
        }
        

    }
}
