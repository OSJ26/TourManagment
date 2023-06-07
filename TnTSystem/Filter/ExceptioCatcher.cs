using System;
using System.IO;
using System.Web.Http.Filters;

namespace TnTSystem.Filter
{
    public class ExceptioCatcher : ExceptionFilterAttribute
    {
        private readonly string path;
        public ExceptioCatcher() {
            path = @"F:\C# Advance\TnTSystem\TnTSystem\bin\Log";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var controllerName = actionExecutedContext.ActionContext.ControllerContext.Controller;
            var exception = actionExecutedContext.Exception.Message;
            var actionMethod = actionExecutedContext.Request.Method;

            var logMessage = $"DateTime : {DateTime.Now} {Environment.NewLine}ControllerName : {controllerName}" +
                $"{Environment.NewLine}ActionMethod : {actionMethod} {Environment.NewLine}Exception : {exception}";
                       
            string fileName = Path.Combine(path, $"{DateTime.Now :d/MM/yyy_Exception.bin}");
            
            using (StreamWriter objWriter = new StreamWriter(fileName, true))
            {
                objWriter.WriteLine("===============================================");
                objWriter.WriteLine("Exception Log");
                objWriter.WriteLine(logMessage);
                objWriter.WriteLine("===============================================");
            }
        }
    }
}