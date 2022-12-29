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
    public Transform targetChairPos;

    SOWManager SOWManager;
    Guest GuestManager;

    public int mGuestNum;                       // Ÿ�� �մ� ��ȣ
    public int cloudSpeed;                      // ���� �ӵ�
    public List<EmotionInfo> mFinalEmotions;    // ��ȯ�� ������ ����Ʈ

    // ó�� �޾ƿ;� �ϴ� ��
    // 1) Cloud Spawner�κ��� �������� �޾ƿ´�.

    // ���ο��� �����ؾ� �� ���
    // 1) �մ԰��� �浹 ó�� (�浹 �� ����Ѵٴ� ����)
    // 2) �浹 �� �ش� �մ��� �������� ��ȭ�ϰԲ� ����

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        SOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();
        GuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();

        // �׽�Ʈ�� ���� ������ �ֱ�
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
            for (int i = 0; i < mFinalEmotions.Count; ++i)
            {
                GuestManager.SetEmotion(mGuestNum, (int)mFinalEmotions[i].Key, mFinalEmotions[i].Value);
                Debug.Log(mGuestNum + "�� �մ� ������ȯ �Լ� ȣ��" + (int)mFinalEmotions[i].Key + " " + mFinalEmotions[i].Value);
            }

            // TODO : ������ ����� �մ��� ������ ���� (LIST : ���� ��, �����Ѽ� ���, ������ ����)

            // ���� �����Ѽ� �˻�
            if (GuestManager.IsExcessLine(mGuestNum) != -1)
                GuestManager.mGuestInfo[mGuestNum].isDisSat = true;
            else
                Debug.Log("�����Ѽ��� ħ������ �ʾҽ��ϴ�.");

            // ������ �� ����
            GuestManager.RenewakSat(mGuestNum);

            // ���� �� ��ȯ�Լ� ȣ�� ��, ����
            Destroy(this.gameObject);

            // ������ ���� �մ��� �����·� ����
            GuestManager.mGuestInfo[mGuestNum].isUsing = true;
        }
        else
        {
            Vector3 temp;
            temp = new Vector3(-0.4f, 1.4f, 0f);

            // TODO : ���ڸ� �ɴ� ���⿡ ���� targetChairPos�� ���� ���� ��ȯ�����ش�.
            transform.position = Vector2.MoveTowards(transform.position, targetChairPos.position + temp, cloudSpeed * Time.deltaTime);
        }
    }


    // �Ʒ��� 4���� �Լ��� ������ ������ �� �����ʿ��� ����ϴ� �Լ��̴�.

    public void SetTargetChair(int guestNum)
    {
        targetChairPos = SOWManager.mChairPos[GuestManager.mGuestInfo[guestNum].mSitChairIndex].transform;
    }

    public void SetValue(List<EmotionInfo> CloudData)
    {
        mFinalEmotions = CloudData;
    }

    public void SetGuestNum(int _guestNum)
    {
        mGuestNum = _guestNum;
    }

    public void SetSprite(Sprite sprite)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

}
