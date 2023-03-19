using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//GuestObject.prefab�� �߰� ����
public class RLHReader : MonoBehaviour
{
	// �ҷ��� ���� ����
	private Guest mGuestManager;

	[SerializeField]
	private RLHDB mRLHDB;						// ��ȭ ������ ������ ���� DB

	private string mDialogText;                 // ������ ȭ�鿡 ��½�ų ����

	private string tText;						// ��ȭ�� ���� �� �ؽ�Ʈ

	// Start is called before the first frame update
	void Awake()
    {
		tText = "";
		mGuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();
	}

	//ProfileManager.cs
	public string LoadRecordInfo(int guest_num)
	{
		List<RLHDBEntity> Record;
		Record = mRLHDB.SetHintByGuestNum(guest_num);

		if(Record == null) { return ""; }

		for(int num = 0; num < Record.Count; num++)
		{
			if (Record[num].GuestID == guest_num + 1
				&& Record[num].Type == "record")
			{ tText = "";  tText = Record[num].KOR; }
		}
		return tText;
	}

	//GuestObject.cs
    public void LoadHintInfo(int guest_num)
	{
		List<RLHDBEntity> Hint;
		Hint = mRLHDB.SetHintByGuestNum(guest_num);

		if(Hint == null) { return; }

		List<int> satEmotions = new List<int>();

		int leastDiffEmotion =	mGuestManager.SpeakLeastDiffEmotion(guest_num);
		int mostDiffEmotion =	mGuestManager.SpeakEmotionDialog(guest_num);

		if (mostDiffEmotion != -1) { satEmotions.Add(mostDiffEmotion); }	// ���������� ���� �ָ� ������ ������ ���
		if (leastDiffEmotion != -1											// �ش� ������ ������ ��,
			&& satEmotions.Count > 0										// ������ ���� ���� ū ������ ������ ��(�������� ������, ���̰� ���� ���� ������ ����X)
			&& !satEmotions.Contains(leastDiffEmotion))						// ������ ���� ���� ū ������ ���� ���� ������ ���� ������ �ƴ� ��(������ ������ ���� ������ �ϳ��� ��츦 Ȯ��)
		{ satEmotions.Add(leastDiffEmotion); }

		if (satEmotions.Count <= 0) { return; }								// ������ ������ ��� ������ ������ return;

		for(int num = 0; num < Hint.Count; num++)
		{
			if (Hint[num].GuestID == guest_num + 1							// �մ��� ��ȣ�� ��ġ
				&& Hint[num].Type == "hint")								// RHL�׸��� hint�� ���
			{
				foreach(int emotion in satEmotions)
				{
					if (Hint[num].Emotion == emotion) { tText += Hint[num].KOR; tText += "\n"; }
				} 
			}
		}
	}

	// UIManager.object (Scene Of Weather)
	public string LoadLetterInfo(int guest_num)
	{
		List<RLHDBEntity> letter;
		letter= mRLHDB.SetHintByGuestNum(guest_num);

		if (letter == null) { return ""; }

		for (int num = 0; num < letter.Count; num++)
		{
			if (letter[num].GuestID == guest_num + 1
				&& letter[num].Type == "letter")
			{ tText = ""; tText = letter[num].KOR; }
		}
		return tText;
	}

	public string PrintHintText()
	{
		return tText;
	}
}
