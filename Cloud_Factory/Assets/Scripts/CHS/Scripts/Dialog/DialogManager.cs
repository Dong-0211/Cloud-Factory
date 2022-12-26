using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Animations;

public class DialogManager : MonoBehaviour
{
    // �ҷ��� ���� ����
    private Guest mGuestManager;
    private SOWManager mSOWManager;

    public int mGuestNum;                       // �մ��� ��ȣ�� �Ѱܹ޴´�.
    private int mGuestSat;                      // �մ��� ���� ������
    private int mGuestVisitCount;               // �մ��� ���� �湮 Ƚ��
    string mTestName;                           // �׽�Ʈ�� ���� �ӽ� �̸� ( �մ��� �̸��� �����Դٰ� ����)

    [SerializeField]
    private DialogDB mDialogDB;                 // ��ȭ ������ ������ ���� DB
    [SerializeField]
    private string[] mTextList;                 // ��ȭ ������ �ҷ��ͼ� �����ص� ����Ʈ
    private int[] mGuestImageList;              // ��ȭ ���뿡 �´� ǥ���� �����ص� ����Ʈ
    private int[] mIsGuset;                     // ���� �̾߱��ϴ� �������� ó���ϱ�

    // �� ȭ�鿡 ���� �ؽ�Ʈ�� �� ���� 
    private string mDialogGuestName;            // ȭ�鿡 ��½�ų �մ� �̸�
    private string mDialogText;                 // ������ ȭ�鿡 ��½�ų ����

    // �� ȭ�鿡 ���� �ؽ�Ʈ ������Ʈ ����
    public GameObject gTextPanel;               // ��ȭ â
    public GameObject gTakeGuestPanel;          // �մ� �ޱ�/ ���� ��ư

    public Text tText;                          // ��ȭ�� ���� �� �ؽ�Ʈ
    public Text tGuestName;                     // ��ȭ���� �մ��� �̸��� ǥ�õ� �ؽ�Ʈ
    public Text tPlayerText;                    // ��ȭ�߿� �÷��̾��� ��ȭ�� ���� �� �ؽ�Ʈ
    public Text tGuestText;                     // ��ȭ�߿� �÷��̾��� ��ȭ�� ���� �� �ؽ�Ʈ

    // �մ��� �̹����� ���µ� �ʿ��� ������ ����
    public Sprite[] sGuestImageArr;             // �̹��� �ε�����
    public GameObject gGuestSprite;             // ���� ȭ�鿡 ��µǴ� �̹��� ������Ʈ
    private SpriteRenderer sGuestSpriteRender;  // ������Ʈ�� Sprite ������Ʈ�� �о�� SpriteRenderer
    public Animator mGuestAnimator;

    // ��ȭ ������ �ʿ��� ������ ����
    private int mDialogIndex;                   // �ش� �������� ���ϴ� ������ �ε���s
    private int mDialogCharIndex;               // ������ ȭ�鿡 ��½�Ű�� ������ �ε���
    private int mDialogImageIndex;              // ������ ȭ�鿡 ��½�Ű�� �̹����� �ε���
    private bool isReading;                     // ���� ��ȭâ���� ��ȭ�� ����ϴ� ���ΰ�?
    private bool isLastDialog;                  // ������ ��ȭ�� �ҷ��Դ°�?

    // ����/���� �гο� �ʿ��� �ؽ�Ʈ ������Ʈ �ޱ�
    [SerializeField]
    private Text tPanelName;                    // �湮 �մ��� �̸�
    [SerializeField]
    private Text tPanelAge;                     // �湮 �մ��� ����
    [SerializeField]
    private Text tPanelJob;                     // �湮 �մ��� ����
    [SerializeField]
    private Image iPanelPortrait;               // �湮 �մ��� �ʻ�ȭ


    // �׽�Ʈ �Լ�
    // ��ȭâ���� �ٸ� ĳ���� Ȥ�� �ٸ� �������� �ؽ�Ʈ�� �޾ƿ��� ��� ������ �ִ��� Ȯ���ϱ� ���� �Լ�

    void Awake()
    {

        initDialogManager();

        // �湮�ֱⰡ ���� ������ �մ��� ������ �ʴ´�.
        if (mGuestManager.isTimeToTakeGuest)
        {
            LoadDialogInfo();
            ReadDialog();

            initAnimator();
            initTakeGuestPanel();

            // ��ȭ �г��� Ȱ��ȭ
            gTextPanel.SetActive(true);

            // �մ� �̹����� Ȱ��ȭ
            gGuestSprite.SetActive(true);
        }
    }

    void initDialogManager()
    {
        mDialogIndex = 0;
        mDialogIndex = GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex;
        mDialogCharIndex = 0;
        mDialogImageIndex = 0;
        tText.text = "";

        mSOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();
        sGuestSpriteRender = gGuestSprite.GetComponent<SpriteRenderer>();
        mGuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();

        mGuestAnimator = gGuestSprite.GetComponent<Animator>();

        mGuestNum = mGuestManager.mGuestIndex;
        mGuestSat = mGuestManager.mGuestInfos[mGuestNum].mSatatisfaction;
        mGuestVisitCount = mGuestManager.mGuestInfos[mGuestNum].mVisitCount;
        tGuestName.text = mGuestManager.mGuestInfos[mGuestNum].mName;

        mGuestImageList = new int[20];
        mTextList = new string[20];
        mIsGuset = new int[20];
        isReading = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void initAnimator()
    {
        // �ʻ�ȭ�� �ִϸ��̼� Ŭ���� �ʱ�ȭ�Ѵ�.
        mGuestAnimator.runtimeAnimatorController = GetComponent<DrawAniClip>().animators[mGuestNum];
    }

    public void MoveSceneToWeatherSpace()
    {
        SceneManager.LoadScene("Space Of Weather");
    }

    // �ش� �մԿ� ���� ��ȭ�� ������ �ҷ����� �Լ�
    private void LoadDialogInfo()
    {
        // ���� ���� GameManager �Ѱ��� �����ϰ�, �� ������ �Ϸ縶�� 5���� �մ��� �����Ͽ� �����ǿ� �÷��̾ ���� �ð��� ���Ͽ� �����ϰ� �Ѹ� �湮��Ų��.
        // GameManager���� ������ �մ��� ��ȣ�� �޾ƿ���, �մ��� ��ȣ�� �´� �մ��� ������ �����´�.

        int i;
        int j = 0;

        // �մ� ��ȣ -> �湮 Ƚ�� -> ������ ������ ���� �ؽ�Ʈ ������ üũ�Ѵ�.
        for (i = 0; i < mDialogDB.DialogText.Count; ++i)
        {
            if (mDialogDB.DialogText[i].GuestID == mGuestNum + 1)
            {
                if (mDialogDB.DialogText[i].VisitCount == mGuestVisitCount)
                {
                    if (mDialogDB.DialogText[i].Sat == mGuestSat)
                    {
                        mTextList[j] = mDialogDB.DialogText[i].Text;
                        mGuestImageList[j] = mDialogDB.DialogText[i].DialogImageNumber;
                        mIsGuset[j] = mDialogDB.DialogText[i].isGuest;
                        Debug.Log(j + " " + mIsGuset[j]);
                        j++;
                    }
                }
            }
        }
        mTextList[j] = "End";
    }

    private void InitDialog()
    {
        mDialogCharIndex = 0;
        tGuestText.text = "";
        tPlayerText.text = "";

        // �մ��� �����
        if (mIsGuset[GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex] == 1)
        {
            tText = tGuestText;
        }
        else
        {
            tText = tPlayerText;
        }
        Debug.Log(mDialogIndex + " " + mIsGuset[mDialogIndex]);
    }
    public string GetDialog(int dialogindex) // ������ , ��ȭ ���� ����
    {
        return mTextList[dialogindex];
    }

    private void ReadDialogAtAll()
    {
        tText.text += GetDialog(GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex);
        isReading = false;
    }

    private void ReadDialogAtOne()
    {
        isReading = true;
        if (tText.text == GetDialog(GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex))
        {
            // �ؽ�Ʈ�� ��� ����� �� ��쿡 Ŭ�� ��, ���� ������ ��µȴ�.
            if (GetDialog(GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex) != "End")
            {
                GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex += 1;
                mDialogImageIndex++;
                isReading = false;
            }
            return;
        }
        tText.text += GetDialog(GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex)[mDialogCharIndex];
        mDialogCharIndex++;

        Invoke("ReadDialogAtOne", 0.05f);
    }

    // �մ԰��� ��ȭ�� ��������ִ� �Լ�
    public void ReadDialog()
    {
        InitDialog();

        mGuestAnimator.SetInteger("index", mGuestImageList[GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex]);

        // ������ End ���ڿ��� ������ ��� ( ��ȭ�� ��� �ҷ��� ���)
        if (GetDialog(GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex) == "End")
        {
            isLastDialog = true;
            // ��ȭ ������ ��� ����ϰ� ���� �մ� ���뿡 ���� ���θ� �÷��̾�� ���´�. (�޴´�/ ���� �ʴ´�)
            TakeGuest();
            return;
        }
        // ��ȭ�� ������� ���߿� Ŭ���� ���, ������ �ѹ��� ����� �ȴ�.
        if (isReading == true)
        {
            ReadDialogAtAll();
            return;
        }
        // �⺻������ �� �ؽ�Ʈ���� ��ȭ ������ �� ���ھ� �߰��Ͽ� ����ϰ� ������ �ϱ⸦ �ݺ��Ѵ�.
        ReadDialogAtOne();
        return;
    }

    private void TakeGuest()
    {
        gTakeGuestPanel.SetActive(true);
    }

    // �մ� �����ϱ�
    public void AcceptGuest()
    {
        Debug.Log("�մ��� �޽��ϴ�.");
        mSOWManager.InsertGuest(mGuestNum);
        mSOWManager.isNewGuest = true;

        mGuestManager.InitGuestTime();

        // �մ��� �̵������Ƿ� �����ǿ� �ִ� �͵��� �ʱ�ȭ �����ش�.
        ClearGuest();
        MoveSceneToWeatherSpace();
    }

    // �մ� �����ϱ�
    public void RejectGuest()
    {
        Debug.Log("�մ��� ���� �ʽ��ϴ�.");

        // �湮���� �ʴ� Ƚ���� 3���� �����Ѵ�. (3�ϰ� �湮 X)
        mGuestManager.mGuestInfos[mGuestNum].mNotVisitCount = 3;
        mGuestManager.InitGuestTime();

        // �մ��� �̵������Ƿ� �����ǿ� �ִ� �͵��� �ʱ�ȭ �����ش�.
        ClearGuest();
        MoveSceneToWeatherSpace();
    }

    // �������� �ʱ�ȭ �����ش�.
    private void ClearGuest()
    {
        // �湮Ƚ�� 1ȸ ����
        mGuestManager.mGuestInfos[mGuestNum].mVisitCount++;

        // �մ��� �����ǿ� ���ٰ� ǥ��
        mGuestManager.isGuestInLivingRoom = false;

        // ��ȭ �ε����� 0���� �ʱ�ȭ
        GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex = 0;

        // ��ȭ �г��� ��Ȱ��ȭ
        gTextPanel.SetActive(false);

        // �մ� �̹����� ��Ȱ��ȭ
    }

    // ����/���� �ϴ� �г��� �湮�� �մ��� ������ �ʱ�ȭ �Ѵ�.
    private void initTakeGuestPanel()
    {
        GuestInfo guest = mGuestManager.mGuestInfos[mGuestNum];

        tPanelName.text = "�̸�: " + guest.mName;
        tPanelAge.text = "����: " + guest.mAge;
        tPanelJob.text = "����: " + guest.mJob;
        iPanelPortrait.sprite = sGuestImageArr[mGuestNum];
    }

}