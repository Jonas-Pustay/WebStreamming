using System;
using System.Diagnostics;
using System.Text;

namespace WebStreamming;

public class HttpScript
{
    public static Process CreateCommandPrompt()
    {
        Process cmd = new Process();
        cmd.StartInfo.FileName = "/bin/bash";
        cmd.StartInfo.RedirectStandardInput = true;
        cmd.StartInfo.RedirectStandardOutput = false;
        cmd.StartInfo.CreateNoWindow = true;
        cmd.StartInfo.UseShellExecute = false;
        cmd.Start();
        return cmd;
    }

    public static string GetMovieScreen()
    {
        Program.Count++;

        Program.ffmpeg.StandardInput.WriteLine("ffmpeg -loglevel quiet -ss 00:08:39 -i videoplayback.mp4 -r 3.5 -c:v libwebp -frames 1 -lossless 0 -compression_level 0 -quality 100 -cr_threshold 0 -cr_size 16 test" +Program.Count + ".webp -y");
        Program.ffmpeg.StandardInput.Flush();

        return "data:image/jpeg;base64," +Convert.ToBase64String(File.ReadAllBytes(Directory.GetCurrentDirectory() + "/test" + Program.Count + ".webp"));
    }
}