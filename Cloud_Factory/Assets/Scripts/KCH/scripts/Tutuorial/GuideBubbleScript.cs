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
		tGuideText = transform.Find("Text").gameObject.GetComponent<Text>();
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

		mDialog = new string[7, 30];

		for (int num1 = 0; num1 < 7; num1++)
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
		mDialog[0, 5] = "BlackOut1";

		mDialog[1, 0] = "��Ƽ�� ���ϴ� ��Ʈ�� ���� ���� ������ ������ �� �ľ��غ���.";
	}


	private void ReadDialog()
	{
		if (currentDialogNum == presentDialogNum) { return; }
		else currentDialogNum = presentDialogNum;

		if(mDialog[mDialogIndex, currentDialogNum] == " ") { 
			Destroy(this.gameObject);
			return;
		}

		// ù ��° ������ ���� ȭ�� ���̵� �ƿ�
		// Ʃ�丮�� ���� Ȯ�� ���δ� ������ ��ư���� ó��
		if(mDialog[mDialogIndex, currentDialogNum] == "BlackOut1") {
			mTutorialManager.FadeOutSpaceOfWeather();
			Destroy(this.gameObject);
			return;
		}

		tGuideText.text = mDialog[mDialogIndex, currentDialogNum];
	}

	public void UpdateText()
	{
		presentDialogNum++;
	}
}
