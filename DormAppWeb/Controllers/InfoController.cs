using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DormApp.Entities;
using DormApp.Domain;
using DormAppWeb.Models;
using System.Diagnostics;
using DormAppWeb.Classes;

namespace DormAppWeb.Views
{
    public class InfoController : Controller
    {
        public int PageSize = 2;
        public int PageSizeTariffs = 2;

        public ViewResult Dorms(int page = 1)
        {
            DormListViewModel model = new DormListViewModel
            {
                Dormitories = GetDormitories()
                .OrderBy(x => x.name)
                .Skip((page - 1) * PageSize)
                .Take(PageSize).ToList(),

                PageInfo = new PageInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = GetDormitories().Count()
                }
            };

            return View("Dorms", model);
        }


        public ViewResult Tariffs(int page = 1, int? dormId = null)
        {
            if ((dormId == null) || (dormId == 0))
            {
                TariffListViewModel model = new TariffListViewModel
                {
                    Tariffs = GetTariffList()
                    .OrderBy(x => x.Title)
                    .ThenByDescending(x => x.DateStart)
                    .Skip((page - 1) * PageSizeTariffs)
                    .Take(PageSizeTariffs)
                    .ToList(),

                    PageInfo = new PageInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = PageSizeTariffs,
                        TotalItems = GetTariffList().Count()
                    },
                    CurrentTranslitDormName = "All"
                };
                return View("Tariffs", model);
            }
            else
            {
                TariffListViewModel model = new TariffListViewModel
                {
                    Tariffs = GetTariffList(dormId.Value)
                   .OrderBy(x => x.Title)
                   .ThenByDescending(x => x.DateStart)
                   .Skip((page - 1) * PageSizeTariffs)
                   .Take(PageSizeTariffs)
                   .ToList(),

                    PageInfo = new PageInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = PageSizeTariffs,
                        TotalItems = GetTariffList(dormId.Value).Count()
                    },
                    CurrentDormId = dormId.Value,
                    CurrentTranslitDormName = Transliteration.Front(DormInfo.GetDormName(dormId.Value))
                };
                ViewBag.DormName = ". " + DormInfo.GetDormName(dormId.Value); //GetDormName(dormId.Value);

                return View("Tariffs", model);
            }
        }


        private List<DormType> GetDormitories()
        {
            List<DormType> Dorms = new List<DormType>();
            using (var unitOfWork = new UnitOfWork(new Dormitory_Entities()))
            {
                Dorms = unitOfWork.Dormitories.GetAll().ToList();
                unitOfWork.Dispose();
            }
            return Dorms;
        }

        private List<Tariff> GetTariffList()
        {
            List<Tariff> list = new List<Tariff>();
            using (var unitOfWork = new UnitOfWork(new Dormitory_Entities()))
            {
                list = unitOfWork.GetAllTariffs().ToList();
                unitOfWork.Dispose();
            }
            return list;
        }

        private List<Tariff> GetTariffList(int dormId)
        {
            List<Tariff> list = new List<Tariff>();
            using (var unitOfWork = new UnitOfWork(new Dormitory_Entities()))
            {
                list = unitOfWork.GetTariffs(dormId).ToList();
                unitOfWork.Dispose();
            }
            return list;
        }

    }
}