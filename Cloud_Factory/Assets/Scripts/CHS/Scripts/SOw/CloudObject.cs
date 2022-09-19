using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct GavedGuestInfo
{
    int mGuestNum;
    Dictionary<Emotion, int> emotionList;
}

public class CloudObject : MonoBehaviour
{
    // ó�� �޾ƿ;� �ϴ� ��
    // 1) Cloud Spawner�κ��� �������� �޾ƿ´�.
    public Transform targetChairPos;

    SOWManager SOWManager;
    Guest GuestManager;

    public int mGuestNum;                       // Ÿ�� �մ� ��ȣ
    public int cloudSpeed;                      // ���� �ӵ�
    public Dictionary<int, int> emotionList;    // ��ȯ�� ������ ��ųʸ�

    // ���ο��� �����ؾ� �� ���
    // 1) �մ԰��� �浹 ó�� (�浹 �� ����Ѵٴ� ����)
    // 2) �浹 �� �ش� �մ��� �������� ��ȭ�ϰԲ� ����

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        SOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();
        GuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();

        // �׽�Ʈ�� ���� ������ �ֱ�
        mGuestNum = 1;
        cloudSpeed = 1;

        // �׽�Ʈ�� ���� ���� �� �ֱ�
        targetChairPos = SOWManager.mChairPos[0].transform;
    }


    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(this.transform.position.x - targetChairPos.position.x) < 0.1f)
        {
            // ���� �� ��ȯ�Լ� ȣ��
            GuestManager.SetEmotion(mGuestNum, 0, 1, 10, 10);
            Debug.Log("������ȯ �Լ� ȣ��");

            // ���� �� ��ȯ�Լ� ȣ�� ��, ����
            Destroy(this.gameObject);
            Debug.Log("������ ȭ��󿡼� ����");
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, targetChairPos.position, cloudSpeed * Time.deltaTime);
        }

    }

    public void SetTargetChair()
    {
        targetChairPos = SOWManager.mChairPos[0].transform;
    }
    
    
}
