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

    public GameObject emptyScreen;              // ȭ�� Ŭ���� �������� ������Ʈ
    private GameObject emptyScreenObject;       // ��ǳ���� �Բ� ����/���� �Ǵ� ������Ʈ
    private GameObject blockScreenTouchObject;  // ��ǳ���� ��� ȭ���� ��ġ�� ���� �� ���Ǵ� ������Ʈ

    public GameObject guideSpeechBubble;    // ���̵� ��ǳ�� ������Ʈ(child�� charImage, Text, button ����)
    private GameObject guideSpeechBubbleObject;        // ������Ʈ�� �����ϱ� ���� ������Ʈ

    public GameObject commonFadeOutScreen;   // ȭ���� ��� ������ ���� Fade Out ��ũ��
    public GameObject fadeOutScreen1;
    public GameObject storageFadeOutScreen;
    public GameObject decoFadeOutScreen;
    private GameObject fadeOutScreenObject;

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

            if (SceneManager.GetActiveScene().name == "Space Of Weather"
                && !isFinishedTutorial[0]
                && guideSpeechBubbleObject == null
                && fadeOutScreenObject == null)
            {
                mSOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();
                TutorialOfSOW1();
            }

            if (SceneManager.GetActiveScene().name == "Drawing Room"
                && !isFinishedTutorial[1]
                && guideSpeechBubbleObject == null
                && fadeOutScreenObject == null)
            { TutorialDrawingRoom(); }

            if (SceneManager.GetActiveScene().name == "Space Of Weather"
                && isFinishedTutorial[0]
                && !isFinishedTutorial[2]
                && guideSpeechBubbleObject == null
                && fadeOutScreenObject == null)
            { TutorialOfSOW2(); }

            if (SceneManager.GetActiveScene().name == "Cloud Factory"
                && !isFinishedTutorial[3]
                && guideSpeechBubbleObject == null
                && fadeOutScreenObject == null)
            { TutorialCloudFactory1(); }

            if (SceneManager.GetActiveScene().name == "Give Cloud"
                && !isFinishedTutorial[4]
                && guideSpeechBubbleObject == null
                && fadeOutScreenObject == null)
            { TutorialGiveCloud(); }

            if (SceneManager.GetActiveScene().name == "Cloud Factory"
				&& isFinishedTutorial[3]
				&& !isFinishedTutorial[5]
                && guideSpeechBubbleObject == null
                && fadeOutScreenObject == null)
            { TutorialCloudFactory2(); }

            if (SceneManager.GetActiveScene().name == "DecoCloud"
                && !isFinishedTutorial[6]
                && guideSpeechBubbleObject == null
                && fadeOutScreenObject == null)
            { TutorialCloudDeco(); }

            if (SceneManager.GetActiveScene().name == "Cloud Storage"
                && !isFinishedTutorial[7]
                && guideSpeechBubbleObject == null
                && fadeOutScreenObject == null)
            { TutorialCloudStorage(); }

            if (SceneManager.GetActiveScene().name == "Space Of Weather"
                && isFinishedTutorial[0]
                && isFinishedTutorial[2]
                && !isFinishedTutorial[8]
                && guideSpeechBubbleObject == null
                && fadeOutScreenObject == null)
            { TutorialSOW3(); }

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
                && mSOWManager.mUsingGuestObjectList.Count <= 0
                && guideSpeechBubbleObject.activeSelf == false
                && blockScreenTouchObject != null))
            {
                SetActiveGuideSpeechBubble(true);
                Destroy(blockScreenTouchObject);
            }
        }

	}

    public bool IsGuideSpeechBubbleExist()
    {
        if (guideSpeechBubbleObject != null) { return true; }
        return false;
    }

    public void SetActiveGuideSpeechBubble(bool _bool)  { if (guideSpeechBubbleObject != null) { guideSpeechBubbleObject.SetActive(_bool); } }
    public void SetActiveFadeOutScreen(bool _bool)      { if (fadeOutScreenObject != null) { fadeOutScreenObject.SetActive(_bool); } }

    // ������ ����(1) Ʃ�丮��
    // ��ǳ���� ����. ��� �� ��� �ѱ�� ��ư ��
    // ������ ��ư�� ����ǥ ����
    // ������ �� ��Ӱ�, ������ ��ư�� Ŭ�� ����
    public void TutorialOfSOW1()
    {
        InstantiateBasicObjects(0);

    }

    // ������ Ʃ�丮��
    public void TutorialDrawingRoom()
    {
        InstantiateBasicObjects(1);
	}

    // ä�� Ʃ�丮��(����ǥ�� �߰� �ʿ�)
    public void TutorialOfSOW2()
    {
        InstantiateBasicObjects(2);
    }

    public void TutorialCloudFactory1()
    {
        InstantiateBasicObjects(3);
    }

    public void TutorialGiveCloud()
    {
        InstantiateBasicObjects(4);
    }

    public void TutorialCloudFactory2()
    {
        InstantiateBasicObjects(5);
    }

    public void TutorialCloudDeco()
    {
        InstantiateBasicObjects(6);
    }

    public void TutorialCloudStorage()
    {
        InstantiateBasicObjects(7);
    }
    public void TutorialSOW3()
    {
		InstantiateBasicObjects(8);
	}


    public void FinishTutorial1()
    {
        isFinishedTutorial[0] = true;
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

    public void InstantiateBlockScreenTouchObject()
    {
        blockScreenTouchObject = Instantiate(emptyScreen);
		blockScreenTouchObject.transform.SetParent(GameObject.Find("Canvas").transform);
		blockScreenTouchObject.transform.localPosition = new Vector3(0f, 0f, 0f);
	}

    public void DestroyAllObject()
    {
        if(guideSpeechBubbleObject != null) { Destroy(guideSpeechBubbleObject); }
        if(fadeOutScreenObject != null) { Destroy(fadeOutScreenObject); }
        if(emptyScreenObject != null) { Destroy(emptyScreenObject); }
        if(blockScreenTouchObject != null) { Destroy(blockScreenTouchObject); }
    }
}
