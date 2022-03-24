using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_PBF : TestClass
{
    public VR_Trigger liftLeverTrigger;
    public VR_Trigger liftLeftTrigger;
    public VR_Trigger liftRightTrigger;
    public VR_Trigger productDoorLeftTrg;
    public VR_Trigger productDoorRightTrg;

    public Door mainDoor;

    public Transform productDoor;
    public Transform lift;
    public Transform liftHand;
    public Transform supplyContainer;
    public Transform productContainer;

    public Transform PBF_Main;
    public Transform supplyStation;

    public Transform[] highLightArea;

    public override IEnumerator testContent()
    {
        float startTime = Time.time;

        yield return TestStep("메인 도어 열기", () => mainDoor.FullOpened, mainDoor.trigger);

        targetLiftLevel = 0;
        yield return TestStep("리프트 높이 조절", () => nowLiftLevel == targetLiftLevel, liftLeverTrigger);

        yield return TestStep("리프트 잡기", () => liftLeftHold && liftRightHold, liftLeftTrigger, liftRightTrigger);

        yield return TestStep("원재료 컨테이너 들기", () => carryingSupplyContainer, highLightArea[0]);

        targetLiftLevel = 2;
        yield return TestStep("리프트 높이 조절", () => nowLiftLevel == targetLiftLevel, liftLeverTrigger);

        carryChangeCoolTime = false;
        yield return TestStep("원재료 컨테이너 삽입", () => !carryingSupplyContainer, highLightArea[3]);

        testData.time1 = Time.time - startTime;
        startTime = Time.time;

        yield return TestStep("생산품 스테이션 도어 열기", () => productDoorOpen, productDoorLeftTrg, productDoorRightTrg);

        targetLiftLevel = 1;
        yield return TestStep("리프트 높이 조절", () => nowLiftLevel == targetLiftLevel, liftLeverTrigger);

        carryChangeCoolTime = false;
        yield return TestStep("지정 위치로 리프트 이동", () => carryingProductContainer, highLightArea[4]);
        lift.localPosition = highLightArea[4].localPosition;
        lift.localEulerAngles = highLightArea[4].localEulerAngles;
        liftFixed = true;

        targetLiftLevel = 3;
        yield return TestStep("리프트 높이 조절", () => nowLiftLevel == targetLiftLevel, liftLeverTrigger);
        liftFixed = false;

        carryChangeCoolTime = false;
        yield return TestStep("생산품 컨테이너 삽입", () => !carryingProductContainer, highLightArea[2]);

        yield return TestStep("리프트 뒤로 이동", () => Vector3.Distance(lift.transform.position, highLightArea[1].position) < 0.5f, highLightArea[1]);

        yield return TestStep("메인 도어 닫기", () => mainDoor.Closed, mainDoor.trigger);

        testData.time2 = Time.time - startTime;
        startTime = Time.time;

        yield return SceneLoader.Instance.SceneChangeEffectShowing(true);
        yield return new WaitForSeconds(1);
        yield return SceneLoader.Instance.SceneChangeEffectShowing(false);

        yield return TestStep("제작 완료\n메인 도어 열기", () => mainDoor.FullOpened, mainDoor.trigger);

        carryChangeCoolTime = false;
        yield return TestStep("생산품 컨테이너 들기", () => carryingProductContainer, highLightArea[2]);

        yield return TestStep("생산품 스테이션으로 이동", () => Vector3.Distance(lift.transform.position, highLightArea[4].position) < 0.1f, highLightArea[4]);
        lift.localPosition = highLightArea[4].localPosition;
        lift.localEulerAngles = highLightArea[4].localEulerAngles;
        liftFixed = true;

        targetLiftLevel = 1;
        carryChangeCoolTime = false;
        yield return TestStep("리프트 높이 조절", () => nowLiftLevel == targetLiftLevel, liftLeverTrigger);
        liftFixed = false;

        yield return TestStep("지정 위치로 리프트 이동", () => Vector3.Distance(lift.transform.position, highLightArea[1].position) < 0.5f, highLightArea[1]);
        liftFixed = true;

        yield return TestStep("생산품 스테이션 닫기", () => !productDoorOpen, productDoorLeftTrg, productDoorRightTrg);

        yield return TestStep("메인 도어 닫기", () => mainDoor.Closed, mainDoor.trigger);

        testData.time3 = Time.time - startTime;

        VRUI.Instance.ChangeIndicate("모든 공정 완료\n다음 단계로 이동");
        yield return new WaitForSeconds(3);
    }

    bool productDoorOpen = false;
    Vector3 productDoorFirstVector;
    float startPos = 0;
    bool productDoorRightHold = false;
    public void ProductDoorLeftClick()
    {
        productDoorFirstVector = productDoorLeftTrg.detector.transform.position - PBF_Main.position;
        startPos = productDoor.localPosition.z;
    }

    public void ProductDoorLeftHold()
    {
        if (Vector3.Distance(productDoorLeftTrg.detector.transform.position, productDoorLeftTrg.transform.position) < 0.1f && productDoorRightHold)
            ProductDoorHold();
        else
        {
            productDoorLeftTrg.detector.LoseTarget();
            ProductDoorRightOff();
        }
    }

    public void ProductDoorRightHold()
    {
        if (Vector3.Distance(productDoorRightTrg.detector.transform.position, productDoorRightTrg.transform.position) < 0.1f)
            productDoorRightHold = true;
        else
        {
            productDoorRightTrg.detector.LoseTarget();
            ProductDoorRightOff();
        }
    }

    public void ProductDoorHold()
    {
        Vector3 doorNowVector = productDoorLeftTrg.detector.transform.position - PBF_Main.position;
        doorNowVector = new Vector3(doorNowVector.x, 0, doorNowVector.z);

        float distance = startPos + (productDoorFirstVector.x - doorNowVector.x);
        productDoor.transform.localPosition = new Vector3(0, 0, Mathf.Clamp(distance, 0, 1.236f));

        if (productDoor.transform.localPosition.z >= 1.23f)
            productDoorOpen = true;
        else if (productDoor.transform.localPosition.z <= 0.05f)
            productDoorOpen = false;
    }

    public void ProductDoorLeftOff()
    {
        //productDoorLeftHold = false;
    }

    public void ProductDoorRightOff()
    {
        productDoorRightHold = false;
    }



    bool liftLeftHold = false;
    bool liftRightHold = false;
    public void LiftLeftHold()
    {
        if (Mathf.Abs(liftLeftTrigger.detector.transform.position.y - liftLeftTrigger.transform.position.y) > 0.1f)
        {
            liftLeftTrigger.detector.LoseTarget();
            liftLeftHold = false;
            LiftLeftOff();
        }
        else
        {
            if (liftRightHold)
            {
                liftLeftHold = true;
                LiftHold();
            }
        }
    }
    public void LiftRightHold()
    {
        if (Mathf.Abs(liftRightTrigger.detector.transform.position.y - liftRightTrigger.transform.position.y) > 0.1f)
        {
            liftRightTrigger.detector.LoseTarget();
            liftRightHold = false;
            LiftRightOff();
        }
        else
        {
            liftRightHold = true;
        }
    }
    bool liftFixed = false;
    bool carryingSupplyContainer;
    bool carryingProductContainer;
    public void LiftHold()
    {
        if (Vector3.Distance(liftLeftTrigger.detector.transform.position, liftRightTrigger.detector.transform.position) < 0.4f)
        {
            if (!liftFixed)
            {
                float x = (liftLeftTrigger.detector.transform.position.x + liftRightTrigger.detector.transform.position.x) / 2;
                float z = (liftLeftTrigger.detector.transform.position.z + liftRightTrigger.detector.transform.position.z) / 2;
                lift.transform.position = new Vector3(x, 0, z);

                Vector3 v3 = liftLeftTrigger.detector.transform.position - liftRightTrigger.detector.transform.position;
                v3 = new Vector3(v3.x, 0, v3.z);
                float angle = Vector3.Angle(v3, Vector3.right);
                if (v3.z < 0)
                    angle = 360 - angle;

                lift.eulerAngles = new Vector3(0, 180 - angle, 0);
                lift.transform.position += lift.transform.forward * 0.629f;
            }

            if (!carryChangeCoolTime)
            {
                if (Vector3.Distance(carryPoint[nowLiftLevel], lift.localPosition) < 0.5f)
                {
                    switch (nowLiftLevel)
                    {
                        case 0:
                            CarryChange0();
                            break;
                        case 1:
                            if (productDoorOpen)
                                CarryChange1();
                            break;
                        case 2:
                            if (mainDoor.FullOpened)
                                CarryChange2();
                            break;
                        case 3:
                            if (mainDoor.FullOpened)
                                CarryChange3();
                            break;
                    }
                    carryChangeCoolTime = true;
                }
            }
        }
        else
        {
            liftLeftTrigger.detector.LoseTarget();
            liftLeftHold = false;
            LiftLeftOff();

            liftRightTrigger.detector.LoseTarget();
            liftRightHold = false;
            LiftRightOff();
        }
    }

    void CarryChange0()
    {
        if (carryingSupplyContainer)
        {
            supplyContainer.parent = supplyStation;
            supplyContainer.localPosition = new Vector3(0, 0.6277f, 0);
            supplyContainer.localEulerAngles = Vector3.zero;
        }
        else
        {
            supplyContainer.parent = liftHand;
            supplyContainer.localPosition = Vector3.zero;
            supplyContainer.localEulerAngles = Vector3.zero;
        }
        carryingSupplyContainer = !carryingSupplyContainer;
    }
    void CarryChange2()
    {
        if (carryingSupplyContainer)
        {
            supplyContainer.parent = PBF_Main;
            supplyContainer.localPosition = new Vector3(-0.613f, 1.931f, 0);
            supplyContainer.localEulerAngles = Vector3.zero;
        }
        else
        {
            supplyContainer.parent = liftHand;
            supplyContainer.localPosition = Vector3.zero;
            supplyContainer.localEulerAngles = Vector3.zero;
        }
        carryingSupplyContainer = !carryingSupplyContainer;
    }
    void CarryChange1()
    {
        if (carryingProductContainer)
        {
            productContainer.parent = productDoor;
            productContainer.localPosition = new Vector3(0, 0.9655f, 0.195f);
            productContainer.localEulerAngles = Vector3.zero;
        }
        else
        {
            productContainer.parent = liftHand;
            productContainer.localPosition = new Vector3(0, 0, 0.152f);
            productContainer.localEulerAngles = new Vector3(0, 90, 0);
        }
        carryingProductContainer = !carryingProductContainer;
    }
    void CarryChange3()
    {
        if (carryingProductContainer)
        {
            productContainer.parent = PBF_Main;
            productContainer.localPosition = new Vector3(-0.613f, 1.239f, 0.074f);
            productContainer.localEulerAngles = new Vector3(0, 90, 0);
        }
        else
        {
            productContainer.parent = liftHand;
            productContainer.localPosition = new Vector3(0, 0, 0.152f);
            productContainer.localEulerAngles = new Vector3(0, 90, 0);
        }
        carryingProductContainer = !carryingProductContainer;
    }

    bool carryChangeCoolTime = false;

    public void LiftLeftOff()
    {
        liftLeftHold = false;
    }

    public void LiftRightOff()
    {
        liftRightHold = false;
    }

    //1.236f

    float[] LeverLevel = { 0.6277f, 1.114f, 1.942f, 1.2435f };//supply,product,m1,m2 
    Vector3[] carryPoint = {
        new Vector3(-0.468f, 0, 2.547f),
        new Vector3(2.941f, 0, 1.194f),
        new Vector3(0.126f, 0, -0.116f),
        new Vector3(0.126f, 0, -0.116f)};
    int nowLiftLevel = -1;
    int targetLiftLevel = 0;

    public void LiftLeverClick()
    {
        if (!liftMoveDoing)
            StartCoroutine(LiftHandMoving());
    }
    bool liftMoveDoing = false;
    IEnumerator LiftHandMoving()
    {

        liftMoveDoing = true;

        float delta = 0.001f;
        if (liftHand.localPosition.y > LeverLevel[targetLiftLevel])
        {
            while (liftHand.localPosition.y > LeverLevel[targetLiftLevel])
            {
                liftHand.localPosition -= new Vector3(0, delta, 0);
                yield return null;
            }
        }
        else
        {
            while (liftHand.localPosition.y < LeverLevel[targetLiftLevel])
            {
                liftHand.localPosition += new Vector3(0, delta, 0);
                yield return null;
            }
        }

        nowLiftLevel = targetLiftLevel;

        liftHand.localPosition = new Vector3(liftHand.localPosition.x, LeverLevel[targetLiftLevel], liftHand.localPosition.z);

        liftMoveDoing = false;
    }

    public override void Remove()
    {
        Destroy(gameObject);
    }
}