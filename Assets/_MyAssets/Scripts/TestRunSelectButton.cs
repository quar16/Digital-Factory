using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestRunSelectButton : MonoBehaviour
{
    Image button;
    public int index;
    private void Start()
    {
        button = GetComponent<Image>();
    }

    bool testRun = true;

    public void TestRunChange()
    {
        testRun = !testRun;
        FlowManager.isTestRun[index] = testRun;

        if (testRun)
            button.color = new Color(1, 1, 1);
        else
            button.color = new Color(0, 0, 0);
    }
}
