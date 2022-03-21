using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRUI : MonoSingleton<VRUI>
{
    public GameObject body;
    public RectTransform BG; // 0~550
    public Text content;
    public Text Indicator;

    public Transform cameraT;

    public static IEnumerator ShowMessage(string message)
    {
        yield return Instance._ShowMessage(message);
    }
    IEnumerator _ShowMessage(string message)
    {
        transform.eulerAngles = new Vector3(0, cameraT.eulerAngles.y, 0);
        transform.position = cameraT.position;
        transform.position += transform.forward * 1.2f;
        content.text = message;
        body.SetActive(true);

        for (int i = 0; i < 30; i++)
        {
            BG.sizeDelta = new Vector2(1100, Mathf.Sin(i / 60f * 3.14f) * 550);
            yield return null;
        }
        BG.sizeDelta = new Vector2(1100, 550);

        yield return new WaitWhile(() => body.activeSelf);
    }


    public void HideMessage()
    {
        StartCoroutine(HidingMessage());
    }
    IEnumerator HidingMessage()
    {

        for (int i = 0; i < 30; i++)
        {
            BG.sizeDelta = new Vector2(1100, (1 - Mathf.Sin(i / 60f * 3.14f)) * 550);
            yield return null;
        }
        BG.sizeDelta = new Vector2(1100, 0);

        body.SetActive(false);
    }

    public void ChangeIndicate(string str)
    {
        Indicator.text = str;
    }
}
