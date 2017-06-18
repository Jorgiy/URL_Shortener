﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            if (model.TokenCreated)
            {
                var cookies = new HttpCookie("URLShortenerTokenCookie")
                {
                    ["token"] = model.Token,
                    Expires = DateTime.Now.AddYears(1)
                };
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
                    Request.Cookies["URLShortenerTokenCookie"]?["token"]);
            }
            catch (BuisenessException buisExc)
            {
                Logger.LogAsync(buisExc.ErrorLevel, buisExc.Message, DateTime.Now);
                return new JsonResult
                {
                    Data = new CreationLinkResultModel() {Success = false, ErrorMessage = "Произошла ошибка"}
                };
            }
            catch (Exception exc)
            {
                Logger.LogAsync(ErrorType.Critical, exc.Message, DateTime.Now);
                return new JsonResult
                {
                    Data = new CreationLinkResultModel() { Success = false, ErrorMessage = "Произошла ошибка" }
                };
            }
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