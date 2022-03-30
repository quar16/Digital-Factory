using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class MainSceneManager : MonoBehaviour
{
    static string tempID = "";

    public InputField IDfield;
    public void TestStart()
    {
        DataManager.ID = IDfield.text != "" ? IDfield.text : DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        tempID = IDfield.text;
        SceneLoader.Instance.SceneLoad(SCENE.FACTORY);
    }
    public void ToRecord()
    {
        SceneLoader.Instance.SceneLoad(SCENE.RECORD);
    }

    public void Start()
    {
        for (int i = 0; i < 7; i++)
            FlowManager.isTestRun[i] = true;

        IDfield.text = tempID;
    }

    public void JsonToCSV()
    {
        DataManager.JsonToCSV();
    }

    public void SideMirrorTest()
    {
        DataManager.ID = IDfield.text != "" ? IDfield.text : DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        tempID = IDfield.text;
        SceneLoader.Instance.SceneLoad(SCENE.SIDEMIRROR);
    }
}
