using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowManager_SideMirror : MonoSingleton<FlowManager_SideMirror>
{
    public Test_SideMirror sideMirrorTest;

    public Transform startPoint;

    public int loopCount = 15;
    [HideInInspector]
    public int nowCount = 0;

    public void Start()
    {
        StartCoroutine(Flowing());
    }

    IEnumerator flow;
    IEnumerator Flowing()
    {
        DataManager.sideMirrorTestList = new List<TestData>();
        DataManager.sideMirrorSuccessed = new List<bool>();

        Controller.Init();
        yield return new WaitForSeconds(1);
        yield return SceneLoader.Instance.SceneChangeEffectShowing(true);
        yield return VR_Camera.Instance.SetCamera(startPoint, false);


        for (int i = 0; i < loopCount; i++)
        {
            nowCount++;
            Test_SideMirror tsm = Instantiate(sideMirrorTest);

            yield return SceneLoader.Instance.SceneChangeEffectShowing(false);

            flow = tsm.TestFlowing();
            yield return StartCoroutine(flow);

            yield return SceneLoader.Instance.SceneChangeEffectShowing(true);
            tsm.Remove();
        }
        yield return SceneLoader.Instance.SceneChangeEffectShowing(false);

        yield return VRUI.ShowMessage("모든 공정이 완료되었습니다. 테스트를 종료합니다.");

        TestEnd();
    }

    public void TestStop()
    {
        StopCoroutine(flow);
        TestEnd();
    }

    public void TestEnd()
    {
        DataManager.SaveSideMirrorData();
        SceneLoader.Instance.SceneLoad(SCENE.MAIN);
    }
}
