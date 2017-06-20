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
        public ActionResult Show(int pagesize = 2, int page = 1,
            UserDataDisplay.SortCpoumnTypes Column = UserDataDisplay.SortCpoumnTypes.CreatioDate,
            SortDirection Direction = SortDirection.Ascending)
        {
            try
            {
                ViewBag.pagesize = pagesize;
                ViewBag.pagenumber = page;
                ViewBag.sortcolumn = Column;
                ViewBag.direction = Direction;

                var result = _userDataDisplay.GetUserPaginatedLinks(
                    Request.Cookies["URLShortenerTokenCookie"]?["token"],
                    pagesize, Direction, (int) Column, page);

                //TODO : DELETE!!
                var displayedLinks = result as IList<IDisplayedLink> ?? result.ToList();
                displayedLinks.Add(new DisplayedLink() { OriginalLink = "http://vk.com", ShortedLink = "fgi7ua", CreationDate = DateTime.Now, Follows = 4 });
                displayedLinks.Add(new DisplayedLink() { OriginalLink = "http://ya.ru", ShortedLink = "fhi6ip", CreationDate = DateTime.Now, Follows = 100 });
                displayedLinks.Add(new DisplayedLink() { OriginalLink = "http://vk.com", ShortedLink = "fgi7ua", CreationDate = DateTime.Now, Follows = 4 });
                displayedLinks.Add(new DisplayedLink() { OriginalLink = "http://ya.ru", ShortedLink = "fhi6ip", CreationDate = DateTime.Now, Follows = 100 });
                displayedLinks.Add(new DisplayedLink() { OriginalLink = "http://vk.com", ShortedLink = "fgi7ua", CreationDate = DateTime.Now, Follows = 4 });
                displayedLinks.Add(new DisplayedLink() { OriginalLink = "http://ya.ru", ShortedLink = "fhi6ip", CreationDate = DateTime.Now, Follows = 100 });

                var sortOptions = new GridSortOptions
                {
                    Direction = Direction,
                    Column = Column.ToString()
                };

                ViewData["sort"] = sortOptions;

                return View(result.AsPagination(page, pagesize));
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