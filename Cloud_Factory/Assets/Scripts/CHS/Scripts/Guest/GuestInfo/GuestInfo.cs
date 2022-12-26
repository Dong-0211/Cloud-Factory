using UnityEngine;

[System.Serializable]
public struct SSatEmotion
{
    public int emotionNum;                                      // ���� ��ȣ
    public int up;                                              // ������ ���� ���Ѽ�
    public int down;                                            // ������ ���� ���Ѽ�
}

[System.Serializable]
public struct SLimitEmotion
{
    public int upLimitEmotion;                                  // �մ��� ���� ���Ѽ�
    public int upLimitEmotionValue;                             // �մ��� ���� ���Ѽ� ��

    public int downLimitEmotion;                                // �մ��� ���� ���Ѽ�
    public int downLimitEmotionValue;                           // �մ��� ���� ���Ѽ� ��
}

[CreateAssetMenu(menuName = "Scriptable/Guest_info", fileName = "Guest Info")]

public class GuestInfo : ScriptableObject
{
    public string mName;                                            // �մ��� �̸�
    public int[] mSeed;                                             // �մ��� �ɰ� �� �� �ִ� ����� �ε��� ��
    public int[] mEmotion = new int[20];                            // �մ��� ���� ��. �� 20����
    public int  mAge;
    public string  mJob;

    public int mSatatisfaction;                                     // �մ��� ������
    public Sprite[] sImg;                                           // �մ��� �̹��� -> ������ ���������� ����     

    public SSatEmotion[] mSatEmotions = new SSatEmotion[5];         // �մ��� ������ ���� 5����
    public SLimitEmotion[] mLimitEmotions = new SLimitEmotion[2];

    public bool isDisSat;                                           // �Ҹ� ��Ƽ���� Ȯ��
    public bool isCure;                                             // �մ��� ������ 5�� ä�� ��� ġ���Ͽ����� Ȯ�� 
    public int  mVisitCount;                                        // ���� �湮 Ƚ��
    public int  mNotVisitCount;                                     // �湮���� �ʴ� Ƚ��
    public bool isChosen;                                           // ���õǾ����� Ȯ���ϴ� ����

    public int[] mUsedCloud = new int[10];                          // ����� ���� ����Ʈ�� �����Ѵ�. �ִ� 10��

    public int   mSitChairIndex;                                    // �մ��� ���� ���� ���� �ε���
    public bool isUsing = false;
}

