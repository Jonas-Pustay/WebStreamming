using System.Diagnostics;
using WatsonWebserver;
using WatsonWebserver.Core;
using HttpMethod = WatsonWebserver.Core.HttpMethod;

namespace WebStreamming;

class Program
{
    public static Process ffmpeg = HttpScript.CreateCommandPrompt();
    public static int Count = 0;

    static void Main(string[] args)
    {
        Webserver server = new Webserver(new WebserverSettings("localhost", 9000, false), DefaultRoute);

        // add static routes
        server.Routes.PreAuthentication.Static.Add(HttpMethod.GET, "/Movie/", StreammingContent);

        // start the server
        server.Start();

        Console.WriteLine("Press ENTER to exit");
        Console.ReadLine();

        ffmpeg.Close();
    }

    static async Task DefaultRoute(HttpContextBase ctx) => await ctx.Response.Send("Hello from the default route!");

    static async Task StreammingContent(HttpContextBase ctx) => await ctx.Response.Send(HttpScript.GetMovieScreen());
}
