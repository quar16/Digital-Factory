using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordReplayer : MonoBehaviour
{
    public Transform HMD;
    public Transform leftController;
    public Transform rightController;
    public VR_RecordDetector leftDetector;
    public VR_RecordDetector rightDetector;

    public Text leftControllerValues;
    public Text rightControllerValues;

    public Text countText;

    public TestClass[] Tests;
    public TestClass nowTest;

    List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
    bool dataChanged = false;
    string[] keys;

    Vector3 hmdPosition;
    Vector3 hmdRotation;

    Vector3 lPosition;
    Vector3 lRotation;
    bool lClick;
    Vector3 rPosition;
    Vector3 rRotation;
    bool rClick;
    bool lastLClick;
    bool lastRClick;
    public void SetRecord(string ID, TestName testName)
    {
        try
        {
            data = CSV.Read("_MyAssets/CSV/" + ID + "/" + testName.ToString());
        }
        catch
        {
            Debug.Log("테스트 ID : " + ID + " 에는 테스트 " + testName + " 에 대한 데이터가 없습니다.\nAssets/_MyAssets/CSV 폴더를 확인하세요.");
            return;
        }


        dataChanged = true;
        keys = new string[data[0].Keys.Count];
        data[0].Keys.CopyTo(keys, 0);

        if (nowTest != null)
        {
            HighLightManager.AreaHighLightHide();
            HighLightManager.BallHighLightHide();
            nowTest.Remove();
            nowTest = null;
        }
        nowTest = Instantiate(Tests[(int)testName]);

        StartCoroutine(nowTest.testContent());
    }
    public void Start()
    {
        StartCoroutine(Replaying());
    }

    public int frame = 0;

    IEnumerator Replaying()
    {
        yield return new WaitUntil(() => nowTest != null);

        frame = 0;
        while (true)
        {
            yield return new WaitForFixedUpdate();

            if (data == null)
                continue;

            if (dataChanged)
            {
                dataChanged = false;
                frame = 0;
            }
            if (frame + 1 >= data.Count)
            {
                yield return new WaitForSeconds(1);

                Reset();

                continue;
            }
            var dic = data[frame + 1];
            int i = 0;
            hmdPosition = new Vector3(ParseF(dic[keys[++i]]), ParseF(dic[keys[++i]]), ParseF(dic[keys[++i]]));
            hmdRotation = new Vector3(ParseF(dic[keys[++i]]), ParseF(dic[keys[++i]]), ParseF(dic[keys[++i]]));

            lPosition = new Vector3(ParseF(dic[keys[++i]]), ParseF(dic[keys[++i]]), ParseF(dic[keys[++i]]));
            lRotation = new Vector3(ParseF(dic[keys[++i]]), ParseF(dic[keys[++i]]), ParseF(dic[keys[++i]]));
            lClick = ParseB(dic[keys[++i]]);

            rPosition = new Vector3(ParseF(dic[keys[++i]]), ParseF(dic[keys[++i]]), ParseF(dic[keys[++i]]));
            rRotation = new Vector3(ParseF(dic[keys[++i]]), ParseF(dic[keys[++i]]), ParseF(dic[keys[++i]]));
            rClick = ParseB(dic[keys[++i]]);

            leftControllerValues.text = string.Format($"Left Controller" +
                $"\nPosition: {lPosition.ToString("F3")}" +
                $"\nRotation: {lRotation.ToString("F0")}" +
                $"\nClick: {lClick}");
            rightControllerValues.text = string.Format($"Right Controller" +
                $"\nPosition: {rPosition.ToString("F3")}" +
                $"\nRotation: {rRotation}" +
                $"\nClick: {rClick}");

            countText.text = frame + "/" + data.Count;

            HMD.position = hmdPosition + nowTest.pivot.position;
            HMD.eulerAngles = hmdRotation;
            leftController.position = lPosition + nowTest.pivot.position;
            leftController.eulerAngles = lRotation;
            rightController.position = rPosition + nowTest.pivot.position;
            rightController.eulerAngles = rRotation;

            switch (lastLClick, lClick)
            {
                case (false, false):
                    leftDetector.clickState = ClickState.NONE;
                    break;
                case (false, true):
                    leftDetector.clickState = ClickState.CLICK;
                    break;
                case (true, false):
                    leftDetector.clickState = ClickState.OFF;
                    break;
                case (true, true):
                    leftDetector.clickState = ClickState.STAY;
                    break;
            }
            switch (lastRClick, rClick)
            {
                case (false, false):
                    rightDetector.clickState = ClickState.NONE;
                    break;
                case (false, true):
                    rightDetector.clickState = ClickState.CLICK;
                    break;
                case (true, false):
                    rightDetector.clickState = ClickState.OFF;
                    break;
                case (true, true):
                    rightDetector.clickState = ClickState.STAY;
                    break;
            }
            frame++;

            lastLClick = lClick;
            lastRClick = rClick;

        }
    }

    public void Reset()
    {
        frame = 0;

        TestName testName = nowTest.testName;

        HighLightManager.AreaHighLightHide();
        HighLightManager.BallHighLightHide();
        nowTest.Remove();
        nowTest = Instantiate(Tests[(int)testName]);
        StartCoroutine(nowTest.testContent());
    }

    bool ParseB(object obj)
    {
        return bool.Parse(obj.ToString());
    }
    float ParseF(object obj)
    {
        return float.Parse(obj.ToString());
    }
}
