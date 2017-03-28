using System;
using System.IO;
using System.Threading.Tasks;
/// <summary>
/// TheSoulWebServerErrorLog2의 요약 설명입니다.
/// </summary>
public class TheSoulWebServerErrorLog2
{
    private static string gHMSTime
    {
        get
        {
            string tmp = string.Format("{0:D2}:{1:D2}:{2:D2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            return tmp;
        }
    }
    private static string gHTime
    {
        get
        {
            string tmp = string.Format("{0:D2}", DateTime.Now.Hour);
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

    public static void WriteLog(string savePath, string Message, string reqURL, string reqval)
    {
        string MyWriteFile = savePath + @"\log\errorlog" + (gYMNTime + "-" + gHTime) + ".txt";
        // 폴더가 존재하지 않으면 새로 생성
        DirectoryInfo dir = new DirectoryInfo(savePath + @"\log\");
        if (dir.Exists == false)
        {
            dir.Create();
        }
        FileStream filesavestream = File.Open(MyWriteFile, FileMode.Append);
        StreamWriter FileWriter = new StreamWriter(filesavestream, System.Text.Encoding.Unicode);
        string log = string.Format("[{0}] {1}", gHMSTime, Message);
        FileWriter.WriteLine(log);
        FileWriter.WriteLine("URL : " + reqURL);
        FileWriter.WriteLine("parameter : " + reqval);
        FileWriter.WriteLine("");
        FileWriter.Close();
    }

    public static void WriteError(string savePath, string Message, string reqURL, string reqval)
    {
        // 람다식을 이용 Task객체 생성
        Task t1 = new Task(() =>
        {
            WriteLog(savePath, Message, reqURL, reqval);
        });
        // Task 쓰레드 시작
        t1.Start();
        // Task가 끝날 때까지 대기
        t1.Wait();
    }
}