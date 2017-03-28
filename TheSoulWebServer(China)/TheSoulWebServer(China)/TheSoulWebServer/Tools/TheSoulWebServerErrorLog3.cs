using System;
using System.IO;
/// <summary>
/// TheSoulWebServerErrorLog2의 요약 설명입니다.
/// </summary>
public class TheSoulWebServerErrorLog3
{
    private static string gHMSTime
    {
        get
        {
            string tmp = string.Format("{0:D2}:{1:D2}:{2:D2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            return tmp;
        }
    }
    private static string gYMNTime
    {
        get
        {
            string tmp = string.Format("{0:D4}-{1:D2}-{2:D2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            return tmp;
        }
    }
    public static void WriteError(string Message, string reqURL, string reqval)
    {
        string MyWriteFile = @"D:\WebServer\AOW\log\(" + gYMNTime + ")_errorlog2.txt";
        FileStream filesavestream = File.Open(MyWriteFile, FileMode.Append);
        StreamWriter FileWriter = new StreamWriter(filesavestream, System.Text.Encoding.Unicode);
        string log = string.Format("[{0}] {1}", gHMSTime, Message);
        FileWriter.WriteLine(log);
        FileWriter.WriteLine("URL : " + reqURL);
        FileWriter.WriteLine("parameter : " + reqval);
        FileWriter.WriteLine("");
        FileWriter.Close();
    }
}