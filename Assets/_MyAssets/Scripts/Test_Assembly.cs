using EPOOutline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Assembly : TestClass
{
    public Outlinable ol_SideMirror1;
    public Outlinable ol_SideMirror2;
    public Outlinable ol_Seat1;
    public Outlinable ol_Seat2;
    public Outlinable ol_Seat3;
    public Outlinable ol_Seat4;
    public Outlinable ol_B_Pillar1;
    public Outlinable ol_B_Pillar2;
    public Outlinable ol_B_Pillar3;

    public AssemblePart ap_SideMirror1;
    public AssemblePart ap_Seat1;
    public AssemblePart ap_Seat2;
    public AssemblePart ap_Seat3;
    public AssemblePart ap_B_Pillar1;
    public AssemblePart ap_B_Pillar2;

    public Transform[] samples;

    public GameObject desk1;
    public GameObject desk2;
    public GameObject desk3;

    public override IEnumerator testContent()
    {
        desk1.SetActive(true);
        desk2.SetActive(false);
        desk3.SetActive(false);

        float startTime = Time.time;

        ol_SideMirror1.enabled = true;
        ol_SideMirror2.enabled = true;

        yield return TestStep("사이드 미러 조립 - 1", () => ap_SideMirror1.isCombine);

        testData.time1 = Time.time - startTime;
        startTime = Time.time;

        ol_SideMirror1.enabled = false;
        ol_SideMirror2.enabled = false;

        yield return SceneLoader.Instance.SceneChangeEffectShowing(true);
        desk1.SetActive(false);
        desk2.SetActive(true);
        desk3.SetActive(false);
        yield return SceneLoader.Instance.SceneChangeEffectShowing(false);

        ol_Seat1.enabled = true;
        ol_Seat2.enabled = true;

        yield return TestStep("시트 조립 - 1", () => ap_Seat1.isCombine);

        ol_Seat2.enabled = false;
        ol_Seat3.enabled = true;

        yield return TestStep("시트 조립 - 2", () => ap_Seat2.isCombine);

        ol_Seat3.enabled = false;
        ol_Seat4.enabled = true;

        yield return TestStep("시트 조립 - 3", () => ap_Seat3.isCombine);

        testData.time2 = Time.time - startTime;
        startTime = Time.time;

        ol_Seat1.enabled = false;
        ol_Seat4.enabled = false;

        yield return SceneLoader.Instance.SceneChangeEffectShowing(true);
        desk1.SetActive(false);
        desk2.SetActive(false);
        desk3.SetActive(true);
        yield return SceneLoader.Instance.SceneChangeEffectShowing(false);
        
        ol_B_Pillar1.enabled = true;
        ol_B_Pillar2.enabled = true;

        yield return TestStep("B 필러 조립 - 1", () => ap_B_Pillar1.isCombine);

        ol_B_Pillar1.enabled = false;
        ol_B_Pillar3.enabled = true;

        yield return TestStep("B 필러 조립 - 2", () => ap_B_Pillar2.isCombine);

        testData.time3 = Time.time - startTime;

        ol_B_Pillar2.enabled = false;
        ol_B_Pillar3.enabled = false;

        VRUI.Instance.ChangeIndicate("모든 공정 완료");
        yield return new WaitForSeconds(3);
    }


    public override void Remove()
    {
        Destroy(ol_SideMirror1.gameObject);
        Destroy(ol_SideMirror2.gameObject);
        Destroy(ol_Seat1.gameObject);
        Destroy(ol_Seat2.gameObject);
        Destroy(ol_Seat3.gameObject);
        Destroy(ol_Seat4.gameObject);
        Destroy(ol_B_Pillar1.gameObject);
        Destroy(ol_B_Pillar2.gameObject);
        Destroy(ol_B_Pillar3.gameObject);
        Destroy(gameObject);
    }

    private void Update()
    {
        foreach (Transform sample in samples)
        {
            sample.Rotate(new Vector3(0, 0.1f, 0));
        }
    }
}