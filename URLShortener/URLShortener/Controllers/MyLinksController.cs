using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AspNet.Mvc.Grid;
using AspNet.Mvc.Grid.Pagination;
using AspNet.Mvc.Grid.Sorting;
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
        /// <param name="page">номер страницы</param>
        /// <param name="Column">колонка для сортировки</param>
        /// <param name="Direction">направление сортировки</param>
        /// <returns></returns>
        public ActionResult Show(int page = 1,
            UserDataDisplay.SortCpoumnTypes Column = UserDataDisplay.SortCpoumnTypes.CreationDate,
            SortDirection Direction = SortDirection.Ascending)
        {
            try
            {
                var result = _userDataDisplay.GetUserPaginatedLinks(
                    Request.Cookies["token"]?.Value,
                    10, Direction, (int) Column, page);

                var sortOptions = new GridSortOptions
                {
                    Direction = Direction,
                    Column = Column.ToString()
                };

                ViewData["sort"] = sortOptions;

                return View(result.AsPagination(page, 10));
            }
            catch (BuisenessException buisExc)
            {
                Logger.Log(buisExc.ErrorLevel, $"{buisExc.Message}. {buisExc.InnerException?.Message}", DateTime.Now);
                ViewBag.ErrorMessage = "При загрузке \"Моих ссылок\" произошла ошибка";
            }
            catch (Exception exc)
            {
                Logger.Log(ErrorType.Critical, exc.Message, DateTime.Now);
                ViewBag.ErrorMessage = "При загрузке \"Моих ссылок\" произошла ошибка";
            }

            return View();
        }
    }
}