using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Assembly : TestClass
{
    public Outline ol_SideMirror1;
    public Outline ol_SideMirror2;
    public Outline ol_Seat1;
    public Outline ol_Seat2;
    public Outline ol_Seat3;
    public Outline ol_Seat4;
    public Outline ol_B_Pillar1;
    public Outline ol_B_Pillar2;
    public Outline ol_B_Pillar3;

    public AssemblePart ap_SideMirror1;
    public AssemblePart ap_Seat1;
    public AssemblePart ap_Seat2;
    public AssemblePart ap_Seat3;
    public AssemblePart ap_B_Pillar1;
    public AssemblePart ap_B_Pillar2;

    public Transform[] samples;

    public override IEnumerator testContent()
    {
        float startTime = Time.time;

        ol_SideMirror1.OutlineWidth = 3;
        ol_SideMirror2.OutlineWidth = 3;

        yield return TestStep("사이드 미러 조립 - 1", () => ap_SideMirror1.isCombine);

        testData.time1 = Time.time - startTime;
        startTime = Time.time;

        ol_SideMirror1.OutlineWidth = 0;
        ol_SideMirror2.OutlineWidth = 0;

        ol_Seat1.OutlineWidth = 3;
        ol_Seat2.OutlineWidth = 3;

        yield return TestStep("시트 조립 - 1", () => ap_Seat1.isCombine);

        ol_Seat2.OutlineWidth = 0;
        ol_Seat3.OutlineWidth = 3;

        yield return TestStep("시트 조립 - 2", () => ap_Seat2.isCombine);

        ol_Seat3.OutlineWidth = 0;
        ol_Seat4.OutlineWidth = 3;

        yield return TestStep("시트 조립 - 3", () => ap_Seat3.isCombine);

        testData.time2 = Time.time - startTime;
        startTime = Time.time;

        ol_Seat1.OutlineWidth = 0;
        ol_Seat4.OutlineWidth = 0;

        ol_B_Pillar1.OutlineWidth = 3;
        ol_B_Pillar2.OutlineWidth = 3;

        yield return TestStep("B 필러 조립 - 1", () => ap_B_Pillar1.isCombine);

        ol_B_Pillar1.OutlineWidth = 0;
        ol_B_Pillar3.OutlineWidth = 3;

        yield return TestStep("B 필러 조립 - 2", () => ap_B_Pillar2.isCombine);

        testData.time3 = Time.time - startTime;

        ol_B_Pillar2.OutlineWidth = 0;
        ol_B_Pillar3.OutlineWidth = 0;

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