using System.Net;
using System.Text;
using System.Text.Json.Nodes;

namespace InPostAPIEmulator;

[RequestHandler]
public class TestHandler
{
    [GetRequest("/hello")]
    public static async Task HelloHandler(HttpListenerRequest req, HttpListenerResponse resp)
    {
        byte[] data = Encoding.UTF8.GetBytes("Hello!");
        resp.ContentType = "text/html";
        resp.ContentEncoding = Encoding.UTF8;
        resp.ContentLength64 = data.LongLength;
        
        await resp.OutputStream.WriteAsync(data, 0, data.Length);
    }

    [GetRequest("/v4/parcels/tracked")]
    public static async Task TrackedHandler(HttpListenerRequest req, HttpListenerResponse resp)
    {
        byte[] data = File.ReadAllBytes("test_parcels.json");
        resp.ContentLength64 = data.LongLength;
        resp.StatusCode = 200;

        await resp.OutputStream.WriteAsync(data, 0, data.Length);
    }

    [PostRequest("/v2/collect/validate")]
    public static async Task ValidateHandler(HttpListenerRequest req, HttpListenerResponse resp)
    {
        string guid = Guid.NewGuid().ToString();
        JsonObject sendData = new JsonObject()
        {
            ["sessionUuid"] = guid
        };
        
        byte[] data = Encoding.UTF8.GetBytes(sendData.ToJsonString());
        resp.ContentLength64 = data.LongLength;
        resp.StatusCode = 200;

        await resp.OutputStream.WriteAsync(data, 0, data.Length);
    }
    
    [PostRequest("/v1/collect/compartment/open")]
    public static async Task OpenHandler(HttpListenerRequest req, HttpListenerResponse resp)
    {
        string guid = Guid.NewGuid().ToString();
        JsonObject sendData = new JsonObject()
        {
            ["sessionUuid"] = guid
        };
        
        byte[] data = Encoding.UTF8.GetBytes(sendData.ToJsonString());
        resp.ContentLength64 = data.LongLength;
        resp.StatusCode = 200;

        await resp.OutputStream.WriteAsync(data, 0, data.Length);
    }
    
    [PostRequest("/v1/collect/compartment/terminate")]
    public static async Task TerminateHandler(HttpListenerRequest req, HttpListenerResponse resp)
    {
        string guid = Guid.NewGuid().ToString();
        JsonObject sendData = new JsonObject()
        {
            ["sessionUuid"] = guid
        };
        
        byte[] data = Encoding.UTF8.GetBytes(sendData.ToJsonString());
        resp.ContentLength64 = data.LongLength;
        resp.StatusCode = 200;

        await resp.OutputStream.WriteAsync(data, 0, data.Length);
    }
}