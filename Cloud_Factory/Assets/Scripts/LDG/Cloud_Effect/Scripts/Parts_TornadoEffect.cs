using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parts_TornadoEffect : MonoBehaviour
{
    // ���̵�ƿ���Ű�� �κ�
    public GameObject[] Parts = new GameObject[3];
    // �̵���Ű�� �κ�
    public GameObject[] PartBodys = new GameObject[3];

    // ����1�� �����ӹ��⺤��
    private Vector3[] MoveDir_Part1 = new Vector3[2];

    // ����2�� �����ӹ��⺤��
    private Vector3[] MoveDir_Part2 = new Vector3[2];

    // ����3�� �����ӹ��⺤��
    private Vector3[] MoveDir_Part3 = new Vector3[2];

    // �������� �����ӹ����ϴ� ��ȯ�ð�
    private float[] ChangeDir_Times = new float[3];

    // �����Ӻ�ȯ�Ҷ��� ����üũ�ϴ� bool����
    private bool[] ChangeDir_Check = new bool[3];

    // ���̵�ƿ�ȿ���� ���¸� ��ȭ�ϱ� ���� �ð�����
    private float[] FadeOut_StartTime = new float[3];

    // ���̵�ƿ����� ���� �����ϴ� ����
    private float[] transparency = new float[3];

    // 1,2,3������ ������� ������ �ð�����
    private float StartEffect;

    private float speed = 0.5f;
    private float frequency = 0.1f;
    private float amplitude = 0.1f;

    void Start()
    {
        MoveDir_Part1[0] = new Vector3(-1.2f, 1.0f, 0f);    // ����1�� ���ʹ���
        MoveDir_Part1[1] = new Vector3(1.2f, 1.0f, 0f);     // ����1�� �����ʹ���
        MoveDir_Part2[0] = new Vector3(-1.2f, 1.1f, 0f);    // ����2�� ���ʹ���
        MoveDir_Part2[1] = new Vector3(1.2f, 1.1f, 0f);     // ����2�� �����ʹ���
        MoveDir_Part3[0] = new Vector3(-1.5f, 1.0f, 0f);    // ����3�� ���ʹ���
        MoveDir_Part3[1] = new Vector3(1.5f, 1.0f, 0f);     // ����3�� �����ʹ���

        for (int i = 0; i < 3; i++) { ChangeDir_Times[i] = 0.0f; }

        // true���� ����, false���� ������
        ChangeDir_Check[0] = true;
        ChangeDir_Check[1] = true;
        ChangeDir_Check[2] = false;

        for(int i = 0; i < 3; i++) { FadeOut_StartTime[i] = 0.0f; }
        for(int i = 0; i < 3; i++) { transparency[i] = 1.0f; }

        StartEffect = 0.0f;
    }

    void Update()
    {
        StartEffect += Time.deltaTime;        
        if(StartEffect > 0.1f) { ChangeDir_Part1(ChangeDir_Check[0]); }
        if(StartEffect > 0.3f) { ChangeDir_Part2(ChangeDir_Check[1]); }
        if(StartEffect > 0.7f) { ChangeDir_Part3(ChangeDir_Check[2]); }
    }

    //����1�� ���������� �����̰��ϴ� �Լ�
    void MovingPartBody1_LR(Vector3 dir1)
    {
        ChangeDir_Times[0] += Time.deltaTime;
        FadeOut_StartTime[0] += Time.deltaTime;
        PartBodys[0].transform.Translate(dir1 * (Time.deltaTime / 2));
        if (ChangeDir_Times[0] >= 0.5f)
        {
            ChangeDir_Check[0] = !(ChangeDir_Check[0]);
            ChangeDir_Times[0] = 0.0f;
            ChangeDir_Part1(ChangeDir_Check[0]);
        }
        if(FadeOut_StartTime[0] >= 1.5f)
        {
            transparency[0] -= 0.02f;
            Parts[0].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, transparency[0]);
        }
    }

    // ����2�� ���������� �����̰��ϴ� �Լ�
    void MovingPartBody2_LR(Vector3 dir2)
    {
        ChangeDir_Times[1] += Time.deltaTime;
        FadeOut_StartTime[1] += Time.deltaTime;
        PartBodys[1].transform.Translate(dir2 * (Time.deltaTime / 2));
        if (ChangeDir_Times[1] >= 0.5f)
        {
            ChangeDir_Check[1] = !(ChangeDir_Check[1]);
            ChangeDir_Times[1] = 0.0f;
            ChangeDir_Part2(ChangeDir_Check[1]);
        }
        if (FadeOut_StartTime[1] >= 1.5f)
        {
            transparency[1] -= 0.02f;
            Parts[1].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, transparency[1]);
        }
    }

    // ����3 �� ���������� �����̰� �ϴ� �Լ�
    void MovingPartBody3_LR(Vector3 dir3)
    {
        ChangeDir_Times[2] += Time.deltaTime;
        FadeOut_StartTime[2] += Time.deltaTime;
        PartBodys[2].transform.Translate(dir3 * (Time.deltaTime / 1.5f));
        if (ChangeDir_Times[2] >= 0.5f)
        {
            ChangeDir_Check[2] = !(ChangeDir_Check[2]);
            ChangeDir_Times[2] = 0.0f;
            ChangeDir_Part3(ChangeDir_Check[2]);
        }
        if (FadeOut_StartTime[2] >= 1.5f)
        {
            transparency[2] -= 0.02f;
            Parts[2].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, transparency[2]);
        }
    }

    // ������ ������ȯ �Լ�
    void ChangeDir_Part1(bool checkDir1)
    {
        if (checkDir1) { MovingPartBody1_LR(MoveDir_Part1[0]); }
        else { MovingPartBody1_LR(MoveDir_Part1[1]); }
    }

    void ChangeDir_Part2(bool checkDir2)
    {
        if (checkDir2) { MovingPartBody2_LR(MoveDir_Part2[0]); }
        else { MovingPartBody2_LR(MoveDir_Part2[1]); }
    }

    void ChangeDir_Part3(bool checkDir3)
    {
        if (checkDir3) { MovingPartBody3_LR(MoveDir_Part3[0]); }
        else { MovingPartBody3_LR(MoveDir_Part3[1]); }
    }
}
