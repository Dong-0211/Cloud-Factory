using System.Linq; // list ����
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using AESWithJava.Con;
using System;
using UnityEditor;

public class SaveUnitManager : MonoBehaviour
{
    // SaveUnitManager �ν��Ͻ��� ��� ���� ����
    private static SaveUnitManager instance = null;

    private InventoryManager mInvenManager;
    private Guest mGuestManager;
    private TutorialManager mTutorialManager;    

    // ��� ���� �־� ���� ���̱� ������ �ߺ��� �ı�ó��
    // ��� ������ ����ǰ� �ε�� ������ �𸣱� ������
    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        mInvenManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        mGuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();
        mTutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
    }


    // Awake->OnEnable->Start������ �����ֱ�
    void OnEnable()
    {
        // �� �Ŵ����� sceneLoaded�� ü���� �Ǵ�.
        // �� �߰�
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // ü���� �ɾ �� �Լ��� �� ������ ȣ��ȴ�.
    // ���� ����� ������ ȣ��ȴ�.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Save_Func();
    }

    public void Save_Func()
    {
        // ������ ������ ���ٸ� �����ϱ�
        if (!File.Exists(Application.dataPath + "/Data/"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Data/");
        }

        // �κ񿡼��� ������ �ʿ䰡 ����
        if (SceneManager.GetActiveScene().name != "Lobby" && SceneData.Instance && !mGuestManager.isLoad) // null check && lobby ����
        {
            //String key = "key";

            // ���� �� ����
            // Save_SceneIdx(scene, mode, key); ���� ������ ������ �������� �̾��ϱ� ������ �ּ�ó��
            // ��¥ ���� ����
            Save_SeasonDate();
            // �κ��丮 ����
            Save_Inventory();

            // GuestManager ����
            Save_GuestInfo();
            Save_SOWSaveData();
            Save_SOWManagerData();
            Save_LetterControlData();

            // Ʃ�丮�� ������ ����.
            // Ʃ�丮���� ���� �� �����Ѵ�.
            //Save_Tutorial();

            Debug.Log("�����Ѵ�.");
        }
    }


    void Save_SceneIdx(Scene scene, LoadSceneMode mode, String Key)
    {
        // ���Ӱ� �ε��� ���� �����͸� �����Ѵ�
        SceneData.Instance.currentSceneIndex = scene.buildIndex;

        // �����ϴ� �Լ� ȣ��
        // �ϴ��� �ϳ��ϱ� �̷��� �ְ� �������� Ŭ���� ���� �����ϱ�
        FileStream fSceneBuildIndexStream
            // ���� ��� + ���� ���� ���� ��ο� json ���� / ���� SAVE
            = new FileStream(Application.dataPath + "/Data/SceneBuildIndex.json", FileMode.OpenOrCreate);

        // sData�� ������ ����ȭ�Ѵ�        
        // ���� �� �ε��� ����
        string sSceneData = JsonConvert.SerializeObject(SceneData.Instance.currentSceneIndex);
        // ��ȣȭ
        sSceneData = AESWithJava.Con.Program.Encrypt(sSceneData, Key);

        // text �����ͷ� ���ڵ��Ѵ�
        byte[] bSceneData = Encoding.UTF8.GetBytes(sSceneData);

        // text �����͸� �ۼ��Ѵ�
        fSceneBuildIndexStream.Write(bSceneData, 0, bSceneData.Length);
        fSceneBuildIndexStream.Close();
    }

    void Save_SeasonDate()
    {
        // jsonUtility
        string mSeasonDatePath = Path.Combine(Application.dataPath + "/Data/", "SeasonDate.json");

        // �����ϴ� ���� Ŭ���� ����
        // Class�� Json���� �ѱ�� self ���� �ݺ��� �Ͼ�� ������
        // �ܺζ��̺귯���� �����ϰ� ����Ƽ Utility�� Ȱ���Ѵ�.

        // �ϳ��� json���Ͽ� �����ϱ� ���ؼ� Ŭ���� ���Ӱ� ���� �� Ŭ���� ������ ����
        // ���ο� ������Ʈ�� Ŭ���� ���� �� ������Ʈ
        GameObject gSeasonDate = new GameObject();
        SeasonDateCalc seasonDate = gSeasonDate.AddComponent<SeasonDateCalc>();

        // ������Ʈ
        seasonDate.mSecond = SeasonDateCalc.Instance.mSecond;
        seasonDate.mDay = SeasonDateCalc.Instance.mDay;
        seasonDate.mSeason = SeasonDateCalc.Instance.mSeason;
        seasonDate.mYear = SeasonDateCalc.Instance.mYear;

        // Ŭ������ �ɹ��������� json���Ϸ� ��ȯ�Ѵ� (class, prettyPrint) true�� �б� ���� ���·� ��������
        // seasonDataSaveBox Ŭ���� ������ json ��ȯ
        string sSeasonData = JsonUtility.ToJson(gSeasonDate.GetComponent<SeasonDateCalc>(), true);
        Debug.Log(sSeasonData);
        // ��ȣȭ
        // sSeasonData = AESWithJava.Con.Program.Encrypt(sSeasonData, key);

        //Debug.Log(sSeasonData);

        File.WriteAllText(mSeasonDatePath, sSeasonData);
    }

    void Save_Inventory()
    {
        if (mInvenManager) // null check
        {
            // ��ȣȭ�� ���߿� �ѹ��� �ϱ�

            // ������ �ִٸ�
            if (System.IO.File.Exists(Path.Combine(Application.dataPath + "/Data/", "InventoryData.json")))
            {
                // ����
                System.IO.File.Delete(Path.Combine(Application.dataPath + "/Data/", "InventoryData.json"));

            }
            // ���� �� �ٽ� ����
            // ������, �������� ���� �� ��쿡 json�� �ʱ�ȭ ���� �ʰ� ���� ����� ������ ���� �ִ� �����ͺ��� ���� ���
            // �ڿ� ���� ������ ����� ���Ͽ� ������ȭ ���� �߻���
            // �������� �����ϴ� ��찡 �ƴ� ��� (ex, ���� �� �ε��� ��)�� ��� ����
            // ���� ��Ʈ�� ����
            FileStream stream = new FileStream(Application.dataPath + "/Data/InventoryData.json", FileMode.OpenOrCreate);

            // ������ ������ ��� Ŭ���� ����
            InventoryData mInventoryData = new InventoryData();

            // ������ ������Ʈ
            mInventoryData.mType = mInvenManager.mType.ToList();
            mInventoryData.mCnt = mInvenManager.mCnt.ToList();
            mInventoryData.minvenLevel = mInvenManager.minvenLevel;
            mInventoryData.mMaxInvenCnt = mInvenManager.mMaxInvenCnt;
            mInventoryData.mMaxStockCnt = mInvenManager.mMaxStockCnt;

            // ������ ����ȭ
            string jInventoryData = JsonConvert.SerializeObject(mInventoryData);

            // json �����͸� Encoding.UTF8�� �Լ��� ����Ʈ �迭�� �����
            byte[] bInventoryData = Encoding.UTF8.GetBytes(jInventoryData);
            Debug.Log(jInventoryData);
            // �ش� ���� ��Ʈ���� ���´�.                
            stream.Write(bInventoryData, 0, bInventoryData.Length);
            // ��Ʈ�� �ݱ�
            stream.Close();
        }
    }

    void Save_GuestInfo()
    {
        if (mGuestManager) // null check
        {
            // ������ �ִٸ�
            if (System.IO.File.Exists(Path.Combine(Application.dataPath + "/Data/", "GuestManagerData.json")))
            {
                // ����
                System.IO.File.Delete(Path.Combine(Application.dataPath + "/Data/", "GuestManagerData.json"));
            }

            FileStream stream = new FileStream(Application.dataPath + "/Data/GuestManagerData.json", FileMode.OpenOrCreate);

            // ������ ������ ��� Ŭ���� ����
            GuestManagerSaveData mGuestManagerData = new GuestManagerSaveData();

            // ������ ������Ʈ
            {
                const int NUM_OF_GUEST = 20;
                GuestInfoSaveData[] GuestInfos = new GuestInfoSaveData[NUM_OF_GUEST];

                for(int i = 0; i < NUM_OF_GUEST; i++)
                {
                    GuestInfos info = mGuestManager.mGuestInfo[i];
                    GuestInfoSaveData data = new GuestInfoSaveData();

                    data.mEmotion = info.mEmotion;
                    data.mSatatisfaction = info.mSatatisfaction;
                    data.mSatVariation = info.mSatVariation;
                    data.isChosen = info.isChosen;
                    data.isDisSat = info.isDisSat;
                    data.isCure = info.isCure;
                    data.mVisitCount = info.mVisitCount;
                    data.mNotVisitCount = info.mNotVisitCount;
                    data.mSitChairIndex = info.mSitChairIndex;
                    data.isUsing = info.isUsing;

                    GuestInfos[i] = data;
                }

                mGuestManagerData.GuestInfos = GuestInfos.Clone() as GuestInfoSaveData[];
                //mGuestManagerData.GuestInfos = mGuestManager.mGuestInfo.Clone() as GuestInfoSaveData[];
            }
            mGuestManagerData.isGuestLivingRoom = /*���⸸ �ְ������ ������ ��*/ mGuestManager.isGuestInLivingRoom;
            mGuestManagerData.isTimeToTakeGuest = mGuestManager.isTimeToTakeGuest;
            mGuestManagerData.mGuestIndex = mGuestManager.mGuestIndex;
            mGuestManagerData.mTodayGuestList = mGuestManager.mTodayGuestList.Clone() as int[];
            mGuestManagerData.mGuestCount = mGuestManager.mGuestCount;
            mGuestManagerData.mGuestTime = mGuestManager.mGuestTime;

            // ������ ����ȭ
            string jData = JsonConvert.SerializeObject(mGuestManagerData);

            // json �����͸� Encoding.UTF8�� �Լ��� ����Ʈ �迭�� �����
            byte[] bData = Encoding.UTF8.GetBytes(jData);
            Debug.Log(jData);
            // �ش� ���� ��Ʈ���� ���´�.                
            stream.Write(bData, 0, bData.Length);
            // ��Ʈ�� �ݱ�
            stream.Close();
        }
    }

    void Save_SOWSaveData()
    {
        if (mGuestManager) // null check
        {
            // ������ �ִٸ�
            if (System.IO.File.Exists(Path.Combine(Application.dataPath + "/Data/", "SOWSaveData.json")))
            {
                // ����
                System.IO.File.Delete(Path.Combine(Application.dataPath + "/Data/", "SOWSaveData.json"));
            }

            FileStream stream = new FileStream(Application.dataPath + "/Data/SOWSaveData.json", FileMode.OpenOrCreate);

            // ������ ������ ��� Ŭ���� ����
            SOWSaveData mGuestManagerData = new SOWSaveData();            

            // UsingObjectsData�� WaitObjectsData�� �������� ä���.
            SOWManager mSOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();

            if (mSOWManager == null) return;

            List<GuestObjectSaveData> UsingObjectsData = new List<GuestObjectSaveData>();
            List<GuestObjectSaveData> WaitObjectsData = new List<GuestObjectSaveData>();

            foreach(GameObject obj in mSOWManager.mWaitGuestObjectQueue)
            {
                GuestObjectSaveData temp = new GuestObjectSaveData();
                temp.xPos = obj.transform.position.x;
                temp.yPos = obj.transform.position.y;
                temp.xScale = obj.transform.localScale.x;

                GuestObject Info = obj.GetComponent<GuestObject>();

                temp.mLimitTime = Info.mLimitTime;
                temp.mGuestNum = Info.mGuestNum;
                temp.mTargetChairIndex = Info.mTargetChiarIndex;
                temp.isSit = Info.isSit;
                temp.isUsing = Info.isMove;
                temp.isGotoEntrance = Info.isGotoEntrance;
                temp.isEndUsingCloud = Info.isEndUsingCloud;

                WayPoint wayPoint = obj.GetComponent<WayPoint>();

                temp.WayNum = wayPoint.WayNum;

                WaitObjectsData.Add(temp);
            }

            /*
            foreach (GameObject obj in mSOWManager.mUsingGuestObjectList)
            {
                GuestObjectSaveData temp = new GuestObjectSaveData();
                temp.xPos = obj.transform.position.x;
                temp.yPos = obj.transform.position.y;
                temp.xScale = obj.transform.localScale.x;

                GuestObject Info = obj.GetComponent<GuestObject>();

                temp.mGuestNum = Info.mGuestNum;
                temp.mLimitTime = Info.mLimitTime;
                temp.mTargetChairIndex = Info.mTargetChiarIndex;
                temp.isSit = Info.isSit;
                temp.isUsing = Info.isMove;
                temp.isGotoEntrance = Info.isGotoEntrance;
                temp.isEndUsingCloud = Info.isEndUsingCloud;

                WayPoint wayPoint = obj.GetComponent<WayPoint>();

                temp.WayNum = wayPoint.WayNum;

                UsingObjectsData.Add(temp);
            }
            */

            //string jBData = JsonConvert.SerializeObject(WaitObjectsData);
            //Debug.Log("=======Save :  WaitObjectsData  =========");
            //Debug.Log(jBData);
            //Debug.Log("=======Save=========");

            //string jCData = JsonConvert.SerializeObject(UsingObjectsData);
            //Debug.Log("=======Save :  UsingObjectsData  =========");
            //Debug.Log(jCData);
            //Debug.Log("=======Save=========");

            mGuestManager.SaveSOWdatas.mCheckChairEmpty     = mSOWManager.mCheckChairEmpty;
            mGuestManager.SaveSOWdatas.WaitObjectsData      = WaitObjectsData.ToList<GuestObjectSaveData>();
            mGuestManager.SaveSOWdatas.UsingObjectsData     = UsingObjectsData.ToList<GuestObjectSaveData>();
            mGuestManager.SaveSOWdatas.mMaxChairNum         = mSOWManager.mMaxChairNum;

            // ������ ������Ʈ    
            mGuestManagerData.UsingObjectsData  = mGuestManager.SaveSOWdatas.UsingObjectsData.ToList();
            mGuestManagerData.WaitObjectsData   = mGuestManager.SaveSOWdatas.WaitObjectsData.ToList();
            mGuestManagerData.mMaxChairNum      = mGuestManager.SaveSOWdatas.mMaxChairNum;
            mGuestManagerData.mCheckChairEmpty  = new Dictionary<int, bool>(mGuestManager.SaveSOWdatas.mCheckChairEmpty);

            // ������ ����ȭ
            string jData = JsonConvert.SerializeObject(mGuestManagerData);

            // json �����͸� Encoding.UTF8�� �Լ��� ����Ʈ �迭�� �����
            byte[] bData = Encoding.UTF8.GetBytes(jData);
            Debug.Log(jData);
            // �ش� ���� ��Ʈ���� ���´�.                
            stream.Write(bData, 0, bData.Length);
            // ��Ʈ�� �ݱ�
            stream.Close();
        }
    }

    public void Save_Tutorial()
    {
        if (mTutorialManager) // null check
        {
            // ��ȣȭ�� ���߿� �ѹ��� �ϱ�

            // ������ �ִٸ�
            if (System.IO.File.Exists(Path.Combine(Application.dataPath + "/Data/", "TutorialData.json")))
            {
                // ����
                System.IO.File.Delete(Path.Combine(Application.dataPath + "/Data/", "TutorialData.json"));

            }
            // ���� �� �ٽ� ����
            // ������, �������� ���� �� ��쿡 json�� �ʱ�ȭ ���� �ʰ� ���� ����� ������ ���� �ִ� �����ͺ��� ���� ���
            // �ڿ� ���� ������ ����� ���Ͽ� ������ȭ ���� �߻���
            // �������� �����ϴ� ��찡 �ƴ� ��� (ex, ���� �� �ε��� ��)�� ��� ����
            // ���� ��Ʈ�� ����
            FileStream stream = new FileStream(Application.dataPath + "/Data/TutorialData.json", FileMode.OpenOrCreate);

            // ������ ������ ��� Ŭ���� ����
            TutorialData mTutorialData = new TutorialData();

            // ������ ������Ʈ 
            mTutorialData.isTutorial = mTutorialManager.isTutorial;

            // ������ ����ȭ
            string jTutorialData = JsonConvert.SerializeObject(mTutorialData);

            // json �����͸� Encoding.UTF8�� �Լ��� ����Ʈ �迭�� �����
            byte[] bTutorialData = Encoding.UTF8.GetBytes(jTutorialData);
            Debug.Log(jTutorialData);
            // �ش� ���� ��Ʈ���� ���´�.                
            stream.Write(bTutorialData, 0, bTutorialData.Length);
            // ��Ʈ�� �ݱ�
            stream.Close();
        }
    }

    public void Save_SOWManagerData()
    {
        SOWManager mSOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();

        if (mSOWManager == null) 
            return;

        // ������ �ִٸ�
        if (System.IO.File.Exists(Path.Combine(Application.dataPath + "/Data/", "SOWManagerData.json")))
        {
            // ����
            System.IO.File.Delete(Path.Combine(Application.dataPath + "/Data/", "SOWManagerData.json"));

        }
        // ���� �� �ٽ� ����
        // ������, �������� ���� �� ��쿡 json�� �ʱ�ȭ ���� �ʰ� ���� ����� ������ ���� �ִ� �����ͺ��� ���� ���
        // �ڿ� ���� ������ ����� ���Ͽ� ������ȭ ���� �߻���
        // �������� �����ϴ� ��찡 �ƴ� ��� (ex, ���� �� �ε��� ��)�� ��� ����
        // ���� ��Ʈ�� ����
        FileStream stream = new FileStream(Application.dataPath + "/Data/SOWManagerData.json", FileMode.OpenOrCreate);

        // ������ ������ ��� Ŭ���� ����
        SOWManagerSaveData mSOWManagerData = new SOWManagerSaveData();

        // ������ ������Ʈ
        mSOWManagerData.yardGatherCount = mSOWManager.yardGatherCount.Clone() as int[];

        // ������ ����ȭ
        string jSOWManagerData = JsonConvert.SerializeObject(mSOWManagerData);

        // json �����͸� Encoding.UTF8�� �Լ��� ����Ʈ �迭�� �����
        byte[] bTutorialData = Encoding.UTF8.GetBytes(jSOWManagerData);
        Debug.Log(jSOWManagerData);
        // �ش� ���� ��Ʈ���� ���´�.                
        stream.Write(bTutorialData, 0, bTutorialData.Length);
        // ��Ʈ�� �ݱ�
        stream.Close();
    }

    public void Save_LetterControlData()
    {
        LetterController mLetterController = GameObject.Find("GuestManager").GetComponent<LetterController>();

        if (mLetterController == null)
            return;

        // ������ �ִٸ�
        if (System.IO.File.Exists(Path.Combine(Application.dataPath + "/Data/", "LetterControllerData.json")))
        {
            // ����
            System.IO.File.Delete(Path.Combine(Application.dataPath + "/Data/", "LetterControllerData.json"));

        }
        // ���� �� �ٽ� ����
        // ������, �������� ���� �� ��쿡 json�� �ʱ�ȭ ���� �ʰ� ���� ����� ������ ���� �ִ� �����ͺ��� ���� ���
        // �ڿ� ���� ������ ����� ���Ͽ� ������ȭ ���� �߻���
        // �������� �����ϴ� ��찡 �ƴ� ��� (ex, ���� �� �ε��� ��)�� ��� ����
        // ���� ��Ʈ�� ����
        FileStream stream = new FileStream(Application.dataPath + "/Data/LetterControllerData.json", FileMode.OpenOrCreate);

        // ������ ������ ��� Ŭ���� ����
        LetterControllerData mLetterControllerData = new LetterControllerData();

        // ������ ������Ʈ
        mLetterControllerData.satGuestList = mLetterController.satGuestList.Clone() as int[];
        mLetterControllerData.listCount = mLetterController.listCount;

        // ������ ����ȭ
        string jLetterControllerData = JsonConvert.SerializeObject(mLetterControllerData);

        // json �����͸� Encoding.UTF8�� �Լ��� ����Ʈ �迭�� �����
        byte[] bLetterControllerData = Encoding.UTF8.GetBytes(jLetterControllerData);
        Debug.Log(jLetterControllerData);
        // �ش� ���� ��Ʈ���� ���´�.                
        stream.Write(bLetterControllerData, 0, bLetterControllerData.Length);
        // ��Ʈ�� �ݱ�
        stream.Close();
    }

    // ����� ��
    void OnDisable()
    {
        // ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}