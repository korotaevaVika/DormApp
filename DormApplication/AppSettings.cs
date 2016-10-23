using DormApp.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DormApplication
{
    class AppSettings
    {
        private static string _dormName;
        private static string _admin;
        private static int _dormId;
        
        public static string Admin {
            get { return _admin; }
            set { if (_admin == null) { _admin = value; } }
        }

        public static string DormName
        {
            get { return _dormName; }
            set
            {
                if (_dormName == null)
                {
                    _dormName = value;
                    using (var unitOfWork = new UnitOfWork(new DormApp.Entities.Dormitory_Entities()))
                    {
                        _dormId = unitOfWork.Dormitories.GetDormIdByName(_dormName);
                        unitOfWork.Dispose();
                    }
                }
            }
        }

        public static int DormId { get { return _dormId; } } 
        
        //Добавить ссылку на System.Configuration и using 
        public static string GetConString() {
            return ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }
        
    }
}
