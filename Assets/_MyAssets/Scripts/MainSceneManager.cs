using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneManager : MonoBehaviour
{
    public InputField IDfield;
    public void TestStart()
    {
        DataManager.ID = IDfield.text != "" ? IDfield.text : DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        SceneLoader.Instance.SceneLoad(SCENE.FACTORY);
    }
    public void ToRecord()
    {
        SceneLoader.Instance.SceneLoad(SCENE.RECORD);
    }
}
