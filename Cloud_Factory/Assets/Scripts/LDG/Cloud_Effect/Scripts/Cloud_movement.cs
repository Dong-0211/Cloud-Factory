using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud_movement : MonoBehaviour
{
    public int num = 0;
    //public float limintMin = -5.0f;
    //private bool Act = false;
    public GameObject cloud_part;        // 
    public GameObject parts;        // �������� �ν��Ͻ�ȭ�Ͽ� ���� ���ӿ�����Ʈ
    public GameObject Parts_fly;    // part_fly ��ũ��Ʈ ������ִ� �����մ��� ���ӿ�����Ʈ

    void Start()
    {
        num = Random.Range(1, 5); // �װ�����ġ �����������ϴ� ����
        //GameObject go = Instantiate(parts);
        switch (num)
        {
            case 1:
                cloud_part.transform.localPosition = new Vector2(-0.5f, 0.23f);         // ������ 
                InvokeRepeating("fly", 2f, 2f);                                                   // ������ ���󰡴� �Լ� �ݺ��ϴ°�(�Լ��� 2���ĺ��� ����ǰ� 2���ֱ�� ����)
                Parts_fly.GetComponent<Parts_fly>().change_speed_1_2();              // 
                break;

            case 2:
                cloud_part.transform.localPosition = new Vector2(0.57f, 0.31f);
                InvokeRepeating("fly", 2f, 2f);
                Parts_fly.GetComponent<Parts_fly>().change_speed_1_2();
                break;

            case 3:
                cloud_part.transform.localPosition = new Vector2(-0.29f, -0.29f);
                InvokeRepeating("fly", 2f, 2f);
                Parts_fly.GetComponent<Parts_fly>().change_speed_3();
                break;

            case 4:
                cloud_part.transform.localPosition = new Vector2(0.34f, -0.1f);
                InvokeRepeating("fly", 2f, 2f);
                Parts_fly.GetComponent<Parts_fly>().change_speed_4();
                break;


        }




    }

    void fly()
    {
        GameObject go = Instantiate(parts);
        go.transform.position = cloud_part.transform.position;
    }

    void Update()
    { 
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameObject go = Instantiate(parts);
            go.transform.position = cloud_part.transform.position;
        }
    }
}
