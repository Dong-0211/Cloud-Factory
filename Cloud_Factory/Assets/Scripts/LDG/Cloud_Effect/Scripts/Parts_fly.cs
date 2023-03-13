using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Parts_fly : MonoBehaviour
{
    Rigidbody2D Rigidbody;
    public float fly_speed_x; // x�������� ������ ���󰡴� �ӵ�
    public float fly_speed_y; // y�������� ������ ���󰡴� �ӵ�

    public Vector2 final_fly_speed = new Vector2(0f, 0f);

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Rigidbody.velocity = new Vector2(transform.position.x * fly_speed_x, transform.position.y * fly_speed_y);
    }


    // 4��и�(x>0,y<0)���� ���󰡴� ������ �ӵ��͹���
    public void Fourquadrant_change_speed1()    // ������
    {
        fly_speed_x = -1.0f;
        fly_speed_y = -9.0f;
    }
    public void Fourquadrant_change_speed2()    // ��������
    {
        fly_speed_x = 0.5f;
        fly_speed_y = -9.0f;
    }
    public void Fourquadrant_change_speed3()    // ���ʹ�
    {
        fly_speed_x = -1.0f;
        fly_speed_y = -5.0f;
    }
    public void Fourquadrant_change_speed4()    // �����ʹ�
    {
        fly_speed_x = 0.5f;
        fly_speed_y = -5.0f;
    }

    // 4��и鿡�� x�� 4���� 0�� ����������� ���󰡴� �����Ǽӵ��� ����
    // ������������ ������ ������ �ӵ��� ������ �̻��ؼ� ���θ����Լ�
    //public void Fourquadrant_2case_change_speed2()
    //{
    //    fly_speed_x = 0.8f;
    //    fly_speed_y = 18.0f;
    //}



    //------------------------------------------------------------------------------------------------------------------------


    // 1��и�(x>0,y>0)���� ���󰡴� ������ �ӵ��͹���
    public void Onequadrant_change_speed1()
    {
        fly_speed_x = -1.3f;
        fly_speed_y = 6.0f;
    }
    public void Onequadrant_change_speed2()
    {
        fly_speed_x = 1.3f;
        fly_speed_y = 6.0f;
    }
    public void Onequadrant_change_speed3()
    {
        fly_speed_x = -1.3f;
        fly_speed_y = 20.0f;
    }
    public void Onequadrant_change_speed4()
    {
        fly_speed_x = 1.3f;
        fly_speed_y = 20.0f;
    }

    // 1��и鿡�� x�� 0�� ����������� ���󰡴� ������ �ӵ��͹���
    public void Onequadrant_2case_change_speed1()
    {
        fly_speed_x = -2.0f;
        fly_speed_y = 5.0f;
    }
    public void Onequadrant_2case_change_speed2()
    {
        fly_speed_x = 1.5f;
        fly_speed_y = 5.0f;
    }
    public void Onequadrant_2case_change_speed3()
    {
        fly_speed_x = -3.0f;
        fly_speed_y = 5.0f;
    }
    public void Onequadrant_2case_change_speed4()
    {
        fly_speed_x = 1.5f;
        fly_speed_y = 5.0f;
    }

    // 1��и鿡�� x�� 4.5���� 3�� ����������� ���󰡴� �����Ǽӵ��͹���
    public void Onequadrant_3case_change_speed3()
    {
        fly_speed_x = -0.7f;
        fly_speed_y = 15.0f;
    }
    public void Onequadrant_3case_change_speed4()
    {
        fly_speed_x = 0.7f;
        fly_speed_y = 15.0f;
    }
    // 1��и鿡�� ����3,4��ǥ�� 0.3�����϶� ���󰡴� ������ �ӵ��͹���
    public void Onequadrant_4case_change_speed3()
    {
        fly_speed_x = -0.7f;
        fly_speed_y = -40.0f;
    }
    public void Onequadrant_4case_change_speed4()
    {
        fly_speed_x = 0.7f;
        fly_speed_y = -40.0f;
    }

    // 1��и鿡�� 0.7>x>0���� ���󰡴� 3�������� �ӵ��͹���
    public void Onequadrant_5case_change_speed3()
    {
        fly_speed_x = 10.0f;
        fly_speed_y = 9.0f;
    }



    public void Twoquadrant_2case_change_speed1()
    {
        fly_speed_x = 1.0f;
        fly_speed_y = 4.5f;
    }
    public void Twoquadrant_2case_change_speed2()
    {
        fly_speed_x = -1.0f;
        fly_speed_y = 4.5f;
    }
    public void Twoquadrant_2case_change_speed3()
    {
        fly_speed_x = 10.0f;
        fly_speed_y = 5.0f;
    }
    public void Twoquadrant_2case_change_speed4()
    {
        fly_speed_x = -0.5f;
        fly_speed_y = 4.5f;
    }


    //------------------------------------------------------------------------------------------------------------------------


    // 2��и�(x<0,y>0)���� ���󰡴� ������ �ӵ��͹���
    public void Twoquadrant_change_speed1()
    {
        fly_speed_x = 1.0f;
        fly_speed_y = 9.0f;
    }
    public void Twoquadrant_change_speed2()
    {
        fly_speed_x = -0.5f;
        fly_speed_y = 9.0f;
    }
    public void Twoquadrant_change_speed3()
    {
        fly_speed_x = 1.0f;
        fly_speed_y = 9.0f;
    }
    public void Twoquadrant_change_speed4()
    {
        fly_speed_x = -0.5f;
        fly_speed_y = 9.0f;
    }

    
}
