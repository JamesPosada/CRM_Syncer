using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrezzorXCrm
{
    public static class Settings {
        public static class CrmCredentials {
            public static string UserName = @"j.posada";

            public static string Domain = "datacenter";
            public static string Server = @"https://frezzor.dataresolution.net";
            public static byte[] Password {
                get {
                    return Utilities.PKEncode(@"YWRhc29wc2VtYWo=");
                }
            }
            public static string DomainWithUserName {
                get { return string.Format(@"{0}\{1}", Domain, UserName); }
            }
        }

        #region Constants
        public const string Salutation = "salutation";
        public const string Contact = "contact";
        public const string FirstName = "firstname";
        public const string LastName = "lastname";
        public const string FrezzorID = "new_frezzorid";
        public const string FrezzorLastModDate = "new_exigolastmodifieddate";
        public const string ModifedOnCrmDate = "modifiedon";
        public const string Email = "emailaddress1";
        public const string Email3 = "emailaddress2";
        public const string HomePhone = "telephone2";
        public const string BusinessPhone = "telephone1";
        public const string MobilePhone = "mobilephone";
        public const string DayOfBirth = "new_birthdayofmonth";
        public const string MonthOfBirth = "new_birthmonth";
        public const string YearOfBirth = "new_birthyear";
        public const string Rank = "new_rank";
        public const string Language = "new_frezzor_language";
        public const string PhoneSync = "new_phonesync";
        public const string EnrollerId = "new_enrollerid";
        public const string EnrollerWebAlias = "new_enrollerwebalias";
        public const string SkypeID = "new_skypeid";
        public const string WebAlias = "nickname";
        public const string FrezzorCustomerType = "new_type";
        public const string FrezzorStatus = "new_status";
        public const string ContactGUID = "contactid";
        public const string Address1Line1 = "address1_line1";
        public const string Address1Line2 = "address1_line2";
        public const string Address1City = "address1_city";
        public const string Address1State = "address1_stateorprovince";
        public const string Address1ZipCode = "address1_postalcode";
        public const string Address1Country = "address1_country";
        public const string Address2Line1 = "address2_line1";
        public const string Address2Line2 = "address2_line2";
        public const string Address2City = "address2_city";
        public const string Address2State = "address2_stateorprovince";
        public const string Address2ZipCode = "address2_postalcode";
        public const string Address2Country = "address2_country";



        #endregion

    }
     
}
