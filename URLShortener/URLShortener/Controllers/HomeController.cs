using System;
using System.Web.Mvc;
using CoreServices.Interfaces;
using CoreServices.Models;
using NLog;
using URLShortener.Models;

namespace URLShortener.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly ILinkCoreService _linkCoreService;
       
        private readonly ITokenCoreService _tokenCoreService;

        private readonly Logger _logger;
        
        public HomeController(ILinkCoreService linkCoreService, ITokenCoreService tokenCoreService)
        {
            _linkCoreService = linkCoreService;
            _tokenCoreService = tokenCoreService;
            _logger = LogManager.GetCurrentClassLogger();
        }

        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Link creation attempt
        /// </summary>
        /// <param name="url">target url</param>
        /// <param name="timeOffset">timezone offset</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CreateShortLink(string url, int timeOffset)
        {
            try
            {
                var creationResult = _linkCoreService.CreateLink(url);
                if (!creationResult.Success)
                {
                    return new JsonResult
                    {
                        Data = new CreationLinkResultModel { Success = false, ErrorMessage = creationResult.ErrorMessage }
                    };
                }

                var tokenResult = _tokenCoreService.MapLinkToToken(creationResult.LinkId,
                    DateTime.UtcNow.AddHours(timeOffset * -1), Request.Cookies["token"]?.Value);

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
                _logger.Error($"{buisExc.Message}. {buisExc.InnerException?.Message}");
                return new JsonResult
                {
                    Data = new CreationLinkResultModel {Success = false, ErrorMessage = "Error occured"}
                };
            }
            catch (Exception exc)
            {
                _logger.Fatal(exc.Message);
                return new JsonResult
                {
                    Data = new CreationLinkResultModel() { Success = false, ErrorMessage = "Error occured" }
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

            var originalLink = _linkCoreService.ReturnOriginalLink(shorturl);

            if (originalLink == null)
            {
                ViewBag.Page = shorturl;
                return View();
            }
            else
            {
                _linkCoreService.IncrementFollows(shorturl);
                Response.Redirect(originalLink);
                return View();
            }
        }
    }
}