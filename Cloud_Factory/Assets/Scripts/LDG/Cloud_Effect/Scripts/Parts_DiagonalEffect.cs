using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parts_DiagonalEffect : MonoBehaviour
{
    // ���̵���,�ƿ��� scale�� ��ȭ��Ű�� �κ�
    public GameObject[] Parts = new GameObject[3];
    // �̵���Ű�� �κ�
    public GameObject[] PartBodys = new GameObject[3];

    private Vector3[] Target_pos = new Vector3[3];

    private float speed = 1.0f;

    private float StartEffectTime = 0.0f;

    void Start()
    {
        Target_pos[0] = new Vector3(-1.5f, -0.3f, 0.0f);
        Target_pos[1] = new Vector3(0.5f, -0.8f, 0.0f);
        Target_pos[2] = new Vector3(0.5f, -0.1f, 0.0f);

        Parts[0].GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 255.0f, 1.0f);
    }

    void Update()
    {
        StartEffectTime += Time.deltaTime;
        Diagonal_Move_Part1();
        if(StartEffectTime>= 1.0f) 
        {
            Parts[1].GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 255.0f, 1.0f);
            Diagonal_Move_Part2(); 
        }
        if(StartEffectTime>= 1.5f) 
        {
            Parts[2].GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 255.0f, 1.0f);
            Diagonal_Move_Part3(); 
        }
    }

    void Diagonal_Move_Part1()
    {
        PartBodys[0].transform.localPosition = Vector3.Lerp(PartBodys[0].transform.localPosition, Target_pos[0], 2 * speed * Time.deltaTime); 
        if(PartBodys[0].transform.localPosition.x <= -1.47f) { Parts[0].GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 255.0f, 0.0f); }
    }

    void Diagonal_Move_Part2()
    {
        PartBodys[1].transform.localPosition = Vector3.Lerp(PartBodys[1].transform.localPosition, Target_pos[1], 2 * speed * Time.deltaTime);
        if(PartBodys[1].transform.localPosition.x <= 0.53f) { Parts[1].GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 255.0f, 0.0f); }
    }

    void Diagonal_Move_Part3()
    {
        PartBodys[2].transform.localPosition = Vector3.Lerp(PartBodys[2].transform.localPosition, Target_pos[2], 2.5f * speed * Time.deltaTime);
        if (PartBodys[2].transform.localPosition.x <= 0.52f) { Parts[2].GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 255.0f, 0.0f); }
    }
}
