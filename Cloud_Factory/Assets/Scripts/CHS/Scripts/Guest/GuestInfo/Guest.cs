using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Guest : MonoBehaviour
{
    // ��� ����
    private const int NUM_OF_GUEST = 20;                                // �մ��� �� �ο� ��
    private const int NUM_OF_TODAY_GUEST_LIST = 6;                      // �Ϸ翡 �湮�ϴ� �մ��� �� �ο� ��

    [Header ("[�մ� ������ ����Ʈ]")]
    public GuestInfos[] mGuestInfo;                                     // �մԵ��� ������
    [SerializeField]
    private GuestInfo[] mGuestInitInfos;                                // Scriptable Objects���� ������ ��� �ִ� �迭

    [Header ("[�մ� �湮 ���� ����]")]
    public bool isGuestInLivingRoom;                                    // �����ǿ� �մ��� �湮���ִ°�?
    public bool isTimeToTakeGuest;                                      // ��Ƽ �湮�ֱⰡ �������� Ȯ��

    [Space(10f)]
    public int mGuestIndex;                                             // �̹��� �湮�� ��Ƽ�� ��ȣ
    public int[] mTodayGuestList = new int[NUM_OF_TODAY_GUEST_LIST];    // ���� �湮 ������ ��Ƽ ���
    [SerializeField]
    private int mGuestCount;                                            // �̹��� �湮�� ��Ƽ�� ����

    [Space (10f)]
    public float mGuestTime;                                            // ��Ƽ�� �湮 �ֱ��� ���� ��
    public float mMaxGuestTime;                                         // ��Ƽ�� �湮 �ֱ�


    [SerializeField]
    private int mGuestMax;                                              // ���� �湮�ϴ� ��Ƽ�� �ִ� ����


    private static Guest instance = null;                               // �̱��� ����� ���� instance ����
    private void Awake()
    {
        // �̱��� ��� ���
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            mGuestTime = 0;
            mGuestCount = -1;
            mGuestMax = 0;
            mMaxGuestTime = 5.0f;
            
            mGuestInfo = new GuestInfos[NUM_OF_GUEST];
            for (int i = 0; i< NUM_OF_GUEST; i++)
            {
                InitGuestData(i);
            }

            InitDay();

            isTimeToTakeGuest = false;
            isGuestInLivingRoom = false;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ��Ƽ�� �湮�ֱ⸦ ������.
        if (mGuestTime < mMaxGuestTime)
        {
            mGuestTime += Time.deltaTime;
        }
        else if (mGuestTime >= mMaxGuestTime && isTimeToTakeGuest == false)
        {
            // ��� �ε����� �� ���� �ʴ� �� ��Ƽ �湮�ֱⰡ �ٵȰ�� ���ο� ��Ƽ�� �鿩������.
            if (mGuestCount < mGuestMax - 1) 
            {
                isTimeToTakeGuest = true;
                TakeGuest();
                // ������ �̵��ϴ� ��ư�鿡 ���� ��ȣ�ۿ�
            }
            else
            {
                Debug.Log("��� ��Ƽ�� �湮�Ͽ����ϴ�.");
            }
        }
        // �̱��� ��� Ȯ���� ���� �׽�Ʈ�ڵ�
        if (Input.GetKeyDown(KeyCode.A))
        {
            TakeGuest();
        }
        // ������ ������ ���� �Լ� �׽�Ʈ (����)
        if (Input.GetKeyDown(KeyCode.C))
        {
            RenewakSat(0);
        }
        // �����Ѽ� ħ�� Ȯ���� ���� �Լ� �׽�Ʈ (����)
        if (Input.GetKeyDown(KeyCode.D))
        {
            mGuestInfo[0].isDisSat = CheckIsDisSat(0);
            Debug.Log(mGuestInfo[0].isDisSat);
        }
    }

    public void TakeGuest()
    {
        if (isTimeToTakeGuest == true && isGuestInLivingRoom == false)
        {
            mGuestCount++;
            mGuestIndex = mTodayGuestList[mGuestCount];
            isGuestInLivingRoom = true;
        }
    }

    // ��Ƽ�� ���������� �޾ƿ��� API
    public string GetName(int gusetNum)
    {
        return mGuestInfo[gusetNum].mName;
    }

    public bool CheckIsDisSat(int guestNum)
    {
        int temp = IsExcessLine(guestNum);                      // ħ���ϴ� ��쿡 �������� ���Ƿ� ������ ����

        // ������ ���� ħ���� ��츦 Ȯ��
        if (temp != -1)
        {
            mGuestInfo[guestNum].isDisSat = true;              // �Ҹ� ��Ƽ�� ��ȯ
            mGuestInfo[guestNum].mSatatisfaction = 0;          // ������ 0 ���� ����
            mGuestInfo[guestNum].mVisitCount = 0;              // ���� �湮Ƚ�� 0���� ����

            // TODO : ġ���� ������� �Ҹ� ��Ƽ�� �� ���¿� �մ� ��ȣ, � ���� ��ȭ�� ���� ������ �������ֱ�


            return true;
        }
        return false;
    }

    // ��Ƽ�� ������ ���濡 �ʿ��� API 
    public void SetEmotion(int guestNum, int emotionNum, int value)
    {
        mGuestInfo[guestNum].mEmotion[emotionNum] += value;
    }

    public int IsExcessLine(int guestNum) // ���� �����Ѽ��� ħ���ߴ��� Ȯ���ϴ� �Լ�. 
    {
        SLimitEmotion[] limitEmotion = mGuestInfo[guestNum].mLimitEmotions;

        for (int i = 0; i < 2; i++)
        {
            if (mGuestInfo[guestNum].mEmotion[limitEmotion[i].upLimitEmotion] >= limitEmotion[i].upLimitEmotionValue) // �����Ѽ��� ħ���� ���
            {
                Debug.Log("�����Ѽ��� ħ���Ͽ����ϴ�");
                return limitEmotion[i].upLimitEmotion;
            }
            else if (mGuestInfo[guestNum].mEmotion[limitEmotion[i].downLimitEmotion] <= limitEmotion[i].downLimitEmotionValue)
            {
                Debug.Log("�����Ѽ��� ħ���Ͽ����ϴ�");
                return limitEmotion[i].downLimitEmotion;
            }
        }

        // �����Ѽ� ��� ħ������ �ʴ� ���
        Debug.Log("�����Ѽ��� ħ������ �ʾҽ��ϴ�");
        return -1;
    }

    public void RenewakSat(int guestNum)     // �������� �����ϴ� �Լ�. -> ���� ���� ���� 4������ ����
    {
        int temp = 0;

        for (int i = 0; i < 5; i++)
        {
            // ������ ���� ���� ������ Ȯ��
            if (mGuestInfo[guestNum].mEmotion[mGuestInfo[guestNum].mSatEmotions[i].emotionNum] <= mGuestInfo[guestNum].mSatEmotions[i].up &&
             mGuestInfo[guestNum].mEmotion[mGuestInfo[guestNum].mSatEmotions[i].emotionNum] >= mGuestInfo[guestNum].mSatEmotions[i].down)
            {
                temp++;
            }
        }
        mGuestInfo[guestNum].mSatatisfaction = temp;
        Debug.Log(temp);
    }

    // TODO : �Լ� ����
    // ��Ƽ ����Ʈ�� ���� �����ϴ� �Լ�
    public int[] NewChoiceGuest()
    {
        int[] guestList = new int[6];                   // ��ȯ��ų ��Ƽ�� ����Ʈ
        int possibleToTake = 6;                         // ���� �� �ִ� �� ��Ƽ�� ��

        int totalGuestNum = 20;                         // �� ��Ƽ�� ��
        int possibleGuestNum = 0;                       // �湮�� ������ ��Ƽ�� ��

        List<int> VisitedGuestNum = new List<int>();    // �湮 �̷��� �ִ� ��Ƽ�� ����Ʈ
        List<int> NotVisitedGuestNum = new List<int>(); // �湮 �̷��� ���� ��Ƽ�� ����Ʈ

        // �湮 Ƚ���� ���� ��Ƽ�� �������� 5�� �� ��Ƽ�� ���ܵǾ�� �ϹǷ� ���� ����Ʈ���� ������.
        for (int i = 0; i < totalGuestNum; i++)
        {
            if (mGuestInfo[i].mVisitCount != 10 && mGuestInfo[i].isCure == false)
            {
                if (mGuestInfo[i].mVisitCount == 0)
                {
                    NotVisitedGuestNum.Add(i);
                }
                else
                {
                    VisitedGuestNum.Add(i);
                }
            }
            if (mGuestInfo[i].isDisSat == false && mGuestInfo[i].mNotVisitCount == 0 && mGuestInfo[i].mVisitCount != 10 && mGuestInfo[i].isCure == false)
            {
                possibleGuestNum++;
            }
        }

        int GuestIndex = 0;
        bool isOverLap = true;

        // �湮 �̷��� �ִ� ��Ƽ�� 5�� �̻��� ���� ���
        // ��� �湮 �̷��� �ִ� ��Ƽ�� �̰� �������� �湮 �̷��� ���� ��Ƽ�� ä���.
        if (VisitedGuestNum.Count < possibleToTake - 1)
        {
            Debug.Log("�湮 �̷��� �ִ� ��Ƽ�� 5���̻��� ���� �ʽ��ϴ�");
            for (int i = 0; i < VisitedGuestNum.Count; i++)
            {
                int temp = -1;
                while (isOverLap)
                {
                    // ���� ����
                    temp = Random.Range(0, VisitedGuestNum.Count);
                    int count = 0;
                    for (int j = 0; j <= GuestIndex; j++)
                    {
                        // �̹� ���� ����־� �ߺ��Ǵ� ���
                        if (VisitedGuestNum[temp] == guestList[j])
                        {
                            count++;
                        }
                    }
                    int rejectCount = 0;
                    // �Ҹ� ��Ƽ�̰ų� �湮 �Ұ� ���� ��Ƽ�� ���� ���ؾ� �Ѵ�.
                    for (int j = 0; j < GuestIndex; j++)
                    {
                        if (mGuestInfo[guestList[j]].isDisSat == true || mGuestInfo[guestList[j]].mNotVisitCount != 0)
                        {
                            rejectCount++;
                        }
                    }
                    //Debug.Log("reject Count : " + rejectCount);
                    if ((mGuestInfo[VisitedGuestNum[temp]].isDisSat == true || mGuestInfo[VisitedGuestNum[temp]].mNotVisitCount != 0)
                        && rejectCount >= possibleToTake - 2)
                    {
                        if (possibleGuestNum >= 2)
                        {
                            count++;
                        }
                    }

                    if (count == 0)
                    {
                        isOverLap = false;
                    }
                }
                guestList[GuestIndex] = VisitedGuestNum[temp];
                GuestIndex++;
                isOverLap = true;

            }
            for (int i = 0; i < possibleToTake - VisitedGuestNum.Count; i++)
            {
                int temp = -1;
                while (isOverLap)
                {
                    // ���� ����
                    temp = Random.Range(0, NotVisitedGuestNum.Count);
                    int count = 0;
                    for (int j = 0; j <= GuestIndex; j++)
                    {
                        // �̹� ���� ����־� �ߺ��Ǵ� ���
                        if (NotVisitedGuestNum[temp] == guestList[j])
                        {
                            count++;
                        }
                    }
                    int rejectCount = 0;
                    // �Ҹ� ��Ƽ�̰ų� �湮 �Ұ� ���� ��Ƽ�� ���� ���ؾ� �Ѵ�.
                    for (int j = 0; j < GuestIndex; j++)
                    {
                        if (mGuestInfo[guestList[j]].isDisSat == true || mGuestInfo[guestList[j]].mNotVisitCount != 0)
                        {
                            rejectCount++;
                        }
                    }
                    if ((mGuestInfo[NotVisitedGuestNum[temp]].isDisSat == true || mGuestInfo[NotVisitedGuestNum[temp]].mNotVisitCount != 0)
                        && rejectCount >= possibleToTake - 2)
                    {
                        if (possibleGuestNum >= 2)
                        {
                            count++;
                        }
                    }
                    if (count == 0)
                    {
                        isOverLap = false;
                    }
                }
                guestList[GuestIndex] = NotVisitedGuestNum[temp];
                GuestIndex++;
                isOverLap = true;

            }
        }
        // �湮 �̷��� ���� ��Ƽ�� ���� ���
        // ��� ��Ƽ�� �湮 �̷��� �ִ� ��Ƽ�߿��� �̴´�.
        else if (NotVisitedGuestNum.Count == 0)
        {
            Debug.Log("�湮 �̷��� ���� ��Ƽ�� �����ϴ�");
            for (int i = 0; i < possibleToTake; i++)
            {
                int temp = -1;
                while (isOverLap)
                {
                    // ���� ����
                    temp = Random.Range(0, VisitedGuestNum.Count);
                    int count = 0;
                    for (int j = 0; j <= GuestIndex; j++)
                    {
                        // �̹� ���� ����־� �ߺ��Ǵ� ���
                        if (VisitedGuestNum[temp] == guestList[j])
                        {
                            count++;
                            //Debug.Log("�� �ߺ�.");
                        }
                    }
                    int rejectCount = 0;
                    // �Ҹ� ��Ƽ�̰ų� �湮 �Ұ� ���� ��Ƽ�� ���� ���ؾ� �Ѵ�.
                    for (int j = 0; j < GuestIndex; j++)
                    {
                        if (mGuestInfo[guestList[j]].isDisSat == true || mGuestInfo[guestList[j]].mNotVisitCount != 0)
                        {
                            rejectCount++;
                        }
                    }
                    if ((mGuestInfo[VisitedGuestNum[temp]].isDisSat == true || mGuestInfo[VisitedGuestNum[temp]].mNotVisitCount != 0)
                        && rejectCount >= possibleToTake - 2)
                    {
                        if (possibleGuestNum >= 2)
                        {
                            count++;
                        }
                    }

                    if (count == 0)
                    {
                        isOverLap = false;
                    }
                }
                guestList[GuestIndex] = VisitedGuestNum[temp];

                GuestIndex++;
                isOverLap = true;

            }
        }
        // �� ���� ��쿡�� �湮 �̷��� �ִ� ��Ƽ 5��, �湮 �̷��� ���� ��Ƽ 1���� �̴´�.
        else
        {
            Debug.Log("�湮�̷� ��Ƽ 5��, �湮 �̷��� ���� ��Ƽ 1���� �̽��ϴ�.");
            for (int i = 0; i < possibleToTake - 1; i++)
            {
                int temp = -1;
                while (isOverLap)
                {
                    // ���� ����
                    temp = Random.Range(0, VisitedGuestNum.Count);
                    int count = 0;
                    for (int j = 0; j <= GuestIndex; j++)
                    {
                        // �̹� ���� ����־� �ߺ��Ǵ� ���
                        if (VisitedGuestNum[temp] == guestList[j])
                        {
                            count++;
                        }
                    }
                    int rejectCount = 0;
                    // �Ҹ� ��Ƽ�̰ų� �湮 �Ұ� ���� ��Ƽ�� ���� ���ؾ� �Ѵ�.
                    for (int j = 0; j < GuestIndex; j++)
                    {
                        if (mGuestInfo[guestList[j]].isDisSat == true || mGuestInfo[guestList[j]].mNotVisitCount != 0)
                        {
                            rejectCount++;
                        }
                    }
                    if ((mGuestInfo[VisitedGuestNum[temp]].isDisSat == true || mGuestInfo[VisitedGuestNum[temp]].mNotVisitCount != 0)
                        && rejectCount >= possibleToTake - 2)
                    {
                        if (possibleGuestNum >= 2)
                        {
                            count++;
                        }
                    }

                    if (count == 0)
                    {
                        isOverLap = false;
                    }
                }
                guestList[GuestIndex] = VisitedGuestNum[temp];
                GuestIndex++;
                isOverLap = true;

            }
            for (int i = 0; i < 1; i++)
            {
                int temp = -1;
                while (isOverLap)
                {
                    // ���� ����
                    temp = Random.Range(0, NotVisitedGuestNum.Count);
                    int count = 0;
                    for (int j = 0; j <= GuestIndex; j++)
                    {
                        // �̹� ���� ����־� �ߺ��Ǵ� ���
                        if (NotVisitedGuestNum[temp] == guestList[j])
                        {
                            count++;
                        }
                    }
                    int rejectCount = 0;
                    // �Ҹ� ��Ƽ�̰ų� �湮 �Ұ� ���� ��Ƽ�� ���� ���ؾ� �Ѵ�.
                    for (int j = 0; j < GuestIndex; j++)
                    {
                        if (mGuestInfo[guestList[j]].isDisSat == true || mGuestInfo[guestList[j]].mNotVisitCount != 0)
                        {
                            rejectCount++;
                        }
                    }
                    if ((mGuestInfo[NotVisitedGuestNum[temp]].isDisSat == true || mGuestInfo[NotVisitedGuestNum[temp]].mNotVisitCount != 0)
                        && rejectCount >= possibleToTake - 2)
                    {
                        if (possibleGuestNum >= 2)
                        {
                            count++;
                        }
                    }
                    if (count == 0)
                    {
                        isOverLap = false;
                    }
                }
                guestList[GuestIndex] = NotVisitedGuestNum[temp];

                GuestIndex++;
                isOverLap = true;
            }
        }
        // �Ҹ� ��Ƽ��� ��������.
        int[] tempList = new int[6];
        int a = 0;
        for (int i = 0; i < possibleToTake; i++)
        {
            tempList[i] = -1;
        }
        for (int i = 0; i < possibleToTake; i++)
        {
            if (mGuestInfo[guestList[i]].isDisSat == false && mGuestInfo[guestList[i]].mNotVisitCount == 0)
            {
                tempList[a] = guestList[i];
                a++;
                mGuestMax++;
            }
        }
        Debug.Log(mGuestMax);
        guestList = tempList;

        return guestList;
    }

    // �ش� ��Ƽ�� �ʱ�ȭ �����ִ� �Լ�
    public void InitGuestData(int guestNum) // ���Ŀ� ����
    {
        // ��ũ���ͺ� ������Ʈ�� ����� ���� �ʱ� �����Ͱ��� �޾ƿͼ� �ʱ�ȭ�� ��Ų��.

        GuestInfos temp         = new GuestInfos();
        temp.mName              = mGuestInitInfos[guestNum].mName;
        temp.mSeed              = mGuestInitInfos[guestNum].mSeed;
        temp.mEmotion           = mGuestInitInfos[guestNum].mEmotion;
        temp.mAge               = mGuestInitInfos[guestNum].mAge;
        temp.mJob               = mGuestInitInfos[guestNum].mJob;
        temp.mSatatisfaction    = mGuestInitInfos[guestNum].mSatatisfaction;
        temp.mSatEmotions       = mGuestInitInfos[guestNum].mSatEmotions;
        temp.mLimitEmotions     = mGuestInitInfos[guestNum].mLimitEmotions;
        temp.isDisSat           = mGuestInitInfos[guestNum].isDisSat;
        temp.isCure             = mGuestInitInfos[guestNum].isCure;
        temp.mVisitCount        = mGuestInitInfos[guestNum].mVisitCount;
        temp.mNotVisitCount     = mGuestInitInfos[guestNum].mNotVisitCount;
        temp.isChosen           = mGuestInitInfos[guestNum].isChosen;
        temp.mUsedCloud         = mGuestInitInfos[guestNum].mUsedCloud;
        temp.mSitChairIndex     = mGuestInitInfos[guestNum].mSitChairIndex;
        temp.isUsing            = mGuestInitInfos[guestNum].isUsing;

        mGuestInfo[guestNum]    = temp;

        Debug.Log(mGuestInitInfos[guestNum].mName);
    }

    // TODO : �Լ� ����
    // �湮�ֱ⸦ �ʱ�ȭ ���ִ� �Լ�
    public void InitGuestTime()
    {
        mGuestTime = 0.0f;
        isTimeToTakeGuest = false;
        Debug.Log("�湮�ֱ� �ʱ�ȭ");
    }

    // �Ϸ簡 �����鼭 �ʱ�ȭ�� �ʿ��� �������� ��ȯ���ش�.
    public void InitDay()
    {
        // ������ ������ ���� �����ִ� ��Ƽ���� �Ҹ� ��Ƽ�� �����.


        // ���ο� �湮 ��Ƽ ����Ʈ�� �̴´�.
        mGuestIndex = 0;
        mGuestCount = -1;
        mGuestMax = 0;

        // ���ο� ����Ʈ�� �̴� �Լ��� ȣ�� (�׽�Ʈ�� ���ؼ� ��� �ּ�ó��)
        int[] list = { 0, 1, 0, 1, 0, 1 };
        mGuestMax = NUM_OF_TODAY_GUEST_LIST;
        mTodayGuestList = list;

        //mTodayGuestList = NewChoiceGuest();

        // �湮 �ֱ⸦ �ʱ�ȭ�Ѵ�.
        InitGuestTime();

        // ä�������� �ٽ� ���ŵȴ�.
    }

    // TODO : �Լ� ����
    public int SpeakEmotionEffect(int guestNum)
    {
        int result = -1;            // �����Ѽ��� ���� ������ ���� ��ȣ
        int temp = -1;              // result�� �����Ѽ����� ���̰�

        // ���Ѽ� ������ ���ų� ���Ѽ����� ���ٸ� �Ҹ� ��Ƽ�̹Ƿ� ǥ���� ���� ���� ������ ������� �ʴ´�.
        // ù��° �����Ѽ� �� �߿��� �� �����Ѽ��� ������ ���� �ʱ� ��������� ���´�. 
        if (mGuestInfo[guestNum].mEmotion[mGuestInfo[guestNum].mLimitEmotions[0].downLimitEmotion]
            - mGuestInfo[guestNum].mLimitEmotions[0].downLimitEmotionValue >
            mGuestInfo[guestNum].mLimitEmotions[0].upLimitEmotionValue
            - mGuestInfo[guestNum].mEmotion[mGuestInfo[guestNum].mLimitEmotions[0].upLimitEmotion])
        {
            result = mGuestInfo[guestNum].mLimitEmotions[0].upLimitEmotion;
            temp = mGuestInfo[guestNum].mLimitEmotions[0].upLimitEmotionValue
            - mGuestInfo[guestNum].mEmotion[mGuestInfo[guestNum].mLimitEmotions[0].upLimitEmotion];
        }
        else
        {
            result = mGuestInfo[guestNum].mLimitEmotions[0].downLimitEmotion;
            temp = mGuestInfo[guestNum].mEmotion[mGuestInfo[guestNum].mLimitEmotions[0].downLimitEmotion]
            - mGuestInfo[guestNum].mLimitEmotions[0].downLimitEmotionValue;
        }
        if (temp > mGuestInfo[guestNum].mLimitEmotions[1].upLimitEmotionValue
             - mGuestInfo[guestNum].mEmotion[mGuestInfo[guestNum].mLimitEmotions[1].upLimitEmotion])
        {
            result = mGuestInfo[guestNum].mLimitEmotions[1].upLimitEmotion;
            temp = mGuestInfo[guestNum].mLimitEmotions[1].upLimitEmotionValue
             - mGuestInfo[guestNum].mEmotion[mGuestInfo[guestNum].mLimitEmotions[1].upLimitEmotion];
        }
        if (temp > mGuestInfo[guestNum].mEmotion[mGuestInfo[guestNum].mLimitEmotions[1].downLimitEmotion]
            - mGuestInfo[guestNum].mLimitEmotions[1].downLimitEmotionValue)
        {
            result = mGuestInfo[guestNum].mLimitEmotions[1].downLimitEmotion;
            temp = mGuestInfo[guestNum].mEmotion[mGuestInfo[guestNum].mLimitEmotions[1].downLimitEmotion]
            - mGuestInfo[guestNum].mLimitEmotions[1].downLimitEmotionValue;
        }

        return result;
    }

    public int SpeakEmotionDialog(int guestNum)
    {
        int result = -1;         // ��ȯ�� ���� ��ȣ
        int temp = -1;           // �ӽ÷� ������ ������ �������� ���̰�
        int maxValue = -1;       // ���̰� �߿��� ���� ū ���� �����ϴ� ��

        for (int i = 0; i < 5; i++)
        {
            // ������ �������� ���� ���� ���ٸ�
            if (mGuestInfo[guestNum].mEmotion[mGuestInfo[guestNum].mSatEmotions[i].emotionNum]
                > mGuestInfo[guestNum].mSatEmotions[i].up)
            {
                temp = mGuestInfo[guestNum].mEmotion[mGuestInfo[guestNum].mSatEmotions[i].emotionNum]
                    - mGuestInfo[guestNum].mSatEmotions[i].up;
            }
            // ������ �������� ���� ���� ���ٸ�
            else if (mGuestInfo[guestNum].mEmotion[mGuestInfo[guestNum].mSatEmotions[i].emotionNum]
                < mGuestInfo[guestNum].mSatEmotions[i].down)
            {
                temp = mGuestInfo[guestNum].mSatEmotions[i].down
                    - mGuestInfo[guestNum].mEmotion[mGuestInfo[guestNum].mSatEmotions[i].emotionNum];
            }
            // �̿��� ���� ���������ȿ� �ִ� ���̹Ƿ� �����Ѵ�.
            // temp���� ���� ����� ������ ������ ������ �ִٸ� �����Ѵ�.
            if (maxValue < temp)
            {
                maxValue = temp;
                result = mGuestInfo[guestNum].mSatEmotions[i].emotionNum;
            }
        }
        return result;
    }
}
