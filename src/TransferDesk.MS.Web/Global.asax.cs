using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using TransferDesk.Services;
using TransferDesk.Services.Manuscript;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Integration.WebApi;
using TransferDesk.Contracts.Logging;
using TransferDesk.DAL.Manuscript.Repositories;
using TransferDesk.Logger;

namespace TransferDesk.MS.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private SimpleInjector.Container _simpleInjectorcontainer = null;
        private IApplicationLog _applicationLog = null;

        protected void Application_Start()
        {
            //test
            //List<string> listOfString = new List<string>();
            StringBuilder stringBuilder = null;
            IFileLogger _fileLogger = null;
            try
            {
                stringBuilder = new StringBuilder();

                AreaRegistration.RegisterAllAreas();
                GlobalConfiguration.Configure(WebApiConfig.Register);
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                BundleConfig.RegisterBundles(BundleTable.Bundles);
                //compositon root for object graph using the IoC Container
                _simpleInjectorcontainer = new SimpleInjector.Container();

                //Register all components
                RegisterAllComponents(stringBuilder, _simpleInjectorcontainer);

                var logger = _simpleInjectorcontainer.GetInstance<ILogger>();

                _applicationLog  = logger as IApplicationLog;
                _fileLogger = logger as IFileLogger;

                //fileLogger.FilePath = "d:\\TransferdeskLog\\";
                string iterationInfo = "Transferdesk";//todo:setto config

                _fileLogger.FilePath = System.Web.HttpRuntime.AppDomainAppPath + iterationInfo + "Log\\";

                if (System.IO.Directory.Exists(_fileLogger.FilePath) == false)
                {
                    System.IO.Directory.CreateDirectory(_fileLogger.FilePath);
                }

                _fileLogger.FileName = "TransferDeskLog";
                
                stringBuilder.AppendLine("Try Register the container as  IDependencyResolver.");

                _fileLogger.WriteStringBuilderToAppLogAndClear(stringBuilder);
                
                try
                {
                    // Register the container as  IDependencyResolver.
                    DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(_simpleInjectorcontainer));

                    GlobalConfiguration.Configuration.DependencyResolver =
                                               new SimpleInjectorWebApiDependencyResolver(_simpleInjectorcontainer);
                }
                catch (Exception exception)
                {
                    _fileLogger.LogException(exception, stringBuilder);
                }
                //throw new Exception("test app start exception");
            }
            catch (Exception exception)
            {
                if (_applicationLog == null)
                {
                    string pendingLogWrites = string.Empty;
                    if (stringBuilder != null)
                    {
                        pendingLogWrites = stringBuilder.ToString();
                    }
                    Trace.WriteLine("Transferdesk exception " + pendingLogWrites + exception.ToString());
                }
                else
                {
                    _applicationLog.ApplicationExceptionLog(exception,stringBuilder);
                }

            }

        }

        private void RegisterAllComponents(StringBuilder stringBuilder, Container simpleInjectorContainer)
        {
            try
            {
                // Set the scoped lifestyle one directly after creating the container//web request works per request
                simpleInjectorContainer.Options.DefaultScopedLifestyle = new WebRequestLifestyle(true);

                var conString = Convert.ToString(ConfigurationManager.AppSettings["dbTransferDeskService"]);

                //below is a static class registered as singleton to manage mutiple thread instance sync locked.
                simpleInjectorContainer.RegisterSingleton<ILogger, LogHelper>();

                stringBuilder.AppendLine("Application started on computer " + System.Environment.MachineName + " DomainName " + System.Environment.UserDomainName);

                stringBuilder.AppendLine("Try Register all objects");

                //////simpleInjectorContainer.Register<ManuscriptService>(() => new ManuscriptService(conString, conString), Lifestyle.Scoped);

                //////stringBuilder.AppendLine("Registered Manuscript Service");

                //////simpleInjectorContainer.Register<ManuscriptDBRepositoryReadSide>(() => new ManuscriptDBRepositoryReadSide(conString), Lifestyle.Scoped);

                //////stringBuilder.AppendLine("Registered Manuscript DB repository for readside");

                simpleInjectorContainer.RegisterWebApiControllers(GlobalConfiguration.Configuration);
                simpleInjectorContainer.RegisterMvcControllers(Assembly.GetExecutingAssembly());


                stringBuilder.AppendLine("Registered MVC Controllers");

                stringBuilder.AppendLine("DI container try verify");

                //verify the container's configuration success for given dependencies.
                simpleInjectorContainer.Verify();

                stringBuilder.AppendLine("DI container verify done");



            }
            catch (Exception exception)
            {
                //register is called before logger instance so write to string builder if exception in registering
                stringBuilder.AppendLine(exception.ToString());
            }
        }

        protected void Application_End(object sender, EventArgs e)
        {
            _applicationLog.ApplicationLog("Application stopped");
            _simpleInjectorcontainer.Dispose();
        }
        protected void Application_EndRequest()
        {
            HttpRuntime runtime = (HttpRuntime)typeof(HttpRuntime).InvokeMember("_theRuntime", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, null, null);

            if (runtime != null)
            {
                BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField;
                string shutDownMessage = (string)runtime.GetType().InvokeMember("_shutDownMessage",
                    bindingFlags,
                    null,
                    runtime,
                    null);
                string shutDownStack = (string)runtime.GetType().InvokeMember("_shutDownStack",
                    bindingFlags,
                    null,
                    runtime,
                    null);

                if (!String.IsNullOrEmpty(shutDownMessage))
                {
                    if (_applicationLog != null)
                    {
                        _applicationLog.ApplicationLog(String.Format("_shutDownMessage={0}\r\n\r\n_shutDownStack={1}", shutDownMessage,shutDownStack));
                    }
                }
            }
        }
    }
}

