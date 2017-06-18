using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using URLShortener.Services;

namespace URLShortener.Controllers
{
    public class MyLinksController : Controller
    {
        /// <summary>
        /// Метод для отображения ссылок пользователя
        /// </summary>
        /// <param name="pagesize">размер страницы</param>
        /// <param name="pagenumber">номер страницы</param>
        /// <param name="sortcolumn">колонка для сортировки</param>
        /// <param name="direction">направление сортировки</param>
        /// <returns></returns>
        public ActionResult Show(int pagesize = 10, int pagenumber = 1,
            UserDataDisplay.SortCpoumnTypes sortcolumn = UserDataDisplay.SortCpoumnTypes.CreatioDate,
            SortDirection direction = SortDirection.Ascending)
        {
            return View();
        }
    }
}