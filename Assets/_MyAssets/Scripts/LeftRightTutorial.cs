using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftRightTutorial : MonoBehaviour
{
    public Transform indicdator;
    public Transform leftController;
    public Transform rightController;
    bool leftDone;
    bool rightDone;

    public void LeftDone(bool b)
    {
        leftDone = b;
    }
    public void RightDone(bool b)
    {
        rightDone = b;
    }

    // Update is called once per frame
    void Update()
    {
        if (leftDone)
        {
            FlowManager.FlowMessage = "LeftRightTutorialDone";
            indicdator.parent = leftController;
            indicdator.transform.localPosition = Vector3.zero;
            indicdator.transform.localEulerAngles = Vector3.zero;
            gameObject.SetActive(false);
        }
        else if (rightDone)
        {
            FlowManager.FlowMessage = "LeftRightTutorialDone";
            indicdator.parent = rightController;
            indicdator.transform.localPosition = Vector3.zero;
            indicdator.transform.localEulerAngles = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
}
