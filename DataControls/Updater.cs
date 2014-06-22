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
    public class Updater
    {

        #region Properties


        private bool _currentAccountIsModified;
        private Contact _currentAccount;
        private List<Contact> _changedAccounts;
        //private List<Contact> _accountsToCreate;



        #endregion


        public Updater()
        {
            Settings.Logging.logger.SetLogName(Settings.Logging.AccountsUpdateLog.Name, Settings.Logging.AccountsUpdateLog.Headers);

            _changedAccounts = new List<Contact>();
            //_accountsToCreate = new List<Contact>();

        }

        #region Public Methods
        /// <summary>
        /// Comapares an Exigo Account and a CRM Account if account has changed returns CRM Account with only
        /// updates made on it.
        /// </summary>
        /// <param name="exigoContact"></param>
        /// <param name="crmContact"></param>
        public void CheckForUpdate(ExigoContact exigoContact, Contact crmContact)
        {
            var guid = crmContact.Id.ToString();
            _currentAccountIsModified = false;
            _currentAccount = new Contact();
            if (crmContact != null && crmContact.Id.ToString() != "00000000-0000-0000-0000-000000000000")
            {
                _currentAccount.Id = crmContact.Id;
            }
            if (!stringFieldMatch(exigoContact.FirstName, crmContact.FirstName, guid, FirstName, exigoContact.ExigoID))
            {
                _currentAccount.FirstName = exigoContact.FirstName;
            }

            if (!stringFieldMatch(exigoContact.LastName, crmContact.LastName, guid, LastName, exigoContact.ExigoID))
            {
                _currentAccount.LastName = exigoContact.LastName;
            }

            if (!string.IsNullOrEmpty(exigoContact.MobilePhone) && !stringFieldMatch(exigoContact.MobilePhone, crmContact.MobilePhone, guid, MobilePhone, exigoContact.ExigoID))
            {
                _currentAccount.MobilePhone = exigoContact.MobilePhone;
            }

            if (!string.IsNullOrEmpty(exigoContact.BusinessPhone) && !stringFieldMatch(exigoContact.BusinessPhone, crmContact.Telephone1, guid, BusinessPhone, exigoContact.ExigoID))
            {
                _currentAccount.Telephone1 = exigoContact.BusinessPhone;
            }

            if (!string.IsNullOrEmpty(exigoContact.HomePhone) && !stringFieldMatch(exigoContact.HomePhone, crmContact.Telephone2, guid, HomePhone, exigoContact.ExigoID))
            {
                _currentAccount.Telephone2 = exigoContact.HomePhone;
            }

            if (!stringFieldMatch(exigoContact.StreetAddress, crmContact.Address1_Line1, guid, Address1Line1, exigoContact.ExigoID))
            {
                _currentAccount.Address1_Line1 = exigoContact.StreetAddress;
            }

            if (!string.IsNullOrEmpty(exigoContact.StreetAddress_Line2) && !stringFieldMatch(exigoContact.StreetAddress_Line2, crmContact.Address1_Line2, guid, Address1Line2, exigoContact.ExigoID))
            {
                _currentAccount.Address1_Line2 = exigoContact.StreetAddress_Line2;
            }

            if (!stringFieldMatch(exigoContact.City, crmContact.Address1_City, guid, Address1City, exigoContact.ExigoID))
            {
                _currentAccount.Address1_City = exigoContact.City;
            }

            if (!stringFieldMatch(exigoContact.Zip, crmContact.Address1_PostalCode, guid, Address1ZipCode, exigoContact.ExigoID))
            {
                _currentAccount.Address1_PostalCode = exigoContact.Zip;
            }

            if (!stringFieldMatch(exigoContact.Country, crmContact.Address1_Country, guid, Address2Country, exigoContact.ExigoID))
            {
                _currentAccount.Address1_Country = exigoContact.Country;
            }




            /// checking nullable fields
            if (crmContact.new_FREZZORID == null)
            {
                _currentAccount.new_FREZZORID = exigoContact.ExigoID;
                UpdateNullField(guid, FrezzorID, exigoContact.ExigoID.ToString(), exigoContact.ExigoID);
            }
            else
            {
                if (!intFieldMatch(exigoContact.ExigoID, (int)crmContact.new_FREZZORID, guid, FrezzorID, exigoContact.ExigoID))
                {
                    _currentAccount.new_FREZZORID = exigoContact.ExigoID;
                }
            }

            if (crmContact.new_EnrollerID == null)
            {
                _currentAccount.new_EnrollerID = exigoContact.EnrollerID;
                UpdateNullField(guid, EnrollerId, exigoContact.EnrollerID.ToString(), exigoContact.ExigoID);
            }
            else
            {
                if (!intFieldMatch(exigoContact.EnrollerID, (int)crmContact.new_EnrollerID, guid, EnrollerId, exigoContact.ExigoID))
                {
                    _currentAccount.new_EnrollerID = exigoContact.EnrollerID;
                }
            }

            if (crmContact.new_FREZZOR_Language == null)
            {
                _currentAccount.new_FREZZOR_Language = exigoContact.LangID;
                UpdateNullField(guid, Language, exigoContact.LangID.ToString(), exigoContact.ExigoID);
            }
            else
            {
                if (!intFieldMatch(exigoContact.LangID, (int)crmContact.new_FREZZOR_Language, guid, Language, exigoContact.ExigoID))
                {
                    _currentAccount.new_FREZZOR_Language = exigoContact.LangID;
                }
            }

            if (crmContact.New_Type == null)
            {
                _currentAccount.New_Type = (int)exigoContact.CustomerType;
                UpdateNullField(guid, FrezzorCustomerType, exigoContact.CustomerType.ToString(), exigoContact.ExigoID);
            }
            else
            {
                if (!intFieldMatch((int)exigoContact.CustomerType, (int)crmContact.New_Type, guid, FrezzorCustomerType, exigoContact.ExigoID))
                {
                    _currentAccount.New_Type = (int)crmContact.New_Type;
                }
            }

            if (crmContact.New_Status == null)
            {
                _currentAccount.New_Status = (int)exigoContact.Status;
                UpdateNullField(guid, FrezzorStatus, Enum.GetName(exigoContact.Status.GetType(), (int)exigoContact.Status), exigoContact.ExigoID);
            }
            else
            {
                if (!intFieldMatch((int)exigoContact.Status, (int)crmContact.New_Status, guid, FrezzorStatus, exigoContact.ExigoID))
                {
                    _currentAccount.New_Status = (int)exigoContact.Status;
                }
            }

            /// before we change emails we must move email stored in email1 to email3
            if (!stringFieldMatch(exigoContact.Email, crmContact.EMailAddress1, guid, Email, exigoContact.ExigoID))
            {
                _currentAccount.EMailAddress3 = crmContact.EMailAddress1;
                _currentAccount.EMailAddress1 = exigoContact.Email;
            }

            if (exigoContact.GetBirthYear() != "1900")
            {
                if (crmContact.New_BirthMonth.HasValue == false)
                {
                    UpdateNullField(guid, MonthOfBirth, exigoContact.GetBirthMonth().ToString(), exigoContact.ExigoID);
                }
                else
                {
                    if (!intFieldMatch(exigoContact.GetBirthMonth(), (int)crmContact.New_BirthMonth, guid, MonthOfBirth, exigoContact.ExigoID))
                    {
                        _currentAccount.New_BirthMonth = exigoContact.GetBirthMonth();
                    }
                }
                if (crmContact.New_BirthdayofMonth.HasValue == false)
                {
                    UpdateNullField(guid, DayOfBirth.ToString(), exigoContact.GetBirthDay().ToString(), exigoContact.ExigoID);
                }
                else
                {
                    if (!intFieldMatch(exigoContact.GetBirthDay(), (int)crmContact.New_BirthdayofMonth, guid, DayOfBirth, exigoContact.ExigoID))
                    {
                        _currentAccount.New_BirthdayofMonth = exigoContact.GetBirthDay();
                    }
                }
                if (!stringFieldMatch(exigoContact.GetBirthYear(), (string)crmContact.New_BirthYear, guid, YearOfBirth, exigoContact.ExigoID))
                {
                    _currentAccount.New_BirthYear = exigoContact.GetBirthYear();
                }
            }

            /// only update rank if rank in crm is less than current rank
            /// CRM only takes a rank of 1 or above, exigo ranks start a 0
            /// to avoid throwing exception must not use rank of 0
            if (crmContact.New_Rank == null)
            {
                if (exigoContact.Rank > 0)
                {
                    _currentAccount.New_Rank = exigoContact.Rank;
                    UpdateNullField(guid, Rank, exigoContact.Rank.ToString(), exigoContact.ExigoID);
                }
            }
            else
            {
                if (exigoContact.Rank > 0 && crmContact.New_Rank < exigoContact.Rank)
                {
                    if (!intFieldMatch(exigoContact.Rank, (int)crmContact.New_Rank, guid, Rank, exigoContact.ExigoID))
                    {
                        _currentAccount.New_Rank = exigoContact.Rank;
                    }
                }
            }

            /// crm does not use states for countries other than US
            if (exigoContact.Country == "USA")
            {
                if (!stringFieldMatch(crmContact.Address1_StateOrProvince, exigoContact.State, guid, Address1State, exigoContact.ExigoID))
                {
                    _currentAccount.Address1_StateOrProvince = exigoContact.State;
                }
            }
            else
            {
                if (exigoContact.Country != "USA")
                {
                    _currentAccount.Address1_StateOrProvince = string.Empty;
                }

            }




            if (crmContact.new_ExigoLastModifiedDate == null)
            {
                UpdateNullField(guid, FrezzorLastModDate, exigoContact.LastModified.ToString(), exigoContact.ExigoID);
                _currentAccount.new_ExigoLastModifiedDate = exigoContact.LastModified;
            }
            else
            {
                if (!stringFieldMatch(exigoContact.LastModified.AddHours(7).ToString(), crmContact.new_ExigoLastModifiedDate.ToString(), guid, FrezzorLastModDate, exigoContact.ExigoID))
                {
                    _currentAccount.new_ExigoLastModifiedDate = exigoContact.LastModified;
                }
            }

            /// if gender is unknown exigo contact model returns empty string
            if (!string.IsNullOrEmpty(exigoContact.Salutation))
            {
                if (!stringFieldMatch(exigoContact.Salutation, crmContact.Salutation, guid, Salutation, exigoContact.ExigoID))
                {
                    _currentAccount.Salutation = exigoContact.Salutation;
                }
            }
            if (exigoContact.CustomerType == ExigoCustomerType.Independant)
            {
                if (!stringFieldMatch(exigoContact.WebAlias, crmContact.NickName, guid, WebAlias, exigoContact.ExigoID))
                {
                    _currentAccount.NickName = exigoContact.WebAlias;
                }
            }

            /// update enroller webalias if not distributor and enroller has webalias
            if (exigoContact.CustomerType != ExigoCustomerType.Independant && !string.IsNullOrEmpty(exigoContact.EnrollerWebAlias))
            {
                if (!stringFieldMatch(exigoContact.EnrollerWebAlias, (string)crmContact.new_EnrollerWebAlias, guid, EnrollerWebAlias, exigoContact.ExigoID))
                {
                    _currentAccount.new_EnrollerWebAlias = exigoContact.EnrollerWebAlias;
                }
            }

            if (exigoContact.CustomerType != ExigoCustomerType.Independant && !string.IsNullOrEmpty(exigoContact.WebAlias))
            {
                _currentAccount.NickName = string.Empty;
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

        public List<Contact> RetreiveAccounts()
        {
            List<Contact> temp = _changedAccounts.ToList();
            _changedAccounts.Clear();
            return temp;

        }

        #endregion



        #region Private Methods
        private void ProccessUpDates()
        {
            XrmV2.Context crmContext = DataAccess.Utilities.GetCRMContext();
            if (_changedAccounts.Count > 0)
            {
                foreach (var contact in _changedAccounts)
                {
                    crmContext.Update(contact);
                }
            }
        }

        #endregion Private Methods

        #region Logging Dictionary Methods


        private bool stringFieldMatch(string incoming, string existing, string Guid, string FieldUpdated, int exigoID)
        {

            if (incoming == existing)
            {

                return true;
            }
            if (incoming != existing)
            {
                Settings.Logging.logger.LogData(Settings.Logging.AccountsUpdateLog.Name, new List<string>() { Guid, exigoID.ToString(), FieldUpdated, existing, incoming });

                _currentAccountIsModified = true;
                return false;
            }
            return true;

        }

        private bool intFieldMatch(int incoming, int existing, string Guid, string FieldUpdated, int exigoID)
        {
            if (incoming == existing)
            {

                return true;
            }
            if (incoming != existing)
            {
                Settings.Logging.logger.LogData(Settings.Logging.AccountsUpdateLog.Name, new List<string>() { Guid, exigoID.ToString(), FieldUpdated, existing.ToString(), incoming.ToString() });

                _currentAccountIsModified = true;
                return false;
            }
            return true;

        }

        private void UpdateNullField(string Guid, string FieldUpdated, string incoming, int exigoID)
        {

            Settings.Logging.logger.LogData(Settings.Logging.AccountsUpdateLog.Name, new List<string>() { Guid, exigoID.ToString(), FieldUpdated, "null", incoming });
            _currentAccountIsModified = true;
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
        const string WebAlias = "nickname";
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
