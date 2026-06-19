using System.Collections.Specialized;
using System.Net;
using System.Reflection;
using System.Text;
using NLog;

namespace InPostAPIEmulator;

public static class Server
{
    private static HttpListener _listener = new HttpListener();
    private static readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();
    private static Type[] _requestHandlers;
    public static async Task RunServer(string host = "localhost", ushort port = 8000)
    {
        bool runServer = true;
        
        Type[] types = Assembly.GetExecutingAssembly().GetTypes();
        _requestHandlers = types.Where(t => t.GetCustomAttribute<RequestHandlerAttribute>() != null).ToArray();

        foreach (var type in _requestHandlers)
        {
            Logger.Debug($"found a handler class: {type.Name}");
        }
        
        _listener.Prefixes.Add($"http://{host}:{port}/");
        _listener.Start();
        Logger.Info($"listening on http://{host}:{port}/");

        while (runServer)
        {
            HttpListenerContext ctx = await _listener.GetContextAsync();

            HttpListenerRequest req = ctx.Request;
            HttpListenerResponse resp = ctx.Response;

            Logger.Info($"{req.HttpMethod} {req.Url.AbsolutePath}");
            Logger.Trace("Headers:");

            foreach (string key in req.Headers.AllKeys)
            {
                string[] values = req.Headers.GetValues(key);
                if (values != null)
                {
                    foreach (string value in values)
                    {
                        Logger.Trace($" {key}: {value}");
                    }
                }
            }

            Logger.Trace("data:");

            using (var reader = new StreamReader(req.InputStream, req.ContentEncoding))
            {
                string requestBody = reader.ReadToEnd();
                Logger.Trace(requestBody);
            }

            if ((req.HttpMethod == "POST") && (req.Url.AbsolutePath == "/shutdown"))
            {
                Logger.Info("shutdown requested");
                runServer = false;
            }

            if (req.HttpMethod == "POST")
            {
                foreach (Type controller in _requestHandlers)
                {
                    MethodInfo[] methods = controller.GetMethods();
                    
                    foreach (var method in methods)
                    {
                        Logger.Trace($"trying {controller.Name}.{method.Name}");
                        var attr = (PostRequestAttribute)Attribute.GetCustomAttribute(method, typeof(PostRequestAttribute));
                        
                        if (attr != null && attr.Path == req.Url.AbsolutePath)
                        {
                            object[] parameters = new object[] { req, resp };
                            Logger.Debug($"found endpoint: {attr.Path} handled by {method.Name}");
                            var task = (Task)method.Invoke(controller, parameters);
                            await task;
                            resp.Close();
                            break;
                        }
                    }
                }
            }
            else if (req.HttpMethod == "GET")
            {
                foreach (Type controller in _requestHandlers)
                {
                    MethodInfo[] methods = controller.GetMethods();
                    
                    foreach (var method in methods)
                    {
                        Logger.Trace($"trying {controller.Name}.{method.Name}");
                        var attr = (GetRequestAttribute)Attribute.GetCustomAttribute(method, typeof(GetRequestAttribute));
                        
                        if (attr != null && attr.Path == req.Url.AbsolutePath)
                        {
                            object[] parameters = new object[] { req, resp };
                            Logger.Debug($"found endpoint: {attr.Path} handled by {method.Name}");
                            var task = (Task)method.Invoke(controller, parameters);
                            await task;
                            resp.Close();
                            break;
                        }
                    }
                }
            }
            else
            {
                resp.StatusCode = 405;
                await resp.OutputStream.WriteAsync(new byte[] {}, 0, 0);
                resp.Close();
            }
            
            resp.Close();
        }
        
        _listener.Stop();
    }
}