using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SOWManager : MonoBehaviour
{
    [Header("[������ ������ ��ġ�� �մ� ����]")]
    public Queue<int> mWaitGuestList;                   // �����ǿ��� ������ �ް� �Ѿ�� �մԵ��� ����Ʈ
    public List<int> mUsingGuestList;                   // ������ �������� �ڸ��� �ɾ� ������ �������� �غ� �� �մԵ��� ����Ʈ

    [HideInInspector]
    public Queue<GameObject> mWaitGuestObjectQueue;     // ��� �մ� ������Ʈ���� ������ ����Ʈ
    [HideInInspector]
    public Queue<GameObject> mUsingGuestObjectList;     // ��� �մ� ������Ʈ���� ������ ����Ʈ
    
    [Header("[���� & ��������Ʈ ����]")]
    public GameObject[] mChairPos;                      // �մ��� �ɾƼ� ������ ����� ����(����)
    public GameObject[] mWayPoint;                      // �մ��� �ɾ�ٴϸ� ��å�ϴ� ��ε�
    public int[]mSitDir = {1,1,1,1};                                // �մ��� ���ڿ� �ɴ� ���� ( �� : 0 , �� : 1�� ǥ��)

    [HideInInspector]
    public Dictionary<int, bool> mCheckChairEmpty;      // ���ڸ��� ���ڰ� ����ִ����� Ȯ���ϴ� ��ųʸ� ����
    [HideInInspector]
    public bool isNewGuest;                             // �����ǿ��� �Ѿ�ö� ���ο� �մ��� ���°�?
    [HideInInspector]
    public int mMaxChairNum;                            // ���� �ܰ迡 ���� ������ ����

    [Header("[������]")]
    [SerializeField]
    private GameObject mGuestObject;                    // �ν��Ͻ��Ͽ� ������ �մ� ������Ʈ ������

    private Guest mGuestManager;                        // GuestManager�� �����´�.
    private static SOWManager instance = null;

    [HideInInspector]
    public int mCloudGuestNum;
    [HideInInspector]
    public StoragedCloudData mStorageCloudData;
    [HideInInspector]
    public bool isCloudGet;

    private int mTempGuestNum;                          // �ӽ� �մ� ��ȣ��


    private float[][][] ChairPosList = new float[][][]{ 
        new float[][]{ new float[]{ 1.75f, -2.97f }, new float[] { -3.54f, -0.38f }, new float[] { -0.48f, 0.31f }, new float[] { 1.55f, 0.53f } } ,
        new float[][]{ new float[]{ 1.39f, -2.18f }, new float[] { -3.46f, -0.17f }, new float[] { -0.12f, -2.8f }, new float[] { -1.64f, -2.07f } } ,
        new float[][]{ new float[]{ 3.11f, -2.76f }, new float[] { 0.02f, -2.44f }, new float[] { 0.98f, 0.22f }, new float[] { 1.97f, 0.23f } } ,
        new float[][]{ new float[]{ 1.9f, -3.21f }, new float[] { -1.11f, 0.19f }, new float[] { 2.06f, 0.15f }, new float[] { -0.16f, 0.17f } }
    };

    private float[][][] WayPosList = new float[][][]{
        new float[][]{ new float[]{ -6.71f, -1.34f }, new float[] { -3.93f, -0.63f }, new float[] { -0.71f, 0.08f }, new float[] { 1.97f, -1.14f }, new float[] { 4.68f, -2.24f }, new float[] { -0.91f, 0.08f }, new float[] { -4.14f, -0.69f } } ,
        new float[][]{ new float[]{ -6.84f, -1.56f }, new float[] { -3.51f, -0.69f }, new float[] { -0.65f, -1.27f }, new float[] { 1.36f, -0.52f }, new float[] { 2.47f, 0.09f}, new float[] { -0.64f, -1.27f }, new float[] { -3.25f, -0.76f } } ,
        new float[][]{ new float[]{ -6.73f, -1.22f }, new float[] { 0.49f, 0.58f }, new float[] { 5.64f, -2.16f }, new float[] { 1.57f, -4.22f}, new float[] { -2.11f, -2.13f }, new float[] { 1.97f, -0.36f }, new float[] { 0.36f, 0.53f } } ,
        new float[][]{ new float[]{ -6.26f, -1.58f }, new float[] { -2.6f, -0.87f }, new float[] { 2.04f, -2.49f }, new float[] { 4.25f, -1.96f}, new float[] { 2.19f, -2.47f }, new float[] { -2.53f, -0.79f }, new float[] { -5.08f, -1.38f} } 
    };

    private int[][] SitDirList = new int[][] {
        new int[]{ 1,1,1,1 },
        new int[]{ 0,1,1,1 },
        new int[]{ 0,1,0,1 },
        new int[]{ 1,0,0,1 }
    };


    private void Awake()
    {
        if (null == instance)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);

            mGuestManager           = GameObject.Find("GuestManager").GetComponent<Guest>();
            mWaitGuestList          = new Queue<int>();
            mUsingGuestList         = new List<int>();
            mWaitGuestObjectQueue   = new Queue<GameObject>();
            mUsingGuestObjectList   = new Queue<GameObject>();
            mCheckChairEmpty        = new Dictionary<int, bool>();
            isNewGuest              = false;
            isCloudGet              = false;

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
        if(Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log(mUsingGuestList.Count);
        }

        // ���ο� �մ��� ������ �������� �Ѿ�� ���
        if (isNewGuest)
        {
            isNewGuest = false;

            GameObject tempObject;

            // �մ� ������Ʈ ���� �� �ʱ�ȭ
            tempObject = Instantiate(mGuestObject);

            // �մ� ���� Ȯ���� ���� �����
            Debug.Log("�մ� ����");

            // �մ� ������Ʈ�� �ش��ϴ� ��ȣ�� �־��ش�.
            tempObject.GetComponent<GuestObject>().setGuestNum(mTempGuestNum);
            tempObject.GetComponent<GuestObject>().initAnimator();
            tempObject.GetComponent<GuestObject>().init();

            // ��å�θ� �����Ѵ�. 
            tempObject.GetComponent<WayPoint>().WayPos = mWayPoint;

            // �⺻ ��ġ���� �����Ѵ�. <- ��å���� ù��° ������ ����
            tempObject.transform.position = mWayPoint[0].transform.position;

            // ������� �մ� ť�� �ش� �մ��� �߰��Ѵ�.
            mWaitGuestObjectQueue.Enqueue(tempObject);
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

        // ������ ���� ���� �մ��� �����ϱ� <- ��ȣ�ۿ�
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(vec, Vector2.zero);
            if (hit.collider != null)
            {
                GameObject hitObject = hit.transform.root.gameObject;
                if (hit.transform.gameObject.tag == "Guest")
                {
                    Debug.Log(hitObject.GetComponent<GuestObject>().mGuestNum + "�� �մ��� Ŭ���Ͽ����ϴ�.");
                    hit.transform.gameObject.GetComponent<GuestObject>().SpeakEmotion();
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
    public void InitSOW()
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

        int Season = GameObject.Find("Season Date Calc").GetComponent<SeasonDateCalc>().mSeason;
        ChangeWeatherObject(Season-1);
    }

    // ��� ����Ʈ�� �մ��� �߰������ִ� �Լ�
    public void InsertGuest(int guestNum)
    {
        mWaitGuestList.Enqueue(guestNum);
        mTempGuestNum = guestNum;
    }


    // ��� ����Ʈ���� �մ��� ���� �޴� ����Ʈ�� �߰������ִ� �Լ�
    private void MoveToUsingList(int chairNum)
    {
        // ����� ������� �̵��� �մ��� ��ȣ�� �޾ƿ´�.
        int guestNum = mWaitGuestList.Dequeue();

        // �޾ƿ� �մ��� ������ ����� ������� �ִ´�.
        mUsingGuestList.Add(guestNum);

        // tempObject�� ���� ���ڸ� �����Ѵ�.    
        GameObject tempObject = mWaitGuestObjectQueue.Dequeue();

        tempObject.GetComponent<GuestObject>().mTargetChiarIndex = chairNum;

        // ������Ʈ�� ����� ������Ʈ ������� �ִ´�.
        mUsingGuestObjectList.Enqueue(tempObject);

        // Guest�� �ο����� ���� �ε����� ����
        mGuestManager.mGuestInfo[guestNum].mSitChairIndex = chairNum;
    }

    // �Ϸ簡 ���� �� Queue�� �����ִ� ��Ƽ���� �Ҹ� ��Ƽ�� ������ش�.
    public void MakeGuestDisSat()
    {
        for (int i = 0; i < mWaitGuestList.Count; i++)
        {
            mGuestManager.mGuestInfo[mWaitGuestList.Dequeue()].isDisSat = true;
        }
        for (int i = 0; i < mUsingGuestList.Count; i++)
        {
            int guestNum = mUsingGuestList[i];
            mGuestManager.mGuestInfo[guestNum].isDisSat = true;
            mGuestManager.mGuestInfo[guestNum].mSitChairIndex = -1;
        }

        // �����ϴ� ��� ������ �����Ѵ�.
        GameObject[] Clouds = GameObject.FindGameObjectsWithTag("Cloud");
        if (Clouds != null)
        {
            foreach(GameObject i in Clouds)
            {
                i.SendMessage("StopToUse");
            }
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


    public void ChangeWeatherObject(int weather)
    {
        for(int i = 0; i< 4; i++)
        {
            mChairPos[i].transform.position = new Vector3(ChairPosList[weather][i][0], ChairPosList[weather][i][1],0f);
            mSitDir[i] = SitDirList[weather][i];
        }
        for (int i = 0; i < 7; i++)
        {
            mWayPoint[i].transform.position = new Vector3(WayPosList[weather][i][0], WayPosList[weather][i][1], 0f);
        }
    }

}

