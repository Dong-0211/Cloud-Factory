using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Parts_fly : MonoBehaviour
{
    Rigidbody2D Rigidbody;
    public float fly_speed_x; // x�������� ������ ���󰡴� �ӵ�
    public float fly_speed_y; // y�������� ������ ���󰡴� �ӵ�


    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Rigidbody.velocity = new Vector2(transform.position.x * fly_speed_x, transform.position.y * fly_speed_y);
    }

    public void Change_speed_1()  // ���������� �����Ǵ� ���������󰡴¼ӵ�
    {
        fly_speed_x = -0.5f;
        fly_speed_y = -5.0f;
    }

    public void Change_speed_2()  // ������������ �����Ǵ� ���������󰡴¼ӵ�
    {
        fly_speed_x = 0.5f;
        fly_speed_y = -5.0f;
    }

    public void Change_speed_3()    // ���ʹؿ��� �����Ǵ� ������ ���󰡴� �ӵ�
    {
        fly_speed_x = -0.5f;
        fly_speed_y = -5.0f;
    }

    public void Change_speed_4()    // �����ʹؿ��� �����Ǵ� ������ ���󰡴� �ӵ�
    {
        fly_speed_x = 0.5f;
        fly_speed_y = -5.0f;
    }

}
