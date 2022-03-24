using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class MainSceneManager : MonoBehaviour
{
    public InputField IDfield;
    public void TestStart()
    {
        DataManager.ID = IDfield.text != "" ? IDfield.text : DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        if (AssetDatabase.IsValidFolder("Assets/_MyAssets/CSV/" + DataManager.ID))
        {
            Debug.Log("중복된 아이디입니다.");
        }
        else
        {
            SceneLoader.Instance.SceneLoad(SCENE.FACTORY);
        }
    }
    public void ToRecord()
    {
        SceneLoader.Instance.SceneLoad(SCENE.RECORD);
    }

    public void Start()
    {
        for (int i = 0; i < 7; i++)
            FlowManager.isTestRun[i] = true;
    }

    public void JsonToCSV()
    {
        DataManager.JsonToCSV();
    }
}
