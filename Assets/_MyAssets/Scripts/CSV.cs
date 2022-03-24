using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public class CSV
{
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    public static List<Dictionary<string, object>> Read(string filePath)
    {
        StreamReader reader = new StreamReader(Application.dataPath + "/" + filePath + ".csv", Encoding.UTF8);

        string[] lines = Regex.Split(reader.ReadToEnd(), LINE_SPLIT_RE);
        string[] keyRow = SplitCsvLine(lines[0]);

        var outputDic = new List<Dictionary<string, object>>();

        for (int y = 0; y < lines.Length; y++)
        {
            var dic = new Dictionary<string, object>();
            string[] row = SplitCsvLine(lines[y]);

            for (int x = 0; x < row.Length; x++)
            {
                dic.Add(keyRow[x], row[x]);
            }
            outputDic.Add(dic);
        }
        reader.Close();
        return outputDic;
    }

    static string[] SplitCsvLine(string line)
    {
        return (from Match m in Regex.Matches(line, @"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)", RegexOptions.ExplicitCapture) select m.Groups[1].Value).ToArray();
    }

    public static void Write(List<Dictionary<string, object>> rowData, string filePath)
    {
        int length = rowData.Count;
        string[] output = new string[length];

        for (int i = 0; i < length; i++)
        {
            List<string> list = new List<string>();
            foreach (object str in rowData[i].Values)
            {
                list.Add(str.ToString());
            }
            output[i] = string.Join(",", list);
        }
        string allText = "";
        allText += output[0];

        for (int index = 1; index < length; index++)
        {
            allText += "\n";
            allText += output[index];
        }
            File.WriteAllText(Application.dataPath + "/" + filePath + ".csv", allText);
    }
}