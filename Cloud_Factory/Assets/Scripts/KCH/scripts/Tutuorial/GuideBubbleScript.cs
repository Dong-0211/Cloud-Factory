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

	TutorialManager mTutorialManager;

	void Awake()
	{
		tGuideText = transform.Find("Button").gameObject.transform.Find("Text").GetComponent<Text>();
		mTutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();

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

		mDialog = new string[8, 30];

		for (int num1 = 0; num1 < 8; num1++)
		{ for(int num2 = 0; num2 < 30; num2++)
			{
				mDialog[num1, num2] = " ";
			}
		}

		mDialog[0, 0] = "test1";
		mDialog[0, 1] = "test2";
		mDialog[0, 2] = "�̰� ������ ��ư...";
		mDialog[0, 3] = "�����ǿ� ����ǥ ǥ�ð� ������....";
		mDialog[0, 4] = "�����ǿ� ���� ���� �Դ��� Ȯ���غ��ô�.";
		mDialog[0, 5] = "FadeOut1";

		mDialog[1, 0] = "�̰��� ������ �Դϴ�. Ŭ���� ���丮�� ã�ƿ� �մԵ��� �̾߱⸦ ���� �� �ִ� ����Դϴ�.";
		mDialog[1, 1] = "DialogSpace1";
		mDialog[1, 2] = "��Ƽ�� ���ϴ� ��Ʈ�� ���� ���� ������ ������ �� �ľ��غ���.";
		mDialog[1, 3] = "DialogSpace2";
		mDialog[1, 4] = "Ŭ���� ���丮�� ù��° �մ��� �޾ƺ��ô�.";

		mDialog[2, 0] = "��Ƽ�� ������ �����ޱ� ������ �� �������� ��⸦ ��.";
		mDialog[2, 1] = "��Ƽ�� Ŭ���ϸ�, ���� ������ ���� ������ ���� �� �־�.";
		mDialog[2, 2] = "������ �����ϱ� ���� ��Ḧ ä���غ���.";
		mDialog[2, 3] = "���� ������ Ŭ���ϸ� ��Ḧ ä���� �� �־�. �� �� Ŭ���غ���?";
		mDialog[2, 4] = "Gathering";
		mDialog[2, 5] = "����? �̷��� ��� ä���� �� ���� �־�.";
		mDialog[2, 6] = "���� ������������ �̵��ؼ� ������ ����� ����.";
		mDialog[2, 7] = "FadeOut2";

		mDialog[3, 0] = "���� ���� ��踦 Ŭ������.";
		mDialog[3, 1] = "FadeOut3";

		mDialog[4, 0] = "���⼭ ������ ������ �� �־�.";
		mDialog[4, 1] = "��Ḧ ���� ��迡 �־��?";
		mDialog[4, 2] = "��Ḧ ��� �־�����, '�����ϱ�' ��ư�� ������.";
		mDialog[4, 3] = "FadeOut4";                     
		mDialog[4, 4] = "������ ����������� �ٽ� �������� ���ư���.";
		mDialog[4, 5] = "FadeOut5";

		mDialog[5, 0] = "������ ��迡�� ������ Ŭ���Ѵ�.";
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
		if(mDialog[mDialogIndex, currentDialogNum] == "FadeOut1") {
			mTutorialManager.FadeOutScreen();
			GameObject.Find("B_Drawing Room").transform.SetAsLastSibling();
			this.gameObject.SetActive(false);
			return;
		}

		if(mDialog[mDialogIndex, currentDialogNum] == "DialogSpace1"
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

		if(mDialog[mDialogIndex, currentDialogNum] == "FadeOut2")
		{
			mTutorialManager.FadeOutScreen();
			GameObject.Find("B_Cloud Factory").transform.SetAsLastSibling();
			this.gameObject.SetActive(false); ;
			return;
		}

		if (mDialog[mDialogIndex, currentDialogNum] == "FadeOut3")
		{
			mTutorialManager.FadeOutScreen();
			GameObject.Find("B_GiveCloud").transform.SetAsLastSibling();
			this.gameObject.SetActive(false);
			return;
		}

		if(mDialog[mDialogIndex, currentDialogNum] == "FadeOut4")
		{
			presentDialogNum++;
			currentDialogNum++;
			this.gameObject.SetActive(false);
		}

		if(mDialog[mDialogIndex, currentDialogNum] == "FadeOut5")
		{
			mTutorialManager.FadeOutScreen();
			GameObject.Find("B_Back").transform.SetAsLastSibling();
			this.gameObject.SetActive(false);
			return;
		}

		tGuideText.text = mDialog[mDialogIndex, currentDialogNum];
	}

	public void UpdateText()
	{
		presentDialogNum++;
	}
}
