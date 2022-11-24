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
    public List<EmotionInfo> mFinalEmotions;    // ��ȯ�� ������ ����Ʈ

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
            Debug.Log(mFinalEmotions.Count);

            // ���� �� ��ȯ�Լ� ȣ�� ��, ����
            Destroy(this.gameObject);
            Debug.Log("������ ȭ��󿡼� ����");

            // ������ ���� �մ��� �����·� ����
            GuestManager.mGuestInfos[mGuestNum].isUsing = true;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, targetChairPos.position, cloudSpeed * Time.deltaTime);
        }
    }

    public void SetTargetChair(int guestNum)
    {
        targetChairPos = SOWManager.mChairPos[GuestManager.mGuestInfos[guestNum].mSitChairIndex].transform;
    }

    public void SetValue(List<EmotionInfo> CloudData)
    {
        mFinalEmotions = CloudData;
    }

    public void SetGuestNum(int _guestNum)
    {
        mGuestNum = _guestNum;
    }

    public void SetSprite(Texture2D texture)
    {
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

}
