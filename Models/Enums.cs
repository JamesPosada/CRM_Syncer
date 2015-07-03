using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public enum ExigoGenderType
    {
        Unknown = 1,
        Male = 2,
        Female = 3
    }

    public enum ExigoStatusTypes
    {
        Deleted = 0,
        Active = 1,
        Terminated = 2,
        Suspended = 3,
        DoNotUse = 4,
        Cancelled = 5,
        Pending = 6
    }

    public enum ExigoCustomerType
    {
        Retail = 1,
        Preferred = 2,
        Independant = 3,
        BusinessBuilder = 4
    }
    public enum CrmCustomerType 
    {
        Prospect = 10

    }
}
