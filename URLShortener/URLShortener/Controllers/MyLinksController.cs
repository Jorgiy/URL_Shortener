using System;
using System.Web.Mvc;
using AspNet.Mvc.Grid;
using AspNet.Mvc.Grid.Pagination;
using AspNet.Mvc.Grid.Sorting;
using CommonTypes;
using CoreServices.Interfaces;
using CoreServices.Models;
using NLog;

namespace URLShortener.Controllers
{
    public class MyLinksController : Controller
    {
        private readonly IUserDataCoreService _userDataCoreService;

        private readonly Logger _logger;
        
        public MyLinksController(IUserDataCoreService userDataCoreService)
        {
            _userDataCoreService = userDataCoreService;
            _logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// User's links display method 
        /// </summary>
        /// <param name="page">page number</param>
        /// <param name="column">sort column number</param>
        /// <param name="direction">sort direction</param>
        /// <returns></returns>
        public ActionResult Show(int page = 1, SortColumnTypes column = SortColumnTypes.CreationDate    , SortDirection direction = SortDirection.Ascending)
        {
            try
            {
                var result =
                    _userDataCoreService.GetUserPaginatedLinks(Request.Cookies["token"]?.Value, direction,
                        (int) column);

                var sortOptions = new GridSortOptions
                {
                    Direction = direction,
                    Column = column.ToString()
                };

                ViewData["sort"] = sortOptions;

                return View(result.AsPagination(page, 10));
            }
            catch (BuisenessException buisExc)
            {
                _logger.Error($"{buisExc.Message}. {buisExc.InnerException?.Message}");
                ViewBag.ErrorMessage = "Error occured while \"My links\" loading";
            }
            catch (Exception exc)
            {
                _logger.Fatal(exc.Message);
                ViewBag.ErrorMessage = "Error occured while \"My links\" loading";
            }

            return View();
        }
    }
}