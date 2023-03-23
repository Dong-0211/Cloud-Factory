using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CloudStorageProfile : MonoBehaviour
{

    public GameObject[] Profiles;
    SOWManager SOWManager;
    Guest GuestManager;
    RecordUIManager UIManager;

    [SerializeField]
    Image[] iPortrait = new Image[20];

    public Button btNextBtn;           // ���������� ��ư
    public Button btPrevBtn;           // ���������� ��ư
    public Button btGiveBtn;           // ���� ���� ��ư

    // ���� �տ� �ִ� ������ ������Ʈ�� �ε��� ����
    [SerializeField]
    int frontProfileInfo;
    public GameObject mGetCloudContainer;

    // ȭ��� ������ �ִ� �մ��� �մԹ�ȣ
    int frontGuestIndex;

    // ������ �������� �� �ִ� �մԵ��� �մԹ�ȣ ����Ʈ
    [SerializeField]
    int[]   UsingGuestNumList;
    int     UsingGuestIndex;

    // ������ �������� �� �ִ� �մԵ��� ����Ʈ�� ����
    int numOfUsingGuestList;

    void Awake()
    {
        SOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();
        GuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();
        UIManager = GameObject.Find("UIManager").GetComponent<RecordUIManager>();

        // �������� �մ� ��, ������ �������� ���� �մԸ� ���� ���� ����Ʈ�� �ִ´�.
        List<int> temp = new List<int>();
        foreach(int i in SOWManager.mUsingGuestList)
        {
            if (GuestManager.mGuestInfo[i].isUsing == false)
            { 
                temp.Add(i);
                Debug.Log(i + "�� �մ��� ���� ������ �����մϴ�.");
            }
            else
            {
                Debug.Log(i + "�� �մ��� ���� ������ �������� �ʽ��ϴ�.");
            }
        }
        UsingGuestNumList = temp.ToArray();

        numOfUsingGuestList = UsingGuestNumList.Length;

        frontProfileInfo = 0;
        UsingGuestIndex = 0;

        if (numOfUsingGuestList != 0)
            frontGuestIndex = UsingGuestNumList[UsingGuestIndex];

        Profiles = new GameObject[3];

        Profiles[0] = GameObject.Find("I_ProfileBG1");
        Profiles[1] = GameObject.Find("I_ProfileBG2");
        Profiles[2] = GameObject.Find("I_ProfileBG3");

        btGiveBtn = GameObject.Find("B_CloudGIve").GetComponent<Button>();

        initProfileList();
    }

    public void GetNextProfile()
    {
        // ���� �������� �ҷ��´�.
        frontProfileInfo = (frontProfileInfo + 1 ) % 3;
        UsingGuestIndex++;

        // �Ǿտ� ���� �մ��� �ε����� 
        if (UsingGuestIndex >= numOfUsingGuestList)
        {
            UsingGuestIndex = 0;
        }
        frontGuestIndex = UsingGuestNumList[UsingGuestIndex];

        updateProfileList();
    }

    public void GetPrevProfile()
    {
        // ���� �������� �ҷ��´�.
        frontProfileInfo = (frontProfileInfo - 1 + 3) % 3;
        UsingGuestIndex--;
        
        if (UsingGuestIndex < 0)
        {
            UsingGuestIndex = numOfUsingGuestList - 1;
        }

        frontGuestIndex = UsingGuestNumList[UsingGuestIndex];
        updateProfileList();
    }

    void initProfileList()
    {
        updateProfileList();
    }

    void updateProfileList()
    {

        GameObject Profile = Profiles[frontProfileInfo];
        // Button
        btGiveBtn = Profile.transform.GetChild(1).GetComponent<Button>();

        // �մ��� ���� ��쿡�� ���� ������Ʈ�� ���� �ʴ´�.
        if (numOfUsingGuestList == 0)
        {
            btGiveBtn.interactable = false;
            Debug.Log("���������� ������ �մ��� �������� �ʽ��ϴ�.");
            return;   
        }

        // Image
        Image iProfile = Profile.transform.GetChild(0).GetComponent<Image>();

        // ���� �̸� ����
        Text tName = Profile.transform.GetChild(7).GetComponent<Text>();
        Text tAge = Profile.transform.GetChild(8).GetComponent<Text>();
        Text tJob = Profile.transform.GetChild(9).GetComponent<Text>();

        // ������, �� �� ���
        Text tSat = Profile.transform.GetChild(10).GetComponent<Text>();
        Text tSentence = Profile.transform.GetChild(11).GetComponent<Text>();

        // ��Ƽ ������ �����´�.
        GuestInfos info = GuestManager.GetComponent<Guest>().mGuestInfo[frontGuestIndex];

        // ������ �������� �̿��Ͽ� ä���.
        iProfile.sprite = UIManager.sBasicProfile[frontGuestIndex];
        tName.text = info.mName;
        tAge.text = "" + info.mAge;
        tJob.text = info.mJob;


        tSat.text = "" + info.mSatatisfaction;
		tSentence.text = info.mSentence;

		// Sentence ���� ������Ʈ�� �ʿ��� -> ���� ����


	}

    void updateButton()
    {

    }

    public void GiveCloud()
    {
      
        SOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();

        // ���� �����ϴ� �޼ҵ� ȣ��
        StoragedCloudData storagedCloudData
            = mGetCloudContainer.GetComponent<CloudContainer>().mSelecedCloud;

        //���� ���� ����ó��
        if (storagedCloudData == null)
        {
            return;
        }

        int guestNum = frontGuestIndex;
        GuestManager.mGuestInfo[guestNum].isUsing = true;

        //�����κ��丮���� ���� ���� ����
        mGetCloudContainer.GetComponent<CloudContainer>().deleteSelectedCloud();

        // ����Ʈ���� ������ �մ� �����ϱ�
        SOWManager sow = GameObject.Find("SOWManager").GetComponent<SOWManager>();
        int count = sow.mUsingGuestList.Count;

        SOWManager.SetCloudData(guestNum, storagedCloudData);

        SceneManager.LoadScene("Space Of Weather");

        Debug.Log("�������� �޼ҵ� ȣ��");
    }
}