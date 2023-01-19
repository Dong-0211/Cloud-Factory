using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Parts_fly : MonoBehaviour
{
    Rigidbody2D rigidbody;
    public static float fly_speed; // ������ ���󰡴� �ӵ�

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = transform.position * fly_speed;
    }

    //void fly()
    //{
    //    rigidbody = GetComponent<Rigidbody2D>();
    //    rigidbody.velocity = transform.position * fly_speed;
    //}

    public void change_speed_1_2()  // �������� ������������ �����Ǵ� ���������󰡴¼ӵ�
    {
        fly_speed = 10.0f;
    }

    public void change_speed_3()    // ���ʹؿ��� �����Ǵ� ������ ���󰡴� �ӵ�
    {
        fly_speed = 25.0f;
    }

    public void change_speed_4()    // �����ʹؿ��� �����Ǵ� ������ ���󰡴� �ӵ�
    {
        fly_speed = 15.0f;
    }

}
