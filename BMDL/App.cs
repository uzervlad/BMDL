using System;
using System.IO;
using System.Web;
using BMDL.API;
using BMDL.Console;

namespace BMDL
{
    public static class App
    {
        public static APIAccess API = new APIAccess();

        public static APIDownloadQueue DownloadQueue = new APIDownloadQueue();

        public static AppConsole Console;

        public static bool DEBUG = false;

        public static string SONGS_PATH = @"";

        public static void Start(bool debug)
        {
            DEBUG = debug;
            if(File.Exists("./songs_path.txt"))
                SONGS_PATH = File.ReadAllText("./songs_path.txt");
            Console = new AppConsole();
            Console.Run();
        }
    }

    public static class StringsExtensions
    {
        public static string Crop(this string s, int length)
        {
            return s.Length <= length ? s : s.Substring(0, length - 2) + "..";
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