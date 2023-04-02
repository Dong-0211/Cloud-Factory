using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SOWManagerSaveData
{
    public int[] yardGatherCount = new int[4];
}


public class SOWManager : MonoBehaviour
{
    [Header("[������ ������ ��ġ�� �մ� ����]")]
    public Queue<int> mWaitGuestList;                   // �����ǿ��� ������ �ް� �Ѿ�� �մԵ��� ����Ʈ
    public List<int> mUsingGuestList;                   // ������ �������� �ڸ��� �ɾ� ������ �������� �غ� �� �մԵ��� ����Ʈ

    [HideInInspector]
    public Queue<GameObject> mWaitGuestObjectQueue;     // ��� �մ� ������Ʈ���� ������ ����Ʈ
    [HideInInspector]
    public List<GameObject> mUsingGuestObjectList;     // ��� �մ� ������Ʈ���� ������ ����Ʈ
    
    [Header("[���� & ��������Ʈ ����]")]
    public GameObject[] mChairPos;                      // �մ��� �ɾƼ� ������ ����� ����(����)
    public GameObject[] mWayPoint;                      // �մ��� �ɾ�ٴϸ� ��å�ϴ� ��ε�
    public int[]mSitDir = {1,1,1,1};                    // �մ��� ���ڿ� �ɴ� ���� ( �� : 0 , �� : 1�� ǥ��)

    [HideInInspector]
    public Dictionary<int, bool> mCheckChairEmpty;      // ���ڸ��� ���ڰ� ����ִ����� Ȯ���ϴ� ��ųʸ� ����
    [HideInInspector]
    public bool isNewGuest;                             // �����ǿ��� �Ѿ�ö� ���ο� �մ��� ���°�?
    [HideInInspector]
    public int mMaxChairNum;                            // ���� �ܰ迡 ���� ������ ����

    [Header("[������]")]
    [SerializeField]
    public GameObject mGuestObject;                    // �ν��Ͻ��Ͽ� ������ �մ� ������Ʈ ������

    public int[] yardGatherCount = new int[4];

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
        new float[][]{ new float[]{ 1.68f, -2.81f }, new float[] { -3.52f, -0.36f }, new float[] { -0.51f, 0.39f }, new float[] { 1.55f, 0.53f } } ,
        new float[][]{ new float[]{ 1.545f, -2.154f }, new float[] { -3.436f, -0.178f }, new float[] { -0.27f, -2.8f }, new float[] { -1.64f, -2.07f } } ,
        new float[][]{ new float[]{ 3.186f, -2.646f }, new float[] { 0.134f, -2.355f }, new float[] { 1.057f, 0.297f }, new float[] { 1.97f, 0.23f } } ,
        new float[][]{ new float[]{ 1.9f, -3.21f }, new float[] { -1.11f, 0.19f }, new float[] { 2.06f, 0.15f }, new float[] { -0.16f, 0.17f } }
    };

    private float[][][] WayPosList = new float[][][]{
        new float[][]{ new float[] { -8.66f, -2.4f }, new float[]{ -6.82f, -1.43f }, new float[] { -3.91f, -0.63f }, new float[] { -0.95f, 0.13f }, new float[] { 1.97f, -0.97f }, new float[] { 4.64f, -2.1f }, new float[] { -0.91f, 0.08f }, new float[] { -4.14f, -0.51f } } ,
        new float[][]{ new float[] { -8.8f, -2.33f }, new float[]{ -6.84f, -1.56f }, new float[] { -3.81f, -0.62f }, new float[] { -0.76f, -1.1f }, new float[] { 1.36f, -0.39f }, new float[] { 2.25f, 0.2f}, new float[] { -0.64f, -1.14f }, new float[] { -4.14f, -0.64f } } ,
        new float[][]{ new float[] { -8.8f, -2.22f }, new float[]{ -6.73f, -1.39f }, new float[] { 0.49f, 0.51f }, new float[] { 5.64f, -2.16f }, new float[] { 1.57f, -4.22f}, new float[] { -2.11f, -2.13f }, new float[] { 1.97f, -0.36f }, new float[] { 0.35f, 0.59f } } ,
        new float[][]{ new float[] { -8.37f, -2.39f }, new float[]{ -6.26f, -1.58f }, new float[] { -2.6f, -0.87f }, new float[] { 2.04f, -2.49f }, new float[] { 4.25f, -1.96f}, new float[] { 2.19f, -2.47f }, new float[] { -2.53f, -0.79f }, new float[] { -4.08f, -0.92f} } 
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
            mUsingGuestObjectList   = new List<GameObject>();
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
			tempObject.GetComponent<RLHReader>().LoadHintInfo(mTempGuestNum);
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
                    // DEMO �߰� ���
                    // ��ȣ�ۿ� ���
                    return;

                    Debug.Log(hitObject.GetComponent<GuestObject>().mGuestNum + "�� �մ��� Ŭ���Ͽ����ϴ�.");
                    hit.transform.gameObject.GetComponent<GuestObject>().SpeakEmotion();
                }
                else
                {
                    Debug.Log(hit.transform.gameObject);
                }
            }
        }
        // Test. ��͵�4 ��Ḧ �̿��� ������ �������� ��� Ŭ���� ��Ʈ�� �����ϴ� ���� ��Ŭ������ �׽�Ʈ�Ѵ�.
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(vec, Vector2.zero);
            if (hit.collider != null)
            {
                GameObject hitObject = hit.transform.root.gameObject;
                if (hit.transform.gameObject.tag == "Guest")
                {
                    // DEMO �߰� ���
                    // ��Ʈ ���
                    return;

                    Debug.Log(hitObject.GetComponent<GuestObject>().mGuestNum + "�� �մ��� Ŭ���Ͽ����ϴ�.");
                    hit.transform.gameObject.GetComponent<GuestObject>().Hint();
                }
                else
                {
                    Debug.Log(hit.transform.gameObject);
                }
            }
        }

        if (isCloudGet)
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

        for (int i = 0; i < 4; i++) { yardGatherCount[i] = 1; }

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
        mUsingGuestObjectList.Add(tempObject);

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
        for (int i = 0; i < 8; i++)
        {
            mWayPoint[i].transform.position = new Vector3(WayPosList[weather][i][0], WayPosList[weather][i][1], 0f);
        }
    }

    void LoadSaveData()
    {
        // ������ �´� UI�ҷ�����

        // �մ� ������Ʈ ����Ʈ�� �����ߴ��� üũ
        // -> �־��ٸ� ������Ʈ�� �����Ͽ� �������� �ִ´�.

        // yardGatherCount[]�� int�� �ҷ�����
       

    }

}

