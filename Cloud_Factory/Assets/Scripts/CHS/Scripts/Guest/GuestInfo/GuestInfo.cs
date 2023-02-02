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
// �մ��� �ʱ� ���������� ScriptableObject�������� ������ �ִ´�.
public class GuestInfo : ScriptableObject
{
    [Header(" [�մ� ����] ")]
    public string mName;                                            // �մ��� �̸�
    public int mSeed;                                               // �մ��� �ɰ� �� �� �ִ� ����� �ε��� ��
    public int  mAge;                                               // �մ��� ����
    public string  mJob;                                            // �մ��� ����

    [Header ("[���� ����]")]
    public int[] mEmotion = new int[20];                            // �մ��� ���� ��. �� 20����
    public SSatEmotion[] mSatEmotions = new SSatEmotion[5];         // �մ��� ������ ���� 5����
    public SLimitEmotion[] mLimitEmotions = new SLimitEmotion[2];   // �մ��� �����Ѽ� ���� ���� 2����

    [Header("[������ ����]")]
    public int mSatatisfaction;                                     // �մ��� ������
    public int mSatVariation;                                       // �մ��� ������ ���� ����
    public bool isDisSat;                                           // �Ҹ� ��Ƽ���� Ȯ��
    public bool isCure;                                             // �մ��� ������ 5�� ä�� ��� ġ���Ͽ����� Ȯ�� 

    [Header("[�湮 Ƚ�� ����]")]
    public int  mVisitCount;                                        // ���� �湮 Ƚ��
    public int  mNotVisitCount;                                     // �湮���� �ʴ� Ƚ��

    [Header("[������ ���� ��ġ ����]")]
    public bool isChosen;                                           // ���õǾ����� Ȯ���ϴ� ����
    public int   mSitChairIndex;                                    // �մ��� ���� ���� ���� �ε���
    public bool isUsing = false;                                    // ������ �����޾Ҵ���

    [Header("[��Ÿ]")]
    public int[] mUsedCloud = new int[10];                          // ����� ���� ����Ʈ�� �����Ѵ�. �ִ� 10��
}


// GuestManager���� �����ϴ� �մ� ��������
[System.Serializable]
public class GuestInfos
{
    [Header(" [�մ� ����] ")]
    public string mName;                                            // �մ��� �̸�
    public int mSeed;                                             // �մ��� �ɰ� �� �� �ִ� ����� �ε��� ��
    public int mAge;                                                // �մ��� ����
    public string mJob;                                             // �մ��� ����

    [Header("[���� ����]")]
    public int[] mEmotion = new int[20];                            // �մ��� ���� ��. �� 20����
    public SSatEmotion[] mSatEmotions = new SSatEmotion[5];         // �մ��� ������ ���� 5����
    public SLimitEmotion[] mLimitEmotions = new SLimitEmotion[2];   // �մ��� �����Ѽ� ���� ���� 2����

    [Header("[������ ����]")]
    public int mSatatisfaction;                                     // �մ��� ������
	public int mSatVariation;                                       // �մ��� ������ ���� ����
	public bool isDisSat;                                           // �Ҹ� ��Ƽ���� Ȯ��
    public bool isCure;                                             // �մ��� ������ 5�� ä�� ��� ġ���Ͽ����� Ȯ�� 

    [Header("[�湮 Ƚ�� ����]")]
    public int mVisitCount;                                         // ���� �湮 Ƚ��
    public int mNotVisitCount;                                      // �湮���� �ʴ� Ƚ��

    [Header("[������ ���� ��ġ ����]")]
    public bool isChosen;                                           // ���õǾ����� Ȯ���ϴ� ����
    public int mSitChairIndex;                                      // �մ��� ���� ���� ���� �ε���
    public bool isUsing = false;                                    // ������ �����޾Ҵ���

    [Header("[��Ÿ]")]
    public int[] mUsedCloud = new int[10];                          // ����� ���� ����Ʈ�� �����Ѵ�. �ִ� 10��
}

