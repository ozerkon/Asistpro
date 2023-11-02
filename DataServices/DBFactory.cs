using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServices
{
    public static class DbFactory
    {
        public static DbBase CreateDbBase(byte dbType) // 0: litedb, 1: mysql
        {
            if (dbType == 0)
            {
                return new DbBaseLiteDb();
            }
            else if (dbType == 1)
            {
                return new DbBaseMySql();
            }
            return null;
        }

        public static ICompanyDataService CreateCompanyDataService(byte dbType)
        {
            if (dbType == 0)
            {
                return new CompanyDataServiceLdb();
            }
            else if (dbType == 1)
            {
                return new CompanyDataServiceMySql();
            }
            return null;
        }
        public static ILinksDataService CreateLinksDataService(byte dbType)
        {
            if (dbType == 0)
            {
                return new LinksDataServiceLdb();
            }
            else if (dbType == 1)
            {
                return new LinksDataServiceMySql();
            }
        return null;
        }
        public static IPkcDataService CreatePkcDataService(byte dbType)
        {
            if (dbType == 0)
            {
                return new PkcDataServiceLdb();
            }
            else if (dbType == 1)
            {
                return new PkcDataServiceMySql();
            }
            return null;
        }
        public static ISgkDataService CreateSgkDataService(byte dbType)
        {
            if (dbType == 0)
            {
                return new SgkDataServiceLdb();
            }
            else if (dbType == 1)
            {
                return new SgkDataServiceMySql();
            }
            return null;
        }
        public static IUserDataService CreateUserDataService(byte dbType)
        {
            if (dbType == 0)
            {
                return new UserDataServiceLdb();
            }
            else if (dbType == 1)
            {
                return new UserDataServiceMySql();
            }
            return null;
        }
        public static IPersonalDataService CreatePersonalDataService(byte dbType)
        {
            if (dbType == 0)
            {
                return new PersonalDataServiceLDB(); 
            }
            else if (dbType == 1)
            {
                return new PersonalDataServiceMySql();
            }
            return null;
        }
        public static ILeaveDataService CreateLeaveDataService(byte dbType)
        {
            if (dbType == 0)
            {
                return new LeaveDataServiceLdb();
            }
            else if (dbType == 1)
            {
                return new LeaveDataServiceMySql();
            }
            return null;
        }
        public static ILeavePeriodDataService CreateLeavePeriodDataService(byte dbType)
        {
            if (dbType == 0)
            {
                return new LeavePeriodDataServiceLdb();
            }
            else if (dbType == 1)
            {
                return new LeavePeriodDataServiceMySql();
            }
            return null;
        }
        public static IAccrualDataService CreateAccuralDataService(byte dbType)
        {
            if (dbType == 0)
            {
                return new AccrualDataServiceLdb();
            }
            else if (dbType == 1)
            {
                return new AccuralDataServiceMySql();
            }
            return null;
        }
    }
}
