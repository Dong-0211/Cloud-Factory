using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    [Header("Profile")]
    public GameObject[] iProfileBG = new GameObject[3];         // ������ ���� 3�� ����
    private GameObject tempProfile;                             // ������ ���� �� �ӽ÷� ������ ����
    private Image[] sProfileColor = new Image[3];               // ������ ������ ������ ������ �̹��� ��ũ��Ʈ ����
    private Color[] cProfileColor = new Color[3];               // �� ������ ������ ���� ����

    public GameObject tDialogText;

    [Header("Profile BackGround")]
    public Sprite sProfileBG;
    public Sprite sDialogBG;
    public Sprite sUpsetBG;
    public Sprite sUpsetDialogBG;

    public Image dialogBGImage;

    [Header("Profile Image")]
    public Sprite[] sCuredProfile = new Sprite[20]; // ġ���� ������
	public Sprite[] sBasicProfile = new Sprite[20]; // �⺻ ������
	public Sprite[] sUpsetProfile = new Sprite[20]; // ȭ�� ������

	[Header("Upset Stamp")]
	public GameObject[] iCloudStamp = new GameObject[3];        // �������� ������ ǥ���� ������
	public GameObject iDialogStamp;                             // dialog�� ǥ���� ������
	private GameObject tempStamp;



	private int[] disSatGuestList;      // �Ҹ� ��Ƽ�� ��ȣ���� ������ �迭

    private GuestInfos[] mGuestInfo;    // GuestManager�� ���� ���� Guest Info

    private int nextProfileIndex;       // next ��ư Ŭ���� �߰��� �մ��� ��ȣ
    private int prevProfileIndex;       // prev ��ư Ŭ���� �߰��� �մ��� ��ȣ

    private bool isUpset;               // '�Ҹ� ��Ƽ�� ����'�� Ȱ��ȭ �Ǿ����� Ȯ���ϴ� �� ����

	Guest mGuestManager;
    RecordUIManager mUIManager;

	void Awake()
    {
        mGuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();
        mUIManager = GameObject.Find("UIManager").GetComponent<RecordUIManager>();

        mGuestInfo = mGuestManager.mGuestInfo;

        for(int num = 0; num < 3; num++)
        {
            sProfileColor[num] = iProfileBG[num].GetComponent<Image>();
			cProfileColor[num] = sProfileColor[num].color;

		}
		InitProfile();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // �ʱ� ������ ����
    public void InitProfile()
    {
		isUpset = false;

		for (int num = 0; num < 3; num++) {
            ChangeProfileInfo(num, num); 
            nextProfileIndex = 3; 
            prevProfileIndex = 19; 
        }

        // ���� �տ��ִ� �������� ��Ƽ�� �Ҹ� ��Ƽ�� ���, dialog ���� ����, �ƴҽ� ������� ������
        UpdateDialogPaper();

		// GuestManager���� �Ҹ� ��Ƽ�� ���� ����� �޾� ������
		disSatGuestList = new int[mGuestManager.DisSatGuestList().Length];
		disSatGuestList = mGuestManager.DisSatGuestList();
	}

    // �Ҹ� ��Ƽ�� ���� ��ư�� ������ �� ����
    public void InitUpsetProfile()
    {
        isUpset = true;

        // ��ư�� ������ �Ҹ� ��Ƽ ����Ʈ�� �ֽ�ȭ
        disSatGuestList = new int[mGuestManager.DisSatGuestList().Length];
        disSatGuestList = mGuestManager.DisSatGuestList();

        // �Ҹ� ��Ƽ�� ���� ��, ������ ����� ��������
        if (disSatGuestList.Length <= 0) { for (int num = 0; num < 3; num++) { ChangeProfileEmpty(num); } }
        else {
            // Profile Index�� �ʱⰪ�� ã�ư��� ����, nextProfileIndex�� ���� ���� �� �ִ� �Ҹ� ��Ƽ�� ���� �پ��ϱ� ������ �������� ����
            prevProfileIndex = disSatGuestList.Length - 1;
            nextProfileIndex = prevProfileIndex;
            for (int num = 0; num < 4; num++)
            {
                nextProfileIndex++;
                CheckIndexInRange(disSatGuestList.Length);
                if (num < 3)
                {
                    ChangeProfileInfo(num, disSatGuestList[nextProfileIndex]);
                }
            }
        }
    }

    public void UpdateNextProfile()
    {
        if (isUpset) { Invoke("InvokeUpdateUpsetNextProfile", 0.18f); }
        else { Invoke("InvokeUpdateAllNextProfile", 0.7f); }
    }

    public void UpdatePrevProfile()
    {
        if (isUpset) { Invoke("InvokeUpdateUpsetPrevProfile", 0.18f); }
        else { Invoke("InvokeUpdateAllPrevProfile", 0.18f); }
    }

    // next buttonŬ���� ������ ������Ʈ
    private void InvokeUpdateAllNextProfile()
    {
        //next, prev �մ��� ��ȣ�� ������ ����� �ʵ��� ����
		CheckIndexInRange(mGuestInfo.Length);

        //�� �� ������ ����: iProfileBG[0]���� ������ �ǵ��� ��ȯ
        SwapProfile(0, 2);

        //�� �ڷΰ��� ������ ���� ����
		ChangeProfileInfo(2, nextProfileIndex);

        UpdateDialogPaper();

		nextProfileIndex++;
		prevProfileIndex++;
	}

    // prev buttonŬ���� ������ ������Ʈ
    private void InvokeUpdateAllPrevProfile()
    {
        CheckIndexInRange(mGuestInfo.Length);

        SwapProfile(2, 0);

		ChangeProfileInfo(0, prevProfileIndex);

        UpdateDialogPaper();

		nextProfileIndex--;
		prevProfileIndex--;
	}

    // �Ҹ� ��Ƽ�� ǥ���� ��� disSatGuestList�� �����ϱ� ������ ��ü ��Ƽ ǥ�ÿ� ���� ����ϱ� ���ؼ� �μ��� �޾ƾ� �� >> invoke ��� �Ұ������� �Լ� �и�
    private void InvokeUpdateUpsetNextProfile()
    {
        CheckIndexInRange(disSatGuestList.Length);

        SwapProfile(0, 2);

        // �Ҹ� ��Ƽ�� ǥ���� ��� �� �������� �߻��� �� �����Ƿ� üũ ����
		if (disSatGuestList.Length <= 0) { ChangeProfileEmpty(2); }
        else { ChangeProfileInfo(2, disSatGuestList[nextProfileIndex]); }

		nextProfileIndex++;
		prevProfileIndex++;
	}

	private void InvokeUpdateUpsetPrevProfile()
	{
		CheckIndexInRange(disSatGuestList.Length);

        SwapProfile(2, 0);

		if (disSatGuestList.Length <= 0) { ChangeProfileEmpty(0); }
		else { ChangeProfileInfo(0, disSatGuestList[prevProfileIndex]); }

		nextProfileIndex--;
		prevProfileIndex--;
	}

    // �������� �߰��Ǿ�� �� �������� index�� �迭 ���� �ִ� index���� üũ ��, ������ �Ѿ ��� ��������
	private void CheckIndexInRange(int max_list_index)
    {
		if (nextProfileIndex >= max_list_index) { nextProfileIndex = 0; }
		else if (nextProfileIndex < 0) { nextProfileIndex = max_list_index - 1; }
		if (prevProfileIndex < 0) { prevProfileIndex = max_list_index - 1; }
		else if (prevProfileIndex >= max_list_index) { prevProfileIndex = 0; }
	}

    private void ChangeProfileInfo(int profile_num, int profile_index)
    {
        // ������ ���� ����
		if (iProfileBG[profile_num].transform.Find("I_Portrait").gameObject.activeSelf == false)
		        { iProfileBG[profile_num].transform.Find("I_Portrait").gameObject.SetActive(true); }

        // ��Ƽ�� ����(ġ��, �Ҹ�, �⺻)�� ���� �̹��� ����
        if (mGuestInfo[profile_index].isDisSat) { iProfileBG[profile_num].transform.Find("I_Portrait").gameObject.GetComponent<Image>().sprite = sUpsetProfile[profile_index]; }
        else if (mGuestInfo[profile_index].isCure) { iProfileBG[profile_num].transform.Find("I_Portrait").gameObject.GetComponent<Image>().sprite = sCuredProfile[profile_index]; }
        else { iProfileBG[profile_num].transform.Find("I_Portrait").gameObject.GetComponent<Image>().sprite = sBasicProfile[profile_index]; }

		iProfileBG[profile_num].transform.Find("T_Name(INPUT)").gameObject.GetComponent<Text>().text = mGuestInfo[profile_index].mName;
		iProfileBG[profile_num].transform.Find("T_Age(INPUT)").gameObject.GetComponent<Text>().text = mGuestInfo[profile_index].mAge.ToString();
		iProfileBG[profile_num].transform.Find("T_Job(INPUT)").gameObject.GetComponent<Text>().text = mGuestInfo[profile_index].mJob;

        // ��Ƽ�� ���¿� ���� ������ ���� ����, ���� ����
        // �Ҹ� ��Ƽ�� ���
        if (mGuestInfo[profile_index].isDisSat) {
			iProfileBG[profile_num].transform.Find("I_Portrait").gameObject.GetComponent<Image>().sprite = sUpsetProfile[profile_index];
			iProfileBG[profile_num].GetComponent<Image>().sprite = sUpsetBG; 
            iProfileBG[profile_num].GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f); 
        }
        // �������� ��Ƽ�� ���
        else { 
            iProfileBG[profile_num].GetComponent<Image>().sprite = sProfileBG; 
            for(int num = 0; num < 3; num++)
            {
                if (iProfileBG[profile_num].GetComponent<Image>() == sProfileColor[num])
                {
                    iProfileBG[profile_num].GetComponent<Image>().color = cProfileColor[num];
                    break;
                }
            }
        }

        // �ش� ��Ƽ�� �Ҹ� ��Ƽ�� ��� ������ ���
		if (mGuestInfo[profile_index].isDisSat && !iCloudStamp[profile_num].activeSelf) { iCloudStamp[profile_num].SetActive(true); }
        else if (!mGuestInfo[profile_index].isDisSat && iCloudStamp[profile_num].activeSelf) { iCloudStamp[profile_num].SetActive(false); }
	}

	// ��Ƽ ����Ʈ�� ����ִ� ���(�Ҹ� ��Ƽ�� �� �� ���� ���)�� ���: �Ҹ� ��Ƽ�� ǥ�� ����
	private void ChangeProfileEmpty(int profile_num)
    {
        if (iProfileBG[profile_num].transform.Find("I_Portrait").gameObject.activeSelf == true) 
                { iProfileBG[profile_num].transform.Find("I_Portrait").gameObject.SetActive(false); }
		iProfileBG[profile_num].transform.Find("I_Portrait").gameObject.GetComponent<Image>().sprite = null;
		iProfileBG[profile_num].transform.Find("T_Name(INPUT)").gameObject.GetComponent<Text>().text = " ";
		iProfileBG[profile_num].transform.Find("T_Age(INPUT)").gameObject.GetComponent<Text>().text = " ";
		iProfileBG[profile_num].transform.Find("T_Job(INPUT)").gameObject.GetComponent<Text>().text = " ";
	}

    // dialog text�� �Ҹ� ��Ƽ���� �ƴ����� ���� ��� ���� ����
    private void UpdateDialogPaper()
    {
		if (iCloudStamp[0].activeSelf)
		{
			iDialogStamp.SetActive(true);
			tDialogText.SetActive(false);
			dialogBGImage.sprite = sUpsetDialogBG;
		}
		else
		{
			iDialogStamp.SetActive(false);
			tDialogText.SetActive(true);
			dialogBGImage.sprite = sDialogBG;
		}
	}

    // Swap iProfileBG, iCloudStamp
    private void SwapProfile(int start_num, int end_num)
    {
        GameObject temp_object;
        if(start_num < end_num)
        {
            temp_object = iProfileBG[start_num];
            for(int num = start_num; num < end_num; num++) { iProfileBG[num] = iProfileBG[num + 1]; }
            iProfileBG[end_num] = temp_object;

			temp_object = iCloudStamp[start_num];
			for (int num = start_num; num < end_num; num++) { iCloudStamp[num] = iCloudStamp[num + 1]; }
			iCloudStamp[end_num] = temp_object;
		}
        else
        {
			temp_object = iProfileBG[start_num];
			for (int num = start_num; num > end_num; num--) { iProfileBG[num] = iProfileBG[num - 1]; }
			iProfileBG[end_num] = temp_object;

			temp_object = iCloudStamp[start_num];
			for (int num = start_num; num > end_num; num--) { iCloudStamp[num] = iCloudStamp[num - 1]; }
			iCloudStamp[end_num] = temp_object;
		}
	}
}
