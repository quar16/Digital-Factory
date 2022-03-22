using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_DED : TestClass
{
    public VR_Trigger bottleTrg;
    public VR_Trigger capTrg;
    public VR_Trigger jigTrg;

    public Door mainDoor;

    public Rigidbody bottleRig;
    public Rigidbody jigRig;

    public Transform jig;
    public Transform jigPlace;
    public GameObject result;
    public Transform bottle;
    public Transform cap;

    public Transform supplyHighLight;

    public BottleParticle supply;

    public Cart cart;

    public override IEnumerator testContent()
    {
        float startTime = Time.time;

        yield return TestStep("메인 도어 열기", () => mainDoor.isOpen, mainDoor.trigger);

        yield return TestStep("원재료 투입구 열기", () => !capClosed, capTrg);

        yield return TestStep("원재료 투입", () => supply.done, bottleTrg, supplyHighLight);

        yield return TestStep("원재료 통 내려놓기", () => bottleLose, bottleTrg);

        testData.time1 = Time.time - startTime;
        startTime = Time.time;

        yield return TestStep("원재료 투입구 닫기", () => capClosed, capTrg);

        yield return TestStep("지그 설치 하기", () => jigDone, jigTrg, jigPlace);

        yield return TestStep("메인 도어 닫기", () => !mainDoor.isOpen, mainDoor.trigger);

        result.SetActive(true);

        testData.time2 = Time.time - startTime;
        startTime = Time.time;

        yield return SceneLoader.Instance.SceneChangeEffectShowing(true);
        yield return new WaitForSeconds(1);
        yield return SceneLoader.Instance.SceneChangeEffectShowing(false);

        yield return TestStep("제작 완료\n 메인 도어 열기", () => mainDoor.isOpen, mainDoor.trigger);

        yield return TestStep("생산품 꺼내기", () => cart.CheckEvent(Layer.DOWN, State.ENTER, jig), jigTrg, cart.downHighLight);

        yield return TestStep("메인 도어 닫기", () => !mainDoor.isOpen, mainDoor.trigger);

        testData.time3 = Time.time - startTime;

        VRUI.Instance.ChangeIndicate("모든 공정 완료\n다음 단계로 이동");
        yield return new WaitForSeconds(3);
    }

    public void BottleClick()
    {
        bottleLose = false;
        bottle.parent = bottleTrg.detector.transform;
        bottleRig.constraints = RigidbodyConstraints.FreezeAll;
    }

    //bool bottleDone = false;
    public void BottleHold()
    {
        //if (bottle.localEulerAngles.x < -95)
        //    bottleDone = true;
    }


    public void BottleOff()
    {
        bottleLose = true;
        StartCoroutine(DropOff());

    }
    IEnumerator DropOff()
    {
        float delta = 0;
        while (delta < 0.1f)
        {
            delta += Time.deltaTime;
            if (!bottleLose)
                yield break;
            yield return new WaitForFixedUpdate();
        }
        bottle.parent = null;
        bottleRig.constraints = RigidbodyConstraints.None;
    }

    bool bottleLose = true;


    bool capClosed = true;
    bool isCapRun = false;
    public void CapClick()
    {
        if (isCapRun)
            return;
        isCapRun = true;

        StartCoroutine(CapChange());
    }
    IEnumerator CapChange()
    {
        isCapRun = true;

        for (int i = 0; i < 90; i++)
        {
            if (capClosed)
                cap.localEulerAngles = new Vector3(0, 180, 360 - i / 90f * 150);
            else
                cap.localEulerAngles = new Vector3(0, 180, 210 + i / 90f * 150);
            yield return null;
        }

        if (capClosed)
            cap.localEulerAngles = new Vector3(0, 180, 210);
        else
            cap.localEulerAngles = new Vector3(0, 180, 360);

        capClosed = !capClosed;
        isCapRun = false;
    }

    public void JigClick()
    {
        jigLose = false;
        jig.parent = jigTrg.detector.transform;
        jigRig.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void JigHold()
    {
        //if() //liqid
    }
    bool jigDone = false;
    bool jigLose = true;
    public void JigOff()
    {
        if (Vector3.Distance(jig.position, jigPlace.position) < 0.2f)
        {
            jig.parent = null;
            jig.position = jigPlace.position;
            jig.eulerAngles = Vector3.zero;
            jigDone = true;
            return;
        }
        jigLose = true;
        StartCoroutine(JigDropOff());
    }

    IEnumerator JigDropOff()
    {
        float delta = 0;
        while (delta < 0.1f)
        {
            delta += Time.deltaTime;
            if (!jigLose)
                yield break;
            yield return new WaitForFixedUpdate();
        }
        jig.parent = null;
        jigRig.constraints = RigidbodyConstraints.None;
    }


    public override void Remove()
    {
        Destroy(jig.gameObject);
        Destroy(bottle.gameObject);
        Destroy(gameObject);
    }
}
