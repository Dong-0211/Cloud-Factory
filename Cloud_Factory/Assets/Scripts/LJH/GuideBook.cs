using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideBook : MonoBehaviour
{
    public GameObject gRareGroup; // ��� ��͵� �з� UI
    public GameObject gPlantGroup_main;
    // ���UI
    public GameObject[] gPlantGroup_rare1 = new GameObject[4];
    public GameObject[] gPlantGroup_rare2 = new GameObject[4];
    public GameObject[] gPlantGroup_rare3 = new GameObject[4];
    public GameObject[] gPlantGroup_rare4 = new GameObject[4];

    public GameObject gNextBtn; // ���� ������ ��ư
    public GameObject gPrevBtn;

    public Image iNextBtn; // ���� ������ ��ư �̹���
    public Image iPrevBtn;

    public Image[] iRareChange = new Image[3]; // ��͵��� ���� �ٲ�� UI (���, ������(����, ����) �ѱ�)
    public Sprite[] sRareBG = new Sprite[5]; // ��� [0] �⺻
    public Sprite[] sRarePage = new Sprite[5]; // ������ �ѱ� [0] �⺻

    public Text tGuideText; // ���� ���̵� �ؽ�Ʈ

    private int gPageIndex; // ������ �ε���
    private bool[] gChapter = new bool[3]; // ������ ��з� , 0 : ���� ~
    private bool[] gPlantChapter = new bool[4]; // ��� ������ �з� , 0 : ��͵� 1 ~

    private int Emotion_page_num;
    private int Cloud_page_num;
    public GameObject[] Emotion_Page = new GameObject[4];
    public GameObject[] Cloud_Page = new GameObject[4];
    private static int Plant_num = 3;

    private bool[] Plant_get = new bool[12];
    public GameObject[] Plant_Info = new GameObject[12];
    public GameObject[] QM_Info = new GameObject[12];

    
    private void Awake()
    {
        gPageIndex = 1; // �⺻ 1 ���������� ����
        gChapter[0] = true; // �⺻ ���� ����
        gPlantChapter[0] = true; // �⺻ ��͵� 1
        Emotion_page_num = 0;
        Cloud_page_num = 0;

        for(int i = 0; i < Plant_num; i++)
        {
            Plant_get[i] = false;
        }
    }

    void Update()
    {
        if (gChapter[0])
        {
            tGuideText.text = "���� ����" + gPageIndex.ToString() + "������";
            UpdateGuideUI(0);
            Update_Emotion_Page(true, gPageIndex - 1);
            Update_Cloud_Page(false, gPageIndex - 1);
        }            
        else if(gChapter[1])
        {
            tGuideText.text = "���� ����" + gPageIndex.ToString() + "������";
            UpdateGuideUI(0);
            Update_Emotion_Page(false, gPageIndex - 1);
            Update_Cloud_Page(true, gPageIndex - 1);
        }
            
        else if (gChapter[2])
        {
            if (gPlantChapter[0])
            {
                tGuideText.text = "��͵�1 ��� ����" + gPageIndex.ToString() + "������";
                UpdateGuideUI(1);
                UpdatePlant_UI();
                Update_Emotion_Page(false, gPageIndex - 1);
                Update_Cloud_Page(false, gPageIndex - 1);
            }
                
            else if (gPlantChapter[1])
            {
                tGuideText.text = "��͵�2 ��� ����" + gPageIndex.ToString() + "������";
                UpdateGuideUI(2);
                UpdatePlant_UI();
                Update_Emotion_Page(false, gPageIndex - 1);
                Update_Cloud_Page(false, gPageIndex - 1);
            }               
            else if (gPlantChapter[2])
            {
                tGuideText.text = "��͵�3 ��� ����" + gPageIndex.ToString() + "������";
                UpdateGuideUI(3);
                UpdatePlant_UI();
                Update_Emotion_Page(false, gPageIndex - 1);
                Update_Cloud_Page(false, gPageIndex - 1);
            }             
            else if (gPlantChapter[3])
            {
                tGuideText.text = "��͵�4 ��� ����" + gPageIndex.ToString() + "������";
                UpdateGuideUI(4);
                UpdatePlant_UI();
                Update_Emotion_Page(false, gPageIndex - 1);
                Update_Cloud_Page(false, gPageIndex - 1);
            }             
        }

        if (gPageIndex >= 4)
        {
            gNextBtn.SetActive(false);
            iNextBtn.color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 0 / 255f);
        }            
        else
            gNextBtn.SetActive(true);

        if (gPageIndex <= 1)
        {
            gPrevBtn.SetActive(false);
            iPrevBtn.color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 0 / 255f);
        }            
        else
            gPrevBtn.SetActive(true);


        if (Input.GetKey(KeyCode.A))
        {
            for(int i = 0; i < 3; i++)
            {
                Plant_get[i] = true;
            }
        }
        for(int i = 0; i < 3; i++)
        {
            if (Plant_get[i] == true)
            {
                Plant_Info[i].SetActive(true);
                QM_Info[i].SetActive(false);
            }
        }
    }

    void UpdateGuideUI(int _iIndex)
    {
        iRareChange[0].sprite = sRareBG[_iIndex];
        iRareChange[1].sprite = sRarePage[_iIndex];
        iRareChange[2].sprite = sRarePage[_iIndex];
    }

    #region ��ư�̹���ȣ����
    public void ActiveNextBtnImage()
    {
        if (gPageIndex < 4)
            iNextBtn.color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
    }
    public void UnActiveNextBtnImage()
    {
        iNextBtn.color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 0 / 255f);
    }
    public void ActivePrevBtnImage()
    {
        if (gPageIndex > 1)
            iPrevBtn.color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
    }
    public void UnActivePrevBtnImage()
    {
        iPrevBtn.color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 0 / 255f);
    }
    #endregion

    public void ClickNextBtn()
    {
        if (gPageIndex < 4)
            ++gPageIndex;
    }
    public void ClickPrevBtn()
    {
        if (gPageIndex > 1)
            --gPageIndex;
    }

    #region ���� ���� ��� ��ư
    public void ActiveFeel()
    {
        ChangeChapter(false, false, 1, true, false, false);
    }
    public void ActiveCloud()
    {
        ChangeChapter(false, false, 1, false, true, false);
    }
    public void ActivePlant()
    {
        ChangeChapter(true, true, 1, false, false, true);
        ClickRare1();
    }
    #endregion

    void ChangeChapter(bool _bRare, bool _bGroupPlant ,int _iPageIndex, bool _bFeel, bool _bCloud, bool _bPlant)
    {
        gRareGroup.SetActive(_bRare);
        gPlantGroup_main.SetActive(_bGroupPlant); // ��� UI
        gPageIndex = 1; // ������ �ʱ�ȭ
        gChapter[0] = _bFeel;
        gChapter[1] = _bCloud;
        gChapter[2] = _bPlant;
    }

    #region ��͵�
    public void ClickRare1()
    {
        UpdateRareUI(0);
    }
    public void ClickRare2()
    {
        UpdateRareUI(1);
    }
    public void ClickRare3()
    {
        UpdateRareUI(2);
    }
    public void ClickRare4()
    {
        UpdateRareUI(3);
    }
    #endregion

    void UpdateRareUI(int _iIndex)
    {
        for (int i = 0; i < 4; i++)
        {
            gPlantChapter[i] = false;
        }
        gPlantChapter[_iIndex] = true;
        gPageIndex = 1;
    }

    void UpdatePlant_UI()
    {
        for (int i = 0; i < 4; i++)
        {
            gPlantGroup_rare1[i].SetActive(false);
            gPlantGroup_rare2[i].SetActive(false);
            gPlantGroup_rare3[i].SetActive(false);
            gPlantGroup_rare4[i].SetActive(false);
        }
        if (gPlantChapter[0])
        {
            for(int i = 0; i < 4; i++)
            {
                gPlantGroup_rare1[i].SetActive(false);
            }
            gPlantGroup_rare1[gPageIndex - 1].SetActive(true);
        }
        else if (gPlantChapter[1])
        {
            for (int i = 0; i < 4; i++)
            {
                gPlantGroup_rare2[i].SetActive(false);
            }
            gPlantGroup_rare2[gPageIndex - 1].SetActive(true);
        }
        else if (gPlantChapter[2])
        {
            for (int i = 0; i < 4;  i++)
            {
                gPlantGroup_rare3[i].SetActive(false);
            }
            gPlantGroup_rare3[gPageIndex - 1].SetActive(true);
        }
        else if (gPlantChapter[3])
        {
            for (int i = 0; i < 4; i++)
            {
                gPlantGroup_rare4[i].SetActive(false);
            }
            gPlantGroup_rare4[gPageIndex - 1].SetActive(true);
        }
    }

    void Update_Cloud_Page(bool page_on, int Page_num)
    {
        //Cloud_page_num = Page_num;
        //for (int i = 0; i < 4; i++)
        //{
        //    Page_num = 1;
        //    Cloud_Page[i].SetActive(false);
        //}
        //if (page_on)
        //{
        //    Cloud_Page[Cloud_page_num].SetActive(true);
        //}
    }

    void Update_Emotion_Page(bool page_on, int Page_num)
    {
        //Emotion_page_num = Page_num;
        //for (int i = 0; i < 4; i++)
        //{
        //    Page_num = 1;
        //    Emotion_Page[i].SetActive(false);
        //}
        //if (page_on)
        //{
        //    Emotion_Page[Emotion_page_num].SetActive(true);
        //}
    }
}
