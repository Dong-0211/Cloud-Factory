using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestSatMax : MonoBehaviour
{
    private Guest mGuestManager;
    private SeasonDateCalc mSeason;

    public bool[] isMaxSat = new bool[6]; // �մ� ��ȣ�� �ش��ϴ� bool ����
    public  int mPrevDate; // ���� ��¥

    public GameObject mLetter;

    void Awake()
    {
        mGuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();
        mSeason = GameObject.Find("Season Date Calc").GetComponent<SeasonDateCalc>();
    }

    void Update()
    {
        for (int i = 0; i < 6; i++)
        {
            /// ������ 5�� ��Ƽ�� ã�� �۾�
            if (!isMaxSat[i] &&
                5 == mGuestManager.mGuestInfo[mGuestManager.mTodayGuestList[i]].mSatatisfaction)
            {
                // ������ 5 �˾� Bool �ѳ���
                isMaxSat[i] = true;
                // ���� ��¥ �޾ƿ���
                mPrevDate = mSeason.mDay;
            }
        }

        for (int i = 0; i < 6; i++)
        {
            // ������ 5 �̸鼭 ��¥�� ����Ǹ�
            if (isMaxSat[i] && (mPrevDate != mSeason.mDay))
            {
                // ���� ���� �� �� �˾� ����
                Debug.Log("���� �˾�");
                mLetter.SetActive(true);
                // �˾� ������� �ʱ�ȭ
                isMaxSat[i] = false;

                // ������ 5�� ��Ƽ�� �������� ���� for�� ���� ���� ����
                // ����� ��� ��Ƽ�� �־ isMaxSat�迭�� false�� �ٲٰ� �ٷ� ������ True�� �ٲٰ�����
                // ������ 5���� ��Ƽ �������� 1�� �����ų� ���� ���ٴ� ǥ���ϸ� �� ��?
            }
        }
    }

    public void CloseLetter()
    {
        mLetter.SetActive(false);
    }
}
