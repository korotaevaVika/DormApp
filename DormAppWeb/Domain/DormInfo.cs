using DormApp.Domain;
using DormApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DormAppWeb.Classes
{
    public static class DormInfo
    {
        public static string GetDormName(int dormId)
        {
            try
            {
                string str;
                using (var unitOfWork = new UnitOfWork(new Dormitory_Entities()))
                {
                    str = unitOfWork.Dormitories.Get(dormId).name;
                    unitOfWork.Dispose();
                }
                return str;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}