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

    void Start()
    {
        Target_pos[0] = new Vector3(-1.6f, -0.3f, 0.0f);
    }

    void Update()
    {
        Diagonal_Move_Part1();
    }

    void Diagonal_Move_Part1()
    {
        PartBodys[0].transform.localPosition = Vector3.Lerp(PartBodys[0].transform.localPosition, Target_pos[0], speed * Time.deltaTime); 
    }
}
