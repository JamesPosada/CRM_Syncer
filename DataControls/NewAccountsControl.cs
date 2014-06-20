using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Models;

namespace DataControls
{
    class NewAccountsControl
    {
        #region Properties
        protected List<ExigoContact> NewExigoAccounts 
        {
            get
            {
                if (_newExgioAccounts == null)
                {
                    _newExgioAccounts = new List<ExigoContact>();
                }
                return _newExgioAccounts;
            }
        }
        private List<ExigoContact> _newExgioAccounts;

        protected ExigoGetCustomers ExigoCustomersApi
        {
            get
            {
                if (_exigoCustomers == null)
                {
                    _exigoCustomers = new ExigoGetCustomers();
                }
                return _exigoCustomers;
            }
        }
        private ExigoGetCustomers _exigoCustomers;
        #endregion

        #region Constructor
        NewAccountsControl()
        { }

        NewAccountsControl(ExigoGetCustomers exigoGetCustomers)
        {
            _exigoCustomers = exigoGetCustomers;
        }
        
        #endregion
    }
}
