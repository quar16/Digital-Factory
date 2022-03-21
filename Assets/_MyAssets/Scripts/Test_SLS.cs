using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_SLS : TestClass
{
    public VR_Trigger liftLeverTrg;
    public VR_Trigger liftLeftTrg;
    public VR_Trigger liftRightTrg;

    public Door mainLeftDoor;
    public Door mainRightDoor;
    public Door productLeftDoor;
    public Door productRightDoor;
    public Door productUpDoor;

    public Transform SLS_Main;

    public Transform lift;
    public Transform liftHand;
    public Transform liftLever;
    public Transform productContainer;

    public GameObject supply;

    public Transform liftContainerPos;
    public Transform mainContainerPos;
    public Transform stationContainerPos;

    public Transform liftMovePos;

    public override IEnumerator testContent()
    {
        float startTime = Time.time;

        yield return TestStep("메인 도어 열기", () => mainLeftDoor.isOpen && mainRightDoor.isOpen, mainLeftDoor.trigger, mainRightDoor.trigger);

        yield return TestStep("컨테이너 스테이션 상부 도어 열기", () => productUpDoor.isOpen, productUpDoor.trigger);

        yield return TestStep("컨테이너 스테이션 하부 도어 열기", () => productLeftDoor.isOpen && productRightDoor.isOpen, productLeftDoor.trigger, productRightDoor.trigger);

        yield return TestStep("리프트 높이 낮추기", () => leverIndex == 0, liftLeverTrg);

        yield return TestStep("리프트 이동", () => liftLeftHold && liftRightHold, liftLeftTrg, liftRightTrg);

        carryChangeCoolTime = false;
        yield return TestStep("컨테이너 들기", () => containerPosIndex == 1, stationContainerPos);

        yield return TestStep("리프트 이동 및 리프트 높이 조절", () => Vector3.Distance(lift.position, liftMovePos.position) < 0.1f && leverIndex == 1, liftLeverTrg, liftMovePos);

        carryChangeCoolTime = false;
        yield return TestStep("컨테이너 삽입", () => containerPosIndex == 2, mainContainerPos);

        yield return TestStep("리프트 이동", () => Vector3.Distance(lift.position, liftMovePos.position) < 0.1f, liftMovePos);

        yield return TestStep("메인 도어 닫기", () => !mainLeftDoor.isOpen && !mainRightDoor.isOpen, mainLeftDoor.trigger, mainRightDoor.trigger);

        testData.time1 = Time.time - startTime;
        
        yield return SceneLoader.Instance.SceneChangeEffectShowing(true);
        yield return new WaitForSeconds(1);
        yield return SceneLoader.Instance.SceneChangeEffectShowing(false);
        
        supply.SetActive(true);

        yield return TestStep("제작 완료\n메인 도어 열기", () => mainLeftDoor.isOpen && mainRightDoor.isOpen, mainLeftDoor.trigger, mainRightDoor.trigger);

        carryChangeCoolTime = false;
        yield return TestStep("컨테이너 꺼내기", () => containerPosIndex == 1, mainContainerPos);

        yield return TestStep("리프트 이동", () => Vector3.Distance(lift.position, liftMovePos.position) < 0.1f, liftMovePos);

        yield return TestStep("리프트 높이 낮추기", () => leverIndex == 0, liftLeverTrg);

        carryChangeCoolTime = false;
        yield return TestStep("컨테이너 내리기", () => containerPosIndex == 0, stationContainerPos);

        yield return TestStep("리프트 이동", () => Vector3.Distance(lift.position, liftMovePos.position) < 0.1f, liftMovePos);

        yield return TestStep("컨테이너 스테이션 하부 도어 닫기", () => !productLeftDoor.isOpen && !productRightDoor.isOpen, productLeftDoor.trigger, productRightDoor.trigger);

        yield return TestStep("컨테이너 스테이션 상부 도어 닫기", () => !productUpDoor.isOpen, productUpDoor.trigger);

        yield return TestStep("메인 도어 닫기", () => !mainLeftDoor.isOpen && !mainRightDoor.isOpen, mainLeftDoor.trigger, mainRightDoor.trigger);

        testData.time2 = Time.time - startTime;

        VRUI.Instance.ChangeIndicate("모든 공정 완료\n다음 단계로 이동");
        yield return new WaitForSeconds(3);
    }

    bool liftLeftHold = false;
    bool liftRightHold = false;
    public void LiftLeftHold()
    {
        if (Mathf.Abs(liftLeftTrg.detector.transform.position.y - liftLeftTrg.transform.position.y) > 0.1f)
        {
            liftLeftTrg.detector.LoseTarget();
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
        if (Mathf.Abs(liftRightTrg.detector.transform.position.y - liftRightTrg.transform.position.y) > 0.1f)
        {
            liftRightTrg.detector.LoseTarget();
            liftRightHold = false;
            LiftRightOff();
        }
        else
        {
            liftRightHold = true;
        }
    }
    bool liftFixed = false;

    int containerPosIndex = 0;//0 station, 1 lift, 2 main

    public void LiftHold()
    {
        float distance = Vector3.Distance(liftLeftTrg.detector.transform.position, liftRightTrg.detector.transform.position);
        if (distance < 0.6f)//(0.4f < distance && distance < 0.6f)
        {
            if (!liftFixed)
            {
                float x = (liftLeftTrg.detector.transform.position.x + liftRightTrg.detector.transform.position.x) / 2;
                float z = (liftLeftTrg.detector.transform.position.z + liftRightTrg.detector.transform.position.z) / 2;
                lift.transform.position = new Vector3(x, 0, z);

                Vector3 v3 = liftLeftTrg.detector.transform.position - liftRightTrg.detector.transform.position;
                v3 = new Vector3(v3.x, 0, v3.z);
                float angle = Vector3.Angle(v3, Vector3.right);
                if (v3.z < 0)
                    angle = 360 - angle;

                lift.eulerAngles = new Vector3(0, 180 - angle, 0);
                lift.transform.position += lift.transform.forward * 0.456f;
            }

            if (!carryChangeCoolTime)
            {
                if (leverIndex == 0 && Vector3.Distance(liftContainerPos.position, stationContainerPos.position) < 0.1f)
                {
                    CarryChange0();
                    carryChangeCoolTime = true;
                }
                else if (leverIndex == 1 && Vector3.Distance(liftContainerPos.position, mainContainerPos.position) < 0.1f)
                {
                    CarryChange1();
                    carryChangeCoolTime = true;
                }
            }
        }
        else
        {
            liftLeftTrg.detector.LoseTarget();
            liftLeftHold = false;
            LiftLeftOff();

            liftRightTrg.detector.LoseTarget();
            liftRightHold = false;
            LiftRightOff();
        }
    }

    void CarryChange0()
    {
        if (containerPosIndex == 1)
            productContainer.parent = stationContainerPos;
        else
            productContainer.parent = liftContainerPos;

        productContainer.localPosition = Vector3.zero;
        productContainer.localEulerAngles = Vector3.zero;

        containerPosIndex = 1 - containerPosIndex;
    }
    void CarryChange1()
    {
        if (containerPosIndex == 1)
            productContainer.parent = mainContainerPos;
        else
            productContainer.parent = liftContainerPos;

        productContainer.localPosition = Vector3.zero;
        productContainer.localEulerAngles = Vector3.zero;

        containerPosIndex = 3 - containerPosIndex;
    }
    bool carryChangeCoolTime = false;


    public void LiftLeftOff()
    {
        //liftLeftHold = false;
    }

    public void LiftRightOff()
    {
        //liftRightHold = false;
    }

    Vector3 leverNowPos;
    Vector3 leverLastPos;
    int leverIndex = -1;
    public void LiftLeverClick()
    {
        leverNowPos = liftLeverTrg.detector.transform.position;
        leverLastPos = liftLeverTrg.detector.transform.position;
    }
    public void LiftLeverHold()
    {
        leverNowPos = liftLeverTrg.detector.transform.position;

        if (Vector3.Distance(leverNowPos, liftLeverTrg.transform.position) < 0.1f)
        {
            float angle = Vector3.SignedAngle(leverNowPos - liftLever.position, leverLastPos - liftLever.position, liftLever.forward);

            liftLever.localEulerAngles -= new Vector3(0, 0, angle);

            float height = Mathf.Clamp(liftHand.localPosition.y + angle * 0.001f, 0.218f, 0.390f);
            liftHand.localPosition = new Vector3(0, height, 0);
            if (height < 0.221f)
                leverIndex = 0;
            else if (0.385f < height)
                leverIndex = 1;
            else
                leverIndex = -1;
        }

        leverLastPos = liftLeverTrg.detector.transform.position;
    }
    public void LiftLeverOff()
    {

    }

    public override void Remove()
    {
        Destroy(gameObject);
    }
}