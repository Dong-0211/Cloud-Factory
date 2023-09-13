using System.Linq; // list 복사
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
    // SaveUnitManager 인스턴스를 담는 전역 변수
    private static SaveUnitManager instance = null;

    private InventoryManager mInvenManager;
    private Guest mGuestManager;
    private TutorialManager mTutorialManager;    

    // 모든 씬에 넣어 놓을 것이기 때문에 중복은 파괴처리
    // 어느 씬에서 저장되고 로드될 것인지 모르기 때문에
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


    // Awake->OnEnable->Start순으로 생명주기
    void OnEnable()
    {
        // 씬 매니저의 sceneLoaded에 체인을 건다.
        // 씬 추가
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 체인을 걸어서 이 함수는 매 씬마다 호출된다.
    // 씬이 변경될 때마다 호출된다.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Save_Func();
    }

    public void Save_Func()
    {
        // 데이터 폴더가 없다면 생성하기
        if (!File.Exists(Application.dataPath + "/Data/"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Data/");
        }

        // 로비에서는 저장할 필요가 없음
        if (SceneManager.GetActiveScene().name != "Lobby" && SceneData.Instance && !mGuestManager.isLoad) // null check && lobby 제한
        {
            //String key = "key";

            // 현재 씬 저장
            // Save_SceneIdx(scene, mode, key); 이제 무조건 날씨의 공간으로 이어하기 때문에 주석처리
            // 날짜 계절 저장
            Save_SeasonDate();
            // 인벤토리 저장
            Save_Inventory();

            // GuestManager 저장
            Save_GuestInfo();
            Save_SOWSaveData();
            Save_SOWManagerData();
            Save_LetterControlData();
            Save_SoundData();

            // Save_HistoryData();

            // 튜토리얼 데이터 저장.
            // 튜토리얼이 끝날 때 저장한다.
            //Save_Tutorial();

#if UNITY_EDITOR
            Debug.Log("저장한다.");
#endif
        }
    }


    void Save_SceneIdx(Scene scene, LoadSceneMode mode, String Key)
    {
        // 새롭게 로딩된 씬의 데이터를 저장한다
        SceneData.Instance.currentSceneIndex = scene.buildIndex;

        // 저장하는 함수 호출
        // 일단은 하나니까 이렇게 넣고 많아지면 클래스 만들어서 정리하기
        FileStream fSceneBuildIndexStream
            // 파일 경로 + 내가 만든 폴더 경로에 json 저장 / 모드는 SAVE
            = new FileStream(Application.dataPath + "/Data/SceneBuildIndex.json", FileMode.OpenOrCreate);

        // sData로 변수를 직렬화한다        
        // 현재 씬 인덱스 저장
        string sSceneData = JsonConvert.SerializeObject(SceneData.Instance.currentSceneIndex);
        // 암호화
        sSceneData = AESWithJava.Con.Program.Encrypt(sSceneData, Key);

        // text 데이터로 인코딩한다
        byte[] bSceneData = Encoding.UTF8.GetBytes(sSceneData);

        // text 데이터를 작성한다
        fSceneBuildIndexStream.Write(bSceneData, 0, bSceneData.Length);
        fSceneBuildIndexStream.Close();
    }

    void Save_SeasonDate()
    {
        // jsonUtility
        string mSeasonDatePath = Path.Combine(Application.dataPath + "/Data/", "SeasonDate.json");

        // 저장하는 공간 클래스 선언
        // Class를 Json으로 넘기면 self 참조 반복이 일어나기 때문에
        // 외부라이브러리를 제외하고 유니티 Utility를 활용한다.

        // 하나의 json파일에 저장하기 위해서 클래스 새롭게 생성 후 클래스 단위로 저장
        // 새로운 오브젝트에 클래스 선언 후 업데이트
        GameObject gSeasonDate = new GameObject();
        SeasonDateCalc seasonDate = gSeasonDate.AddComponent<SeasonDateCalc>();

        // 업데이트
        seasonDate.mSecond = SeasonDateCalc.Instance.mSecond;
        seasonDate.mDay = SeasonDateCalc.Instance.mDay;
        seasonDate.mSeason = SeasonDateCalc.Instance.mSeason;
        seasonDate.mYear = SeasonDateCalc.Instance.mYear;

        // 클래스의 맴버변수들을 json파일로 변환한다 (class, prettyPrint) true면 읽기 좋은 형태로 저장해줌
        // seasonDataSaveBox 클래스 단위로 json 변환
        string sSeasonData = JsonUtility.ToJson(gSeasonDate.GetComponent<SeasonDateCalc>(), true);
#if UNITY_EDITOR
        Debug.Log(sSeasonData);
#endif
        // 암호화
        // sSeasonData = AESWithJava.Con.Program.Encrypt(sSeasonData, key);

#if UNITY_EDITOR
        //Debug.Log(sSeasonData);
#endif

        File.WriteAllText(mSeasonDatePath, sSeasonData);
    }

    void Save_Inventory()
    {
        if (mInvenManager) // null check
        {
            // 암호화는 나중에 한번에 하기

            // 파일이 있다면
            if (System.IO.File.Exists(Path.Combine(Application.dataPath + "/Data/", "InventoryData.json")))
            {
                // 삭제
                System.IO.File.Delete(Path.Combine(Application.dataPath + "/Data/", "InventoryData.json"));

            }
            // 삭제 후 다시 개방
            // 이유는, 동적으로 생성 될 경우에 json을 초기화 하지 않고 덮어 씌우기 때문에 전에 있던 데이터보다 적을 경우
            // 뒤에 남는 쓰레기 값들로 인하여 역직렬화 오류 발생함
            // 동적으로 생성하는 경우가 아닌 경우 (ex, 현재 씬 인덱스 등)은 상관 없음
            // 파일 스트림 개방
            FileStream stream = new FileStream(Application.dataPath + "/Data/InventoryData.json", FileMode.OpenOrCreate);

            // 저장할 변수가 담긴 클래스 생성
            InventoryData mInventoryData = new InventoryData();

            // 데이터 업데이트
            mInventoryData.mType = mInvenManager.mType.ToList();
            mInventoryData.mCnt = mInvenManager.mCnt.ToList();
            mInventoryData.minvenLevel = mInvenManager.minvenLevel;
            mInventoryData.mMaxInvenCnt = mInvenManager.mMaxInvenCnt;
            mInventoryData.mMaxStockCnt = mInvenManager.mMaxStockCnt;

            // 데이터 직렬화
            string jInventoryData = JsonConvert.SerializeObject(mInventoryData);

            // json 데이터를 Encoding.UTF8의 함수로 바이트 배열로 만들고
            byte[] bInventoryData = Encoding.UTF8.GetBytes(jInventoryData);
#if UNITY_EDITOR
            Debug.Log(jInventoryData);
#endif
            // 해당 파일 스트림에 적는다.                
            stream.Write(bInventoryData, 0, bInventoryData.Length);
            // 스트림 닫기
            stream.Close();
        }
    }

    void Save_GuestInfo()
    {
        if (mGuestManager) // null check
        {
            // 파일이 있다면
            if (System.IO.File.Exists(Path.Combine(Application.dataPath + "/Data/", "GuestManagerData.json")))
            {
                // 삭제
                System.IO.File.Delete(Path.Combine(Application.dataPath + "/Data/", "GuestManagerData.json"));
            }

            FileStream stream = new FileStream(Application.dataPath + "/Data/GuestManagerData.json", FileMode.OpenOrCreate);

            // 저장할 변수가 담긴 클래스 생성
            GuestManagerSaveData mGuestManagerData = new GuestManagerSaveData();

            // 데이터 업데이트
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
            mGuestManagerData.isGuestLivingRoom = /*여기만 넣고싶은거 넣으면 댐*/ mGuestManager.isGuestInLivingRoom;
            mGuestManagerData.isTimeToTakeGuest = mGuestManager.isTimeToTakeGuest;
            mGuestManagerData.mGuestIndex = mGuestManager.mGuestIndex;
            mGuestManagerData.mTodayGuestList = mGuestManager.mTodayGuestList.Clone() as int[];
            mGuestManagerData.mGuestCount = mGuestManager.mGuestCount;
            mGuestManagerData.mGuestTime = mGuestManager.mGuestTime;

            // 데이터 직렬화
            string jData = JsonConvert.SerializeObject(mGuestManagerData);

            // json 데이터를 Encoding.UTF8의 함수로 바이트 배열로 만들고
            byte[] bData = Encoding.UTF8.GetBytes(jData);
#if UNITY_EDITOR
            Debug.Log(jData);
#endif
            // 해당 파일 스트림에 적는다.                
            stream.Write(bData, 0, bData.Length);
            // 스트림 닫기
            stream.Close();
        }
    }

    void Save_SOWSaveData()
    {
        if (mGuestManager) // null check
        {
            // 파일이 있다면
            if (System.IO.File.Exists(Path.Combine(Application.dataPath + "/Data/", "SOWSaveData.json")))
            {
                // 삭제
                System.IO.File.Delete(Path.Combine(Application.dataPath + "/Data/", "SOWSaveData.json"));
            }

            FileStream stream = new FileStream(Application.dataPath + "/Data/SOWSaveData.json", FileMode.OpenOrCreate);

            // 저장할 변수가 담긴 클래스 생성
            SOWSaveData mGuestManagerData = new SOWSaveData();            

            // UsingObjectsData와 WaitObjectsData의 정보들을 채운다.
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
#if UNITY_EDITOR
            //Debug.Log("=======Save :  WaitObjectsData  =========");
            //Debug.Log(jBData);
            //Debug.Log("=======Save=========");
#endif

            //string jCData = JsonConvert.SerializeObject(UsingObjectsData);
#if UNITY_EDITOR
            //Debug.Log("=======Save :  UsingObjectsData  =========");
            //Debug.Log(jCData);
            //Debug.Log("=======Save=========");
#endif

            mGuestManager.SaveSOWdatas.mCheckChairEmpty     = mSOWManager.mCheckChairEmpty;
            mGuestManager.SaveSOWdatas.WaitObjectsData      = WaitObjectsData.ToList<GuestObjectSaveData>();
            mGuestManager.SaveSOWdatas.UsingObjectsData     = UsingObjectsData.ToList<GuestObjectSaveData>();
            mGuestManager.SaveSOWdatas.mMaxChairNum         = mSOWManager.mMaxChairNum;

            // 데이터 업데이트    
            mGuestManagerData.UsingObjectsData  = mGuestManager.SaveSOWdatas.UsingObjectsData.ToList();
            mGuestManagerData.WaitObjectsData   = mGuestManager.SaveSOWdatas.WaitObjectsData.ToList();
            mGuestManagerData.mMaxChairNum      = mGuestManager.SaveSOWdatas.mMaxChairNum;
            mGuestManagerData.mCheckChairEmpty  = new Dictionary<int, bool>(mGuestManager.SaveSOWdatas.mCheckChairEmpty);

            // 데이터 직렬화
            string jData = JsonConvert.SerializeObject(mGuestManagerData);

            // json 데이터를 Encoding.UTF8의 함수로 바이트 배열로 만들고
            byte[] bData = Encoding.UTF8.GetBytes(jData);
#if UNITY_EDITOR
            Debug.Log(jData);
#endif
            // 해당 파일 스트림에 적는다.                
            stream.Write(bData, 0, bData.Length);
            // 스트림 닫기
            stream.Close();
        }
    }

    public void Save_Tutorial()
    {
        if (mTutorialManager) // null check
        {
            // 암호화는 나중에 한번에 하기

            // 파일이 있다면
            if (System.IO.File.Exists(Path.Combine(Application.dataPath + "/Data/", "TutorialData.json")))
            {
                // 삭제
                System.IO.File.Delete(Path.Combine(Application.dataPath + "/Data/", "TutorialData.json"));

            }
            // 삭제 후 다시 개방
            // 이유는, 동적으로 생성 될 경우에 json을 초기화 하지 않고 덮어 씌우기 때문에 전에 있던 데이터보다 적을 경우
            // 뒤에 남는 쓰레기 값들로 인하여 역직렬화 오류 발생함
            // 동적으로 생성하는 경우가 아닌 경우 (ex, 현재 씬 인덱스 등)은 상관 없음
            // 파일 스트림 개방
            FileStream stream = new FileStream(Application.dataPath + "/Data/TutorialData.json", FileMode.OpenOrCreate);

            // 저장할 변수가 담긴 클래스 생성
            TutorialData mTutorialData = new TutorialData();

            // 데이터 업데이트 
            mTutorialData.isTutorial = mTutorialManager.isTutorial;

            // 데이터 직렬화
            string jTutorialData = JsonConvert.SerializeObject(mTutorialData);

            // json 데이터를 Encoding.UTF8의 함수로 바이트 배열로 만들고
            byte[] bTutorialData = Encoding.UTF8.GetBytes(jTutorialData);
#if UNITY_EDITOR
            Debug.Log(jTutorialData);
#endif
            // 해당 파일 스트림에 적는다.                
            stream.Write(bTutorialData, 0, bTutorialData.Length);
            // 스트림 닫기
            stream.Close();
        }
    }

    public void Save_SOWManagerData()
    {
        SOWManager mSOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();

        if (mSOWManager == null) 
            return;

        // 파일이 있다면
        if (System.IO.File.Exists(Path.Combine(Application.dataPath + "/Data/", "SOWManagerData.json")))
        {
            // 삭제
            System.IO.File.Delete(Path.Combine(Application.dataPath + "/Data/", "SOWManagerData.json"));

        }
        // 삭제 후 다시 개방
        // 이유는, 동적으로 생성 될 경우에 json을 초기화 하지 않고 덮어 씌우기 때문에 전에 있던 데이터보다 적을 경우
        // 뒤에 남는 쓰레기 값들로 인하여 역직렬화 오류 발생함
        // 동적으로 생성하는 경우가 아닌 경우 (ex, 현재 씬 인덱스 등)은 상관 없음
        // 파일 스트림 개방
        FileStream stream = new FileStream(Application.dataPath + "/Data/SOWManagerData.json", FileMode.OpenOrCreate);

        // 저장할 변수가 담긴 클래스 생성
        SOWManagerSaveData mSOWManagerData = new SOWManagerSaveData();

        // 데이터 업데이트
        mSOWManagerData.yardGatherCount = mSOWManager.yardGatherCount.Clone() as int[];

        // 데이터 직렬화
        string jSOWManagerData = JsonConvert.SerializeObject(mSOWManagerData);

        // json 데이터를 Encoding.UTF8의 함수로 바이트 배열로 만들고
        byte[] bTutorialData = Encoding.UTF8.GetBytes(jSOWManagerData);
#if UNITY_EDITOR
        Debug.Log(jSOWManagerData);
#endif
        // 해당 파일 스트림에 적는다.                
        stream.Write(bTutorialData, 0, bTutorialData.Length);
        // 스트림 닫기
        stream.Close();
    }

    public void Save_LetterControlData()
    {
        LetterController mLetterController = GameObject.Find("GuestManager").GetComponent<LetterController>();

        if (mLetterController == null)
            return;

        // 파일이 있다면
        if (System.IO.File.Exists(Path.Combine(Application.dataPath + "/Data/", "LetterControllerData.json")))
        {
            // 삭제
            System.IO.File.Delete(Path.Combine(Application.dataPath + "/Data/", "LetterControllerData.json"));

        }
        // 삭제 후 다시 개방
        // 이유는, 동적으로 생성 될 경우에 json을 초기화 하지 않고 덮어 씌우기 때문에 전에 있던 데이터보다 적을 경우
        // 뒤에 남는 쓰레기 값들로 인하여 역직렬화 오류 발생함
        // 동적으로 생성하는 경우가 아닌 경우 (ex, 현재 씬 인덱스 등)은 상관 없음
        // 파일 스트림 개방
        FileStream stream = new FileStream(Application.dataPath + "/Data/LetterControllerData.json", FileMode.OpenOrCreate);

        // 저장할 변수가 담긴 클래스 생성
        LetterControllerData mLetterControllerData = new LetterControllerData();

        // 데이터 업데이트
        mLetterControllerData.satGuestList = mLetterController.satGuestList.Clone() as int[];
        mLetterControllerData.listCount = mLetterController.listCount;

        // 데이터 직렬화
        string jLetterControllerData = JsonConvert.SerializeObject(mLetterControllerData);

        // json 데이터를 Encoding.UTF8의 함수로 바이트 배열로 만들고
        byte[] bLetterControllerData = Encoding.UTF8.GetBytes(jLetterControllerData);
#if UNITY_EDITOR
        Debug.Log(jLetterControllerData);
#endif
        // 해당 파일 스트림에 적는다.                
        stream.Write(bLetterControllerData, 0, bLetterControllerData.Length);
        // 스트림 닫기
        stream.Close();
    }

    public void Save_SoundData()
    {
        //SoundManager mSoundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();

        //if (mSoundManager == null)
        //    return;

        // 파일이 있다면
        if (System.IO.File.Exists(Path.Combine(Application.dataPath + "/Data/", "SoundData.json")))
        {
            // 삭제
            System.IO.File.Delete(Path.Combine(Application.dataPath + "/Data/", "SoundData.json"));

        }
        // 삭제 후 다시 개방
        // 이유는, 동적으로 생성 될 경우에 json을 초기화 하지 않고 덮어 씌우기 때문에 전에 있던 데이터보다 적을 경우
        // 뒤에 남는 쓰레기 값들로 인하여 역직렬화 오류 발생함
        // 동적으로 생성하는 경우가 아닌 경우 (ex, 현재 씬 인덱스 등)은 상관 없음
        // 파일 스트림 개방
        FileStream stream = new FileStream(Application.dataPath + "/Data/SoundData.json", FileMode.OpenOrCreate);

        // 저장할 변수가 담긴 클래스 생성
        SoundData mSoundData = new SoundData();

        // 데이터 업데이트
        mSoundData.mSaveBGM = SceneData.Instance.BGMValue;
        mSoundData.mSaveSFx = SceneData.Instance.SFxValue;
        //mInitData.isFirstPlay = false; // 저장했으니까 처음 플레이가 아님.

        // 데이터 직렬화
        string jSoundData = JsonConvert.SerializeObject(mSoundData);

        // json 데이터를 Encoding.UTF8의 함수로 바이트 배열로 만들고
        byte[] bSoundData = Encoding.UTF8.GetBytes(jSoundData);
#if UNITY_EDITOR
        Debug.Log(jSoundData);
#endif
        // 해당 파일 스트림에 적는다.                
        stream.Write(bSoundData, 0, bSoundData.Length);
        // 스트림 닫기
        stream.Close();
    }

    void Save_HistoryData()
    {
        if (mInvenManager) // null check
        {
            // 암호화는 나중에 한번에 하기

            // 파일이 있다면
            if (System.IO.File.Exists(Path.Combine(Application.dataPath + "/Data/", "HistoryData.json")))
            {
                // 삭제
                System.IO.File.Delete(Path.Combine(Application.dataPath + "/Data/", "HistoryData.json"));

            }
            // 삭제 후 다시 개방
            // 이유는, 동적으로 생성 될 경우에 json을 초기화 하지 않고 덮어 씌우기 때문에 전에 있던 데이터보다 적을 경우
            // 뒤에 남는 쓰레기 값들로 인하여 역직렬화 오류 발생함
            // 동적으로 생성하는 경우가 아닌 경우 (ex, 현재 씬 인덱스 등)은 상관 없음
            // 파일 스트림 개방
            FileStream stream = new FileStream(Application.dataPath + "/Data/HistoryData.json", FileMode.OpenOrCreate);

            // 저장할 변수가 담긴 클래스 생성

            // 재료, 구름 도감 정보 한번에 할 것
            HistoryData mHistoryData = new HistoryData();

            // 데이터 업데이트
            mHistoryData.mIngredientHistoryDatas = mInvenManager.ingredientHistory.ToList();
            
            // Todo
            // 경로 추가되면 string 타입 저장 2개 더 하면 된다.            

            // 데이터 직렬화
            string jHistoryData = JsonConvert.SerializeObject(mHistoryData);

            // json 데이터를 Encoding.UTF8의 함수로 바이트 배열로 만들고
            byte[] bHistoryData = Encoding.UTF8.GetBytes(jHistoryData);
#if UNITY_EDITOR
            Debug.Log(jHistoryData);
#endif
            // 해당 파일 스트림에 적는다.                
            stream.Write(bHistoryData, 0, bHistoryData.Length);
            // 스트림 닫기
            stream.Close();
        }
    }


    // 종료될 때
    void OnDisable()
    {
        // 제거
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}