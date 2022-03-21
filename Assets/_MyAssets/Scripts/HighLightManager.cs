using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLightManager : MonoSingleton<HighLightManager>
{
    public GameObject highLightBall;
    public GameObject highLightBall2;
    public GameObject highLightArea;

    public Material ballMaterial;
    public Material areaMaterial;


    void Start()
    {
        ballColor = ballMaterial.GetColor("_TintColor");
        areaColor = areaMaterial.GetColor("_TintColor");
    }

    Color ballColor;
    Color areaColor;

    float alpha = 0;
    float delta = 0;

    void Update()
    {
        delta += Time.deltaTime * 2;
        alpha = Mathf.Abs(Mathf.Sin(delta)) * 0.5f;
        ballColor.a = alpha;
        areaColor.a = alpha;
        ballMaterial.SetColor("_TintColor", ballColor);
        areaMaterial.SetColor("_TintColor", areaColor);
    }


    public static void BallHighLightSet(VR_Trigger target)
    {
        Instance.highLightBall.transform.parent = target.transform;
        Instance.highLightBall.transform.localPosition = Vector3.zero;
        Instance.highLightBall.transform.localScale = Vector3.one * 1.1f;
    }
    public static void BallHighLightSet(VR_Trigger target1, VR_Trigger target2)
    {
        Instance.highLightBall.transform.parent = target1.transform;
        Instance.highLightBall.transform.localPosition = Vector3.zero;
        Instance.highLightBall.transform.localScale = Vector3.one * 1.1f;
        Instance.highLightBall2.transform.parent = target2.transform;
        Instance.highLightBall2.transform.localPosition = Vector3.zero;
        Instance.highLightBall2.transform.localScale = Vector3.one * 1.1f;
    }
    public static void AreaHighLightSet(Transform target)
    {
        Instance.highLightArea.transform.parent = target;
        Instance.highLightArea.transform.localPosition = Vector3.zero;
        Instance.highLightArea.transform.localEulerAngles = Vector3.zero;
        Instance.highLightArea.transform.localScale = Vector3.one;
    }
    public static void BallHighLightHide()
    {
        Instance.highLightBall.transform.parent = Instance.transform;
        Instance.highLightBall.transform.localPosition = Vector3.zero;
        Instance.highLightBall.transform.localScale = Vector3.one;
        Instance.highLightBall2.transform.parent = Instance.transform;
        Instance.highLightBall2.transform.localPosition = Vector3.zero;
        Instance.highLightBall2.transform.localScale = Vector3.one;
    }
    public static void AreaHighLightHide()
    {
        Instance.highLightArea.transform.parent = Instance.transform;
        Instance.highLightArea.transform.localPosition = Vector3.zero;
        Instance.highLightArea.transform.localEulerAngles = Vector3.zero;
        Instance.highLightArea.transform.localScale = Vector3.one;
    }
}
