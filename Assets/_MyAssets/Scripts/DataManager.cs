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

    public static Dictionary<TestName, TestData> TestDataDictionary;

    public static void ThreadSaveData()
    {
        var _TestDataDictionary = new Dictionary<TestName, TestData>(TestDataDictionary);
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

    static void TestSummaryWrite()
    {
        var lists = CSV.Read("_MyAssets/CSVFormat/AllTestData");
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        string[] keys = new string[lists[0].Keys.Count];
        lists[0].Keys.CopyTo(keys, 0);

        {
            dictionary[keys[0]] = ID;
            dictionary[keys[1]] = avarageWalkSpeed;

            if (TestDataDictionary.ContainsKey(TestName.DED))
            {
                dictionary[keys[2]] = TestDataDictionary[TestName.DED].time1;
                dictionary[keys[3]] = TestDataDictionary[TestName.DED].time2;
                dictionary[keys[4]] = TestDataDictionary[TestName.DED].time3;
            }
            else
            {
                dictionary[keys[2]] = "-";
                dictionary[keys[3]] = "-";
                dictionary[keys[4]] = "-";
            }

            if (TestDataDictionary.ContainsKey(TestName.PBF))
            {
                dictionary[keys[5]] = TestDataDictionary[TestName.PBF].time1;
                dictionary[keys[6]] = TestDataDictionary[TestName.PBF].time2;
                dictionary[keys[7]] = TestDataDictionary[TestName.PBF].time3;
            }
            else
            {
                dictionary[keys[5]] = "-";
                dictionary[keys[6]] = "-";
                dictionary[keys[7]] = "-";
            }
            if (TestDataDictionary.ContainsKey(TestName.FDM))
            {
                dictionary[keys[08]] = TestDataDictionary[TestName.FDM].time1;
                dictionary[keys[09]] = TestDataDictionary[TestName.FDM].time2;
                dictionary[keys[10]] = TestDataDictionary[TestName.FDM].time3;
            }
            else
            {
                dictionary[keys[8]] = "-";
                dictionary[keys[9]] = "-";
                dictionary[keys[10]] = "-";
            }
            if (TestDataDictionary.ContainsKey(TestName.SLA))
            {
                dictionary[keys[11]] = TestDataDictionary[TestName.SLA].time1;
                dictionary[keys[12]] = TestDataDictionary[TestName.SLA].time2;
                dictionary[keys[13]] = TestDataDictionary[TestName.SLA].time3;
            }
            else
            {
                dictionary[keys[11]] = "-";
                dictionary[keys[12]] = "-";
                dictionary[keys[13]] = "-";
            }
            if (TestDataDictionary.ContainsKey(TestName.SLS))
            {
                dictionary[keys[14]] = TestDataDictionary[TestName.SLS].time1;
                dictionary[keys[15]] = TestDataDictionary[TestName.SLS].time2;
            }
            else
            {
                dictionary[keys[14]] = "-";
                dictionary[keys[15]] = "-";
            }
            if (TestDataDictionary.ContainsKey(TestName.ASSEMBLY))
            {
                dictionary[keys[16]] = TestDataDictionary[TestName.ASSEMBLY].time1;
                dictionary[keys[17]] = TestDataDictionary[TestName.ASSEMBLY].time2;
                dictionary[keys[18]] = TestDataDictionary[TestName.ASSEMBLY].time3;

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
        CSV.Write(lists, "_MyAssets/CSV/" + ID + "/TestSummary");
    }

    static void SingleTestWrite(TestName testName)
    {
        TestData data = TestDataDictionary[testName];

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

        CSV.Write(lists, "_MyAssets/CSV/" + ID + "/" + testName.ToString());
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