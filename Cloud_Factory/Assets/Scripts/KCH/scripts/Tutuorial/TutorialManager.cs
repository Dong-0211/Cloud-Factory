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

    /*
     #Ʃ�丮�� ���൵ üũ
     ������ ���� 1
	 ������
	 ������ ���� 2 (ä��)
	 ���� ����
	 ���� ����
	 ���� ����
	 �մ� ���
    */
    [HideInInspector]
	public bool[] isFinishedTutorial;

    public GameObject emptyScreen;          // ȭ�� Ŭ���� �������� ������Ʈ
    private GameObject emptyScreenObject;

    public GameObject guideSpeechBubble;    // ���̵� ��ǳ�� ������Ʈ(child�� charImage, Text, button ����)
    private GameObject guideSpeechBubbleObject;        // ������Ʈ�� �����ϱ� ���� ������Ʈ

    public GameObject fadeOutScreen;        // ������ ����(1) Ʃ�丮�󿡼� ���Ǵ� FadeOutScreen
    private GameObject fadeOutScreenObject;



	private static TutorialManager instance = null;
    void Awake()
    {
        if(null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            isTutorial = true;
            isFinishedTutorial = new bool[7];

            for(int num = 0; num < isFinishedTutorial.Length; num++) { isFinishedTutorial[num] = false; }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().name == "Space Of Weather"
            && !isFinishedTutorial[0]
            && guideSpeechBubbleObject == null
            && fadeOutScreenObject == null)
        { TutorialOfSOW(); }

        // ���̵� ��ǳ���� �������� ȭ�� ��ġ�� ���� emptyScreenObject�� �����ش�.
        if(guideSpeechBubbleObject == null
            && emptyScreenObject != null)
        {
            Destroy(emptyScreenObject.gameObject);
        }
    }

    public bool IsGuideSpeechBubbleExist()
    {
        if (guideSpeechBubbleObject != null) { return true; }
        return false;
    }

    // ������ ����(1) Ʃ�丮��
    // ��ǳ���� ����. ��� �� ��� �ѱ�� ��ư ��
    // ������ ��ư�� ����ǥ ����
    // ������ �� ��Ӱ�, ������ ��ư�� Ŭ�� ����
    public void TutorialOfSOW()
    {
        InstantiateBasicObjects(0);

    }

    public void FadeOutSpaceOfWeather()
    {
        fadeOutScreenObject = Instantiate(fadeOutScreen);
		fadeOutScreenObject.transform.SetParent(GameObject.Find("Canvas").transform);
		fadeOutScreenObject.transform.localPosition = new Vector3(0f, 0f, 0f);
    }

    // ������ Ʃ�丮��
    // ��Ʈ�� ���� �� ��ǳ���� ����
    // DialogManager.cs 284���� ���
    public void TutorialDrawingRoom()
    {
        InstantiateBasicObjects(1);
	}

    public void FinishTutorial1()
    {
        isFinishedTutorial[0] = true;
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
	}
}
