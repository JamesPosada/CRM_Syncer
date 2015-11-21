using ExigoDesktop.Exigo.WebService;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataAccess
{
    public static class Utilities
    {
        public static Models.ExigoGenderType ConvertGender(string gender)
        {
            switch (gender)
            {
                case "Unknown":
                    return ExigoGenderType.Unknown;
                case "Male":
                    return ExigoGenderType.Male;
                case "Female":
                    return ExigoGenderType.Female;
                default:
                    return ExigoGenderType.Unknown;
            }
        }

        public static string ConvertCountryForCRM(string country)
        {
            switch (country)
            {
                case "US":
                    return "USA";
                case "AT":
                    return "Austria";
                case "DE":
                    return "Germany";
                case "FI":
                    return "Finland";
                case "EE":
                    return "Estonia";
                case "SE":
                    return "Sweden";
                case "BE":
                    return "Belgium";
                case "BG":
                    return "Bulgaria";
                case "CY":
                    return "Cyprus";
                case "CZ":
                    return "Czech Republic";
                case "DK":
                    return "Denmark";
                case "FR":
                    return "France";
                case "GR":
                    return "Greece";
                case "HU":
                    return "Hungary";
                case "IE":
                    return "Ireland";
                case "IT":
                    return "Italy";
                case "LV":
                    return "Latvia";
                case "LT":
                    return "Lithuania";
                case "LU":
                    return "Luxembourg";
                case "MT":
                    return "Malta";
                case "NL":
                    return "Netherlands";
                case "PL":
                    return "Poland";
                case "PT":
                    return "Portugal";
                case "RO":
                    return "Romania";
                case "SI":
                    return "Slovenia";
                case "ES":
                    return "Spain";
                case "GB":
                    return "United Kingdom";
                case "CH":
                    return "Switzerland";
                case "AU":
                    return "Australia";
                case "NZ":
                    return "New Zealand";
                case "CA":
                    return "Canada";
                case "JP":
                    return "Japan";
                case "AE":
                    return "United Arab Emirates";
                default:
                    return string.Empty;
            }
        }

        public static string GetWebAlias(int id, ExigoApi ApiContext)
        {

            try
            {                
                GetCustomerSiteResponse webAlias = ApiContext.GetCustomerSite(new GetCustomerSiteRequest() { CustomerID = id });
                return webAlias.WebAlias;
            }
            catch
            {
                return string.Empty;
            }

        }

        public static byte[] PKEncode(string encodeMe)
        {
            return Convert.FromBase64String(encodeMe);
        }

        public static FrezzorXCrm.XCrmContext GetCRMContext()
        {
            return XrmConnection.GetCrmContext();
        }

    }

    public static class ExtentionMethods
    {
        #region FormatPhoneForCRM



        /// <summary>
        /// Removes all non number Chars from string
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static string TakeDigits(this string numbers)
        {
            IEnumerable<char> chars = from ch in numbers
                                      where Char.IsDigit(ch)
                                      select ch;
            return new string(chars.ToArray());
        }

        /// <summary>
        /// /returns string in patter + country code ### #### #########
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        public static string FormatPhoneCRM(this string phone, string country)
        {
            
            return phone.TakeDigits().RemovePrefix(GetCountryPhoneCode(country)).ToPhonePattern().AddPrefix(GetCountryPhoneCode(country)).Substring(0);

        }
        /// <summary>
        /// Removes from begining of prefix from digits
        /// </summary>
        /// <param name="digits"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string RemovePrefix(this string digits, string prefix)
        {
            return (digits.Take(prefix.Length).SequenceEqual(prefix.Select(p => p))) ? new string(digits.Skip(prefix.Length).ToArray()) : digits;

        }

        /// <summary>
        /// Returns digits in pattern ### ### #######
        /// </summary>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static string ToPhonePattern(this string digits)
        {
            if (digits.Length < 4)
            {
                return digits;
            }
            if (digits.Length < 7)
            { return digits.Insert(3, " "); }
            return digits.Substring(0, 3) + " " + digits.Substring(3, 3) + " " + digits.Substring(6);


        }



        /// <summary>
        /// checks if prefix is begining sequence in digits if not returns digits
        /// with prefix
        /// </summary>
        /// <param name="digits"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string AddPrefix(this string digits, string prefix)
        {
            return (digits.Take(prefix.Length).SequenceEqual(prefix.Select(p => p))) ? digits : "+" + prefix + " " + digits;

        }

        /// <summary>
        /// Gets the phone prefix for the
        /// ciuntry that us
        /// </summary>
        /// <param name="DigitCountry"></param>
        /// <returns></returns>
        public static string GetCountryPhoneCode(this string DigitCountry)
        {
            switch (DigitCountry)
            {
                case "USA":
                    return "1";                
                case "Austria":
                    return "43";
                case "Germany":
                    return "49";
                case "Finland":
                    return "358";
                case "Estonia":
                    return "372";
                case "Sweden":
                    return "46";
                case "Belgium":
                    return "32";
                case "Bulgaria":
                    return "359";
                case "Cyprus":
                    return "357";
                case "Czech Republic":
                    return "420";
                case "Denmark":
                    return "45";
                case "France":
                    return "33";
                case "Greece":
                    return "30";
                case "Hungary":
                    return "36";
                case "Ireland":
                    return "353";
                case "Italy":
                    return "39";
                case "Latvia":
                    return "371";
                case "Lithuania":
                    return "370";
                case "Luxembourg":
                    return "352";
                case "Malta":
                    return "356";
                case "Netherlands":
                    return "31";
                case "Poland":
                    return "48";
                case "Portugal":
                    return "351";
                case "Romania":
                    return "40";
                case "Slovenia":
                    return "386";
                case "Spain":
                    return "34";
                case "United Kingdom":
                    return "44";
                case "Switzerland":
                    return "41";
                case "Australia":
                    return "61";
                case"New Zealand":
                    return "64";
                case "Canada":
                    return "1";
                case "United Arab Emirates":
                    return "971";
                case "Japan":
                    return "81";
                default:
                    return string.Empty;
            }

        }
        #endregion

    }
}
