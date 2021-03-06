using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_SideMirror : MonoBehaviour
{
    public Transform pivot;
    public AssemblePart ap_SideMirror1;

    protected TestData testData;

    bool testProcess = false;

    public IEnumerator TestFlowing()
    {
        testProcess = true;
        testData.Init();

        StartCoroutine(TestRecording());

        DataManager.assemblySuccess[0] = true;

        yield return testContent();

        testProcess = false;
        yield return null;


        TestData tempData = new TestData();
        tempData.Init();
        tempData = testData;

        DataManager.sideMirrorTestList.Add(tempData);
        DataManager.sideMirrorSuccessed.Add(DataManager.assemblySuccess[0]);
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

    public IEnumerator testContent()
    {
        float startTime = Time.time;

        VRUI.Instance.ChangeIndicate("?????? ???? ????\n" + FlowManager_SideMirror.Instance.nowCount + "/" + FlowManager_SideMirror.Instance.loopCount);
        yield return new WaitUntil(() => ap_SideMirror1.isCombine);

        testData.time1 = Time.time - startTime;
    }

    public void Remove()
    {
        Destroy(ap_SideMirror1.gameObject);
        Destroy(gameObject);
    }
}
