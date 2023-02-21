using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideBubbleScript : MonoBehaviour
{
	private Text tGuideText;

	private string[,] mDialog;

	[HideInInspector]
	private int mDialogIndex;		// �� ��° Dialog�� �ҷ��� ������ ����(�ܺο��� �Է� ����)
	private int currentDialogNum;   // �ֱ� �ؽ�Ʈ �ѹ�
	[HideInInspector]
	private int presentDialogNum;    // ���� �ؽ�Ʈ �ѹ�, currentDialogNum != presentDialogNum�� ��, currentDialogNum <= presentDialogNum && Update Text

	private TutorialManager mTutorialManager;
	private	SOWManager		mSOWManager;

	void Awake()
	{
		tGuideText = transform.Find("Button").gameObject.transform.Find("Text").GetComponent<Text>();
		mTutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
		mSOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();

		InitDialog();
	}

	void Update()
	{
		ReadDialog();
	}

	public void SetDialogIndex(int idx = 0) { mDialogIndex = idx; }

	// Dialog �ʱ�ȭ, ����ִ� string�� " "���� ����
	private void InitDialog()
	{
		mDialogIndex = 0;
		currentDialogNum = -1;
		presentDialogNum = 0;

		mDialog = new string[9, 30];

		for (int num1 = 0; num1 < 9; num1++)
		{ for(int num2 = 0; num2 < 30; num2++)
			{
				mDialog[num1, num2] = " ";
			}
		}
		// ���� UI fadeOut(����ȭ��)���� �� ������ �̿밡�� �ϵ��� �ؾ� ��

		mDialog[0, 0] = "���丮 ����1";
		mDialog[0, 1] = "���丮 ����2";
		mDialog[0, 2] = "���丮 ����3";
		mDialog[0, 3] = "�����ǿ� ����ǥ ǥ�ð� ������ �մ��� �Դٴ� �� �ǹ��մϴ�.";
		mDialog[0, 4] = "�����ǿ� ���� ���� �Դ��� Ȯ���غ��ô�.";
		// ȭ��ǥ UI
		mDialog[0, 5] = "FadeOut10";

		mDialog[1, 0] = "�̰��� ������ �Դϴ�. Ŭ���� ���丮�� ã�ƿ� �մԵ��� �̾߱⸦ ���� �� �ִ� ����Դϴ�.";
		mDialog[1, 1] = "DialogSpace1";
		mDialog[1, 2] = "��Ƽ�� ���ϴ� ��Ʈ�� ���� ���� ������ ������ �� �ľ��غ���.";
		mDialog[1, 3] = "DialogSpace2";
		mDialog[1, 4] = "Ŭ���� ���丮�� ù��° �մ��� �޾ƺ��ô�.";

		mDialog[2, 0] = "���� �մ��� ������ �������� ������ ��ٸ��ϴ�.";
		mDialog[2, 1] = "�մ��� �ڸ��� �ɾ����� �� Ŭ���ϸ�, �մ��� ������ �� �� �� Ȯ���� �� �ֽ��ϴ�.";
		// ȭ��ǥ UI
		mDialog[2, 2] = "FadeOut20";
		mDialog[2, 3] = "������ ������ �� �ʿ��� ��Ḧ ä���غ��ô�.";
		mDialog[2, 4] = "Gathering";
		mDialog[2, 5] = "���� ������ ���鷯 �������?";
		// ȭ��ǥ UI
		mDialog[2, 6] = "FadeOut21";

		mDialog[3, 0] = "������ ���� ���� ��踦 ���� ������ �� �ֽ��ϴ�.";
		// ȭ��ǥ UI�� ����
		mDialog[3, 1] = "FadeOut30";

		mDialog[4, 0] = "ä���� ������ ��� ����� ������ �����ô�.";
		// ��� ��� ���� �� ȭ��ǥ UI�� ����
		mDialog[4, 1] = "FadeOut40";                     
		mDialog[4, 2] = "������ ����������� �ٽ� �������� ���ư����ô�.";
		// ���ư��� ��ư ȭ��ǥ UI�� ����
		mDialog[4, 3] = "FadeOut41";

		mDialog[5, 0] = "��� ���� ������ ��迡�� ������ �ֽ��ϴ�. ������ ���������� ������ �� ������ ���� ����غ��ô�.";
		// ������ ��� ������ ��, ȭ��ǥ UI
		mDialog[5, 1] = "FadeOut50";

		// fade out emotion list
		mDialog[6, 0] = "������ ����� Ŭ���ϸ� ���õǰ�, ���ϴ� ��ġ�� ������ ���� �� �ֽ��ϴ�.";
		mDialog[6, 1] = "FadeOut60";
		mDialog[6, 2] = "�̰������� �� ������ �մԿ��� � ������ ��ĥ�� ������ �� �ֽ��ϴ�.";
		mDialog[6, 3] = "�մ��� ������ �����ϰ� �;� ������, ���Ŀ� '+'�� �ǵ��� �غ��ô�.";
		mDialog[6, 4] = "GuideSpeechOut60";
		// ȭ��ǥ UI
		mDialog[6, 5] = "�ٸ� ������ ��� �����ϰ� ����� ��ġ�� ��, �ٹ̱⸦ ���ĺ��ô�.";
		mDialog[6, 6] = "FadeOut61";
		// ���ư��� ��ư ����

		// ȭ��ǥ UI
		mDialog[7, 0] = "�ϼ��� ������ ���� �����Կ� ����˴ϴ�.";
		mDialog[7, 1] = "�� ������ ���� �Ⱓ�� ������ �ֽ��ϴ�. ���� �Ⱓ�� ������ ������ ������ϴ�.";
		mDialog[7, 2] = "������ ��Ŭ�� �ϴ� ���� � ��Ḧ ����Ͽ����� Ȯ���� �� �ֽ��ϴ�.";
		// ��Ŭ�� �̺�Ʈ �߰� �ʿ�
		mDialog[7, 3] = "������ ���� �� �����غ��ô�.";
		// ȭ��ǥ UI
		mDialog[7, 4] = "FadeOut70";
		mDialog[7, 5] = "�����ϱ� ��ư�� ���� ��Ƽ���� ������ �ݽô�.";
		mDialog[7, 6] = "FadeOut71";

		mDialog[8, 0] = "FadeOut80";
		mDialog[8, 1] = "������ �ް� ������ ��ȭ�� ���� �մ��� ������ ���ư��ϴ�. ������ �ٽ� �湮������ �𸣰ڳ׿�.";
		mDialog[8, 2] = "FadeOut81";
		mDialog[8, 3] = "��� ���̵带 ���ƽ��ϴ�. ���� ��Ƽ�� �Ƚ�ó�� �� �� �ִ� Ŭ���� ���丮�� �Ǳ� �ٶ��ϴ�.";
		mDialog[8, 4] = "End";
	}


	private void ReadDialog()
	{
		if (currentDialogNum == presentDialogNum) { return; }
		else currentDialogNum = presentDialogNum;

		if(mDialog[mDialogIndex, currentDialogNum] == " ") {
			this.gameObject.SetActive(false);
			return;
		}

		// ù ��° ������ ���� ȭ�� ���̵� �ƿ�
		// Ʃ�丮�� ���� Ȯ�� ���δ� ������ ��ư���� ó��
		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut10") {
			mTutorialManager.FadeOutScreen();
			GameObject.Find("B_Drawing Room").transform.SetAsLastSibling();
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "DialogSpace1"
			|| mDialog[mDialogIndex, currentDialogNum] == "DialogSpace2")
		{
			presentDialogNum++;
			currentDialogNum++;
			this.gameObject.SetActive(false);
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "Gathering") {
			mTutorialManager.FadeOutSpaceOfWeather();
			presentDialogNum++;
			currentDialogNum++;
			this.gameObject.SetActive(false);
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut20")
		{
			mTutorialManager.InstantiateBlockScreenTouchObject();
			GameObject.Find("B_Option").transform.SetAsLastSibling();
			GameObject option_object = GameObject.Find("UIManager").GetComponent<CommonUIManager>().gOption;
			option_object.transform.SetAsLastSibling();
			presentDialogNum++;
			currentDialogNum++;
			this.gameObject.SetActive(false);
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut21")
		{
			mTutorialManager.FadeOutScreen();
			GameObject.Find("B_Cloud Factory").transform.SetAsLastSibling();
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut30")
		{
			mTutorialManager.FadeOutScreen();
			GameObject.Find("B_GiveCloud").transform.SetAsLastSibling();
			this.gameObject.SetActive(false);
			return;
		}

		if(mDialog[mDialogIndex, currentDialogNum] == "FadeOut40")
		{
			presentDialogNum++;
			currentDialogNum++;
			this.gameObject.SetActive(false);
		}

		if(mDialog[mDialogIndex, currentDialogNum] == "FadeOut41")
		{
			mTutorialManager.FadeOutScreen();
			GameObject.Find("B_Back").transform.SetAsLastSibling();
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut50")
		{
			mTutorialManager.FadeOutScreen();
			GameObject.Find("B_GiveCloud").transform.SetAsLastSibling();
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut60")
		{
			presentDialogNum++;
			currentDialogNum++;
			mTutorialManager.FadeOutDecoCloud();
			this.gameObject.transform.SetAsLastSibling();
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "GuideSpeechOut60")
		{
			presentDialogNum++;
			currentDialogNum++;
			this.gameObject.SetActive(false);
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut61")
		{
			this.gameObject.SetActive(false);
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut70")
		{
			mTutorialManager.FadeOutCloudStorage();
			presentDialogNum++;
			currentDialogNum++;
			this.gameObject.SetActive(false);
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut71")
		{
			mTutorialManager.FadeOutScreen();
			GameObject tempButton = GameObject.Find("I_ProfileBG1").transform.Find("B_CloudGIve").gameObject;
			tempButton.transform.SetParent(GameObject.Find("Canvas").transform);
			tempButton.transform.SetAsLastSibling();
			this.gameObject.SetActive(false);
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut80")
		{
			mTutorialManager.InstantiateBlockScreenTouchObject();
			GameObject.Find("B_Option").transform.SetAsLastSibling();
			GameObject option_object = GameObject.Find("UIManager").GetComponent<CommonUIManager>().gOption;
			option_object.transform.SetAsLastSibling();
			presentDialogNum++;
			currentDialogNum++;
			this.gameObject.SetActive(false);
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut81")
		{
			mTutorialManager.InstantiateBlockScreenTouchObject();
			GameObject.Find("B_Option").transform.SetAsLastSibling();
			GameObject option_object = GameObject.Find("UIManager").GetComponent<CommonUIManager>().gOption;
			option_object.transform.SetAsLastSibling();
			presentDialogNum++;
			currentDialogNum++;
			this.gameObject.SetActive(false);
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "End")
		{
			mTutorialManager.isFinishedTutorial[8] = true;
			mTutorialManager.isTutorial = false;
			mTutorialManager.DestroyAllObject();
			return;
		}
			tGuideText.text = mDialog[mDialogIndex, currentDialogNum];
	}

	public void UpdateText()
	{
		// ������ Ʃ�丮�� ��, ������ �������� ��Ƽ�� �ڸ��� �ɱ� ������ �ؽ�Ʈ �ѱ��� ���´�.
		if (mTutorialManager.isFinishedTutorial[1] == true
			&& mTutorialManager.isFinishedTutorial[2] == false
			&& mSOWManager.mUsingGuestObjectList.Count > 0
			&& mSOWManager.mUsingGuestObjectList[0].GetComponent<GuestObject>().isSit == false
			&& presentDialogNum >= 2)
		{ return; }
		presentDialogNum++;
	}
}
