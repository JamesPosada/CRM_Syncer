using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            //ExigoGetCustomers customers = new ExigoGetCustomers();
            //var allAccounts = customers.GetAccountsGreaterThanModfiedOn(new DateTime(2014,06,10));
            //foreach(var t in allAccounts)
            //{
            //    Console.WriteLine(t.ExigoID + " " + t.MobilePhone + " " + t.StartDate);
            //}
            //Console.ReadLine();
            //CrmQueries crmq = new CrmQueries();
            //var result = crmq.SearchForContact("Ronnie", CrmQueries.SerachType.ByFirstNameOnly).ToList();



            //ExigoGetCustomers exigo = new ExigoGetCustomers();
            //var ex = exigo.GetSingleContact(2159039);
            //CrmQueries crm = new CrmQueries();
            //var cr = crm.SearchForContact("Worland", CrmQueries.SerachType.ByLastNameOnly).ToList().FirstOrDefault();
            //DataControls.ContactControl control = new DataControls.ContactControl();
            //var cr2 = crm.SearchForContact("ronnie.sallmen@gmail.com", CrmQueries.SerachType.ByEmailOnly).ToList().FirstOrDefault();
            //var ex2 = exigo.GetSingleContact(2169049);
            //control.CheckForUpdate(ex, cr);
            //control.CheckForUpdate(ex2, cr2);

            var crmq = new CrmQueries();
            var exigoq = new ExigoGetCustomers();
            var control = exigoq.GetAllAccountsWithoutCrmGUIDs();
            
            string y = "";
            foreach(var x in control)
            {
                var t = crmq.FREZZORContactsinCRM.Where(c => c.new_FREZZORID == x.ExigoID).FirstOrDefault();
                if (t == null)
                {
                    y = y + x.ExigoID + "," + x.FirstName + "," + x.LastName + "," + "Not in CRM" + "\r\n";
                }
                else
                {
                    y = y + x.ExigoID + "," + x.FirstName + "," + x.LastName + "," + t.FirstName + "," + t.LastName + "," + "\r\n";
                }
            }
            WriteLog log = new WriteLog(y);
            //var updatedContacts = control.UpdateAllContacts();
            
            

        }
    }
}
