using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ��¥ �� ���� ��� ��ũ��Ʈ
[System.Serializable]
public class SeasonDateCalc : MonoBehaviour
{
    // SeasonDateCalc�� �ν��Ͻ��� ��� ���� ����
    private static SeasonDateCalc instance = null;    
    // SeasonDateCalc Instance�� ������ �� �ִ� ������Ƽ, �ٸ� Ŭ�������� ��밡��
    public  static SeasonDateCalc Instance
    {
        get
        {
            if (null == instance) return null;

            return instance;
        }
    }

    public float    mSecond; // ��, �ð�, 600��=10��=�Ϸ�
    public int      mDay;    // �� (1~20��)
    public int      mWeek;   // �� (5�ϸ��� 1��, 4�ְ� �ִ�)
    public int      mSeason; // ��, ���� (4�ָ��� 1��, ��,����,����,�ܿ� ������ 4��)
    public int      mYear;   // �� (~)    

    private bool    mChangeDay = false;

    // Demo Version
    public bool isSatOrDisSatGuestExist; 

    [Header("�׽�Ʈ ����")]
    [SerializeField]
    private float   MaxSecond = 60.0f; // �Ϸ� ����(��)�� �׽�Ʈ �������� �ٲٱ� ���� ����

    void Awake()
    {
        // �ν��Ͻ� �Ҵ�
        if (null == instance)
        {
            instance = this;
            // ��� ������ ��¥ ����ؾ��ϹǷ�
            // ��, title�������� �����Ѵ�.
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            // �̹� �����ϸ� �������� ����ϴ� ���� �����
            Destroy(this.gameObject);
        }
        isSatOrDisSatGuestExist = false;
    }

    void Update()
    {
        TutorialManager mTutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
        // �κ�, ��������, �������� ȭ�鿡���� ����
        if (SceneManager.GetActiveScene().name != "Lobby"
         && SceneManager.GetActiveScene().name != "Cloud Storage"
         && SceneManager.GetActiveScene().name != "Give Cloud"
         && mTutorialManager.isTutorial == false)
        {
            // �� ���
            mSecond += Time.deltaTime;
            // �� ���
            // 20�� ����

            // Demo Version
            if(mDay >= 29 && SceneManager.GetActiveScene().name != "Demo Thank You") { SceneManager.LoadScene("Demo Thank you"); }
            mDay += CalcDay(ref mSecond);
            /*
            if (mDay > 20) mDay = 1;
            else mDay += CalcDay(ref mSecond);
            // �� ���        
            mWeek = CalcWeek(ref mDay);
            // ��, ���� ���
            mSeason += CalcSeason(ref mWeek);
            // �� ���
            mYear += CalcYear(ref mSeason);
            */

            if (mChangeDay)
            {
                // �Ϸ簡 ���� �� �����Լ� �ҷ�����
                GameObject.Find("SaveUnitManager").GetComponent<SaveUnitManager>().Save_Func();
                mChangeDay = false;
            }
        }

        // ������ �׽�Ʈ�� ���� ��Ű
        if(Input.GetKeyDown(KeyCode.Q))
        {
            mWeek = 5;
            mSeason += CalcSeason(ref mWeek);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            mSecond = 600;
            mDay += CalcDay(ref mSecond);
            // Demo Version
            //mWeek = CalcWeek(ref mDay);
            //mSeason += CalcSeason(ref mWeek);
        }

    }

    // ref�� �����ؼ� ������ �ּ� �� ����
    int CalcDay(ref float second)
    {
        int temp = 0;
        // 10�д� 1��, 600�ʴ� 1�� �߰�
        if (second >= MaxSecond)
        {
            // ��¥ ���ϴ� �κ� -> ��¥���� ��ȯ������ ���⿡ �ۼ�
            if(!GameObject.FindWithTag("Guest"))
            {
                Debug.Log("��� �մ��� �����Ͽ��� ������ �Ϸ簡 �Ѿ�ϴ�");

                // �湮�� �մ� ����Ʈ �ʱ�ȭ
                Guest GuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();
                SOWManager SOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();

                //���� �ٲ� ��, ġ�� ��Ƽ�� ���� ��͵� 4 ��� ���忩�� üũ
                IngredientDataAutoAdder ingredientDataAutoAdder = GameObject.Find("InventoryManager").GetComponent<IngredientDataAutoAdder>();

                if (GuestManager != null && SOWManager != null)
                {
                    GuestManager.InitDay();
                    SOWManager.InitSOW();
                    // Demo Version
                    //ingredientDataAutoAdder.CheckIsCured();
                    if(GuestManager.GetSatGuestList().Length > 0 || GuestManager.DisSatGuestList().Length > 0) { 
                        isSatOrDisSatGuestExist = true;
                        if(SceneManager.GetActiveScene().name == "Space Of Weather")
                        {
                            GameObject.Find("UIManager").GetComponent<DemoUIManager>().SetInactiveFadeROH();
						}
                    
                    }
                    // End Demo Version
                }
                if(SceneManager.GetActiveScene().name == "Space Of Weather") { GameObject.Find("UIManager").GetComponent<InstantiateFadeOutScreen>().InstantiateGif(); }

                temp += 1;
                second = 0;

                // TODO: ���̵� �� - ���̵� �ƿ� ȿ�� �߰�


                mChangeDay = true;                           
            }
            else
            {
                // SOWMangaer�� �ҷ��ͼ� ������ ������ �����ϴ� �մԵ��� ��� �����Ų��.
                SOWManager sowManager;
                sowManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();

                if (sowManager != null)
                {
                    sowManager.MakeGuestDisSat();
                    sowManager.InitSOW();
                }
            }
        }
        return temp;
    }
    int CalcWeek(ref int day)
    {        
        // 0~4���� 1�ְ� ��������
        // ex) day�� 1���� ����, 5���̶��, 5-1 / 5 = 0 >> +1 >> 1����
        // 6-1 / 5 = 1 >> +1 >> 2����
        return ((day - 1) / 5) + 1;
    }
    int CalcSeason(ref int week)
    {
        int temp = 0;
        // 4�ְ� �ִ�, 5�������ʹ� ����
        if (week > 4)
        {
            // �� ���ϴ� �κ� -> �� ���� ��ȯ������ ���⿡ �ۼ�
            // �������� ���ؾ� �ϴ� ���� : (LIST : ���� ��ġ, ���� ������, ��å WayPoint, ��Ƽ �̵� ���� ���)
            // TODO : ���� �� �̵��ؾ� �� ������Ʈ�� �ű�ų� Ȱ��ȭ

            temp += 1;
            week = 1;

            SOWManager sowManager;
            sowManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();

            if (sowManager != null)
                sowManager.ChangeWeatherObject(mSeason % 4);
        }
        return temp;
    }
    int CalcYear(ref int month)
    {
        int temp = 0;
        if (month > 4)
        { 
            // �� ���ϴ� �κ� -> �� ���� ��ȯ������ ���⿡ �ۼ�

            temp += 1;
            month = 1;
        }
        return temp;
    }
}
