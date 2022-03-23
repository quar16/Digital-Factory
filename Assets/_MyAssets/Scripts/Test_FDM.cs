using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_FDM : TestClass
{
    public VR_Trigger canisterTrg;
    public VR_Trigger resultTrg;

    public Door ovenDoor;
    public Door canisterDoor;

    public Rigidbody canisterRig;
    public Rigidbody resultRig;

    public Transform canister;
    public Transform[] canisterSlots;
    public Transform result;

    public Transform canisterHighLight;

    public Cart cart;

    public override IEnumerator testContent()
    {
        float startTime = Time.time;

        yield return TestStep("캐니스터 베이 도어 열기", () => canisterDoor.Opened, canisterDoor.trigger);

        yield return TestStep("캐니스터 삽입", () => canisterDone, canisterTrg, canisterHighLight);


        yield return TestStep("캐니스터 베이 도어 닫기", () => canisterDoor.Closed, canisterDoor.trigger);

        result.gameObject.SetActive(true);

        testData.time1 = Time.time - startTime;
        startTime = Time.time;

        yield return SceneLoader.Instance.SceneChangeEffectShowing(true);
        yield return new WaitForSeconds(1);
        yield return SceneLoader.Instance.SceneChangeEffectShowing(false);

        yield return TestStep("제작 완료 \n 오븐 도어 열기", () => ovenDoor.Opened, ovenDoor.trigger);


        yield return TestStep("생산품 꺼내기", () => !resultLose, resultTrg);

        yield return TestStep("생산품 카트로 이동", () => cart.CheckEvent(Layer.DOWN, State.ENTER, result), resultTrg, cart.downHighLight);

        yield return TestStep("오븐 도어 닫기", () => ovenDoor.Closed, ovenDoor.trigger);

        testData.time2 = Time.time - startTime;
        startTime = Time.time;

        yield return TestStep("-캐니스터 제거-\n 캐니스터 베이 도어 열기", () => canisterDoor.Opened, canisterDoor.trigger);

        yield return TestStep("빈 캐니스터 꺼내기", () => !canisterLose, canisterTrg, canisterHighLight);

        yield return TestStep("빈 캐니스터 카트로 이동", () => cart.CheckEvent(Layer.UP, State.ENTER, canister), cart.upHighLight);

        yield return TestStep("캐니스터 베이 도어 닫기", () => canisterDoor.Closed, canisterDoor.trigger);

        testData.time3 = Time.time - startTime;

        VRUI.Instance.ChangeIndicate("모든 공정 완료\n다음 단계로 이동");
        yield return new WaitForSeconds(3);
    }

    bool canisterDone = false;
    bool canisterLose = true;

    public void CanisterClick()
    {
        canisterLose = false;
        canister.parent = canisterTrg.detector.transform;
        canisterRig.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void CanisterOff()
    {
        foreach (Transform canisterSlot in canisterSlots)
        {
            if (Vector3.Distance(canister.position, canisterSlot.position) < 0.2f)
            {
                canister.parent = null;
                canister.position = canisterSlot.position;

                canister.localEulerAngles = Vector3.zero;
                canisterDone = true;
                canisterLose = true;
                return;
            }
        }
        canisterLose = true;
        StartCoroutine(CanisterDropOff());
    }

    IEnumerator CanisterDropOff()
    {
        float delta = 0;
        while (delta < 0.1f)
        {
            delta += Time.deltaTime;
            if (!canisterLose)
                yield break;
            yield return new WaitForFixedUpdate();
        }
        canister.parent = null;
        canisterRig.constraints = RigidbodyConstraints.None;
    }

    public void ResultClick()
    {
        resultLose = false;
        result.parent = resultTrg.detector.transform;
        resultRig.constraints = RigidbodyConstraints.FreezeAll;
    }

    bool resultLose = true;
    //bool resultDone = false;
    public void ResultHold()
    {
        //if (result.localEulerAngles.x < -95)
        //    resultDone = true;
    }

    public void ResultOff()
    {
        resultLose = true;
        StartCoroutine(DropOff());
    }
    IEnumerator DropOff()
    {
        float delta = 0;
        while (delta < 0.1f)
        {
            delta += Time.deltaTime;
            if (!resultLose)
                yield break;
            yield return new WaitForFixedUpdate();
        }
        result.parent = null;
        resultRig.constraints = RigidbodyConstraints.None;
    }

    public override void Remove()
    {
        Destroy(canister.gameObject);
        Destroy(result.gameObject);
        Destroy(gameObject);
    }
}
