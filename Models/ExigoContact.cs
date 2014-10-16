using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ExigoContact
    {
        public int ExigoID { get; set; }
        public ExigoGenderType Gender { get; set; }
        public int Rank
        {
            get
            {
                return _rank;
            }
            set
            {
                _rank = ExigoContact.Remappings.RankRemap(value);
            }
        }
        private int _rank;
        public ExigoStatusTypes Status { get; set; }
        public string CrmGuid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string WebAlias { get; set; }
        public int EnrollerID { get; set; }
        public string EnrollerWebAlias { get; set; }
        public string StreetAddress { get; set; }
        public string StreetAddress_Line2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string MobilePhone { get; set; }
        public string BusinessPhone { get; set; }
        public string HomePhone { get; set; }
        public int LangID { get; set; }
        public ExigoCustomerType CustomerType { get; set; }
        public string Salutation
        {
            get
            {

                if (this.Gender == ExigoGenderType.Male)
                {
                    return "Mr.";
                }
                else if (this.Gender == ExigoGenderType.Female)
                {
                    return "Ms.";
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public DateTime StartDate { get; set; }
        public DateTime LastModified { get; set; }
        public DateTime Birthdate { get; set; }

        public int GetBirthDay()
        {
            return this.Birthdate.Day;
        }

        public int GetBirthMonth()
        {
            return this.Birthdate.Month;
        }

        public string GetBirthYear()
        {
            return this.Birthdate.Year.ToString();
        }

        public Guid GetGUID()
        {
            return new Guid(CrmGuid);
        }
        /// <summary>
        ///  This class holds static remapping utils
        /// </summary>
        internal static class Remappings
        {
           /// <summary>
           ///  Ranks in Exigo used to have an Emerald as 6, we changed to Diamond.
           ///  CRM New_Rank definition now goes from Platnium == 5 and Diamond == 7
           ///  six has been removed, CRM will throw an exception if 6 is attempted 
           ///  to be inserted.
           /// </summary>
           /// <param name="ExigoRank"></param>
           /// <returns></returns>
            internal static int RankRemap(int? ExigoRank = 1)
            {

               ExigoRank = (ExigoRank != null) ? ExigoRank : 1;
                if (ExigoRank == 6)
                {
                    return 7;
                }
                return (int)ExigoRank;
            }

        }

    }

    

}
