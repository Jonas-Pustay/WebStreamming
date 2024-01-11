using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text;
using System.Web;

namespace WebStreamming;

public class HttpScript
{
    public static Stopwatch sw = new Stopwatch();

    public static Process CreateCommandPrompt()
    {
        Process cmd = new Process();
        cmd.StartInfo.FileName = "/usr/bin/ffmpeg";
        cmd.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
        cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        cmd.StartInfo.RedirectStandardInput = true;
        cmd.StartInfo.RedirectStandardOutput = false;
        cmd.StartInfo.RedirectStandardError = false;
        cmd.StartInfo.CreateNoWindow = true;
        cmd.StartInfo.UseShellExecute = false;
        return cmd;
    }

    public static string GetMovieScreenShot(String Url)
    {
        sw.Restart();
        NameValueCollection UrlParameter = HttpUtility.ParseQueryString(new Uri(Url).Query);

        Program.ffmpeg.StartInfo.Arguments = "-loglevel quiet -skip_frame nokey -ss " + UrlParameter["duration"] + " -i Movie/" + UrlParameter["name"] + "/" + UrlParameter["episode"] + ".avi -r 1 -c:v libwebp -vframes 1 -f image2 -lossless 0 -compression_level 0 -q:v " + UrlParameter["quality"] + " -cr_threshold 0 -preset none -cr_size 16 Streamming.webp -y";
        Program.ffmpeg.Start();
        Program.ffmpeg.StandardInput.Flush();
        Program.ffmpeg.StandardInput.Close();
        Program.ffmpeg.WaitForExit();

        sw.Stop();
        Console.WriteLine("Extract image: " + sw.ElapsedMilliseconds + "ms - " + UrlParameter["name"]);

        return "data:image/webp;base64," + Convert.ToBase64String(File.ReadAllBytesAsync(Directory.GetCurrentDirectory() + "/Streamming.webp").Result);
    }
}