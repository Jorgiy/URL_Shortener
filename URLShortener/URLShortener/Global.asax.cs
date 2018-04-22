using System;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CoreServices.Implementations;
using CoreServices.Interfaces;
using DataLayer.DataContext;
using DataLayer.Interfaces;
using DataLayer.Repositories;
using NLog;
using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Lifestyles;

namespace URLShortener
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RegisterDependencies();
            RegisterLogger();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var logger = LogManager.GetCurrentClassLogger();
            var exc = Server.GetLastError();
            Response.Write("Whoops, it seems that something is wrong :(\n");
            logger.Fatal($"Global Error: {exc}");
            Server.ClearError();
        }

        private void RegisterLogger()
        {
            var config = new NLog.Config.LoggingConfiguration();

            var logfile = new NLog.Targets.FileTarget() { FileName = "logfile.txt", Name = "logfile" };
            var logconsole = new NLog.Targets.ConsoleTarget() { Name = "logconsole" };

            config.LoggingRules.Add(new NLog.Config.LoggingRule("*", LogLevel.Info, logconsole));
            config.LoggingRules.Add(new NLog.Config.LoggingRule("*", LogLevel.Debug, logfile));

            NLog.LogManager.Configuration = config;
        }

        private void RegisterDependencies()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            
            container.Register<ILinkCoreService, LinkCoreService>();
            container.Register<ITokenCoreService, TokenCoreService>();
            container.Register<IUserDataCoreService, UserDataCoreService>();
            container.Register<ILinksRepository, LinksRepository>();
            container.Register<ITokenRepository, TokensRepository>();
            container.Register<ITokenMappingRepository, TokenMappingRepository>();
            container.Register(() => new UrlShortenerContext("name=UrlShortenerBaseEntities"), Lifestyle.Singleton);
            
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
    }
}
