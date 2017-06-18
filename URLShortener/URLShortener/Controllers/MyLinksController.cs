using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using URLShortener.Interfaces;
using URLShortener.Services;
using URLShortener.Tools;

namespace URLShortener.Controllers
{
    public class MyLinksController : Controller
    {
        public MyLinksController()
        {
            _userDataDisplay = new UserDataDisplay();
        }

        /// <summary>
        /// сервис для отображения данных для пользователя
        /// </summary>
        private readonly IUserDataDisplay _userDataDisplay;

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
            try
            {
                return
                    View(_userDataDisplay.GetUserPaginatedLinks(Request.Cookies["URLShortenerTokenCookie"]?["token"],
                        pagesize, direction, (int)sortcolumn, pagenumber));
            }
            catch (BuisenessException buisExc)
            {
                Logger.LogAsync(buisExc.ErrorLevel, buisExc.Message, DateTime.Now);
                ViewBag.ErrorMessage = "При загрузке \"Моих ссылок\" произошла ошибка";
            }
            catch (Exception exc)
            {
                Logger.LogAsync(ErrorType.Critical, exc.Message, DateTime.Now);
                ViewBag.ErrorMessage = "При загрузке \"Моих ссылок\" произошла ошибка";
            }

            return View();
        }
    }
}