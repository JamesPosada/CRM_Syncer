using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XrmV2;


namespace DataAccess
{
    public class CrmQueries
    {
        #region Properties


        ColumnSet DefaultColumnSet
        {
            get
            {
                return new ColumnSet(
                    ContactGUID,
                    Salutation,
                    FirstName,
                    LastName,
                    FrezzorID,
                    FrezzorLastModDate,
                    ModifedOnCrmDate,
                    Email,
                    Email3,
                    HomePhone,
                    BusinessPhone,
                    MobilePhone,
                    DayOfBirth,
                    MonthOfBirth,
                    YearOfBirth,
                    Rank,
                    Language,
                    FrezzorCustomerType,
                    EnrollerId,
                    EnrollerWebAlias,
                    WebAlias,
                    FrezzorStatus,
                    Address1Line1,
                    Address1Line2,
                    Address1City,
                    Address1State,
                    Address1ZipCode,
                    Address1Country);
            }
        }
        #endregion Properties


        public IEnumerable<Contact> FREZZORContactsinCRM
        {
            get
            {
                if (_frezzorContactsinCRM == null)
                {
                    _frezzorContactsinCRM = GetAllExigoContacts();

                }
                return _frezzorContactsinCRM;
            }
        }
        private IEnumerable<Contact> _frezzorContactsinCRM;


        #region Private Methods
        /// <summary>
        /// Will get all Contacts from CRM that have FREZZOR ID Numbers in them. 
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Contact> GetAllExigoContacts()
        {
            QueryExpression AllExigoContacts = new QueryExpression(Contact) { ColumnSet = DefaultColumnSet };
            ConditionExpression condition = new ConditionExpression(FrezzorID, ConditionOperator.NotNull);
            AllExigoContacts.Criteria.AddCondition(condition);

           return XrmConnection.GetOrganizationService()
               .RetrieveMultiple(AllExigoContacts).Entities.Cast<Contact>();           
        }
        #endregion Private Methods


        #region Public Searches
        /// <summary>
        /// Searches CRM by one of the 3 fields specified by searchType param. If no
        /// seachType if specified will default to email. Will only return results that
        /// match excatly the search string.
        /// </summary>
        /// <param name="searchString">string to search for</param>
        /// <param name="searchType">field to search for string in</param>
        /// <returns>IEnumberable of Type XrmV2.Contact</returns>
        public IEnumerable<Contact> SearchForContact(string searchString, SerachType searchType = SerachType.ByEmailOnly)
        {
            QueryExpression SearchQuery = new QueryExpression(Contact) { ColumnSet = DefaultColumnSet };
            switch (searchType)
            {
                case SerachType.ByEmailOnly:
                    SearchQuery.Criteria.AddCondition(new ConditionExpression(Email, ConditionOperator.Equal, searchString));                    
                    break;
                case SerachType.ByFirstNameOnly:
                    SearchQuery.Criteria.AddCondition(new ConditionExpression(FirstName, ConditionOperator.Equal, searchString));
                    break;
                case SerachType.ByLastNameOnly:
                    SearchQuery.Criteria.AddCondition(new ConditionExpression(LastName, ConditionOperator.Equal, searchString));
                    break;
                default:                
                    SearchQuery.Criteria.AddCondition(new ConditionExpression(Email, ConditionOperator.Equal, searchString));
                    break;
            }                        

            return XrmConnection.GetOrganizationService()
                .RetrieveMultiple(SearchQuery).Entities.Cast<Contact>();
        }


        /// <summary>
        /// Searches CRM for contacts that match exactly First and Last Name
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns>IEnumberable of Type XrmV2.Contact</returns>
        //public IEnumerable<Contact> SearchForContact(string firstName, string lastName)
        //{
        //    QueryExpression SearchQuery = new QueryExpression(Contact) { ColumnSet = DefaultColumnSet };
        //    ConditionExpression condition = new ConditionExpression(LastName, ConditionOperator.Equal, lastName);
        //    ConditionExpression condition2 = new ConditionExpression(FirstName, ConditionOperator.Equal, firstName);
        //    SearchQuery.Criteria.AddCondition(condition);
        //    SearchQuery.Criteria.AddCondition(condition2);

        //    return XrmConnection.GetOrganizationService()
        //        .RetrieveMultiple(SearchQuery).Entities.Cast<Contact>();
        //}


        public Contact SearchForContact(Guid id)
        {
            return XrmConnection.GetOrganizationService()
                .Retrieve(Contact, id, DefaultColumnSet).ToEntity<Contact>();
        }

        public IEnumerable<Contact> SearchForContact(Guid id, string firstName, string lastName)
        {
            QueryExpression SearchQuery = new QueryExpression(Contact) { ColumnSet = DefaultColumnSet };
            ConditionExpression condition = new ConditionExpression(LastName, ConditionOperator.Equal, lastName);
            ConditionExpression condition2 = new ConditionExpression(FirstName, ConditionOperator.Equal, firstName);
            ConditionExpression condition3 = new ConditionExpression(ContactGUID, ConditionOperator.Equal, id);
            SearchQuery.Criteria.AddCondition(condition);
            SearchQuery.Criteria.AddCondition(condition2);
            SearchQuery.Criteria.AddCondition(condition3);

            return XrmConnection.GetOrganizationService()
                .RetrieveMultiple(SearchQuery).Entities.Cast<Contact>();

            

        }


        /// <summary>
        /// Searches CRM for contacts that match exactly First Name, Last Name and Email
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <returns>IEnumberable of Type XrmV2.Contact</returns>
        public IEnumerable<Contact> SearchForContact(string firstName, string lastName, string email)
        {
            QueryExpression SearchQuery = new QueryExpression(Contact) { ColumnSet = DefaultColumnSet };
            ConditionExpression condition = new ConditionExpression(LastName, ConditionOperator.Equal, lastName);
            ConditionExpression condition2 = new ConditionExpression(FirstName, ConditionOperator.Equal, firstName);
            ConditionExpression condition3 = new ConditionExpression(Email, ConditionOperator.Equal, email);
            

            SearchQuery.Criteria.AddCondition(condition);
            SearchQuery.Criteria.AddCondition(condition2);
            SearchQuery.Criteria.AddCondition(condition3);

            return XrmConnection.GetOrganizationService()
                .RetrieveMultiple(SearchQuery).Entities.Cast<Contact>();
        }

        public DateTime GetLastExigoModifedDate()
        {
            QueryExpression SearchQuery = new QueryExpression(Contact) { ColumnSet = DefaultColumnSet };
            OrderExpression order = new OrderExpression(FrezzorLastModDate, OrderType.Descending);
            ConditionExpression condition = new ConditionExpression(FrezzorID, ConditionOperator.NotNull);
            SearchQuery.Criteria.AddCondition(condition);
            SearchQuery.Orders.Add(order);
            SearchQuery.TopCount = 1;

            var d = XrmConnection.GetOrganizationService()
                .RetrieveMultiple(SearchQuery).Entities.Cast<Contact>().FirstOrDefault().new_ExigoLastModifiedDate;
            return (DateTime)d;

        }

        public int GetLastIDNumber()
        {
            QueryExpression qExpr = new QueryExpression(Contact) { ColumnSet = new ColumnSet(FrezzorID) };
            OrderExpression orderExpr = new OrderExpression(FrezzorID, OrderType.Descending);
            ConditionExpression condition = new ConditionExpression(FrezzorID, ConditionOperator.NotNull);
           // ConditionExpression condition2 = new ConditionExpression(EnrollerId, ConditionOperator.NotNull);
            qExpr.Criteria.AddCondition(condition);
            //qExpr.Criteria.AddCondition(condition2);
            qExpr.Orders.Add(orderExpr);
            qExpr.TopCount = 1;
            return (int)XrmConnection.GetOrganizationService()
                .RetrieveMultiple(qExpr).Entities.Cast<Contact>().FirstOrDefault().new_FREZZORID;
        }


        #endregion Public Searches






        public enum SerachType
        {
            ByFirstNameOnly =1,
            ByLastNameOnly =2,
            ByEmailOnly =3
        }


        #region Constants
        const string Salutation = "salutation";
        const string Contact = "contact";
        const string FirstName = "firstname";
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