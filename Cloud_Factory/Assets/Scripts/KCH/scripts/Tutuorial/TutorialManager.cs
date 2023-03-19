using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
	// Ʃ�丮���� ������ ����Ǿ����� üũ
	[HideInInspector]
	public bool isTutorial;

	// �ɼ��� �׻� ���� �� �ֵ��� ������ �ʿ�

	/*
     #Ʃ�丮�� ���൵ üũ
     ������ ���� 1
	 ������
	 ������ ���� 2 (ä��)
	 ���� ����
	 ���� ����
     +��������
	 ���� ����
	 �մ� ���
    */
	[HideInInspector]
	public bool[] isFinishedTutorial;

	[Header("ȭ�� ��ġ�� ���� ������Ʈ")]
	public GameObject emptyScreen;              // ȭ�� Ŭ���� �������� ������Ʈ
	private GameObject emptyScreenObject;       // ��ǳ���� �Բ� ����/���� �Ǵ� ������Ʈ
	private GameObject blockScreenTouchObject;  // ��ǳ���� ��� ȭ���� ��ġ�� ���� �� ���Ǵ� ������Ʈ

	[Header("���̵� ��ǳ�� ������Ʈ")]
	public GameObject guideSpeechBubble;        // ���̵� ��ǳ�� ������Ʈ(child�� CharImage, Text����)
	private GameObject guideSpeechBubbleObject; // ������Ʈ�� �����ϱ� ���� ������Ʈ

	[Header("ȭ��ǥ UI ������Ʈ")]
	public GameObject leftArrowNotInCanvas;
	public GameObject leftArrow;
	public GameObject rightArrow;
	private GameObject arrowObject;

	[Header ("���̵� �ƿ� ������ ������Ʈ")]
    public GameObject commonFadeOutScreen;		// ȭ���� ��� ������ ���� Fade Out ��ũ��
    public GameObject fadeOutScreen1;
    public GameObject storageFadeOutScreen;
    public GameObject decoFadeOutScreen;
    private GameObject fadeOutScreenObject;

	[Header("Ʃ�丮��� ��Ƽ ������Ʈ")]
	private GameObject tutorialGuest;

    private Guest mGuestManager;
    private SOWManager mSOWManager;


	private static TutorialManager instance = null;
    void Awake()
    {
        if(null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            isTutorial = true;
            isFinishedTutorial = new bool[9];

            for(int num = 0; num < isFinishedTutorial.Length; num++) { isFinishedTutorial[num] = false; }

            mGuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();

        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        if (isTutorial)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                for (int num = 0; num < 9; num++)
                {
                    isFinishedTutorial[num] = true;
                }
                isTutorial = false;
                DestroyAllObject();
            }

            // ���丮 �Ұ�, ������ �ȳ� Ʃ�丮��
            TutorialOfSOW1();

            // ������ Ʃ�丮��
            TutorialDrawingRoom();

            // ��Ƽ ���� Ȯ��, ��� ä�� Ʃ�丮��
            TutorialOfSOW2();

            // ���� ���� ��� �Ұ� �� �ȳ� Ʃ�丮��
            TutorialCloudFactory1();

            // ���� ���� Ʃ�丮��
            TutorialGiveCloud();

            // ���� �����Ϸ� ���� �� �ȳ� Ʃ�丮��
            TutorialCloudFactory2();

            // ���� ���� Ʃ�丮��
            TutorialCloudDeco();

            // ���� ���� Ʃ�丮��
            TutorialCloudStorage();

            // ���� ���� �� ó�� Ʃ�丮��
            TutorialSOW3();

			// Guide ��ǳ�� ���¿� ����, ȭ�� ��ġ�� ���� ������Ʈ ���� ����
			ChangeEmptyScreenObjectStatus();

			// Arrow UI ������Ʈ ���� ����
			ChangeArrowObjectStatus();
        }
	}

    public bool IsGuideSpeechBubbleExist()
    {
        if (guideSpeechBubbleObject != null) { return true; }
        return false;
    }

    public void SetActiveGuideSpeechBubble(bool _bool)  { if (guideSpeechBubbleObject != null) { guideSpeechBubbleObject.SetActive(_bool); } }
	public GameObject GetActiveGuideSpeechBubble()		{ return guideSpeechBubbleObject; }
    public void SetActiveFadeOutScreen(bool _bool)      { if (fadeOutScreenObject != null) { fadeOutScreenObject.SetActive(_bool); } }
	public void SetActiveArrowUIObject(bool _bool)		{ if (arrowObject != null) { arrowObject.SetActive(_bool); } }

    // ������ ����(1) Ʃ�丮��
    // ��ǳ���� ����. ��� �� ��� �ѱ�� ��ư ��
    // ������ ��ư�� ����ǥ ����
    // ������ �� ��Ӱ�, ������ ��ư�� Ŭ�� ����
    public void TutorialOfSOW1()
    {
		if (SceneManager.GetActiveScene().name == "Space Of Weather"
				&& !isFinishedTutorial[0]
				&& guideSpeechBubbleObject == null
				&& fadeOutScreenObject == null)
		{
			mSOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();
			InstantiateBasicObjects(0);
		}
    }

    // ������ Ʃ�丮��
    public void TutorialDrawingRoom()
    {
		if (SceneManager.GetActiveScene().name == "Drawing Room"
				&& !isFinishedTutorial[1]
				&& guideSpeechBubbleObject == null
				&& fadeOutScreenObject == null)
		{ InstantiateBasicObjects(1); }
	}

    // ä�� Ʃ�丮��
    public void TutorialOfSOW2()
    {
		if (SceneManager.GetActiveScene().name == "Space Of Weather"
				&& isFinishedTutorial[0]
				&& !isFinishedTutorial[2]
				&& guideSpeechBubbleObject == null
				&& fadeOutScreenObject == null)
		{ tutorialGuest = mSOWManager.mUsingGuestObjectList[0]; InstantiateBasicObjects(2); }
    }

    public void TutorialCloudFactory1()
    {
		if (SceneManager.GetActiveScene().name == "Cloud Factory"
				&& !isFinishedTutorial[3]
				&& guideSpeechBubbleObject == null
				&& fadeOutScreenObject == null)
		{ InstantiateBasicObjects(3); }
    }

    public void TutorialGiveCloud()
    {
		if (SceneManager.GetActiveScene().name == "Give Cloud"
	            && !isFinishedTutorial[4]
	            && guideSpeechBubbleObject == null
	            && fadeOutScreenObject == null)
		{ InstantiateBasicObjects(4); }
    }

    public void TutorialCloudFactory2()
    {
		if (SceneManager.GetActiveScene().name == "Cloud Factory"
	        && isFinishedTutorial[3]
	        && !isFinishedTutorial[5]
	        && guideSpeechBubbleObject == null
	        && fadeOutScreenObject == null)
		{ InstantiateBasicObjects(5); }
    }

    public void TutorialCloudDeco()
    {
		if (SceneManager.GetActiveScene().name == "DecoCloud"
			&& !isFinishedTutorial[6]
			&& guideSpeechBubbleObject == null
			&& fadeOutScreenObject == null)
		{ InstantiateBasicObjects(6); }
    }

    public void TutorialCloudStorage()
    {
		if (SceneManager.GetActiveScene().name == "Cloud Storage"
			&& !isFinishedTutorial[7]
			&& guideSpeechBubbleObject == null
			&& fadeOutScreenObject == null)
		{ InstantiateBasicObjects(7); }
		;
    }

    public void TutorialSOW3()
    {
		if (SceneManager.GetActiveScene().name == "Space Of Weather"
			&& isFinishedTutorial[0]
			&& isFinishedTutorial[2]
			&& !isFinishedTutorial[8]
			&& guideSpeechBubbleObject == null
			&& fadeOutScreenObject == null)
		{ InstantiateBasicObjects(8); }
	}

	public void FadeOutScreen()
	{
		fadeOutScreenObject = Instantiate(commonFadeOutScreen);
		fadeOutScreenObject.transform.SetParent(GameObject.Find("Canvas").transform);
		fadeOutScreenObject.transform.localPosition = new Vector3(0f, 0f, 0f);
	}

	public void FadeOutSpaceOfWeather()
	{
		fadeOutScreenObject = Instantiate(fadeOutScreen1);
		fadeOutScreenObject.transform.SetParent(GameObject.Find("Canvas").transform);
		fadeOutScreenObject.transform.localPosition = new Vector3(0f, 0f, 0f);
	}

    public void FadeOutCloudStorage()
    {
		fadeOutScreenObject = Instantiate(storageFadeOutScreen);
		fadeOutScreenObject.transform.SetParent(GameObject.Find("Canvas").transform);
		fadeOutScreenObject.transform.localPosition = new Vector3(0f, 0f, 0f);
	}

    public void FadeOutDecoCloud()
    {
		fadeOutScreenObject = Instantiate(decoFadeOutScreen);
		fadeOutScreenObject.transform.SetParent(GameObject.Find("Canvas").transform);
		fadeOutScreenObject.transform.localPosition = new Vector3(0f, 0f, 0f);
	}

	// Tutorial �� ��� ������ ��µǴ� �⺻ ������Ʈ(emptyScreenObject, guideSpeechBubbleObject)�� �������ش�.
	public void InstantiateBasicObjects(int dialog_index)
    {
		emptyScreenObject = Instantiate(emptyScreen);
		emptyScreenObject.transform.SetParent(GameObject.Find("Canvas").transform);
		emptyScreenObject.transform.localPosition = new Vector3(0f, 0f, 0f);

		// ��ǳ�� ������Ʈ ����
		guideSpeechBubbleObject = Instantiate(guideSpeechBubble);
		guideSpeechBubbleObject.transform.SetParent(GameObject.Find("Canvas").transform);
		guideSpeechBubbleObject.transform.localPosition = new Vector3(0f, -340f, 0f);
		guideSpeechBubbleObject.GetComponent<GuideBubbleScript>().SetDialogIndex(dialog_index);

        if(SceneManager.GetActiveScene().name == "Space Of Weather"
            || SceneManager.GetActiveScene().name == "Drawing Room"
            || SceneManager.GetActiveScene().name == "Cloud Factory")
        { 
            GameObject.Find("B_Option").transform.SetAsLastSibling();

            GameObject option_object = GameObject.Find("UIManager").GetComponent<CommonUIManager>().gOption;
			option_object.transform.SetAsLastSibling();
		}
	}

    public void ChangeEmptyScreenObjectStatus()
    {
		// ���̵� ��ǳ���� �������� ȭ�� ��ġ�� ���� emptyScreenObject�� �����ش�.
		if (guideSpeechBubbleObject == null
			&& emptyScreenObject != null)
		{ Destroy(emptyScreenObject.gameObject); }

		//���̵� ��ǳ���� ���¿� ���� ȭ�� ��ġ�� ���� emptyScreenObject�� ���µ� ����
		if (guideSpeechBubbleObject != null)
		{
			if (guideSpeechBubbleObject.activeSelf == false
			&& emptyScreenObject.activeSelf == true)
			{ emptyScreenObject.SetActive(false); }

			else if (guideSpeechBubbleObject.activeSelf == true
			&& emptyScreenObject.activeSelf == false)
			{ emptyScreenObject.SetActive(true); }
		}

		if ((isFinishedTutorial[1] == true
			&& isFinishedTutorial[2] == false
			&& mSOWManager.mUsingGuestObjectList.Count > 0
			&& mSOWManager.mUsingGuestObjectList[0].GetComponent<GuestObject>().isSpeakEmotion == true
			&& guideSpeechBubbleObject.activeSelf == false
			&& blockScreenTouchObject != null)
			||
			(isFinishedTutorial[0] == true
			&& isFinishedTutorial[2] == true
			&& isFinishedTutorial[8] == false
			&& mSOWManager.mUsingGuestObjectList.Count > 0
			&& mGuestManager.mGuestInfo[0].isUsing == true
			&& guideSpeechBubbleObject.activeSelf == false
			&& blockScreenTouchObject != null)
			||
			(isFinishedTutorial[0] == true
			&& isFinishedTutorial[2] == true
			&& isFinishedTutorial[8] == false
			&& tutorialGuest == null
			&& guideSpeechBubbleObject.activeSelf == false
			&& blockScreenTouchObject != null))
		{
			if (arrowObject != null) { arrowObject.SetActive(false); }
			SetActiveGuideSpeechBubble(true);
			Destroy(blockScreenTouchObject);
		}
	}

	public void ChangeArrowObjectStatus()
	{
		if (isFinishedTutorial[1] == true
			&& isFinishedTutorial[2] == false
			&& arrowObject == null
			&& mSOWManager.mUsingGuestObjectList[0].GetComponent<GuestObject>().isSit == true)
		{
			InstantiateArrowUIObject(mSOWManager.mUsingGuestObjectList[0].transform.localPosition, -1.75f, 1f, false);
			arrowObject.transform.localScale = new Vector3(-0.5f, 0.5f, 1f);
		}


		if (SceneManager.GetActiveScene().name == "Cloud Factory"
			&& isFinishedTutorial[4] == true
			&& isFinishedTutorial[5] == false
			&& arrowObject != null
			&& arrowObject.activeSelf == false
			&& GameObject.Find("B_GoDecoCloud"))
		{ SetActiveArrowUIObject(true); }
	}

	public void InstantiateBlockScreenTouchObject()
    {
        blockScreenTouchObject = Instantiate(emptyScreen);
		blockScreenTouchObject.transform.SetParent(GameObject.Find("Canvas").transform);
		blockScreenTouchObject.transform.localPosition = new Vector3(0f, 0f, 0f);
	}

	public void InstantiateArrowUIObject(Vector3 target_position, float xpos_difference = 0f, float ypos_difference = 0f, bool in_canvas = true)
	{
		if(in_canvas == false) { arrowObject = Instantiate(leftArrowNotInCanvas); }
		else if (xpos_difference <= 0) { arrowObject = Instantiate(leftArrow); }
		else { arrowObject = Instantiate(rightArrow); }

		if (in_canvas == true) { arrowObject.transform.SetParent(GameObject.Find("Canvas").transform); }
		arrowObject.transform.localPosition = new Vector3(xpos_difference, ypos_difference, 0f) + target_position;
	}

	public void DestroyAllObject()
    {
        if(guideSpeechBubbleObject != null) { Destroy(guideSpeechBubbleObject); }
        if(fadeOutScreenObject != null)		{ Destroy(fadeOutScreenObject); }
        if(emptyScreenObject != null)		{ Destroy(emptyScreenObject); }
        if(blockScreenTouchObject != null)	{ Destroy(blockScreenTouchObject); }
		if(arrowObject != null)				{ Destroy(arrowObject); }
    }
}
