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
    private TutorialManager mTutorialManager;

    public int mGuestNum;                       // �մ��� ��ȣ�� �Ѱܹ޴´�.
    private int mGuestSat;                      // �մ��� ���� ������
    private int mGuestVisitCount;               // �մ��� ���� �湮 Ƚ��
    private int mGuestSatVariation;             // �մ��� ���� ������ ������
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
    private bool isTagInDialog;                 // ��� ��, �±�(color) ���̿� ���� �ϴ� �����ΰ�?

    // ����/���� �гο� �ʿ��� �ؽ�Ʈ ������Ʈ �ޱ�
    [SerializeField]
    private Text tPanelName;                    // �湮 �մ��� �̸�
    [SerializeField]
    private Text tPanelAge;                     // �湮 �մ��� ����
    [SerializeField]
    private Text tPanelJob;                     // �湮 �մ��� ����
    [SerializeField]
    private Image iPanelPortrait;               // �湮 �մ��� �ʻ�ȭ

    // Ʃ�丮�󿡼� ���̵� ��ǳ���� ����ϴ� ��ġ�� �����ϴ� ����
    private int hintTextPos;


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
        mTutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();

        mGuestAnimator = gGuestSprite.GetComponent<Animator>();

        mGuestNum = mGuestManager.mGuestIndex;
        mGuestSat = mGuestManager.mGuestInfo[mGuestNum].mSatatisfaction;
        mGuestVisitCount = mGuestManager.mGuestInfo[mGuestNum].mVisitCount;
        mGuestSatVariation = mGuestManager.mGuestInfo[mGuestNum].mSatVariation;
        tGuestName.text = mGuestManager.mGuestInfo[mGuestNum].mName;

        mGuestImageList = new int[30];
        mTextList = new string[30];
        mIsGuset = new int[30];
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
        // ���� ���� GameManager �Ѱ��� �����ϰ�, �� ������ �Ϸ縶�� 6���� �մ��� �����Ͽ� �����ǿ� �÷��̾ ���� �ð��� ���Ͽ� �����ϰ� �Ѹ� �湮��Ų��.
        // GameManager���� ������ �մ��� ��ȣ�� �޾ƿ���, �մ��� ��ȣ�� �´� �մ��� ������ �����´�.


        int i;
        int j = 0;
		hintTextPos = 0;

		List<DialogDBEntity> Dialog;
        Dialog = mDialogDB.SetDialogByGuestNum(mGuestNum);
        int[] speakEmotionEffect = mGuestManager.SpeakEmotionEffect(mGuestNum);
        int tempVisitCount = 0;                                     // ��Ʈ�� �湮 Ƚ���� ������ �ƴ� ������ �Ǿ��ִ� ����� �����ϴ� �ӽ� ����
                                                                    // �ش� ������ 2�� ��, 1<x<10 ������ �Ǵ�

        // Dialog Null ��ȯ�� ���� ���
        if (Dialog == null)
            Debug.Log("��ȭ�� �ҷ����µ��� ������ �߻��Ͽ����ϴ�.");

		if (mGuestVisitCount <= 1 || mGuestVisitCount >= 10) { tempVisitCount = mGuestVisitCount; }
		else { tempVisitCount = 2; }

		// �մ� ��ȣ -> �湮 Ƚ�� -> ������ ������ ���� �ؽ�Ʈ ������ üũ�Ѵ�.
		for (i = 0; i < Dialog.Count; ++i)
        {
            Debug.Log(mGuestNum + " " + mGuestManager.mGuestInfo[mGuestNum].mVisitCount + " " + mGuestVisitCount + " " + mGuestSat);
			if (Dialog[i].GuestID == mGuestNum + 1                  // �Խ�Ʈ ��ȣ
                && Dialog[i].VisitCount == tempVisitCount           // �湮 Ƚ��
                && Dialog[i].Sat == mGuestSat)                      // ������
            {
				if (tempVisitCount >= 10 || (tempVisitCount < 10 && Dialog[i].SatVariation == mGuestSatVariation)) // �湮 Ƚ���� 10�� ��, ������ �������� �������
                {
					//Text�� Hint�̸� xls���� �����Ѽ��� ���� ����� ������ ��� �������� �Ҵ�
					if (Dialog[i].Text == "Hint")
                    {
                        for (int count = 0; count < speakEmotionEffect.Length; count++)
                        {
                            for (int num = 0; num < Dialog.Count; num++)
                            {
                                if (Dialog[num].GuestID == mGuestNum + 1
                                    && Dialog[num].VisitCount == 0                          // ��� ���� �ް� ������ ���ɼ� O
                                    && Dialog[num].Emotion == speakEmotionEffect[count])    // TODO: ���� �ؽ�Ʈ ���� ���� ���� ���� ���� �ʿ�
                                {
                                    if (!mTutorialManager.isFinishedTutorial[1] 
                                        && hintTextPos == 0)                                // ��Ʈ�� ó�� ������ ��ġ�� ����
                                    { hintTextPos = j; }

                                    mTextList[j] += Dialog[num].Text;
                                    mGuestImageList[j] = Dialog[num].DialogImageNumber;
                                    mIsGuset[j] = Dialog[num].isGuest;
                                    j++;
									continue;
                                }
                            }
                        }
                    }
                    else
                    {
                        mTextList[j] = Dialog[i].Text;
                        mGuestImageList[j] = Dialog[i].DialogImageNumber;
                        mIsGuset[j] = Dialog[i].isGuest;
                        Debug.Log(j + " " + mIsGuset[j]);
                        j++;
                    }
                }
            }
        }
        //for (i = 0; i < mDialogDB.DialogText1.Count; ++i)
        //{
        //    if (mDialogDB.DialogText1[i].GuestID == mGuestNum + 1)
        //    {
        //        if (mDialogDB.DialogText1[i].VisitCount == mGuestVisitCount)
        //        {
        //            if (mDialogDB.DialogText1[i].Sat == mGuestSat)
        //            {
        //                mTextList[j] = mDialogDB.DialogText1[i].Text;
        //                mGuestImageList[j] = mDialogDB.DialogText1[i].DialogImageNumber;
        //                mIsGuset[j] = mDialogDB.DialogText1[i].isGuest;
        //                Debug.Log(j + " " + mIsGuset[j]);
        //                j++;
        //            }
        //        }
        //    }
        //}

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

        if(GetDialog(GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex)[mDialogCharIndex] == '<')
        {
            if(GetDialog(GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex)[mDialogCharIndex + 1] == '/') { isTagInDialog = false; }
            else { isTagInDialog = true; }

            // �±װ� ���� �� ����, �Է� �±׸� �ؽ�Ʈ�� �߰� ��, �ڿ� ���� �±׵� �߰�
            while (mDialogCharIndex <= 0 || GetDialog(GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex)[mDialogCharIndex - 1] != '>')
            {
                if(isTagInDialog == true)
                {
					tText.text += GetDialog(GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex)[mDialogCharIndex];
					mDialogCharIndex++;
				}
                else { mDialogCharIndex++; }
			}
            if(isTagInDialog == true) { tText.text += "</color>"; }
		}

        if(isTagInDialog = true) {
			tText.text = InsertCharInString(tText.text.ToString(), GetDialog(GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex)[mDialogCharIndex], mDialogCharIndex);
			mDialogCharIndex++;
		}
        else
        {
			tText.text += GetDialog(GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex)[mDialogCharIndex];
			mDialogCharIndex++;
		}

        Invoke("ReadDialogAtOne", 0.05f);
    }

	private string InsertCharInString(string main_string, char add_char, int _index)
	{
        string temp_string = "";
        for(int idx = 0; idx < _index; idx++)
        {
            temp_string += main_string[idx];
        }
        temp_string += add_char;
        for(int idx = _index; idx < main_string.Length; idx++)
        {
            temp_string += main_string[idx];
        }

        return temp_string;
	}

	// �մ԰��� ��ȭ�� ��������ִ� �Լ�
	public void ReadDialog()
    {
        InitDialog();
		if(mTutorialManager.GetActiveGuideSpeechBubble() == null
            && mTutorialManager.isTutorial == true) { return; }


		mGuestAnimator.SetInteger("index", mGuestImageList[GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex]);

        // ���̵� ��ǳ�� ��� ��, �ٽ� ��µ��� �ʵ��� hintTextPos�� 0���� �����Ѵ�.(��ǳ�� ����� ����)
        if (mTutorialManager.isFinishedTutorial[1] == false
            && GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex == hintTextPos) 
        { mTutorialManager.SetActiveGuideSpeechBubble(true); hintTextPos = 0;  }                                  

        // ������ End ���ڿ��� ������ ��� ( ��ȭ�� ��� �ҷ��� ���)
        if (GetDialog(GameObject.Find("DialogIndex").GetComponent<DialogIndex>().mDialogIndex) == "End")
        {
            isLastDialog = true;
            // ��ȭ ������ ��� ����ϰ� ���� �մ� ���뿡 ���� ���θ� �÷��̾�� ���´�. (�޴´�/ ���� �ʴ´�)
            TakeGuest();
            if (mTutorialManager.isFinishedTutorial[1] == false) { mTutorialManager.SetActiveGuideSpeechBubble(true); }
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
        if(mTutorialManager.isFinishedTutorial[1] == false) { mTutorialManager.isFinishedTutorial[1] = true; }
        gTakeGuestPanel.SetActive(false);

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
        if (!mTutorialManager.isFinishedTutorial[1] ) {  return;  }
        Debug.Log("�մ��� ���� �ʽ��ϴ�.");

        // �湮���� �ʴ� Ƚ���� 3���� �����Ѵ�. (3�ϰ� �湮 X)
        mGuestManager.mGuestInfo[mGuestNum].mNotVisitCount = 3;
        mGuestManager.InitGuestTime();

        // �մ��� �̵������Ƿ� �����ǿ� �ִ� �͵��� �ʱ�ȭ �����ش�.
        ClearGuest();
        MoveSceneToWeatherSpace();
    }

    // �������� �ʱ�ȭ �����ش�.
    private void ClearGuest()
    {
        // �湮Ƚ�� 1ȸ ����
        mGuestManager.mGuestInfo[mGuestNum].mVisitCount++;

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
        GuestInfos guest = mGuestManager.mGuestInfo[mGuestNum];

        tPanelName.text = "�̸�: " + guest.mName;
        tPanelAge.text = "����: " + guest.mAge;
        tPanelJob.text = "����: " + guest.mJob;
        iPanelPortrait.sprite = sGuestImageArr[mGuestNum];
    }

}