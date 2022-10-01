using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SOWManager : MonoBehaviour
{
    [SerializeField]
    public Queue<int> mWaitGuestList;               // �����ǿ��� ������ �ް� �Ѿ�� �մԵ��� ����Ʈ
    [SerializeField]
    public Queue<int> mUsingGuestList;              // ������ �������� �ڸ��� �ɾ� ������ �������� �غ� �� �մԵ��� ����Ʈ
    int mMaxNumOfUsingGuest;                        // mUsingGuestList�� ���� �� �ִ� �ִ��� ũ��
    int mTempGuestNum;                              // �ӽ� �մ� ��ȣ��

    [SerializeField]
    public Queue<GameObject> mWaitGuestObjectList;   // ��� �մ� ������Ʈ���� ������ ����Ʈ
    [SerializeField]
    public Queue<GameObject> mUsingGuestObjectList;  // ��� �մ� ������Ʈ���� ������ ����Ʈ

    [SerializeField]
    private GameObject mGuestObject;           // �ν��Ͻ��Ͽ� ������ �մ� ������Ʈ
    public GameObject[] mChairPos;              // �մ��� �ɾƼ� ������ ����� ����(����)
    public GameObject[] mWayPoint;              // �մ��� �ɾ�ٴϸ� ��å�ϴ� ��ε�
    public Dictionary<int, bool> mCheckChairEmpty;       // ���ڸ��� ���ڰ� ����ִ����� Ȯ���ϴ� ��ųʸ� ����
    public bool isNewGuest;             // �����ǿ��� �Ѿ�ö� ���ο� �մ��� ���°�?
    public int mMaxChairNum;           // ���� �ܰ迡 ���� ������ ����

    private Guest mGuestManager;          // GuestManager�� �����´�.
    private static SOWManager instance = null;

    public int mCloudGuestNum;
    public StoragedCloudData mStorageCloudData;
    public bool isCloudGet;

    void Start()
    {

    }
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);

            mGuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();
            mWaitGuestList = new Queue<int>();
            mUsingGuestList = new Queue<int>();
            mWaitGuestObjectList = new Queue<GameObject>();
            mUsingGuestObjectList = new Queue<GameObject>();
            mCheckChairEmpty = new Dictionary<int, bool>();
            isNewGuest = false;
            isCloudGet = false;
            // ���� �ܰ迡 �´� ���� ���� ����
            mMaxChairNum = 3;
            InitSOW();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ���ο� �մ��� ������ �������� �Ѿ�� ���
        if (isNewGuest)
        {
            isNewGuest = false;

            GameObject tempObject;

            // �մ� ������Ʈ ���� �� �ʱ�ȭ
            //mWaitGuestObjectList.Enqueue(tempObject);
            tempObject = Instantiate(mGuestObject);

            // �մ� ���� Ȯ���� ���� �����
            Debug.Log("�մ� ����");

            // �մ� ������Ʈ�� �ش��ϴ� ��ȣ�� �־��ش�.
            tempObject.GetComponent<GuestObject>().setGuestNum(mTempGuestNum);

            // ��å�θ� �����Ѵ�. <- �������� �޶����� ������ �Ѵ�.
            tempObject.GetComponent<WayPoint>().WayPos = mWayPoint;

            // �⺻ ��ġ���� �����Ѵ�. <- ��å���� ù��° ������ ����
            tempObject.transform.position = mWayPoint[0].transform.position;

            // ������� �մ� ť�� �ش� �մ��� �߰��Ѵ�.
            mWaitGuestObjectList.Enqueue(tempObject);
        }

        // ��å�θ� �ȴ� �մ��� �����ϰ�, �̿� ������ ���ڰ� ����ִٸ� ���ڷ� �̵���Ű��
        if (mWaitGuestList.Count != 0)
        {
            // ���� ���ڰ� �ִٸ� �ش� ���ڿ� ���� �ε����� ��ȯ�ް�, �ܴ̿� -1�� �޾ƿ´�.
            int chairNum = GetRandChiarIndex();

            if (chairNum != -1)
            {
                // ����� ����Ʈ�� �ش� �մ��� �ű��.
                MoveToUsingList(chairNum);
            }
        }

        // ������ ���� ���� �մ��� �����ϱ�
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(vec, Vector2.zero);
            if (hit.collider != null)
            {
                GameObject hitObject = hit.transform.gameObject;
                if (hit.transform.gameObject.tag == "Guest")
                {
                    Debug.Log(hitObject.GetComponent<GuestObject>().mGuestNum + "�� �մ��� Ŭ���Ͽ����ϴ�.");

                    hit.transform.gameObject.GetComponent<GuestObject>().SpeakEmotion();

                    Debug.Log(hit.transform.gameObject.GetComponent<GuestObject>().mTargetChiarIndex);
                }
                else
                {
                    Debug.Log(hit.transform.gameObject);
                }
            }
        }

        if(isCloudGet)
        {
            if (SceneManager.GetActiveScene().name == "Space Of Weather")
            {
                GetCloudToGuest(mCloudGuestNum, mStorageCloudData);
                isCloudGet = false;
            }
        }
    }

    // �Ϸ簡 ������ ������ ������ �ʱ�ȭ�Ѵ�.
    private void InitSOW()
    {
        mWaitGuestList.Clear();
        mUsingGuestList.Clear();
        mCheckChairEmpty.Clear();

        // ���׷��̵� �ܰ迡 ���� mCheckChairEmpty���� Ȯ���ϴ� ������ ������ �پ���.
        // ���� ��ġ�� �ʾ����Ƿ� �ϰ������� 3���� �����ϰ� �����Ѵ�.
        for (int i = 0; i < mMaxChairNum; i++)
        {
            // ��� ���ڴ� ����ִ� ���·� �ʱ�ȭ
            mCheckChairEmpty.Add(i, true);
        }
    }

    // ��� ����Ʈ�� �մ��� �߰������ִ� �Լ�
    public void InsertGuest(int guestNum)
    {
        mWaitGuestList.Enqueue(guestNum);

        Debug.Log(guestNum + "�� �մ��� ��� ����Ʈ�� �߰��Ǿ����ϴ�.");

        mTempGuestNum = guestNum; // �׽�Ʈ

    }


    // ��� ����Ʈ���� �մ��� ���� �޴� ����Ʈ�� �߰������ִ� �Լ�
    private void MoveToUsingList(int chairNum)
    {
        // ����� ������� �̵��� �մ��� ��ȣ�� �޾ƿ´�.
        int guestNum = mWaitGuestList.Dequeue();

        // �޾ƿ� �մ��� ������ ����� ������� �ִ´�.
        mUsingGuestList.Enqueue(guestNum);

        // tempObject�� ���� ���ڸ� �����Ѵ�.    
        GameObject tempObject = mWaitGuestObjectList.Dequeue();

        tempObject.GetComponent<GuestObject>().mTargetChiarIndex = chairNum;

        Debug.Log(chairNum + "�� ���ڸ� �����޾ҽ��ϴ�.");
        // ������Ʈ�� ����� ������Ʈ ������� �ִ´�.
        mUsingGuestObjectList.Enqueue(tempObject);

        // Ȯ���� ���� �����
        Debug.Log(guestNum + "�� �մ��� ��� ����Ʈ���� ����� ����Ʈ�� �̵��Ͽ����ϴ�.");

        // Guest�� �ο����� ���� �ε����� ����
        mGuestManager.mGuestInfos[guestNum].mSitChairIndex = chairNum;
    }

    // �Ϸ簡 ���� �� Queue�� �����ִ� ��Ƽ���� �Ҹ� ��Ƽ�� ������ش�.
    private void MakeGuestDisSat()
    {
        for (int i = 0; i < mWaitGuestList.Count; i++)
        {
            mGuestManager.mGuestInfos[mWaitGuestList.Dequeue()].isDisSat = true;
        }
        for (int i = 0; i < mUsingGuestList.Count; i++)
        {
            int guestNum = mUsingGuestList.Dequeue();
            mGuestManager.mGuestInfos[guestNum].isDisSat = true;
            mGuestManager.mGuestInfos[guestNum].mSitChairIndex = -1;
        }
    }

    // �� ���ڿ� ���� ������ �˻��Ѵ�.
    private int GetRandChiarIndex()
    {
        int result = -1;
        bool isSelect = false;

        // ��� ���ڰ� �� �ִ��� Ȯ��
        int count = 0;
        for (int i = 0; i < mMaxChairNum; i++)
        {
            if (mCheckChairEmpty[i] == false)
            {
                count++;
            }
        }
        if (count == mMaxChairNum)
        {
            //Debug.Log("���ڸ� �������� ���Ͽ����ϴ�");
            return -1;
        }

        // �� ���ڸ� ������ ������ ����
        while (!isSelect)
        {
            result = -1;
            result = Random.Range(0, mMaxChairNum);
            if (mCheckChairEmpty[result] == true)
            {
                isSelect = true;
                mCheckChairEmpty[result] = false;
            }
        }
        return result;
    }

    // ���� �����ʷ� ���������� �ѱ��.
    public void GetCloudToGuest(int guestNum, StoragedCloudData storagedCloudData)
    {
        GameObject.Find("CloudSpawner").GetComponent<CloudSpawner>().SpawnCloud(guestNum, storagedCloudData);
    }

    public void SetCloudData(int guestNum, StoragedCloudData storagedCloudData)
    {
        mCloudGuestNum = guestNum;
        mStorageCloudData = storagedCloudData;
        isCloudGet = true;
    }
}

