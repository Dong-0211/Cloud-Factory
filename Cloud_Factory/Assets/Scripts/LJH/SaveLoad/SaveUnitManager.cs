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
        // ������ ������ ���ٸ� �����ϱ�
        if (!File.Exists(Application.dataPath + "/Data/"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Data/");
        }

        // �κ񿡼��� ������ �ʿ䰡 ����
        if (scene.name != "Lobby" && SceneData.Instance) // null check && lobby ����
        {
            String key = "key";

            // ���� �� ����
            Save_SceneIdx(scene, mode, key);
            // ��¥ ���� ����
            Save_SeasonDate();
            // �κ��丮 ����
            Save_Inventory();

            // GuestManager ����
            Save_GuestInfo();
            Save_SOWSaveData();
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
            // ���� ����Ʈ�� �����ϸ� �� �ǵɵ�
            // mGuestManagerData.GuestInfos = mGuestManager.mGuestInfo.Clone() as GuestInfoSaveData[];
                        
            mGuestManagerData.isGuestLivingRoom = /*���⸸ �ְ������ ������ ��*/ mGuestManager.isGuestInLivingRoom;
            mGuestManagerData.isTimeToTakeGuest = mGuestManager.isTimeToTakeGuest;
            mGuestManagerData.mGuestIndex = mGuestManager.mGuestIndex;
            mGuestManagerData.mTodayGuestList = mGuestManager.mTodayGuestList.Clone() as int[];
            mGuestManagerData.mGuestCount = mGuestManager.mGuestCount;
            mGuestManagerData.mGuestTime = mGuestManager.mGuestTime;

            // ������ ����ȭ
            string jInventoryData = JsonConvert.SerializeObject(mGuestManagerData);

            // json �����͸� Encoding.UTF8�� �Լ��� ����Ʈ �迭�� �����
            byte[] bInventoryData = Encoding.UTF8.GetBytes(jInventoryData);
            Debug.Log(jInventoryData);
            // �ش� ���� ��Ʈ���� ���´�.                
            stream.Write(bInventoryData, 0, bInventoryData.Length);
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

            // ������ ������Ʈ            
            mGuestManagerData.UsingObjectsData = mGuestManager.SaveSOWdatas.UsingObjectsData.ToList();
            mGuestManagerData.WaitObjectsData = mGuestManager.SaveSOWdatas.WaitObjectsData.ToList();
            mGuestManagerData.mMaxChairNum = mGuestManager.SaveSOWdatas.mMaxChairNum;
            mGuestManagerData.mCheckChairEmpty = new Dictionary<int, bool>(mGuestManager.SaveSOWdatas.mCheckChairEmpty);

            // ������ ����ȭ
            string jInventoryData = JsonConvert.SerializeObject(mGuestManagerData);

            // json �����͸� Encoding.UTF8�� �Լ��� ����Ʈ �迭�� �����
            byte[] bInventoryData = Encoding.UTF8.GetBytes(jInventoryData);
            Debug.Log(jInventoryData);
            // �ش� ���� ��Ʈ���� ���´�.                
            stream.Write(bInventoryData, 0, bInventoryData.Length);
            // ��Ʈ�� �ݱ�
            stream.Close();
        }
    }

    // ����� ��
    void OnDisable()
    {
        // ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}