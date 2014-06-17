using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using DataAccess;
using XrmV2;

namespace DataControls
{
    public class ContactControl
    {

        #region Properties

 
        private bool _currentAccountIsModified;
        private Contact _currentAccount;
        private List<Contact> _changedAccounts;
        private List<Contact> _accountsToCreate;



        #endregion


        public ContactControl()
        {
            _changedCrmIDs = new Dictionary<string, bool>();
            _changedAccounts = new List<Contact>();
            _accountsToCreate = new List<Contact>();
           
        }
                
        #region Public Methods
        public void CheckForUpdate(ExigoContact exigoContact, Contact crmContact)
        {
            var guid = crmContact.Id.ToString();
            _currentAccountIsModified = false;
            _currentAccount = new Contact();
            _currentAccount.Id = crmContact.Id;
            if (!stringFieldMatch(exigoContact.FirstName, crmContact.FirstName, guid, FirstName))
            {
                _currentAccount.FirstName = exigoContact.FirstName;
            }

            if(!stringFieldMatch(exigoContact.LastName, crmContact.LastName, guid, LastName)){
                _currentAccount.LastName = exigoContact.LastName;
            }

            if (!string.IsNullOrEmpty(exigoContact.MobilePhone) && !stringFieldMatch(exigoContact.MobilePhone, crmContact.MobilePhone, guid, MobilePhone))
            {
                _currentAccount.MobilePhone = exigoContact.MobilePhone;
            }

            if(!string.IsNullOrEmpty(exigoContact.BusinessPhone) && !stringFieldMatch(exigoContact.BusinessPhone,crmContact.Telephone1,guid,BusinessPhone)){
                _currentAccount.Telephone1 =exigoContact.BusinessPhone;
            }

            if (!string.IsNullOrEmpty(exigoContact.HomePhone) && !stringFieldMatch(exigoContact.HomePhone, crmContact.Telephone2, guid, HomePhone))
            {
                _currentAccount.Telephone2 = exigoContact.HomePhone;
            }

            if (!stringFieldMatch(exigoContact.StreetAddress, crmContact.Address1_Line1, guid, Address1Line1))
            {
                _currentAccount.Address1_Line1 = exigoContact.StreetAddress;
            }

            if (!string.IsNullOrEmpty(exigoContact.StreetAddress_Line2) && !stringFieldMatch(exigoContact.StreetAddress_Line2, crmContact.Address1_Line2, guid, Address1Line2))
            {
                _currentAccount.Address1_Line2 = exigoContact.StreetAddress_Line2;
            }

            if (!stringFieldMatch(exigoContact.City, crmContact.Address1_City, guid, Address1City))
            {
                _currentAccount.Address1_City = exigoContact.City;
            }

            if (!stringFieldMatch(exigoContact.Zip, crmContact.Address1_PostalCode, guid, Address1ZipCode))
            {
                _currentAccount.Address1_PostalCode = exigoContact.Zip;
            }

            if (!stringFieldMatch(exigoContact.Country, crmContact.Address1_Country, guid, Address2Country))
            {
                _currentAccount.Address1_Country = exigoContact.Country;
            }

            


            /// checking nullable fields
            if (crmContact.new_FREZZORID == null)
            {
                _currentAccount.new_FREZZORID = exigoContact.ExigoID;
                UpdateNullField(guid, FrezzorID);
            }
            else{
                if (!intFieldMatch(exigoContact.ExigoID, (int)crmContact.new_FREZZORID, guid, FrezzorID))
                {
                    _currentAccount.new_FREZZORID = exigoContact.ExigoID;
                }
            }

            if(crmContact.new_EnrollerID == null)
            {
                _currentAccount.new_EnrollerID = exigoContact.EnrollerID;
                UpdateNullField(guid, EnrollerId);
            }
            else
            {
                if (!intFieldMatch(exigoContact.EnrollerID, (int)crmContact.new_EnrollerID, guid, EnrollerId))
                {
                    _currentAccount.new_EnrollerID = exigoContact.EnrollerID;
                }
            }

            if (crmContact.new_FREZZOR_Language == null)
            {
                _currentAccount.new_FREZZOR_Language = exigoContact.LangID;
                UpdateNullField(guid, Language);
            }
            else
            {
                if (!intFieldMatch(exigoContact.LangID, (int)crmContact.new_FREZZOR_Language, guid, Language))
                {
                    _currentAccount.new_FREZZOR_Language = exigoContact.LangID;
                }
            }

            if (crmContact.New_Type == null)
            {
                _currentAccount.New_Type = (int)exigoContact.CustomerType;
                UpdateNullField(guid, FrezzorCustomerType);
            }
            else
            {
                if (!intFieldMatch((int)exigoContact.CustomerType, (int)crmContact.New_Type, guid, FrezzorCustomerType))
                {
                    _currentAccount.New_Type = (int)crmContact.New_Type;
                }
            }

            if (crmContact.New_Status == null)
            {
                _currentAccount.New_Status = (int)exigoContact.Status;
                UpdateNullField(guid, FrezzorStatus);
            }
            else
            {
                if (!intFieldMatch((int)exigoContact.Status, (int)crmContact.New_Status, guid, FrezzorStatus))
                {
                    _currentAccount.New_Status = (int)exigoContact.Status;
                }
            }

            /// before we change emails we must move email stored in email1 to email3
            if (!stringFieldMatch(exigoContact.Email, crmContact.EMailAddress1,guid,Email))
            {
                _currentAccount.EMailAddress3 = crmContact.EMailAddress1;
                _currentAccount.EMailAddress1 = exigoContact.Email;
            }

            if(exigoContact.GetBirthYear() != "1900")
            {
                if (!intFieldMatch(exigoContact.GetBirthMonth(), (int)crmContact.New_BirthMonth, guid, MonthOfBirth))
                {
                    _currentAccount.New_BirthMonth = exigoContact.GetBirthMonth();
                }
                if (!intFieldMatch(exigoContact.GetBirthDay(), (int)crmContact.New_BirthdayofMonth, guid, DayOfBirth))
                {
                    _currentAccount.New_BirthdayofMonth = exigoContact.GetBirthDay();
                }
                if (!stringFieldMatch(exigoContact.GetBirthYear(), (string)crmContact.New_BirthYear, guid, YearOfBirth))
                {
                    _currentAccount.New_BirthYear = exigoContact.GetBirthYear();
                }
            }

            /// only update rank if rank in crm is less than current rank
            if (crmContact.New_Rank == null)
            {
               _currentAccount.New_Rank = exigoContact.Rank;
                UpdateNullField(guid, Rank);
            }
            else
            {
                if (crmContact.New_Rank < exigoContact.Rank)
                {
                    if (!intFieldMatch(exigoContact.Rank, (int)crmContact.New_Rank, guid, Rank))
                    {
                        _currentAccount.New_Rank = exigoContact.Rank;
                    }
                }
            }
           
            /// crm does not use states for countries other than US
            if (exigoContact.Country == "USA")
            {
                if (!stringFieldMatch(crmContact.Address1_StateOrProvince, exigoContact.State, guid, Address1State))
                {
                    _currentAccount.Address1_StateOrProvince = exigoContact.State;
                }
            }

            if(crmContact.new_ExigoLastModifiedDate == null)
            {
                UpdateNullField(guid, FrezzorLastModDate);
                _currentAccount.new_ExigoLastModifiedDate = exigoContact.LastModified;
            }
            else 
            {
                if (!stringFieldMatch(exigoContact.LastModified.AddHours(7).ToString(), crmContact.new_ExigoLastModifiedDate.ToString(), guid, FrezzorLastModDate))
                {
                    _currentAccount.new_ExigoLastModifiedDate = exigoContact.LastModified;
                }
            }

            /// if gender is unknown exigo contact model returns empty string
            if(!string.IsNullOrEmpty(exigoContact.Salutation))
            {
                if (!stringFieldMatch(exigoContact.Salutation, crmContact.Salutation, guid, Salutation))
                {
                    _currentAccount.Salutation = exigoContact.Salutation;
                }
            }
            if (exigoContact.CustomerType == ExigoCustomerType.Independant)
            {
                if (!stringFieldMatch(exigoContact.WebAlias, crmContact.New_WebAlias, guid, WebAlias))
                {
                    _currentAccount.New_WebAlias = exigoContact.WebAlias;
                }
            }

            /// update enroller webalias if not distributor and enroller has webalias
            if(!string.IsNullOrEmpty(exigoContact.EnrollerWebAlias) && exigoContact.CustomerType != ExigoCustomerType.Independant)
            {
                if (!stringFieldMatch(exigoContact.EnrollerWebAlias, (string)crmContact.new_EnrollerWebAlias, guid, EnrollerWebAlias))
                {
                    _currentAccount.new_EnrollerWebAlias = exigoContact.EnrollerWebAlias;
                }
            }
            if (_currentAccountIsModified)
            {
                _changedAccounts.Add(_currentAccount);
                
            }
        }
        public List<Contact> UpdateAllContacts()
        {
            ProccessUpDates();   
            var temp = _changedAccounts;
            _changedAccounts.Clear();
            return temp;
           
        }

        public void VerifyAccountNotInExigo(ExigoContact exigoContact)
        {

        }


        #endregion
     


        #region Private Methods
        private void ProccessUpDates()
        {
            XrmV2.Context crmContext =  DataAccess.Utilities.GetCRMContext();
            if (_changedAccounts.Count > 0)
            {
                foreach(var contact in _changedAccounts)
                {
                    crmContext.Update(contact);
                }            
            }
           

        }

        #endregion Private Methods

        #region Logging Dictionary Methods

        public Dictionary<string, bool> ChangedCrmIDs
        {
            get
            {
                return _changedCrmIDs;
            }
        }
        private Dictionary<string, bool> _changedCrmIDs { get; set; }


        private bool stringFieldMatch(string incoming, string existing, string Guid, string FieldUpdated)
        {

            if (incoming == existing)
            {
                _changedCrmIDs.Add(Guid + " " + FieldUpdated, false);
                return true;
            }
            if (incoming != existing)
            {
                _changedCrmIDs.Add(Guid + " " + FieldUpdated, true);
                _currentAccountIsModified = true;
                return false;
            }
            return true;

        }

        private bool intFieldMatch(int incoming, int existing, string Guid, string FieldUpdated)
        {
            if (incoming == existing)
            {
                _changedCrmIDs.Add(Guid + " " + FieldUpdated, false);
                return true;
            }
            if (incoming != existing)
            {
                _changedCrmIDs.Add(Guid + " " + FieldUpdated, true);
                _currentAccountIsModified = true;
                return false;
            }
            return true;

        }

        private void UpdateNullField(string Guid, string FieldUpdated)
        {
            _changedCrmIDs.Add(Guid + " " + FieldUpdated, true);
            _currentAccountIsModified = true;
        }

        public List<string> GetAllUpDates()
        {
            return ChangedCrmIDs.Where(k => k.Value == true).Select(k => k.Key).ToList();
        }

        #endregion Logging Dictionary

        #region Constants
        const string Contact = "contact";
        const string FirstName = "firstname";
        const string Salutation = "salutation";
        const string LastName = "lastname";
        const string FrezzorID = "new_frezzorid";
        const string FrezzorLastModDate = "new_exigolastmodifieddate";
        const string ModifedOnCrmDate = "modifiedon";
        const string Email = "emailaddress1";
        const string Email3 = "emailaddress2";
        const string HomePhone = "telephone2";
        const string BusinessPhone = "telephone1";
        const string MobilePhone = "mobilephone";
        const string DayOfBirth = "new_birthdayofmonth";
        const string MonthOfBirth = "new_birthmonth";
        const string YearOfBirth = "new_birthyear";
        const string Rank = "new_rank";
        const string Language = "new_frezzor_language";
        const string PhoneSync = "new_phonesync";
        const string EnrollerId = "new_enrollerid";
        const string EnrollerWebAlias = "new_enrollerwebalias";
        const string SkypeID = "new_skypeid";
        const string WebAlias = "new_webalias";
        const string FrezzorCustomerType = "new_type";
        const string FrezzorStatus = "new_status";
        const string ContactGUID = "contactid";
        const string Address1Line1 = "address1_line1";
        const string Address1Line2 = "address1_line2";
        const string Address1City = "address1_city";
        const string Address1State = "address1_stateorprovince";
        const string Address1ZipCode = "address1_postalcode";
        const string Address1Country = "address1_country";
        const string Address2Line1 = "address2_line1";
        const string Address2Line2 = "address2_line2";
        const string Address2City = "address2_city";
        const string Address2State = "address2_stateorprovince";
        const string Address2ZipCode = "address2_postalcode";
        const string Address2Country = "address2_country";

        #endregion
    }
}
