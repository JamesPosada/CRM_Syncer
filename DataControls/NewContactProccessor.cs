using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using XrmV2;
using DataLogger;


namespace DataControls
{
    public class NewContactProccessor : DataAccess.CrmQueries
    {
        #region Properties
        protected List<ExigoContact> UncheckedAccounts
        {
            get
            {
                if (_uncheckedAccounts == null)
                {
                    _uncheckedAccounts = new List<ExigoContact>();
                }
                return _uncheckedAccounts;
            }
        }
        private List<ExigoContact> _uncheckedAccounts;

        protected List<Contact> CrmWithExigoIDs
        {
            get
            {
                if (_crmWithExigoIDs == null)
                {
                    _crmWithExigoIDs = base.FREZZORContactsinCRM.ToList();
                }
                return _crmWithExigoIDs;
            }
        }
        private List<Contact> _crmWithExigoIDs;

        protected List<ExigoContact> NoMatchInCRMFound
        {
            get
            {
                if (_noMatchInCRMFound == null)
                {
                    _noMatchInCRMFound = new List<ExigoContact>();
                }
                return _noMatchInCRMFound;
            }
        }
        private List<ExigoContact> _noMatchInCRMFound;

        protected Dictionary<ExigoContact, Contact> MatchInCRMFoundByID
        {
            get
            {
                if (_matchInCRMFoundByID == null)
                {
                    _matchInCRMFoundByID = new Dictionary<ExigoContact, Contact>();
                }
                return _matchInCRMFoundByID;
            }
        }
        private Dictionary<ExigoContact, Contact> _matchInCRMFoundByID;


        protected Dictionary<ExigoContact, Contact> MatchInCRMFoundByInfo
        {
            get
            {
                if (_matchInCRMFoundByInfo == null)
                {
                    _matchInCRMFoundByInfo = new Dictionary<ExigoContact, Contact>();
                }
                return _matchInCRMFoundByInfo;
            }
        }
        private Dictionary<ExigoContact, Contact> _matchInCRMFoundByInfo;
        

        private DataAccess.ExigoGetCustomers exigoContext;


        #endregion


        #region Constructors
        public NewContactProccessor()
        {
            exigoContext = new DataAccess.ExigoGetCustomers();
            Settings.Logging.logger.SetLogName(Settings.Logging.AccountsUpdateLog.Name, Settings.Logging.AccountsUpdateLog.Headers);
            _crmWithExigoIDs = base.FREZZORContactsinCRM.ToList();
        }
        /// <summary>
        /// may provide an ExigoGetCustomers otherwise constructor will create one
        /// </summary>
        /// <param name="econtext"></param>
        public NewContactProccessor(DataAccess.ExigoGetCustomers econtext)
        {
            exigoContext = econtext;
            Settings.Logging.logger.SetLogName(Settings.Logging.AccountCheckingLog.Name, Settings.Logging.AccountCheckingLog.Headers);
            _crmWithExigoIDs = base.FREZZORContactsinCRM.ToList();
        }
        #endregion

        #region Public Methods

        public void ProcessList()
        {
            _uncheckedAccounts.ForEach(CheckCrmforContact);            
            _uncheckedAccounts.Clear();
            
        }

        /// <summary>
        /// Provide a List of Exigo Contacts to check and see if they can be linked to an
        /// existing CRM Account or if a new CRM Account needs to be created.
        /// </summary>
        /// <param name="listToCheck"></param>
        public void SetListToCheck(List<ExigoContact> listToCheck)
        {
            if (UncheckedAccounts.Count >0)
            {
               foreach(var c in listToCheck)
               {
                   if(!UncheckedAccounts.Contains(c))
                   {
                       UncheckedAccounts.Add(c);
                   }
               }
            }
            else
            {
                UncheckedAccounts.AddRange(listToCheck);
            }
        }
        
        #endregion

        #region Private Methods

        private void CheckCrmforContact(ExigoContact exigoContact)
        {
            if(CrmWithExigoIDs.AsParallel().Where(c=>c.new_FREZZORID == exigoContact.ExigoID).Count() > 0)
            {
                var f = CrmWithExigoIDs.AsParallel().Where(c => c.new_FREZZORID == exigoContact.ExigoID).FirstOrDefault();
                MatchInCRMFoundByID.Add(exigoContact, f);
                Settings.Logging.logger.LogData(Settings.Logging.AccountCheckingLog.Name, new List<string>() { exigoContact.ExigoID.ToString(),f.new_FREZZORID.ToString(),  "True", f.Id.ToString(), exigoContact.FirstName, exigoContact.LastName, exigoContact.Email, f.FirstName, f.LastName, f.EMailAddress1 });
                return;
            }
            else
            {
                var t = base.SearchForContact(exigoContact.FirstName, exigoContact.LastName, exigoContact.Email).FirstOrDefault();
                if(t !=null)
                {
                    MatchInCRMFoundByInfo.Add(exigoContact, t);
                    Settings.Logging.logger.LogData(Settings.Logging.AccountCheckingLog.Name, new List<string>() { exigoContact.ExigoID.ToString(), t.new_FREZZORID.ToString(), "True", t.Id.ToString(), exigoContact.FirstName, exigoContact.LastName, exigoContact.Email, t.FirstName, t.LastName, t.EMailAddress1 });
                    return;
                }
                
            }


            NoMatchInCRMFound.Add(exigoContact);
            Settings.Logging.logger.LogData(Settings.Logging.AccountCheckingLog.Name, new List<string>() { exigoContact.ExigoID.ToString(), "", "False", "", exigoContact.FirstName, exigoContact.LastName, exigoContact.Email, "", "","" });
        }
        
        #endregion

        

    }
}
