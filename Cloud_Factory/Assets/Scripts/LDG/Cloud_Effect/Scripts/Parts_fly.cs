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

    void Update()
    {

    }


    //public void Fly()
    //{
    //    rigidbody = GetComponent<Rigidbody2D>();
    //    rigidbody.velocity = transform.position * fly_speed;
    //}

    public void change_speed_1()  // ���������� �����Ǵ� ���������󰡴¼ӵ�
    {
        fly_speed = -1.0f;
    }

    public void change_speed_2()    // ������������ �����Ǵ� ������ ���󰡴¼ӵ�
    {
        fly_speed = 1.0f;
    }

    public void change_speed_3()    // ���ʹؿ��� �����Ǵ� ������ ���󰡴� �ӵ�
    {
        fly_speed = -2.5f;
    }

    public void change_speed_4()    // �����ʹؿ��� �����Ǵ� ������ ���󰡴� �ӵ�
    {
        fly_speed = 1.5f;
    }

}
