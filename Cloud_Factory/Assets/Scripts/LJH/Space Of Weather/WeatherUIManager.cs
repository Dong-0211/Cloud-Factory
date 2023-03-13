using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class WeatherUIManager : MonoBehaviour
{
    private SeasonDateCalc mSeason; // ���� ��� ��ũ��Ʈ
    private TutorialManager mTutorialManager;
    private SOWManager mSOWManager;

	[Header("Gather")]
    public GameObject mGuideGather; // ä���Ұ��� ���Ұ��� �˷��ִ� UI
    public GameObject mGathering;   // ä�� �� ����ϴ� UI
    public GameObject mGatherResult;// ä�� ����� ����ϴ� UI

    public GameObject[] mSeasonObj = new GameObject[4]; // 4���� ������Ʈ
    public GameObject[] mSeasonUIObj = new GameObject[4]; // 4���� UI������Ʈ

    public Animator mGatheringAnim; // ä�� �ִϸ��̼�

    public Text tGatheringText;      // ä�� ��... �ؽ�Ʈ
    private int mGatheringTextCount; // ä�� �� '.' ��� ����

    public RectTransform mGatherImageRect; // ä�� �̹��� Rect Transform

    public RectTransform[] mFxShine = new RectTransform[5]; // 5���� ä�� ��� ȸ�� ȿ��
    public RectTransform[] mGatherRect = new RectTransform[5]; // 5���� ä�� ��� UI �̵�
    public GameObject[] mGatherObj = new GameObject[5]; // 5���� ä�� ���� ������Ʈ

    public int mRandomGather; // ��� ä�� ���� ����

    [Header("BackGround")]
    public GameObject iMainBG; // ���� ��� �̹��� 
    public Sprite[] mBackground = new Sprite[4]; // �������� �޶����� ���

    // ������ ������� �ӽ÷� �������� ���� ������Ʈ��
    private GameObject[] mGardens = new GameObject[4];
    public Sprite[] mSpringGardenSprites = new Sprite[2];
	public Sprite[] mSummerGardenSprites = new Sprite[2];
	public Sprite[] mFallGardenSprites = new Sprite[2];
	public Sprite[] mWinterGardenSprites = new Sprite[2];
    private Sprite[] mSwitchGardenSprites = new Sprite[2];

	//����
	private GameObject selectedYard;
    private void Awake()
    {
        mSeason = GameObject.Find("Season Date Calc").GetComponent<SeasonDateCalc>();
		mTutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
		mSOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();
	}

    void Update()
    {
        if (mGatherResult.activeSelf)
        {
            // ä�� ��� ȿ��
            mFxShine[0].Rotate(0, 0, 25.0f * Time.deltaTime, 0);
            mFxShine[1].Rotate(0, 0, 25.0f * Time.deltaTime, 0);
            mFxShine[2].Rotate(0, 0, 25.0f * Time.deltaTime, 0);
            mFxShine[3].Rotate(0, 0, 25.0f * Time.deltaTime, 0);
            mFxShine[4].Rotate(0, 0, 25.0f * Time.deltaTime, 0);
        }

        switch (mSeason.mSeason)
        {
            case 1:
                UpdateSeasonBg(0);// ��
                UpdateSeasonGarden(0);
				UpdateSeasonGardenSprites(1);
				break;
            case 2:
                UpdateSeasonBg(1);// ����
				UpdateSeasonGarden(1);
				UpdateSeasonGardenSprites(2);
				break;
            case 3:
                UpdateSeasonBg(2);// ����
				UpdateSeasonGarden(2);
				UpdateSeasonGardenSprites(3);
				break;
            case 4:
                UpdateSeasonBg(3); // �ܿ�
				UpdateSeasonGarden(3);
				UpdateSeasonGardenSprites(4);
				break;
            default:
                break;
        }
    }

    void UpdateSeasonBg(int _iCurrent)
    {
        iMainBG.GetComponent<SpriteRenderer>().sprite = mBackground[_iCurrent];
        for (int i = 0; i < 4; i++)
        {
            if (_iCurrent == i) continue;

            mSeasonObj[i].SetActive(false);
            mSeasonUIObj[i].SetActive(false);
            // ��å�� ������Ʈ �߰�

        }

        mSeasonObj[_iCurrent].SetActive(true);
        mSeasonUIObj[_iCurrent].SetActive(true);
        // ��å�� ������Ʈ �߰�

    }

    void UpdateSeasonGarden(int season)
    {
        GameObject seasonObj = mSeasonObj[season];
        
        for(int num = 0; num < 4; num++){
            mGardens[num] = seasonObj.transform.Find("Garden" + (num + 1)).gameObject;
			if (mSOWManager.yardGatherCount[num] <= 0) { mGardens[num].GetComponent<SpriteRenderer>().sprite = mSwitchGardenSprites[0]; }
			else { mGardens[num].GetComponent<SpriteRenderer>().sprite = mSwitchGardenSprites[1]; }
		}
	}

    void UpdateSeasonGardenSprites(int season)
    {
		mSwitchGardenSprites = new Sprite[2];
        switch (season - 1)
        {
            case 0:
                mSwitchGardenSprites = mSpringGardenSprites;
                break;
            case 1:
                mSwitchGardenSprites = mSummerGardenSprites;
                break;
            case 2:
                mSwitchGardenSprites = mFallGardenSprites;
                break;
            case 3:
                mSwitchGardenSprites = mWinterGardenSprites;
                break;
            default:
                break;
        }
    }


    // ���� ��ư Ŭ�� ��, ä���Ͻðھ��ϱ�? ������Ʈ Ȱ��ȭ    
    public void OpenGuideGather()
    {
        selectedYard = EventSystem.current.currentSelectedGameObject;

		// ä�� Ƚ���� ��� �����Ǿ����� ä������ �ʰ� â�� ����� �ʴ´�.
		YardHandleSystem system = selectedYard.GetComponentInParent<YardHandleSystem>();
        system.UpdateYardGatherCount();
		if (system.CanBeGathered(selectedYard) == false)
		{
			Debug.Log("ä�� ���� Ƚ���� ��� �����Ǿ����ϴ�.");
			return;
		}
        if (mTutorialManager.isFinishedTutorial[2] == false)
        { mTutorialManager.SetActiveFadeOutScreen(false); }
		mGuideGather.SetActive(true);
	}
    // ������, ä���Ͻðھ��ϱ�? ������Ʈ ��Ȱ��ȭ    
    public void CloseGuideGather()
    {
		if (mTutorialManager.isFinishedTutorial[2] == false) { return; }
		mGuideGather.SetActive(false);
    }
    // ä���ϱ�
    public void GoingToGather()
    {
		mGuideGather.SetActive(false);
        mGathering.SetActive(true);
        mGatheringTextCount = 0; // �ʱ�ȭ
        tGatheringText.text = "��� ä�� ��"; // �ʱ�ȭ

        if (SeasonDateCalc.Instance) // null check
        {                            // �� �ش��ϴ� �ִϸ��̼� ���
            Invoke("PrintGatheringText", 0.5f); // 0.5�� �����̸��� . �߰�
            if (SeasonDateCalc.Instance.mSeason == 1) // ���̶��
                UpdateGatherAnim(1090, 590, true, false, false, false);
            else if (SeasonDateCalc.Instance.mSeason == 2) // �����̶��
                UpdateGatherAnim(1090, 590, false, true, false, false);
            else if (SeasonDateCalc.Instance.mSeason == 3) // �����̶��
                UpdateGatherAnim(735, 420, false, false, true, false);
            else if (SeasonDateCalc.Instance.mSeason == 4) // �ܿ��̶��
                UpdateGatherAnim(560, 570, false, false, false, true);
        }
        // 5�� ���� ä�� �� ��� ���
        Invoke("Gathering", 5.0f);

        mSOWManager.yardGatherCount[selectedYard.transform.GetSiblingIndex()]--;
	}
    
    void UpdateGatherAnim(int _iX, int _iY, bool _bSpring, bool _bSummer, bool _bFall, bool _bWinter)
    {
        mGatherImageRect.sizeDelta = new Vector2(_iX, _iY); // �̹��� ������ ���߱�
        mGatheringAnim.SetBool("Spring", _bSpring);
        mGatheringAnim.SetBool("Summer", _bSummer);
        mGatheringAnim.SetBool("Fall", _bFall);
        mGatheringAnim.SetBool("Winter", _bWinter);
    }

  
    void Gathering()
    {
        YardHandleSystem system = selectedYard.GetComponentInParent<YardHandleSystem>();

        mRandomGather = Random.Range(0, 5); // 0~4
        if (mTutorialManager.isFinishedTutorial[2] == false) { mRandomGather = 1; }

        // Result Image match
        GatherResultMatchWithUI(system.Gathered(selectedYard, mRandomGather));

        if (!system.CanBeGathered(selectedYard)) { mGardens[selectedYard.transform.GetSiblingIndex()].GetComponent<SpriteRenderer>().sprite = mSwitchGardenSprites[0]; }
        else { mGardens[selectedYard.transform.GetSiblingIndex()].GetComponent<SpriteRenderer>().sprite = mSwitchGardenSprites[1]; }

        if (mRandomGather % 2 == 1) // Ȧ��
        {
            mGatherRect[0].anchoredPosition = new Vector3(125.0f, 0.0f, 0.0f);
            mGatherRect[1].anchoredPosition = new Vector3(-125.0f, 0.0f, 0.0f);
            mGatherRect[2].anchoredPosition = new Vector3(375.0f, 0.0f, 0.0f);
            mGatherRect[3].anchoredPosition = new Vector3(-375.0f, 0.0f, 0.0f);
        }
        else
        {
            mGatherRect[0].anchoredPosition = new Vector3(0, 0.0f, 0.0f);
            mGatherRect[1].anchoredPosition = new Vector3(-225.0f, 0.0f, 0.0f);
            mGatherRect[2].anchoredPosition = new Vector3(225.0f, 0.0f, 0.0f);
            mGatherRect[3].anchoredPosition = new Vector3(-450.0f, 0.0f, 0.0f);
        }

        switch (mRandomGather) // active ����
        {
            case 0:
                ActiveRandGather(true, false, false, false, false);
                break;
            case 1:
                ActiveRandGather(true, true, false, false, false);
                break;
            case 2:
                ActiveRandGather(true, true, true, false, false);
                break;
            case 3:
                ActiveRandGather(true, true, true, true, false);
                break;
            case 4:
                ActiveRandGather(true, true, true, true, true);
                break;
            default:
                break;
        }


        mGathering.SetActive(false);
        mGatherResult.SetActive(true);

        CancelInvoke(); // �κ�ũ �浹 ������ ���ؼ� ��� ����� ������ ��� �κ�ũ ������
    }

    private void GatherResultMatchWithUI(Dictionary<IngredientData, int> results)
    {
        if (results == null) { return; }

        int i = 0;
        foreach (KeyValuePair<IngredientData, int> data in results)
        {
            GameObject targetUI = mGatherObj[i];
            Image image = targetUI.transform.GetChild(1).GetComponent<Image>();
            Text text = targetUI.transform.GetChild(1).GetChild(0).GetComponent<Text>();

            image.sprite = data.Key.image;
            text.text = data.Value.ToString();

            i++;

        }
    }

    void ActiveRandGather(bool _bOne, bool _bTwo, bool _bThree, bool _bFour, bool _bFive)
    {
        mGatherObj[0].SetActive(_bOne);
        mGatherObj[1].SetActive(_bTwo);
        mGatherObj[2].SetActive(_bThree);
        mGatherObj[3].SetActive(_bFour);
        mGatherObj[4].SetActive(_bFive);
    }

    // ����Լ��� ��ħǥ�� ��������� ����Ѵ�
    void PrintGatheringText()
    {
        mGatheringTextCount++;
        tGatheringText.text = tGatheringText.text + ".";

        if (mGatheringTextCount <= 3)
        {
            Invoke("PrintGatheringText", 0.25f); // 0.25�� �����̸��� . �߰�
        }
        else // �ʱ�ȭ
        {
            mGatheringTextCount = 0;
            tGatheringText.text = "��� ä�� ��";
            Invoke("PrintGatheringText", 0.25f); // 0.25�� �����̸��� . �߰�
        }
    }
    // ä�� ��!
    public void CloseResultGather()
    {
        if (mTutorialManager.isFinishedTutorial[2] == false)
        { 
            mTutorialManager.SetActiveGuideSpeechBubble(true);
            GameObject.Find("B_GardenSpring").transform.SetParent(GameObject.Find("Canvas").transform);
            GameObject.Find("B_GardenSpring").transform.SetSiblingIndex(5);
		}

		mGatherResult.SetActive(false);        
    }
}
