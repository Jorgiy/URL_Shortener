using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using URLShortener.Interfaces;
using URLShortener.Models;
using URLShortener.Services;
using URLShortener.Tools;

namespace URLShortener.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
            // DI руками, не все хостинги поддерживают ASP.NET Core
            _linkOperator = new LinkOperator();
            _tokenOperations = new TokenOperations();
        }

        /// <summary>
        /// сервис по созданию ссылок
        /// </summary>
        private readonly ILinkOperator _linkOperator;
        /// <summary>
        /// сервис для определения связи "пользователь - ссылка"
        /// </summary>
        private readonly ITokenOperaions _tokenOperations;

        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Поптка создать ссылку
        /// </summary>
        /// <param name="url">исходная ссылка</param>
        /// <param name="time">оффсет текущего часового пояса</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CreateShortLink(string url, int time)
        {
            try
            {
                var creationResult = _linkOperator.CreateLink(url);
                if (!creationResult.Success)
                {
                    return new JsonResult
                    {
                        Data = new CreationLinkResultModel() { Success = false, ErrorMessage = creationResult.ErrorMessage }
                    };
                }

                var tokenResult = _tokenOperations.CreateToken(creationResult.LinkId,
                    DateTime.UtcNow.AddHours(time * -1), Request.Cookies["token"]?.Value);

                return new JsonResult
                {
                    Data =
                        new CreationLinkResultModel()
                        {
                            Success = true,
                            ErrorMessage = tokenResult.ErrorMessage,
                            ShortUrl = creationResult.ShortLink,
                            Token = tokenResult.Cookie,
                            TokenCreated = tokenResult.NewToken,
                            Url = url,
                            Host = $"{ Request?.Url?.Scheme }://{Request?.Url?.Authority}{Url.Content("~")}"
                        }
                };
            }
            catch (BuisenessException buisExc)
            {
                Logger.Log(buisExc.ErrorLevel, $"{buisExc.Message}. {buisExc.InnerException?.Message}");
                return new JsonResult
                {
                    Data = new CreationLinkResultModel() {Success = false, ErrorMessage = "Произошла ошибка"}
                };
            }
            catch (Exception exc)
            {
                Logger.Log(ErrorType.Critical, exc.Message);
                return new JsonResult
                {
                    Data = new CreationLinkResultModel() { Success = false, ErrorMessage = "Произошла ошибка" }
                };
            }
        }

        /// <summary>
        /// метод для перехода по укороченной ссылке
        /// </summary>
        /// <returns></returns>
        [Route("{url}")]
        public ActionResult RedirectToUrl()
        {
            var shorturl = Url.RequestContext.RouteData.Values["url"]?.ToString(); 

            var originalLink = _linkOperator.ReturnOriginalLink(shorturl);

            if (originalLink == null)
            {
                ViewBag.Page = shorturl;
                return View();
            }
            else
            {
                _linkOperator.IncrementFollows(shorturl);
                Response.Redirect(originalLink);
                return View();
            }
        }
    }
}