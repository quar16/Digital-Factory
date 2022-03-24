using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_SLA : TestClass
{
    public Door upLeftDoor;
    public Door upRightDoor;
    public Door downDoor;

    public VR_Trigger bottleTrg;
    public VR_Trigger bottleAngleTrg;
    public VR_Trigger hoseTrg;
    public VR_Trigger productTrg;
    public VR_Trigger valveTrg;

    public Rigidbody bottleRig;
    public Rigidbody hoseRig;
    public Rigidbody productRig;

    public Transform hose;
    public Transform hosePlace;
    public Transform product;
    public Transform bottle;
    public Transform bottlePlace;
    public Transform valve;

    public Transform ResinArea;
    public Transform hoseHighLightArea;
    public Transform bottleHighLightArea;

    public BottleParticle supply;

    public Collider bottleCollider;

    public Cart cart;

    public override IEnumerator testContent()
    {
        float startTime = Time.time;

        yield return TestStep("메인 도어 열기", () => upLeftDoor.Opened && upRightDoor.Opened, upLeftDoor.trigger, upRightDoor.trigger);

        yield return TestStep("레진 통 들기", () => bottleLose != 0, bottleTrg);

        yield return TestStep("메인 도어 내부에 레진 붓기", () => supply.done, bottleTrg, bottleAngleTrg, ResinArea);

        yield return TestStep("레진 통 내려놓기", () => bottleLose == 0, bottleTrg);

        yield return TestStep("메인 도어 닫기", () => upLeftDoor.Closed && upRightDoor.Closed, upLeftDoor.trigger, upRightDoor.trigger);

        product.gameObject.SetActive(true);

        testData.time1 = Time.time - startTime;
        startTime = Time.time;

        yield return SceneLoader.Instance.SceneChangeEffectShowing(true);
        yield return new WaitForSeconds(1);
        yield return SceneLoader.Instance.SceneChangeEffectShowing(false);

        yield return TestStep("제작 완료\n메인 도어 열기", () => upLeftDoor.Opened && upRightDoor.Opened, upLeftDoor.trigger, upRightDoor.trigger);

        yield return TestStep("생산품 카트로 이동", () => cart.CheckEvent(Layer.UP, State.ENTER, product), productTrg, cart.upHighLight);

        yield return TestStep("메인 도어 닫기", () => upLeftDoor.Closed && upRightDoor.Closed, upLeftDoor.trigger, upRightDoor.trigger);

        testData.time2 = Time.time - startTime;
        startTime = Time.time;

        yield return TestStep("하단의 서브 도어 열기", () => downDoor.FullOpened, downDoor.trigger);

        yield return TestStep("호스를 관에 연결", () => hoseDone, hoseTrg, hoseHighLightArea);

        yield return TestStep("레진 통을 호스에 연결", () => bottleDone, bottleTrg, bottleHighLightArea);

        yield return TestStep("밸브를 열어 레진을 방출", () => !valveLocked, valveTrg);

        yield return SceneLoader.Instance.SceneChangeEffectShowing(true);
        yield return new WaitForSeconds(1);
        yield return SceneLoader.Instance.SceneChangeEffectShowing(false);

        yield return TestStep("방출 완료\n밸브 잠금", () => valveLocked, valveTrg);

        yield return TestStep("호스 제거", () => cart.CheckEvent(Layer.DOWN, State.ENTER, hose), hoseTrg, cart.downHighLight);

        yield return TestStep("레진 통 카트로 이동", () => cart.CheckEvent(Layer.DOWN, State.ENTER, bottle), bottleTrg, cart.downHighLight);

        yield return TestStep("서브 도어 닫기", () => downDoor.Closed, downDoor.trigger);

        testData.time3 = Time.time - startTime;

        VRUI.Instance.ChangeIndicate("모든 공정 완료\n다음 단계로 이동");
        yield return new WaitForSeconds(3);
    }


    Vector3 bottleDetFirstPos;
    Vector3 bottleFirstPos;
    public void BottleClick()
    {
        bottleLose++;

        bottleDetFirstPos = bottleTrg.detector.transform.position;

        bottleFirstPos = bottle.transform.position;

        bottleRig.constraints = RigidbodyConstraints.FreezeAll;
        bottleCollider.isTrigger = true;
    }

    //bool bottleDone = false;
    public void BottleHold()
    {
        bottle.transform.position = bottleFirstPos - bottleDetFirstPos + bottleTrg.detector.transform.position;
    }

    public void BottleOff()
    {
        bottleLose--;

        if (bottleLose == 0)
        {
            if (Vector3.Distance(bottle.position, bottlePlace.position) < 0.2f)
            {
                bottle.parent = null;
                bottle.position = bottlePlace.position;
                bottle.localEulerAngles = new Vector3(0, 270, 0);
                bottleDone = true;
                return;
            }

            BottleAngleOff();
            if (bottleAngleTrg.detector != null)
                bottleAngleTrg.detector.LoseTarget();
            StartCoroutine(DropOff());
        }
    }

    IEnumerator DropOff()
    {
        float delta = 0;
        while (delta < 0.1f)
        {
            delta += Time.deltaTime;
            if (bottleLose != 0)
                yield break;
            yield return new WaitForFixedUpdate();
        }
        bottleRig.constraints = RigidbodyConstraints.None;
        bottleCollider.isTrigger = false;
    }

    int bottleLose = 0;
    bool bottleDone = false;

    Vector3 bottleFirstRot;
    public void BottleAngleClick()
    {
        bottleFirstRot = bottleAngleTrg.detector.transform.position - bottle.position;

        bottleFirstRot = new Vector3(0, bottleFirstRot.y, bottleFirstRot.z);
    }
    public void BottleAngleHold()
    {
        Vector3 bottleNowRot = bottleAngleTrg.detector.transform.position - bottle.position;
        bottleNowRot = new Vector3(0, bottleNowRot.y, bottleNowRot.z);

        float angle = Vector3.SignedAngle(bottleFirstRot, bottleNowRot, -bottle.right);
        angle = Mathf.Abs(angle);
        bottle.localEulerAngles = new Vector3(0, 90, Mathf.Clamp(angle, 0, 179f));
    }
    public void BottleAngleOff()
    {
        StartCoroutine(AngleDropOff());
    }
    IEnumerator AngleDropOff()
    {
        while (bottle.localEulerAngles.z > 1f)
        {
            bottle.localEulerAngles = new Vector3(0, 90, bottle.localEulerAngles.z * 0.9f);
            yield return null;
        }
        bottle.localEulerAngles = new Vector3(0, 90, 0);
    }

    public void HoseClick()
    {
        hoseLose = false;
        hose.parent = hoseTrg.detector.transform;
        hoseRig.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void HoseHold()
    {
        //if() //liqid
    }
    bool hoseDone = false;
    bool hoseLose = true;
    public void HoseOff()
    {
        if (Vector3.Distance(hose.position, hosePlace.position) < 0.2f)
        {
            hose.parent = null;
            hose.position = hosePlace.position;
            hose.eulerAngles = Vector3.zero;
            hoseDone = true;
            return;
        }
        hoseLose = true;
        StartCoroutine(HoseDropOff());
    }

    IEnumerator HoseDropOff()
    {
        float delta = 0;
        while (delta < 0.1f)
        {
            delta += Time.deltaTime;
            if (!hoseLose)
                yield break;
            yield return new WaitForFixedUpdate();
        }
        hose.parent = null;
        hoseRig.constraints = RigidbodyConstraints.None;
    }

    bool valveMoving = false;
    bool valveLocked = true;

    public void ValveClick()
    {
        if (!valveMoving)
            StartCoroutine(ValveChange());
    }

    IEnumerator ValveChange()
    {
        valveMoving = true;
        for (int i = 0; i < 180; i++)
        {
            if (valveLocked)
                valve.eulerAngles -= new Vector3(0, 0, 5 * Mathf.Sin(Mathf.PI * i / 180f));
            else
                valve.eulerAngles += new Vector3(0, 0, 5 * Mathf.Sin(Mathf.PI * i / 180f));

            yield return null;
        }
        valveLocked = !valveLocked;
        valveMoving = false;
    }


    public void ProductClick()
    {
        productLose = false;
        product.parent = productTrg.detector.transform;
        productRig.constraints = RigidbodyConstraints.FreezeAll;
    }

    bool productLose = true;
    //bool productDone = false;
    public void ProductHold()
    {
        //if (product.localEulerAngles.x < -95)
        //    productDone = true;
    }

    public void ProductOff()
    {
        productLose = true;
        StartCoroutine(ProductDropOff());
    }
    IEnumerator ProductDropOff()
    {
        float delta = 0;
        while (delta < 0.1f)
        {
            delta += Time.deltaTime;
            if (!productLose)
                yield break;
            yield return new WaitForFixedUpdate();
        }
        product.parent = null;
        productRig.constraints = RigidbodyConstraints.None;
    }

    public override void Remove()
    {
        Destroy(hose.gameObject);
        Destroy(product.gameObject);
        Destroy(bottle.gameObject);
        Destroy(gameObject);
    }
}