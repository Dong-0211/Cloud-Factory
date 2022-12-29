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
    public bool isGotoEntrance;             // �ⱸ�� ������ ���ΰ�?


    [Header("[��Ÿ]")]
    public Animator     mGuestAnim;         // �մ��� �ִϸ��̼� ����
    private Guest       mGuestManager;
    public SOWManager   mSOWManager;

    const int MAX_GUEST_NUM = 20;


    // �մ԰� ��ȣ�ۿ��� ���� �ʿ��� �ݶ��̴� 
    private Collider2D sitCollider;
    private Collider2D walkCollider;

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
        isGotoEntrance = false;
        mTransform = this.transform;
        mTargetChiarIndex = -1;
        mTargetChair = null;
        mGuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();
        mSOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();
        mGuestAnim = GetComponent<Animator>();

        sitCollider = this.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<CircleCollider2D>();
        walkCollider = this.transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<CircleCollider2D>();
    }

    // �ȴ� �ִϸ��̼� ���
    // �ȴ� �ִϸ��̼��� ����Ʈ �ִϸ��̼����� ����

    private void Update()
    {
        // �Ҵ�޴� ���� ����
        if (mTargetChiarIndex != -1 && isGotoEntrance == false)
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

        // ���ð��� �����ų� �Ҹ���Ƽ�� �� ��쿡
        if ((mLimitTime > mMaxLimitTime || mGuestManager.mGuestInfo[mGuestNum].isDisSat == true))
        {
            // ����� ����Ʈ���� ���ְ�, �ش� ���ڸ� �ٽ� true�� �ٲ��־�� �Ѵ�.
            mSOWManager.mCheckChairEmpty[mTargetChiarIndex] = true;
            mTargetChair = null;
            isSit = false;

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
        if (isGotoEntrance == true && transform.position.x - mSOWManager.mWayPoint[0].transform.position.x <= 0.2f)
        {
            Destroy(this.gameObject);
        }

        // ���ڿ� ������ ���
        if (mTargetChiarIndex != -1)
        {
            if (isGotoEntrance == false && Mathf.Abs(transform.position.x - mTargetChair.transform.position.x) 
                <= 0.1f && Mathf.Abs(transform.position.y - mTargetChair.transform.position.y) <= 0.1f)
            {
                // ���� ��ġ�� �̵�
                transform.localScale = new Vector3(1f, 1f, 1f);
                mGuestAnim.SetBool("isSit", true);

                // TODO : �ݶ��̴� ���� Walking ->Sitting
                sitCollider.enabled = true;
                walkCollider.enabled = false;

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

        // ���ִ� ��쿡�� Ŭ�� �� ��ȣ�ۿ��� ���� ������ ǥ���Ѵ�.
        if (mGuestAnim.GetBool("isSit")) return;

        // ���� ����, ���� ������ ���� ����� ������ ���� ��Ʈ(����Ʈ)


        // ������ �ݿ� �������� ���� �� ������ �˷��ִ� ��ǳ��  -> �մ��� ��ġ���� ���� ��/�� ���� ����
    

    
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
        isGotoEntrance = true;
        mGuestAnim.SetBool("isSit", false);

        // TODO : �ݶ��̴� ���� Sitting -> Walking
        sitCollider.enabled = false;
        walkCollider.enabled = true;

        // �ο����� ���� �ε����� �ʱ�ȭ
        mGuestManager.mGuestInfo[mGuestNum].mSitChairIndex = -1;

        Invoke("ChangeTarget", 3.0f);
    }

    private void ChangeTarget()
    {
        this.GetComponent<AIDestinationSetter>().target = mSOWManager.mWayPoint[0].transform;
    }
}
