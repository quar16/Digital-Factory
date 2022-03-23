using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class FlowManager : MonoSingleton<FlowManager>
{
    public Transform startPoint;

    public LeftRightTutorial LR_Tutorial;
    public TutorialMoveChecker tutorialMoveChecker;

    public TestClass ded;
    public TestClass pbf;
    public TestClass fdm;
    public TestClass sla;
    public TestClass sls;
    public TestClass assembly;

    public GameObject VRcamera;

    public static bool[] isTestRun = { true, true, true, true, true, true, true };

    public static void TestSet(int index)
    {
        isTestRun[index] = !isTestRun[index];
    }

    public void Start()
    {
        StartCoroutine(Flowing());
    }

    IEnumerator flow;
    IEnumerator Flowing()
    {
        DataManager.TestDataDictionary = new Dictionary<TestName, TestData>();
        Controller.Init();

        flow = Tutorial();
        if (isTestRun[0])
            yield return StartCoroutine(flow);
        else
        {
            LR_Tutorial.indicdator.parent = LR_Tutorial.rightController;
            LR_Tutorial.indicdator.transform.localPosition = Vector3.zero;
            LR_Tutorial.indicdator.transform.localEulerAngles = Vector3.zero;
        }

        flow = ded.TestFlowing();
        if (isTestRun[1])
            yield return StartCoroutine(flow);

        tutorialMoveChecker.gameObject.SetActive(false);

        flow = pbf.TestFlowing();
        if (isTestRun[2])
            yield return StartCoroutine(flow);

        flow = fdm.TestFlowing();
        if (isTestRun[3])
            yield return StartCoroutine(flow);

        flow = sla.TestFlowing();
        if (isTestRun[4])
            yield return StartCoroutine(flow);

        flow = sls.TestFlowing();
        if (isTestRun[5])
            yield return StartCoroutine(flow);

        flow = assembly.TestFlowing();
        if (isTestRun[6])
            yield return StartCoroutine(flow);

        yield return VRUI.ShowMessage("모든 공정이 완료되었습니다. 테스트를 종료합니다.");

        TestEnd();
    }

    IEnumerator Tutorial()
    {
        yield return VR_Camera.Instance.SetCamera(startPoint);

        yield return VRUI.ShowMessage("튜토리얼을 시작합니다. 컨트롤러의 포인트를 아래의 보라색 포인트에 대고 트리거를 당기면 상호작용할 수 있습니다.");

        yield return VRUI.ShowMessage("빨간색과 파란색 포인트는 해당하는 색의 컨트롤러만 상호작용 할 수 있습니다. 다음 튜토리얼을 완료하기 전, 컨트롤러를 반대로 들고 있다면 바꿔서 들어주세요.");

        yield return VRUI.ShowMessage("왼쪽과 오른쪽 중 안내메시지를 표시할 컨트롤러로 다음 단계를 진행해주세요.");

        LR_Tutorial.gameObject.SetActive(true);

        yield return new WaitUntil(() => CheckFlowMessage("LeftRightTutorialDone"));

        yield return VRUI.ShowMessage("이번에는 이동을 확인하겠습니다. 전방의 노란 영역까지 이동해주세요.");
        tutorialMoveChecker.gameObject.SetActive(true);

        yield return new WaitUntil(() => CheckFlowMessage("WalkTutorialDone"));

        LR_Tutorial.gameObject.SetActive(false);

        yield return VRUI.ShowMessage("이제 공장 내부로 이동하겠습니다. 이동이 완료되기까지 가만히 기다려 주세요.");
    }

    public void TestStop()
    {
        StopCoroutine(flow);
        TestEnd();
    }

    public void TestEnd()
    {
        DataManager.ThreadSaveData();
        SceneLoader.Instance.SceneLoad(SCENE.MAIN);
    }

    string flowMessage;
    public static string FlowMessage
    {
        set
        {
            Instance.flowMessage = value;
        }
    }
    public static bool CheckFlowMessage(string str)
    {
        if (str == Instance.flowMessage)
        {
            FlowMessage = "";
            return true;
        }
        return false;
    }
}
