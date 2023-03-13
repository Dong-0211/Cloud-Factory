using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Parts_fly : MonoBehaviour
{
    // ������ 1����ġ�� ���ʹ� 3����ġ���� ������ ���������� �ӵ���������ϴ� ��ũ��Ʈ
    Rigidbody2D Rigidbody;            // ���������� rigidbody ������Ʈ

    private float fly_speed_2 = -5.0f;        // �������� �������� �̵��ӵ�
    private float fly_speed= 5.0f;          // �������� �ʱ��̵��ӵ�
    private float fly_angle = 45.0f;       // �������� �߻簢��(������ġ2,4)
    private float height;               // ���������� �ְ����

    public float fly_gravity = 9.8f;    // ���������� �޴� �߷°��ӵ�
    private float flying_time = 0f;     // ���������� �̵��� �ð�


    private Vector2 pos1 = new Vector2(0, 0);

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();

        float x = fly_speed_2 * Mathf.Cos(fly_angle * Mathf.Deg2Rad);
        float y = fly_speed * Mathf.Sin(fly_angle * Mathf.Deg2Rad);
        height = y * y * y / (2f * fly_gravity);

        // ���������� �޴� �߷°��ӵ� ����
        Rigidbody.gravityScale = fly_gravity / Physics2D.gravity.magnitude;
    }

    void FixedUpdate()
    {
        flying_time += Time.fixedDeltaTime;

        // ���������� �̵��ϴµ��� �߷¿����� y������ �̵�(1,3,������)
        float x2 = fly_speed_2 * Mathf.Cos(fly_angle * Mathf.Deg2Rad);
        float y2 = height - (0.5f * fly_gravity * flying_time * flying_time);
        // ���������� ���ο���ġ ���
        Vector2 pos2 = Rigidbody.position + new Vector2(x2, y2) * Time.fixedDeltaTime;
        Rigidbody.MovePosition(pos2);
    }
}
