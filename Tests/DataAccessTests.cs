using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccess;
using System.Collections.Generic;
using System.Linq;
using Models;


namespace Tests
{
    [TestClass]
    public class DataAccessTests
    {
        [TestMethod]
        public void TestGetAllAccountTest()
        {
            ExigoGetCustomers customers = new ExigoGetCustomers();
            var allCustomers = customers.GetAllAccounts();
            Assert.IsTrue(allCustomers.Count() > 1000);           

        }

        [TestMethod]
        public void TestGetCrmContactsWithExigoIDs()
        {
            CrmQueries crm = new CrmQueries();
            Assert.IsNotNull(crm.FREZZORContactsinCRM);
            
        
        }
    }



}
