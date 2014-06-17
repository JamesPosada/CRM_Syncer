using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExigoDesktop.Exigo.WebService;
using Models;


namespace DataAccess
{
    public class ExigoGetCustomers
    {
        #region Properties
        protected GetCustomersRequest ApiRequest { get; set; }
        protected GetCustomersResponse ApiResponse { get; set; }
        protected List<ExigoContact> ContactList { get; set; }
        protected ExigoApi ApiContext { get; set; }

        private string LastMethodUsed { get; set; }
        #endregion Properties


        public ExigoGetCustomers()
        {
            ContactList = new List<ExigoContact>();
            ApiContext = ExigoApiContext.CreateWebServiceContext();
            LastMethodUsed = "NA";
        }




        #region ApiRequests Properties

        private void SingleAccount(int id)
        {
            ApiRequest = new GetCustomersRequest()
            {

                CustomerStatuses = new int[] { 1 },
                CustomerID = id
            };
            ApiResponse = new GetCustomersResponse();
        }

        private void AccountsFromLast2Days()
        {

            ApiRequest = new GetCustomersRequest()
            {
                BatchSize = 500,
                CreatedDateStart = DateTime.Now.Subtract(TimeSpan.FromDays(2)),
                CustomerStatuses = new int[] { 1 }
            };
            ApiResponse = new GetCustomersResponse();
        }

        private void AllAccounts()
        {
            ApiRequest = new GetCustomersRequest()
            {
                BatchSize = 50000,
                CustomerStatuses = new int[] { 1 },
                GreaterThanCustomerID = 5
            };
            ApiResponse = new GetCustomersResponse();
        }

        private void AccountsGreaterThanID(int id)
        {
            ApiRequest = new GetCustomersRequest()
            {
                BatchSize = 50000,
                CustomerStatuses = new int[] { 1 },
                GreaterThanCustomerID = id
            };
            ApiResponse = new GetCustomersResponse();
        }

        private void AccountsGreaterThanModfiedDate(DateTime modifiedDate)
        {
            ApiRequest = new GetCustomersRequest()
            {
                BatchSize = 50000,
                CustomerStatuses = new int[] { 1 },
                GreaterThanModifiedDate = modifiedDate
            };
            ApiResponse = new GetCustomersResponse();
        }
        private void AccountsCreatedAfter(DateTime createdDate)
        {
            ApiRequest = new GetCustomersRequest()
            {
                BatchSize = 50000,
                CustomerStatuses = new int[] { 1 },
                GreaterThanCustomerID = 5
            };
            ApiResponse = new GetCustomersResponse();
        }

        #endregion ApiRequests Properties


        #region Private Methods
        private void SendApiRequest()
        {
            ApiResponse = ApiContext.GetCustomers(ApiRequest);
        }
        private void ConvertResponseToModel()
        {
            if (ApiResponse.RecordCount > 0)
            {

                foreach (var resp in ApiResponse.Customers)
                {
                    ExigoContact exigoContact = new ExigoContact()
                    {
                        ExigoID = resp.CustomerID,
                        Gender = (ExigoGenderType)resp.Gender,
                        FirstName = resp.FirstName,
                        LastName = resp.LastName,
                        LastModified = resp.ModifiedDate,
                        StartDate = resp.CreatedDate,
                        Birthdate = resp.BirthDate,
                        CrmGuid = resp.Field8,
                        CustomerType = (ExigoCustomerType)resp.CustomerType,
                        Status = (ExigoStatusTypes)resp.CustomerStatus,
                        Email = resp.Email,
                        LangID = resp.LanguageID,
                        Rank = resp.RankID,
                        StreetAddress = resp.MainAddress1,
                        StreetAddress_Line2 = resp.MainAddress2,
                        City = resp.MainCity,
                        State = resp.MainState,
                        Zip = resp.MainZip,
                        Country = Utilities.ConvertCountryForCRM(resp.MainCountry),
                        EnrollerID = resp.EnrollerID,

                    };
                    //if (exigoContact.CustomerType == ExigoCustomerType.Independant)
                    //{
                    //    exigoContact.WebAlias = Utilities.GetWebAlias(resp.CustomerID, ApiContext);
                    //}
                    //else
                    //{
                    //    exigoContact.EnrollerWebAlias = Utilities.GetWebAlias(resp.EnrollerID, ApiContext);
                    //}
                    exigoContact.MobilePhone = (string.IsNullOrWhiteSpace(resp.MobilePhone)) ? string.Empty : resp.MobilePhone.FormatPhoneCRM(exigoContact.Country);
                    exigoContact.BusinessPhone = (string.IsNullOrWhiteSpace(resp.Phone)) ? string.Empty : resp.Phone.FormatPhoneCRM(exigoContact.Country);
                    exigoContact.HomePhone = (string.IsNullOrWhiteSpace(resp.Phone2)) ? string.Empty : resp.Phone2.FormatPhoneCRM(exigoContact.Country);
                    ContactList.Add(exigoContact);
                }
                GetWebAlias();
            }
        }


        private void GetWebAlias()
        {
            foreach (var exigoContact in ContactList)
            {
                if (exigoContact.CustomerType == ExigoCustomerType.Independant)
                {
                    exigoContact.WebAlias = Utilities.GetWebAlias(exigoContact.ExigoID, ApiContext);
                }
                else
                {
                    exigoContact.EnrollerWebAlias = Utilities.GetWebAlias(exigoContact.EnrollerID, ApiContext);
                }

            }
        }
        #endregion Private Methods


        #region Public Methods
        /// <summary>
        /// Gets up to 500 accounts that have been entered within the last 2 days
        /// </summary>
        /// <returns>A list of ExigoContacts</returns>
        public List<ExigoContact> GetAccountsFromLast2Days()
        {
            if (LastMethodUsed == "Last2Days")
            {
                return ContactList;
            }
            ContactList.Clear();
            AccountsFromLast2Days();
            SendApiRequest();
            ConvertResponseToModel();
            if (ApiResponse.Result.Status == ResultStatus.Success)
            {
                LastMethodUsed = "Last2Days";
            }
            return ContactList;
        }

        /// <summary>
        /// Gets all Accounts (max of 50000) from with ID's greater than 5
        /// </summary>
        /// <returns>A list of ExigoContacts</returns>
        public List<ExigoContact> GetAllAccounts()
        {
            if (LastMethodUsed == "AllAcounts")
            {
                return ContactList;
            }
            ContactList.Clear();
            AllAccounts();
            SendApiRequest();

            ConvertResponseToModel();
            if (ApiResponse.Result.Status == ResultStatus.Success)
            {
                LastMethodUsed = "AllAcounts";
            }
            return ContactList;
        }

        /// <summary>
        /// Gets all Accounts (max of 50000) with ID's greater than ID
        /// </summary>
        /// <param name="ID">Exigo ID number as int</param>
        /// <returns>A list of ExigoContacts</returns>
        public List<ExigoContact> GetAccountsGreaterThanID(int ID)
        {
            if (LastMethodUsed == "GreaterThanID")
            {
                return ContactList;
            }
            ContactList.Clear();
            AccountsGreaterThanID(ID);
            SendApiRequest();
            ConvertResponseToModel();
            if (ApiResponse.Result.Status == ResultStatus.Success)
            {
                LastMethodUsed = "GreaterThanID";
            }
            return ContactList;

        }


        /// <summary>
        /// Gets all Accounts (max of 50000) that have been modified after modifiedDate
        /// </summary>
        /// <param name="modifiedDate">a datetime</param>
        /// <returns>A list of ExigoContacts</returns>
        public List<ExigoContact> GetAccountsGreaterThanModfiedOn(DateTime modifiedDate)
        {
            if (LastMethodUsed == "GreaterThanModDate")
            {
                return ContactList;
            }
            ContactList.Clear();
            AccountsGreaterThanModfiedDate(modifiedDate);
            SendApiRequest();
            ConvertResponseToModel();
            if (ApiResponse.Result.Status == ResultStatus.Success)
            {
                LastMethodUsed = "GreaterThanModDate";
            }
            return ContactList;

        }





        /// <summary>
        /// Returns all accounts in exigo without CRM GUID's
        /// </summary>
        /// <returns> List of Exigo Contacts</returns>
        public List<ExigoContact> GetAllAccountsWithoutCrmGUIDs()
        {
            if (LastMethodUsed != "AllAcounts")
            {
                GetAllAccounts();
            }
            return ContactList.Where(c => string.IsNullOrEmpty(c.CrmGuid)).ToList();

        }

        /// <summary>
        /// Returns a single 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ExigoContact GetSingleContact(int id)
        {
            if (LastMethodUsed == id.ToString())
            {
                return ContactList.FirstOrDefault();
            }
            ContactList.Clear();
            SingleAccount(id);
            SendApiRequest();
            ConvertResponseToModel();
            if (ApiResponse.Result.Status == ResultStatus.Success)
            {
                LastMethodUsed = id.ToString();
            }
            return ContactList.FirstOrDefault();
        }

        #endregion Public Methods
    }
}
