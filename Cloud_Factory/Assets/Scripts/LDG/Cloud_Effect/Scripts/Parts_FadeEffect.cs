using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parts_FadeEffect : MonoBehaviour
{
    // ȸ����Ű�� �κ�
    public GameObject[] Parts = new GameObject[3];

    // �̵���Ű�� �κ�
    public GameObject[] PartBodys = new GameObject[3];

    // ������ ���󰡴� ���⺤��
    private Vector3[] Move_Dir_Parts = new Vector3[3];

    // ����Ʈ�� �����̰� ȸ���ϰ�, ���̵���,�ƿ� �Ǵ� �ð��� �����ϴ� ����
    private float[] fadeCount_List = new float[3];
    private float StartEffectTime;
    private float DestroyTime;

    void Start()
    {
        // �������� ���󰡴� ��ġ �ʱ�ȭ
        Move_Dir_Parts[0] = new Vector3(-2.5f, -2.0f, 0.0f);
        Move_Dir_Parts[1] = new Vector3(1.5f, -3.0f, 0.0f);
        Move_Dir_Parts[2] = new Vector3(3.0f, 2.5f, 0.0f);
        // ���� ���̵��� �ڷ�ƾ ����
        StartCoroutine(FadeInPart());
        StartCoroutine(FadeInPart2());
        StartCoroutine(FadeInPart3());
    }

    void Update()
    {
        // �����̵����� �ð�
        StartEffectTime += Time.deltaTime;
        if (StartEffectTime > 0.02f) { MoveNRot_PartBodys(Parts[0], PartBodys[0], Move_Dir_Parts[0]); }
        if (StartEffectTime > 0.03f) { MoveNRot_PartBodys(Parts[1], PartBodys[1], Move_Dir_Parts[1]); }
        if (StartEffectTime > 0.04f) { MoveNRot_PartBodys(Parts[2], PartBodys[2], Move_Dir_Parts[2]); }

        if (fadeCount_List[0] >= 0.99f)
        {
            StopCoroutine(FadeInPart());
            StartCoroutine(FadeOutPart());
        }
        if (fadeCount_List[1] >= 0.99f)
        {
            StopCoroutine(FadeInPart2());
            StartCoroutine(FadeOutPart2());
        }
        if (fadeCount_List[2] >= 0.99f)
        {
            StopCoroutine(FadeInPart3());
            StartCoroutine(FadeOutPart3());
        }

        if (DestroyTime >= 4.0f)
        {
            Destroy(gameObject);
        }
    }

    // ������ �̵���Ű�� ȸ����Ű�� �Լ�
    void MoveNRot_PartBodys(GameObject part,GameObject partbody, Vector3 MoveDir)
    {
        part.transform.localEulerAngles += new Vector3(0.0f, 0.0f, 1.0f);
        partbody.transform.Translate(MoveDir * (Time.deltaTime / 10));
    }

    // �� �����鸶�� ���̵���,�ƿ� ��Ű�� �ڷ�ƾ�Լ�
    IEnumerator FadeInPart()
    {
        fadeCount_List[0] = 0.0f;
        while (fadeCount_List[0] <= 1.0f)
        {
            fadeCount_List[0] += 0.02f;
            yield return new WaitForSecondsRealtime(0.02f);
            Parts[0].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, fadeCount_List[0]);
        }
    }
    IEnumerator FadeInPart2()
    {
        fadeCount_List[1] = 0.0f;
        while (fadeCount_List[1] <= 1.0f)
        {
            fadeCount_List[1] += 0.02f;
            yield return new WaitForSecondsRealtime(0.03f);
            Parts[1].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, fadeCount_List[1]);
        }
    }
    IEnumerator FadeInPart3()
    {
        fadeCount_List[2] = 0.0f;
        while (fadeCount_List[2] <= 1.0f)
        {
            fadeCount_List[2] += 0.02f;
            yield return new WaitForSecondsRealtime(0.04f);
            Parts[2].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, fadeCount_List[2]);
        }
    }

    IEnumerator FadeOutPart()
    {
        fadeCount_List[0] = 1.0f;
        while (fadeCount_List[0] >= 0.0f)
        {
            fadeCount_List[0] -= 0.02f;
            yield return new WaitForSecondsRealtime(0.02f);
            Parts[0].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, fadeCount_List[0]);
        }
    }
    IEnumerator FadeOutPart2()
    {
        fadeCount_List[1] = 1.0f;
        while (fadeCount_List[1] >= 0.0f)
        {
            fadeCount_List[1] -= 0.02f;
            yield return new WaitForSecondsRealtime(0.03f);
            Parts[1].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, fadeCount_List[1]);
        }
    }
    IEnumerator FadeOutPart3()
    {
        fadeCount_List[2] = 1.0f;
        while (fadeCount_List[2] >= 0.0f)
        {
            fadeCount_List[2] -= 0.02f;
            yield return new WaitForSecondsRealtime(0.04f);
            Parts[2].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, fadeCount_List[2]);
        }
    }
}

