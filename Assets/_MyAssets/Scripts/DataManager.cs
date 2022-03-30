using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//아래부터 저장을 위한 추가 using
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;
using UnityEditor;
using System.Threading;

public enum TestName { DED, PBF, FDM, SLA, SLS, ASSEMBLY }

public class DataManager
{
    public static string ID;
    public static float avarageWalkSpeed;
    public static bool[] assemblySuccess = { true, true, true };

    public static Dictionary<TestName, TestData> testDataDictionary;

    public static List<TestData> sideMirrorTestList = new List<TestData>();
    public static List<bool> sideMirrorSuccessed = new List<bool>();

    static string savePath = "";

    public static void SaveData()
    {
        if (!AssetDatabase.IsValidFolder("Assets/_MyAssets/CSV/" + ID))
            AssetDatabase.CreateFolder("Assets/_MyAssets/CSV", ID);

        int count = 1;
        while (AssetDatabase.IsValidFolder("Assets/_MyAssets/CSV/" + ID + "/" + count.ToString()))
            count++;

        AssetDatabase.CreateFolder("Assets/_MyAssets/CSV/" + ID, count.ToString());
        savePath = "_MyAssets/CSV/" + ID + "/" + count.ToString() + "/";

        TestSummaryWrite(count.ToString());

        TestName testName = TestName.DED;

        for (int i = 0; i < 6; i++)
        {
            if (testDataDictionary.ContainsKey(testName))
                JsonSave.Save(ID, count.ToString(), testName.ToString(), testDataDictionary[testName]);
            testName++;
        }

        AssetDatabase.Refresh();
    }

    static void TestSummaryWrite(string group)
    {
        var lists = CSV.Read("_MyAssets/CSVFormat/AllTestData");
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        string[] keys = new string[lists[0].Keys.Count];
        lists[0].Keys.CopyTo(keys, 0);

        {
            dictionary[keys[0]] = ID;
            
            if (FlowManager.isTestRun[0])
                dictionary[keys[1]] = avarageWalkSpeed;
            else
                dictionary[keys[1]] = "-";

            if (testDataDictionary.ContainsKey(TestName.DED))
            {
                dictionary[keys[2]] = testDataDictionary[TestName.DED].time1;
                dictionary[keys[3]] = testDataDictionary[TestName.DED].time2;
                dictionary[keys[4]] = testDataDictionary[TestName.DED].time3;
            }
            else
            {
                dictionary[keys[2]] = "-";
                dictionary[keys[3]] = "-";
                dictionary[keys[4]] = "-";
            }

            if (testDataDictionary.ContainsKey(TestName.PBF))
            {
                dictionary[keys[5]] = testDataDictionary[TestName.PBF].time1;
                dictionary[keys[6]] = testDataDictionary[TestName.PBF].time2;
                dictionary[keys[7]] = testDataDictionary[TestName.PBF].time3;
            }
            else
            {
                dictionary[keys[5]] = "-";
                dictionary[keys[6]] = "-";
                dictionary[keys[7]] = "-";
            }
            if (testDataDictionary.ContainsKey(TestName.FDM))
            {
                dictionary[keys[08]] = testDataDictionary[TestName.FDM].time1;
                dictionary[keys[09]] = testDataDictionary[TestName.FDM].time2;
                dictionary[keys[10]] = testDataDictionary[TestName.FDM].time3;
            }
            else
            {
                dictionary[keys[8]] = "-";
                dictionary[keys[9]] = "-";
                dictionary[keys[10]] = "-";
            }
            if (testDataDictionary.ContainsKey(TestName.SLA))
            {
                dictionary[keys[11]] = testDataDictionary[TestName.SLA].time1;
                dictionary[keys[12]] = testDataDictionary[TestName.SLA].time2;
                dictionary[keys[13]] = testDataDictionary[TestName.SLA].time3;
            }
            else
            {
                dictionary[keys[11]] = "-";
                dictionary[keys[12]] = "-";
                dictionary[keys[13]] = "-";
            }
            if (testDataDictionary.ContainsKey(TestName.SLS))
            {
                dictionary[keys[14]] = testDataDictionary[TestName.SLS].time1;
                dictionary[keys[15]] = testDataDictionary[TestName.SLS].time2;
            }
            else
            {
                dictionary[keys[14]] = "-";
                dictionary[keys[15]] = "-";
            }
            if (testDataDictionary.ContainsKey(TestName.ASSEMBLY))
            {
                dictionary[keys[16]] = testDataDictionary[TestName.ASSEMBLY].time1;
                dictionary[keys[17]] = testDataDictionary[TestName.ASSEMBLY].time2;
                dictionary[keys[18]] = testDataDictionary[TestName.ASSEMBLY].time3;

                dictionary[keys[19]] = assemblySuccess[0];
                dictionary[keys[20]] = assemblySuccess[1];
                dictionary[keys[21]] = assemblySuccess[2];
            }
            else
            {
                dictionary[keys[16]] = "-";
                dictionary[keys[17]] = "-";
                dictionary[keys[18]] = "-";

                dictionary[keys[19]] = "-";
                dictionary[keys[20]] = "-";
                dictionary[keys[21]] = "-";
            }
        } // dictionary set
        lists.Add(dictionary);
        CSV.Write(lists, "_MyAssets/CSV/" + ID + "/" + group + "/TestSummary");
    }


    static void SideMirrorSummaryWrite()
    {
        var lists = CSV.Read("_MyAssets/CSVFormat/SideMirrorTestData");
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        string[] keys = new string[lists[0].Keys.Count];
        lists[0].Keys.CopyTo(keys, 0);

        for (int i = 0; i < sideMirrorTestList.Count; i++)
        {
            dictionary[keys[0]] = sideMirrorTestList[i].time1;
            dictionary[keys[1]] = sideMirrorSuccessed[i];
            lists.Add(new Dictionary<string, object>(dictionary));
        }
        CSV.Write(lists, "_MyAssets/CSV/" + ID + "/SideMirror/TestSummary");
    }

    public static void SaveSideMirrorData()
    {
        if (!AssetDatabase.IsValidFolder("Assets/_MyAssets/CSV/" + ID))
            AssetDatabase.CreateFolder("Assets/_MyAssets/CSV", ID);

        if (!AssetDatabase.IsValidFolder("Assets/_MyAssets/CSV/" + ID + "/SideMirror"))
            AssetDatabase.CreateFolder("Assets/_MyAssets/CSV/" + ID, "SideMirror");

        SideMirrorSummaryWrite();

        for (int i = 0; i < sideMirrorTestList.Count; i++)
        {
            JsonSave.Save(ID, "SideMirror", (i + 1).ToString(), sideMirrorTestList[i]);
        }

        AssetDatabase.Refresh();
    }


    public static void JsonToCSV()
    {
        string[] IDs = AssetDatabase.GetSubFolders("Assets/_MyAssets/CSV");

        foreach (string id in IDs)
        {
            string[] sets = AssetDatabase.GetSubFolders(id);
            DirectoryInfo id_dir = new DirectoryInfo(id);
            string _ID = id_dir.Name;

            foreach (string set in sets)
            {
                DirectoryInfo dir = new DirectoryInfo(set);

                FileInfo[] jsons = dir.GetFiles("*.json");

                string group = dir.Name;

                foreach (FileInfo j in jsons)
                {
                    SingleTestWrite(JsonSave.Load(j.FullName), _ID, group, j.Name.Replace(".json", ""));
                    File.Delete(j.FullName);
                    File.Delete(j.FullName + ".meta");
                }
            }
        }

        Debug.Log("convert Done");
        AssetDatabase.Refresh();
    }

    static void SingleTestWrite(TestData data, string _ID, string group, string testName)
    {
        var lists = CSV.Read("_MyAssets/CSVFormat/SingleTestData");

        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        string[] keys = new string[lists[0].Keys.Count];
        lists[0].Keys.CopyTo(keys, 0);

        foreach (string str in keys)
        {
            dictionary.Add(str, "");
        }

        int count = data.leftClick.Count;

        for (int i = 0; i < count; i++)
        {
            dictionary[keys[0]] = data.time[i];

            dictionary[keys[1]] = data.HMDpos[i].x;
            dictionary[keys[2]] = data.HMDpos[i].y;
            dictionary[keys[3]] = data.HMDpos[i].z;

            dictionary[keys[4]] = data.HMDrot[i].x;
            dictionary[keys[5]] = data.HMDrot[i].y;
            dictionary[keys[6]] = data.HMDrot[i].z;

            dictionary[keys[7]] = data.leftPos[i].x;
            dictionary[keys[8]] = data.leftPos[i].y;
            dictionary[keys[9]] = data.leftPos[i].z;

            dictionary[keys[10]] = data.leftRot[i].x;
            dictionary[keys[11]] = data.leftRot[i].y;
            dictionary[keys[12]] = data.leftRot[i].z;

            dictionary[keys[13]] = data.leftClick[i];

            dictionary[keys[14]] = data.rightPos[i].x;
            dictionary[keys[15]] = data.rightPos[i].y;
            dictionary[keys[16]] = data.rightPos[i].z;

            dictionary[keys[17]] = data.rightRot[i].x;
            dictionary[keys[18]] = data.rightRot[i].y;
            dictionary[keys[19]] = data.rightRot[i].z;

            dictionary[keys[20]] = data.rightClick[i];

            lists.Add(new Dictionary<string, object>(dictionary));
        }

        CSV.Write(lists, "_MyAssets/CSV/" + _ID + "/" + group + "/" + testName);
    }

}

#region Data Structs
[Serializable]
public struct TestData
{
    public List<Vector3> HMDpos;
    public List<Vector3> HMDrot;
    public List<Vector3> leftPos;
    public List<Vector3> rightPos;
    public List<Vector3> leftRot;
    public List<Vector3> rightRot;
    public List<float> time;
    public List<bool> leftClick;
    public List<bool> rightClick;
    public float time1;
    public float time2;
    public float time3;
    public void Init()
    {
        HMDpos = new List<Vector3>();
        HMDrot = new List<Vector3>();
        leftPos = new List<Vector3>();
        leftRot = new List<Vector3>();
        rightPos = new List<Vector3>();
        rightRot = new List<Vector3>();
        time = new List<float>();
        leftClick = new List<bool>();
        rightClick = new List<bool>();
        time1 = 0;
        time2 = 0;
        time3 = 0;
    }
}
#endregion


/*
    public static void ThreadSaveData()
    {
        var _TestDataDictionary = new Dictionary<TestName, TestData>(testDataDictionary);
        string _ID = ID;

        string guid = AssetDatabase.CreateFolder("Assets/_MyAssets/CSV", _ID);
        string newFolderPath = AssetDatabase.GUIDToAssetPath(guid);

        TestName testName = TestName.DED;

        CSV.dataPath = Application.dataPath;

        Thread _saveDataThread = new Thread(() =>
        {
            TestSummaryWrite();

            for (int i = 0; i < 6; i++)
            {
                if (_TestDataDictionary.ContainsKey(testName))
                    SingleTestWrite(testName);
                testName++;
            }
            Debug.Log(_ID + " Save Done");
        });
        _saveDataThread.Start();


        void SingleTestWrite(TestName testName)
        {
            TestData data = _TestDataDictionary[testName];

            var lists = CSV.Read("_MyAssets/CSVFormat/SingleTestData");

            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            string[] keys = new string[lists[0].Keys.Count];
            lists[0].Keys.CopyTo(keys, 0);

            foreach (string str in keys)
            {
                dictionary.Add(str, "");
            }

            int count = data.leftClick.Count;

            for (int i = 0; i < count; i++)
            {
                dictionary[keys[0]] = data.time[i];

                dictionary[keys[1]] = data.HMDpos[i].x;
                dictionary[keys[2]] = data.HMDpos[i].y;
                dictionary[keys[3]] = data.HMDpos[i].z;

                dictionary[keys[4]] = data.HMDrot[i].x;
                dictionary[keys[5]] = data.HMDrot[i].y;
                dictionary[keys[6]] = data.HMDrot[i].z;

                dictionary[keys[7]] = data.leftPos[i].x;
                dictionary[keys[8]] = data.leftPos[i].y;
                dictionary[keys[9]] = data.leftPos[i].z;

                dictionary[keys[10]] = data.leftRot[i].x;
                dictionary[keys[11]] = data.leftRot[i].y;
                dictionary[keys[12]] = data.leftRot[i].z;

                dictionary[keys[13]] = data.leftClick[i];

                dictionary[keys[14]] = data.rightPos[i].x;
                dictionary[keys[15]] = data.rightPos[i].y;
                dictionary[keys[16]] = data.rightPos[i].z;

                dictionary[keys[17]] = data.rightRot[i].x;
                dictionary[keys[18]] = data.rightRot[i].y;
                dictionary[keys[19]] = data.rightRot[i].z;

                dictionary[keys[20]] = data.rightClick[i];

                lists.Add(new Dictionary<string, object>(dictionary));
            }

            CSV.Write(lists, "_MyAssets/CSV/" + _ID + "/" + testName.ToString());
        }
    }
 
 
 */