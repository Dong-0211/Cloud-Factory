using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//GuestObject.prefab�� �߰� ����
public class RLHReader : MonoBehaviour
{
	// �ҷ��� ���� ����
	private Guest mGuestManager;
	private int mGuestNum;                       // �մ��� ��ȣ�� �Ѱܹ޴´�.

	[SerializeField]
	private RLHDB mRLHDB;                 // ��ȭ ������ ������ ���� DB

	private string mDialogText;                 // ������ ȭ�鿡 ��½�ų ����

	public Text tText;                          // ��ȭ�� ���� �� �ؽ�Ʈ

	void SetGuestNum(int guest_num = 0) { mGuestNum = guest_num; }

	// Start is called before the first frame update
	void Awake()
    {
		tText.text = "";
		mGuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();
	}

    void LoadHintInfo()
	{
		List<RLHDBEntity> Hint;
		Hint = mRLHDB.SetHintByGuestNum(mGuestNum);

		if(Hint == null) { return; }

		List<int> satEmotions = new List<int>();

		int leastDiffEmotion =	mGuestManager.SpeakLeastDiffEmotion(mGuestNum);
		int mostDiffEmotion =	mGuestManager.SpeakEmotionDialog(mGuestNum);

		if (mostDiffEmotion != -1) { satEmotions.Add(mostDiffEmotion); }	// ���������� ���� �ָ� ������ ������ ���
		if (leastDiffEmotion != -1											// �ش� ������ ������ ��,
			&& satEmotions.Count > 0										// ������ ���� ���� ū ������ ������ ��(�������� ������, ���̰� ���� ���� ������ ����X)
			&& !satEmotions.Contains(leastDiffEmotion))						// ������ ���� ���� ū ������ ���� ���� ������ ���� ������ �ƴ� ��(������ ������ ���� ������ �ϳ��� ��츦 Ȯ��)
		{ satEmotions.Add(leastDiffEmotion); }

		if (satEmotions.Count <= 0) { return; }								// ������ ������ ��� ������ ������ return;

		for(int num = 0; num < Hint.Count; num++)
		{
			if (Hint[num].GuestID == mGuestNum								// �մ��� ��ȣ�� ��ġ
				&& Hint[num].Type == "Hint")								// RHL�׸��� Hint�� ���
			{ 
				foreach(int emotion in satEmotions)
				{
					if (Hint[num].Emotion == emotion) { tText.text += Hint[num].KOR; }
				} 
			}
		}
	}
}

/*
 * �߰� �� �͵�
 * SOWManager.cs 119:0 tempObject.GetComponent<RLHReader>().SetGuestNum(guest_num);
 * */
