using System;
using System.Web;
using BMDL.API;
using BMDL.Console;

namespace BMDL
{
    public class App
    {
        public static APIAccess API = new APIAccess();

        public static AppConsole Console = new AppConsole();

        public void Start()
        {
            Console.Run();
        }
    }
    
    public static class HttpExtensions
    {
        public static Uri AddQuery(this Uri uri, string name, string value)
        {
            var httpValueCollection = HttpUtility.ParseQueryString(uri.Query);

            httpValueCollection.Remove(name);
            httpValueCollection.Add(name, value);

            var ub = new UriBuilder(uri);
            ub.Query = httpValueCollection.ToString();

            return ub.Uri;
        }
    }
}