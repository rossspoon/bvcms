using System;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using UtilityExtensions;

public class Fingerprint
{
    public static string Tag(string path)
    {
        if (HttpRuntime.Cache[path] == null)
        {
            string absolute = HostingEnvironment.MapPath("~" + path);
            var ext = Path.GetExtension(absolute);
            var f = Path.GetFileNameWithoutExtension(absolute);
            var d = path.Remove(path.LastIndexOf('/'));
            DateTime dt = File.GetLastWriteTime(absolute);
#if DEBUG
            const string min = "";
#else
            const string min = ".min";
#endif
            string tag = "?v=" + dt.Ticks;
            string result = "{0}/{1}{2}{3}{4}".Fmt(d, f, min, ext, tag);
            HttpRuntime.Cache.Insert(path, result, new CacheDependency(absolute));
        }

        return HttpRuntime.Cache[path] as string;
    }
}