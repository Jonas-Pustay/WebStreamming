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

        Program.ffmpeg.StartInfo.Arguments = "-loglevel quiet -ss " + UrlParameter["duration"] + " -i Movie/" + UrlParameter["name"] + "/" + UrlParameter["episode"] + ".mp4 -r 1 -f image2 -quality " + UrlParameter["quality"] + " -frames:v 1 Streamming.jpg -y";
        Program.ffmpeg.Start();
        Program.ffmpeg.StandardInput.Flush();
        Program.ffmpeg.StandardInput.Close();
        Program.ffmpeg.WaitForExit();

        sw.Stop();
        Console.WriteLine("Test2: " + sw.ElapsedMilliseconds + "ms");

        return "data:image/jpg;base64," + Convert.ToBase64String(File.ReadAllBytes(Directory.GetCurrentDirectory() + "/Streamming.jpg"));
    }

    public static string ToBase64String(string fileName)
    {   
    	using (FileStream reader = new FileStream(fileName, FileMode.Open))
    	{
    		byte[] buffer = new byte[reader.Length];
    		reader.Read(buffer, 0, (int)reader.Length);
    		return Convert.ToBase64String(buffer);
    	}
    }
}