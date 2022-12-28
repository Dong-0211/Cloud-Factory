using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Pathfinding;

public class GuestObject : MonoBehaviour
{
    // ������Ʈ ������ �ʿ��� ����
    [Header("[�մ� ����]")]
    public float        mLimitTime;         // �մ��� ����� �ð�
    public float        mMaxLimitTime;      // �մ��� ����ϴ� �ð��� �ִ밪
    public int          mGuestNum;          // �ش� ������Ʈ�� �մԹ�ȣ
    private Transform   mTransform;         // ��ġ���� ���ϴ��� Ȯ���ϱ� ���� ����
    public GameObject   mTargetChair;       // ��ǥ�� �ϴ� ������ ��ġ
    public int          mTargetChiarIndex;

    [Header("[FSM ����]")]
    public bool isSit;                      // �ڸ��� �ɾ��ִ°�?
    public bool isUsing;                    // ���� ġ�Ḧ �޴����ΰ�?
    public bool isMove;                     // �̵����ΰ�?   
    public bool isAlreadyUse;               // ����� �Ϸ� �ߴ°�?


    [Header("[��Ÿ]")]
    public Animator     mGuestAnim;         // �մ��� �ִϸ��̼� ����
    private Guest       mGuestManager;
    public SOWManager   mSOWManager;

    const int MAX_GUEST_NUM = 20;

    // �� �մ��� ��ȣ�� ���� �ִϸ����͸� ���� �����Ѵ�.
    public RuntimeAnimatorController[] animators = new RuntimeAnimatorController[MAX_GUEST_NUM];

    // �մ� ��ȣ�� �������ش�.
    public void setGuestNum(int guestNum = 0)
    {
        mGuestNum = guestNum;
    }

    private void Awake()
    {

        DontDestroyOnLoad(this.gameObject);

        // ���ð� �ʱ�ȭ
        mLimitTime = 0.0f;
        mMaxLimitTime = 50.0f;
        isSit = false;
        isUsing = false;
        isMove = false;
        isAlreadyUse = false;
        mTransform = this.transform;
        mTargetChiarIndex = -1;
        mTargetChair = null;
        mGuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();
        mSOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();
        mGuestAnim = GetComponent<Animator>();

    }

    // �ȴ� �ִϸ��̼� ���
    // �ȴ� �ִϸ��̼��� ����Ʈ �ִϸ��̼����� ����

    private void Update()
    {
        // �Ҵ�޴� ���� ����
        if (mTargetChiarIndex != -1 && isAlreadyUse == false)
        {
            mTargetChair = mSOWManager.mChairPos[mTargetChiarIndex];
            this.GetComponent<AIDestinationSetter>().enabled = true;
            this.GetComponent<AIDestinationSetter>().target = mTargetChair.transform;

            // ���ڿ� �������� �ʾҴٸ� AIPATH�� Ȱ��ȭ�Ѵ�.
            if (this.transform != mTargetChair.transform)
            {
                this.GetComponent<WayPoint>().isMove = false;
                this.GetComponent<AIPath>().enabled = true;
            }
            else
            {
                this.GetComponent<AIPath>().enabled = false;
            }
        }

        // ������ �����޴� ���°� �ƴ϶�� ���ð��� ���Ž�Ų��.
        if (isUsing != true)
        {
            mLimitTime += Time.deltaTime;
        }
        

        bool GoHome = false;
        // ���ð��� �����ų� �Ҹ���Ƽ�� �� ��쿡
        if ((mLimitTime > mMaxLimitTime || mGuestManager.mGuestInfo[mGuestNum].isDisSat == true) && GoHome == false)
        {
            // ����� ����Ʈ���� ���ְ�, �ش� ���ڸ� �ٽ� true�� �ٲ��־�� �Ѵ�.
            mSOWManager.mCheckChairEmpty[mTargetChiarIndex] = true;
            mTargetChair = null;
            isSit = false;
            GoHome = true;

            // ���� ��밡�� ����Ʈ���� ����
            int count = mSOWManager.mUsingGuestList.Count;

            for (int i = 0; i < count; i++)
            {
                if (mSOWManager.mUsingGuestList[i] == mGuestNum)
                    mSOWManager.mUsingGuestList.RemoveAt(i);
            }

            // �Ҹ� �մ����� ��ȯ ��, �Ͱ�
            mGuestManager.mGuestInfo[mGuestNum].isDisSat = true;
            MoveToEntrance();
        }

        // �Ա��� ������ ���
        if (isAlreadyUse == true && transform.position.x - mSOWManager.mWayPoint[0].transform.position.x <= 0.2f)
        {
            Destroy(this.gameObject);
        }

        // ���ڿ� ������ ���
        if (mTargetChiarIndex != -1)
        {
            if (isAlreadyUse == false && Mathf.Abs(transform.position.x - mTargetChair.transform.position.x) 
                <= 0.1f && Mathf.Abs(transform.position.y - mTargetChair.transform.position.y) <= 0.1f)
            {
                // ���� ��ġ�� �̵�
                transform.localScale = new Vector3(1f, 1f, 1f);
                mGuestAnim.SetBool("isSit", true);
                this.transform.position = mTargetChair.transform.position;
                isSit = true;

            }
        }

        // ���¿� ���� �ִϸ��̼� ����
        if (isSit)
        {
            if(mGuestManager.mGuestInfo[mGuestNum].isUsing == true)
            {
                isUsing = true;
                Debug.Log("isUsing : true");
                mGuestManager.mGuestInfo[mGuestNum].isUsing = false;
            }
            // ġ�� ���� ��� ġ��ȿ���� ���� �ֱ������� �ִϸ��̼��� ����
            if (isUsing)
            {
                // ���� ���� ������ ���⿡ ���� �ɾ��ִ� ����� ������/������ �� �ϳ��� ���´�.
                // �׽�Ʈ�� ���� �ϴ��� ���� ������� �����Ѵ�.
                mGuestAnim.SetBool("isUsing", true);

                // 5�� �Ŀ� Invoke �Լ��� �̿��ؼ� ������� ���¸� �����ϰ�, �Ͱ��ϴ� ����� ����ִ´�.
                Invoke("TakeCloud", 5.0f);
            }
        }
        else
        {
            // ��å�θ� ������ ������̱� ������ �ȴ� ����� �����Ѵ�.
            // ������Ʈ�� ��ġ���� ������ �ʾҴٸ� ���ִ� �ִϸ��̼��� ������ش�.
            if (mTransform == transform)
            {
                mGuestAnim.SetBool("isStand", true);
            }
            // �ٽ� ���ϴ� ��쿡�� �ȴ� �ִϸ��̼��� ����Ѵ�.
            else
            {
                mGuestAnim.SetBool("isStand", false);
            }

        }

        // �ȴ� ���⿡ ���� �ִϸ��̼��� ������ �ٸ��� �����Ѵ�.
        if (GetComponent<AIPath>().desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (GetComponent<AIPath>().desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        // ���� ��ġ�� �����Ѵ�.
        mTransform = transform;

    }
    
    private void TakeCloud()
    {
        isUsing = false;
        mGuestAnim.SetBool("isUsing", false);
        // �����Ѽ� �� �� ������ ����

        MoveToEntrance();
    }

    public void SpeakEmotion()
    {
        Debug.Log("���� ����� ����մϴ�");
        // ���� ����, ���� ������ ���� ����� ������ ���� ��Ʈ(����Ʈ)
        // ������ �ݿ� �������� ���� �� ������ �˷��ִ� ���
    }

    // �ִϸ��̼� Ŭ������ �մԿ� �°� �ʱ�ȭ�Ѵ�.
    public void initAnimator()
    {
        GetComponent<Animator>().runtimeAnimatorController = animators[mGuestNum];
        Debug.Log("init Guest Anim");
    }

    // �Ա��� �����ϴ� �Լ��̴�.
    private void MoveToEntrance()
    {
        isAlreadyUse = true;
        mGuestAnim.SetBool("isSit", false);

        // �ο����� ���� �ε����� �ʱ�ȭ
        mGuestManager.mGuestInfo[mGuestNum].mSitChairIndex = -1;

        Invoke("ChangeTarget", 3.0f);
    }

    private void ChangeTarget()
    {
        this.GetComponent<AIDestinationSetter>().target = mSOWManager.mWayPoint[0].transform;
    }
}
