// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonConvert.cs" company="mSeed">
//   corpse2
// </copyright>
// <summary>
//   암호화 된 데이터 Json으로 다시 묶기
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.IO;
using System.Net.Json;
public static class JsonConvert
{
    public static void Convert(int resultcode,string EncrytData, ref string convertdata)
    {
        JsonObjectCollection res = new JsonObjectCollection();
        res.Add(new JsonNumericValue("result", resultcode));
        res.Add(new JsonStringValue("returndata", EncrytData));
        convertdata = res.ToString();
    }
}