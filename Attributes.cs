namespace InPostAPIEmulator;

[AttributeUsage(AttributeTargets.Method)]
public class GetRequestAttribute : Attribute
{
    public string Path { get; }
    public GetRequestAttribute(string path) => Path = path;
}

[AttributeUsage(AttributeTargets.Method)]
public class PostRequestAttribute : Attribute
{
    public string Path { get; }
    public PostRequestAttribute(string path) => Path = path;
}

[AttributeUsage(AttributeTargets.Class)]
public class RequestHandlerAttribute : Attribute { }