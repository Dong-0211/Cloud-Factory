using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Parts_fly : MonoBehaviour
{
    Rigidbody2D Rigidbody;
    public static float fly_speed; // ������ ���󰡴� �ӵ�

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Rigidbody.velocity = transform.position * fly_speed;
    }

    public void Change_speed_1()  // ���������� �����Ǵ� ���������󰡴¼ӵ�
    {
        fly_speed = -1.0f;
    }

    public void Change_speed_2()  // ������������ �����Ǵ� ���������󰡴¼ӵ�
    {
        fly_speed = 1.0f;
    }

    public void Change_speed_3()    // ���ʹؿ��� �����Ǵ� ������ ���󰡴� �ӵ�
    {
        fly_speed = -2.5f;
    }

    public void Change_speed_4()    // �����ʹؿ��� �����Ǵ� ������ ���󰡴� �ӵ�
    {
        fly_speed = 1.5f;
    }

}
