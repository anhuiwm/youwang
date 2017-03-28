// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TheSoulDBConnect.cs" company="mSeed">
//   corpse2
// </copyright>
// <summary>
//   dbcon 불러오기
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.IO;
public static class TheSoulDBConnect
{
    public static void ReadFile(string savePath, ref string val)
    {
        string Urim1 =  savePath + @"\dbcon\";//원본 위치
        string filename = "TheSoulDBConnect.ini";
        string sourceFile = Path.Combine(Urim1, filename);

        StreamReader sr = new StreamReader(sourceFile);
        string str = string.Empty;
        while ((str = sr.ReadLine()) != null)
        {
            val = str;
        }
        sr.Close();
    }
}