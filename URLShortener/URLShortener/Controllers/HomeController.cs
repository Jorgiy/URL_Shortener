using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using URLShortener.Interfaces;
using URLShortener.Models;
using URLShortener.Services;

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

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Метод для загрузки страницы после попытки создания ссылки
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(CreationLinkResultModel model)
        {
            if (model.TokenSuccess)
            {
                var cookies = new HttpCookie("TokenCookie") {["token"] = model.Token, Expires = DateTime.Now.AddYears(1)};
                Response.Cookies.Add(cookies);
            }

            return View(model);
        }

        /// <summary>
        /// Поптка создать ссылку
        /// </summary>
        /// <param name="url">исходная ссылка</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CreateShortLink(string url)
        {
            
        }

        /// <summary>
        /// метод для перехода по укороченной ссылке
        /// </summary>
        /// <param name="shorturl"></param>
        /// <returns></returns>
        public RedirectResult RedirectToUrl(string shorturl)
        {
            
        }
    }
}