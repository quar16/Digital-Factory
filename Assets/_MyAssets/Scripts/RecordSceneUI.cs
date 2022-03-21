using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class RecordSceneUI : MonoBehaviour
{
    public Dropdown ID_Dropdown;
    public RecordReplayer recordReplayer;
    public void Start()
    {

        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/_MyAssets/CSV");
        DirectoryInfo[] info = dir.GetDirectories();

        List<string> ID_list = new List<string>();
        foreach (DirectoryInfo f in info)
        {
            ID_list.Add(f.Name);
        }
        ID_Dropdown.AddOptions(ID_list);
    }

    string ID;
    TestName testName;

    public void ChangeID(int index)
    {
        ID = ID_Dropdown.options[index].text;
        recordReplayer.SetRecord(ID, testName);
    }
    public void ChangeTest(int index)
    {
        testName = (TestName)index;
        recordReplayer.SetRecord(ID, testName);
    }

    public void Reset()
    {
        recordReplayer.Reset();
    }

    public void ToMain()
    {
        SceneLoader.Instance.SceneLoad(SCENE.MAIN);
    }
}