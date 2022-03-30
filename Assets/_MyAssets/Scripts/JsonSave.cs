using System.IO;
using UnityEngine;

public class JsonSave
{
    public static void Save(string ID, string group, string testName, TestData content)
    {
        string filePath = "Assets/_MyAssets/CSV/" + ID + "/" + group + "/" + testName + ".json";

        string dataAsJson = JsonUtility.ToJson(content);

        File.WriteAllText(filePath, dataAsJson);
    }

    public static TestData Load(string filePath)
    {
        TestData output;

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            output = JsonUtility.FromJson<TestData>(dataAsJson);
        }
        else
        {
            output = new TestData();
        }

        return output;
    }
}