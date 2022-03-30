using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestClass : MonoBehaviour
{
    public Transform pivot;
    public TestName testName;

    protected TestData testData;

    bool testProcess = false;

    public IEnumerator TestFlowing()
    {
        yield return VR_Camera.Instance.SetCamera(pivot);

        testProcess = true;
        testData.Init();

        StartCoroutine(TestRecording());

        yield return testContent();

        testProcess = false;
        yield return null;
        DataManager.testDataDictionary.Add(testName, testData);
    }

    public IEnumerator TestRecording()
    {
        float startTime = Time.time;
        while (testProcess)
        {
            yield return new WaitForFixedUpdate();

            testData.time.Add(Time.time - startTime);
            testData.HMDpos.Add(VR_Camera.Instance.cameraT.position - pivot.position);
            testData.leftPos.Add(Controller.left.transform.position - pivot.position);
            testData.rightPos.Add(Controller.right.transform.position - pivot.position);
            testData.HMDrot.Add(VR_Camera.Instance.cameraT.eulerAngles);
            testData.leftRot.Add(Controller.left.transform.eulerAngles);
            testData.rightRot.Add(Controller.right.transform.eulerAngles);

            if (Controller.left.clickState == ClickState.CLICK || Controller.left.clickState == ClickState.STAY)
                testData.leftClick.Add(true);
            else
                testData.leftClick.Add(false);

            if (Controller.right.clickState == ClickState.CLICK || Controller.right.clickState == ClickState.STAY)
                testData.rightClick.Add(true);
            else
                testData.rightClick.Add(false);
        }
    }


    public virtual IEnumerator testContent()
    {
        yield break;
    }


    public IEnumerator TestStep(string messsage, Func<bool> predicate)
    {
        VRUI.Instance.ChangeIndicate(messsage);
        yield return new WaitUntil(predicate);
    }

    public IEnumerator TestStep(string messsage, Func<bool> predicate, VR_Trigger trg)
    {
        HighLightManager.BallHighLightSet(trg);
        VRUI.Instance.ChangeIndicate(messsage);
        yield return new WaitUntil(predicate);
        HighLightManager.BallHighLightHide();
    }
    public IEnumerator TestStep(string messsage, Func<bool> predicate, VR_Trigger trg1, VR_Trigger trg2)
    {
        HighLightManager.BallHighLightSet(trg1, trg2);
        VRUI.Instance.ChangeIndicate(messsage);
        yield return new WaitUntil(predicate);
        HighLightManager.BallHighLightHide();
    }
    public IEnumerator TestStep(string messsage, Func<bool> predicate, Transform pos)
    {
        HighLightManager.AreaHighLightSet(pos);
        VRUI.Instance.ChangeIndicate(messsage);
        yield return new WaitUntil(predicate);
        HighLightManager.AreaHighLightHide();
    }
    public IEnumerator TestStep(string messsage, Func<bool> predicate, VR_Trigger trg, Transform pos)
    {
        HighLightManager.BallHighLightSet(trg);
        HighLightManager.AreaHighLightSet(pos);
        VRUI.Instance.ChangeIndicate(messsage);
        yield return new WaitUntil(predicate);
        HighLightManager.AreaHighLightHide();
        HighLightManager.BallHighLightHide();
    }
    public IEnumerator TestStep(string messsage, Func<bool> predicate, VR_Trigger trg1, VR_Trigger trg2, Transform pos)
    {
        HighLightManager.BallHighLightSet(trg1, trg2);
        HighLightManager.AreaHighLightSet(pos);
        VRUI.Instance.ChangeIndicate(messsage);
        yield return new WaitUntil(predicate);
        HighLightManager.AreaHighLightHide();
        HighLightManager.BallHighLightHide();
    }


    public virtual void Remove() { }
}
