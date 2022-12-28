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

        UsingGuestNumList = SOWManager.mUsingGuestList.ToArray();
        numOfUsingGuestList = SOWManager.mUsingGuestList.Count;

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
        frontProfileInfo++;
        UsingGuestIndex++;

        if (frontProfileInfo >= 3)
        {
            frontProfileInfo = 0;
        }
        if (frontGuestIndex >= numOfUsingGuestList)
        {
            UsingGuestIndex = 0;
        }
        frontGuestIndex = UsingGuestNumList[UsingGuestIndex];

        updateProfileList();
    }

    public void GetPrevProfile()
    {
        // ���� �������� �ҷ��´�.
        frontProfileInfo--;
        UsingGuestIndex--;

        if (frontProfileInfo < 0)
        {
            frontProfileInfo = 2;
        }
        if (frontGuestIndex < 0)
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

        int guestNum = frontGuestIndex;

        // ����Ʈ���� ������ �մ� �����ϱ�
        SOWManager sow = GameObject.Find("SOWManager").GetComponent<SOWManager>();
        int count = sow.mUsingGuestList.Count;
        
        for(int i = 0; i< count; i++)
        {
            if (sow.mUsingGuestList[i] == guestNum)
                sow.mUsingGuestList.RemoveAt(i);
        }
       

        SOWManager.SetCloudData(guestNum, storagedCloudData);

        SceneManager.LoadScene("Space Of Weather");

        Debug.Log("�������� �޼ҵ� ȣ��");
    }
}