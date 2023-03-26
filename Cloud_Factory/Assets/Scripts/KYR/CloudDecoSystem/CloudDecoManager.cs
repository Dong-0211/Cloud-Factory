using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;
public class CloudDecoManager : MonoBehaviour
{
    //���� ������ Ŭ����
    [System.Serializable]
    public class PartsMenu
    {
        private GameObject I_Image;
        private GameObject B_Pos;
        private GameObject B_Neg;

        public bool state; //���� Ȥ�� ����?
        public bool isInit;
        public PartsMenu(GameObject _B_decoParts, GameObject _I_PartsMenu, GameObject _B_PosNeg, int _idx)
        {
            I_Image = _I_PartsMenu.transform.GetChild(_idx).GetChild(0).gameObject;
            B_Pos = _B_PosNeg.transform.GetChild(_idx).GetChild(0).gameObject;
            B_Neg = _B_PosNeg.transform.GetChild(_idx).GetChild(1).gameObject;
            isInit = true;

            I_Image.transform.GetComponent<Image>().sprite = _B_decoParts.transform.GetChild(_idx).GetChild(0).GetChild(0).GetComponent<Image>().sprite;
        }

        public void btnClicked(Sprite[] _I_SelectedSticker, Sprite[] _I_UnSelectedSticker)
        {
            if (!state)
            {
                B_Pos.GetComponent<Image>().sprite = _I_SelectedSticker[0];
                B_Neg.GetComponent<Image>().sprite = _I_UnSelectedSticker[1];
                state = true;
            }
            else
            {
                B_Pos.GetComponent<Image>().sprite = _I_UnSelectedSticker[0];
                B_Neg.GetComponent<Image>().sprite = _I_SelectedSticker[1];
                state = false;
            }
        }

        public bool getPartsNPState()
        {
            return state;
        }

    }
    public GameObject CaptureZone;
    //Sprite Merger

    public List<PartsMenu> mLPartsMenu;
    //���ڿ� ��ư �׷�
    public GameObject B_decoParts;
    public GameObject B_PosNeg;
    public GameObject[] B_Edits; //frame : �̵�, Rotate: ȸ��. �ܺο��� ����.

    public GameObject P_FinSBook;
    //Text
    public GameObject[] T_CountInfo; //��������

    //Image
    public GameObject I_targetCloud; //����ġ�� ���� �ø��� ���� �̹��� ���ӿ�����Ʈ
    private GameObject I_BasicDecoCloud;
    public GameObject I_PartsMenu; //������ ���� �޴�

    public Sprite[] I_SelectedSticker; //���õ��� �˷��ִ� ������ �̹���
    public Sprite[] I_UnSelectedSticker; //default img

    public CloudData mBaseCloudDt;// �������忡�� ���� ������ ����.

    private bool cursorChasing;
    private bool isDecoDone;


    private DecoParts selectedParts;
    private List<List<GameObject>> mUIDecoParts;
    private int mCloudPieceDecoMax;
    private InventoryManager inventoryManager;
    private List<GameObject> LDecoParts;
    //����ġ�� �����
    public Vector2 top_right_corner;
    public Vector2 bottom_left_corner;



    private void Start()
    {
        mUIDecoParts = new List<List<GameObject>>();
        LDecoParts = new List<GameObject>();
        //(�� �̵��ÿ��� ����)
        inventoryManager = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();


        initParam();
        init();
    }

    private void initParam()
    {
        cursorChasing = false;
        isDecoDone = false;
        //UI ����
        for (int i = 0; i < 3; i++)
        {
            Transform tmp = B_decoParts.transform.GetChild(i);
            List<GameObject> Ltmp = new List<GameObject>();
            for (int j = 0; j < 3; j++)
                Ltmp.Add(tmp.GetChild(j).gameObject);


            mUIDecoParts.Add(Ltmp);
        }
        mLPartsMenu = new List<PartsMenu>();

    }
    private void init()
    {
        //Cloud data ��������.(���̵��ÿ�������)
        mBaseCloudDt = getTargetCloudData();


        //Ŭ���� �����Ϳ� ���� UI�� �̹��� ����.
        //Set Base Cloud Image on Sketchbook.
        I_targetCloud.transform.GetComponent<Image>().sprite = mBaseCloudDt.getForDecoCloudSprite();

        //Set Deco Parts as Dt cnt: mUIDecoParts[1],mUIDecoParts[2]
        for (int i = 0; i < mBaseCloudDt.getDecoPartsCount(); i++)
        {
            List<GameObject> tmpList = mUIDecoParts[i];
            for (int j = 0; j < 3; j++)
            {
                if (i == 0)        //Set CloudPiece        
                    mUIDecoParts[i][j].transform.GetChild(0).GetComponent<Image>().sprite = mBaseCloudDt.getCloudParts()[j];


                mUIDecoParts[i + 1][j].transform.GetChild(0).GetComponent<Image>().sprite = mBaseCloudDt.getSizeDifferParts(i)[j];
            }

        }

        for (int i = 0; i < mBaseCloudDt.getDecoPartsCount() + 1; i++)
        {
            mLPartsMenu.Add(new PartsMenu(B_decoParts, I_PartsMenu, B_PosNeg, i));

        }

        //���� ���� ����.
        mCloudPieceDecoMax = 10; //���������� 10���� ����.
        List<int> cntList = mBaseCloudDt.getMaxDecoPartsCount();
        for (int i = 0; i < mBaseCloudDt.getDecoPartsCount(); i++)
        {
            T_CountInfo[i].GetComponent<TMP_Text>().text = cntList[i].ToString();
        }
    }

    private CloudData getTargetCloudData()
    {
        return inventoryManager.createdCloudData;
    }

    //UI Button Functions
    public void cloudDecoDoneBtn() //������ ����ġ�� ��� �� OK ��ư
    {
		TutorialManager mTutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
		if (mTutorialManager.isFinishedTutorial[6] == false) { mTutorialManager.isFinishedTutorial[6] = true; }
		List<int> mEmoValues = new List<int>();
        //�������.
        for (int i = 0; i < mLPartsMenu.Count; i++)
        {
            if (mLPartsMenu[i].getPartsNPState())
                mEmoValues.Add(1);
            else
                mEmoValues.Add(-1);
        }
        mBaseCloudDt.addFinalEmotion(mEmoValues);

        inventoryManager.addStock(I_targetCloud);
        //LoadScene
        SceneManager.LoadScene("Cloud Storage");
    }

    public void cloudDecoBackBtn() // ����ġ�� ��� | Reset ��ư
    {
        TutorialManager mTutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
        if (mTutorialManager.isFinishedTutorial[6] == false) { return; }
        Destroy(P_FinSBook.transform.GetChild(0).GetChild(0).gameObject); //����
        I_targetCloud = I_BasicDecoCloud;
        initParam();
        init();

        for (int i = 0; i < B_PosNeg.transform.childCount; i++)
        {
            B_PosNeg.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = I_UnSelectedSticker[0];
            B_PosNeg.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = I_UnSelectedSticker[1];
        }

        P_FinSBook.SetActive(false);
    }

    public void clickedAutoSettingBtn() //�ڵ� ��ġ
    {
        float width_max_range = I_targetCloud.GetComponent<RectTransform>().rect.width/2;                                  // ������ ���� ������ ����� �ʵ��� ���� ����
        float Height_max_range = I_targetCloud.GetComponent<RectTransform>().rect.height/2;                                
        Vector2 I_targetCloudPosition = new Vector2(I_targetCloud.transform.position.x, I_targetCloud.transform.position.y);    // ���� �̹����� �߽� ���͸� �޾ƿ´�.

        GameObject[] cloudParts = new GameObject[mBaseCloudDt.getDecoPartsCount()];      // ���� �������� ��ġ�� ũ�� �񱳸� ���� ���� ������ ����
        int partsIdx = 0;

        // Vector2 top_right_corner = I_targetCloud.Rect
        for (int i = 0; i < mBaseCloudDt.getDecoPartsCount(); i++)
        {
			for (int j = 0; j < int.Parse(T_CountInfo[i].GetComponent<TMP_Text>().text); j++)
            {
                GameObject partsObj = B_decoParts.transform.GetChild(i + 1).GetChild(Random.Range(0,3)).GetChild(0).gameObject; //Image GameObject
                float x = Random.Range(-width_max_range, width_max_range);
				float y = Random.Range(-Height_max_range, Height_max_range);

                // ������ �ٿ��� ���� ������ ������ Ƣ����� �κ��� ����. �� �κп� ������ �����Ǹ�, ��ġ�� �ٽ� �����ϵ���
                while (!IsDecoPartInCloud(x, y))
                {
					x = Random.Range(-width_max_range, width_max_range);
					y = Random.Range(-Height_max_range, Height_max_range);
				}

                cloudParts[partsIdx] = Instantiate(partsObj, new Vector2(0, 0), transform.rotation);
				cloudParts[partsIdx].transform.SetParent(transform);
				cloudParts[partsIdx].transform.position = new Vector2(x, y) + I_targetCloudPosition;        // �������� ������ ��ǥ�� ������(���� �̹����� �߽� ����) �������� ��ġ�� �������ش�.

                //  ���� ������ �ٸ� ������ ������ �˻� �ʿ� transform.localScale�� 
                if (partsIdx > 0) {   // �� ��° ������ ���� �� ������ ���ƴ��� Ȯ��
                    for(int num = 0; num < partsIdx;)
                    {
                        if(IsObjectOverlapped(cloudParts[num], cloudParts[partsIdx]))   // ������ ��������, ���ο� ���� ��ġ �缱��
                        {
							x = Random.Range(-width_max_range, width_max_range);
							y = Random.Range(-Height_max_range, Height_max_range);
							cloudParts[partsIdx].transform.position = new Vector2(x, y) + I_targetCloudPosition;
						}
                        else if(!IsDecoPartInCloud(x, y))
                        {
							x = Random.Range(-width_max_range, width_max_range);
							y = Random.Range(-Height_max_range, Height_max_range);
							cloudParts[partsIdx].transform.position = new Vector2(x, y) + I_targetCloudPosition;
						}
                        else { num++; }
                    }
                }
            }

            T_CountInfo[i].GetComponent<TMP_Text>().text = "0";
        }
    }

    private bool IsObjectOverlapped(GameObject existing_object, GameObject new_object)
    {
        float existing_width_range = existing_object.GetComponent<RectTransform>().rect.width / 2;                                  // ���� ������Ʈ�� ���� ���� / 2
        float existing_height_range = existing_object.GetComponent<RectTransform>().rect.height / 2;                                // ���� ������Ʈ�� ���� ���� / 2
        Vector2 existing_position = new Vector2(existing_object.transform.position.x, existing_object.transform.position.y);        // ���� ������Ʈ�� ��ġ

        float new_width_range = new_object.GetComponent<RectTransform>().rect.width / 2;                                            // ���� ������Ʈ�� ���� ���� / 2
        float new_height_range = new_object.GetComponent<RectTransform>().rect.height / 2;                                         // ���� ������Ʈ�� ���� ���� / 2
        Vector2 new_position = new Vector2(new_object.transform.position.x, new_object.transform.position.y);                       // ���� ������Ʈ�� ��ġ

        // ���� ������Ʈ�� ���� ������Ʈ���� ������ ��ġ�� ���
        if ((new_position.x >= existing_position.x)
            && (new_position.x - existing_position.x >= new_width_range + existing_width_range))                                    // �� ������Ʈ�� x�� ���̰� ũ�Ƿ� ��ĥ �� ����
        { return false; }
        else if((new_position.x >= existing_position.x)
			&& (new_position.x - existing_position.x < new_width_range + existing_width_range))                                     // �� ������Ʈ�� x�� ���̰� ����� ũ�� �����Ƿ� y�� �� ����
        {
            if((new_position.y >= existing_position.y)                                                                              // ���� ������Ʈ�� ���� ������Ʈ���� ����� ��,
                && (new_position.y - existing_position.y >= new_height_range + existing_height_range))                              // y���� ���̰� ũ�Ƿ� ��ġ�� �ʴ´�
            { return false; }
            else if((new_position.y < existing_position.y)
                && (existing_position.y - new_position.y >= new_height_range + existing_height_range))
            { return false; }
            else { return true; }                                                                                                   // x��� y�� �� �� ���̰� ����� ũ�� �����Ƿ� ��ģ�ٰ� �Ǵ�
        }

        //���� ������Ʈ�� ���� ������Ʈ���� ������ ��ġ�� ���
        if ((new_position.x < existing_position.x)
            && (existing_position.x - new_position.x >= new_width_range + existing_width_range))
        { return false; }
        else if ((new_position.x < existing_position.x)
            && (existing_position.x - new_position.x >= new_width_range + existing_width_range))
        {
            if ((new_position.y >= existing_position.y)
                && (new_position.y - existing_position.y >= new_height_range + existing_height_range))
            { return false; }
            else if ((new_position.y < existing_position.y)
                && (existing_position.y - new_position.y >= new_height_range + existing_height_range))
            { return false; }
            else { return true; }
        }
        return true;
	}

    private bool IsDecoPartInCloud(float width_range, float height_range)
    {
		//�����������
		//�����������
		//�����������
		//�����������
		//�����������
		//�����������
        if((width_range <= 100 && height_range >= 200)
            || (width_range <= -200 && height_range >= 100)
            || (width_range <= -300 && height_range >= 0)
            || (width_range <= -300 && height_range <= -100)
            || (width_range <= -100 && height_range <= -200)
            || (width_range >= 200 && height_range >= 200)
            || (width_range >= 300 && height_range >= 0)
            || (width_range >= 400 && height_range <= -100)
            || (width_range >= 200 && height_range <= -200))
        { Debug.Log("Out of Range"); return false; }
		return true;
	}

	public void clickedPosNegButton()
    {
		TutorialManager mTutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();

		GameObject target = EventSystem.current.currentSelectedGameObject.transform.gameObject;
        int idx = target.transform.parent.GetSiblingIndex();// �θ� ���° sibling����.

        if (idx > mBaseCloudDt.getDecoPartsCount()) return;

        Debug.Log("clicked_" + target.name);
        if(mLPartsMenu[idx].isInit) //ó���̸� �Ѵ� üũ�� �ȵǾ�����.
        {
            
            if (mTutorialManager.isFinishedTutorial[6] == false && mLPartsMenu[0].isInit == true)
            {
                if(idx != 0 || target.transform.GetSiblingIndex() != 0) { return; }
                else { 
                    mTutorialManager.SetActiveGuideSpeechBubble(true);
                    mTutorialManager.SetActiveFadeOutScreen(false);
                    mTutorialManager.SetActiveArrowUIObject(false);
                }
            }

            target.transform.GetComponent<Image>().sprite = target.transform.GetSiblingIndex() == 0 ? I_SelectedSticker[0] : I_SelectedSticker[1];
            mLPartsMenu[idx].state = target.transform.GetSiblingIndex() == 0 ? true : false;

            mLPartsMenu[idx].isInit = false;
        }
        else
        {
            if (mTutorialManager.isFinishedTutorial[6] == false && idx == 0) { return; }
            mLPartsMenu[idx].btnClicked(I_SelectedSticker,I_UnSelectedSticker);
        }

    }
    public void EClickedDecoParts()
    {
        GameObject target = EventSystem.current.currentSelectedGameObject;
        int partsIdx = target.transform.parent.GetSiblingIndex(); //parent = type(?)
        int cnt = partsIdx >= 1 ? int.Parse(T_CountInfo[partsIdx - 1].GetComponent<TMP_Text>().text) : 0 ;

            
        //���� ���ҽ�Ű��, ���Ѱ����� �����ϸ� Ŭ�� �� �� ����.
        switch (partsIdx)
        {
            case 0:
                if (mCloudPieceDecoMax == 0) return;
                mCloudPieceDecoMax--;
                break;
            case 1:
                if (cnt == 0) return;
                cnt--;
                T_CountInfo[partsIdx - 1].GetComponent<TMP_Text>().text = cnt.ToString();
                break;
            case 2:
                if (cnt == 0) return;
                cnt--;
                T_CountInfo[partsIdx - 1].GetComponent<TMP_Text>().text = cnt.ToString();
                break;
        }

        GameObject newParts = Instantiate(target.transform.GetChild(0).gameObject, Input.mousePosition, target.transform.rotation);
        newParts.AddComponent<DecoParts>();
        selectedParts = newParts.GetComponent<DecoParts>();
        selectedParts.init(top_right_corner, bottom_left_corner);
        newParts.AddComponent<Button>();
        selectedParts.transform.SetParent(this.transform, true);

        newParts.GetComponent<Button>().onClick.AddListener(EPartsClickedInArea);

        LDecoParts.Add(newParts);//���� �����ϴ� ����Ʈ�� �߰�.

        cursorChasing = true;
    }

    public void EPartsClickedInArea()
    {
        //�ٹ̱Ⱑ �Ϸ�� ���¶�� ����� ���ۿ� �������� �ʴ´�.
        if (isDecoDone)
            return;
        //Ŭ���� ��ü�� �����������
        if(LDecoParts.Count>1 && EventSystem.current.currentSelectedGameObject.transform.parent != selectedParts.transform.parent)
                return;
        
        selectedParts = EventSystem.current.currentSelectedGameObject.GetComponent<DecoParts>();

        if (!selectedParts.canAttached) return; 
        if(!selectedParts.isEditActive)//ó�� ������ ������ ����. ���������� ������ ���� ����.
        {
            cursorChasing = false; //Ŀ�� ����ٴ��� �ʰ� ����.
            selectedParts.ReSettingDecoParts(); //CanEdit = true�� ����.

            //���ο� ��ư ���� ���� ����.
            GameObject B_Frame = Instantiate(B_Edits[0], Vector2.zero, selectedParts.transform.rotation);
            B_Frame.transform.SetParent(selectedParts.transform,false);
            B_Frame.AddComponent<MouseDragMove>();
            B_Frame.GetComponent<Button>().onClick.AddListener(EEditPartsPos);
            B_Frame.GetComponent<RectTransform>().sizeDelta = selectedParts.GetComponent<RectTransform>().sizeDelta;

            GameObject B_Rotate = Instantiate(B_Edits[1], Vector2.zero, selectedParts.transform.rotation);
            B_Rotate.transform.SetParent(selectedParts.transform, false);
            B_Rotate.AddComponent<MouseDragRotate>();


            //Rotation Button Frame ����.
            float rotateImg_H = B_Rotate.GetComponent<RectTransform>().sizeDelta.y/2.0f;
            float PartsImg_H = selectedParts.gameObject.GetComponent<RectTransform>().sizeDelta.y/2.0f;
            float correctionPos = PartsImg_H - rotateImg_H + rotateImg_H * 2;
            B_Rotate.transform.position = new Vector2(B_Rotate.transform.position.x, B_Rotate.transform.position.y+correctionPos);

            B_Frame.SetActive(false);
            B_Rotate.SetActive(false);

            selectedParts.isEditActive = true;
            return;
        }

        //������ �ѹ� �Ǹ� canEdit���´� ������ true�̴�.
        if (!selectedParts.isEditActive) return;
        //����ġ�Ͽ� ������ ���¿����� �Ʒ��ڵ� ���� ����.

        if (!selectedParts.canEdit)
        {
            selectedParts.canEdit = true;
            selectedParts.transform.GetChild(0).gameObject.SetActive(true);
            selectedParts.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    private void EEditPartsPos()
    {
        if (!selectedParts.canEdit)
        {
            selectedParts.canEdit = true;
            selectedParts.transform.GetChild(0).gameObject.SetActive(true);
            selectedParts.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            selectedParts.canEdit = false;
            selectedParts.transform.GetChild(0).gameObject.SetActive(false);
            selectedParts.transform.GetChild(1).gameObject.SetActive(false);
        }

    }


    private void Update_PartsMoving()
    {
        if (!cursorChasing) return;
        selectedParts.transform.position = Input.mousePosition;
    }

    private bool checkIsWorkComplete()
    {
        //���� ������ �Ϸ��ߴ��� üũ
        for(int i = 0; i < mLPartsMenu.Count; i++)
        {
            if (mLPartsMenu[i].isInit == true) return false;
            if (int.Parse(T_CountInfo[i].GetComponent<TMP_Text>().text) != 0) return false;
        }
        if (cursorChasing == true) return false;

        return true;
    }

    private void createFinCloud() //�������� ���� ���� ���ӿ�����Ʈ ����.
    {
        if (isDecoDone) return;

        StartCoroutine(popUpFinSBook());
    }

    IEnumerator popUpFinSBook()
    {
        isDecoDone = true;
        GameObject FinCloud = Instantiate(I_targetCloud, I_targetCloud.transform.position, I_targetCloud.transform.rotation);    

        yield return new WaitForSeconds(1.0f);

		//����ġ�Ͽ� �ٿ��� �������� <CloudDecoManager>�Ʒ��� ����Ǵµ�, �̸� �����Ҷ��� �������̽� ������ �ٲ۴�.
		while (transform.childCount != 0)
		{
			transform.GetChild(0).SetParent(FinCloud.transform);
		}
		GameObject Capture = Instantiate(FinCloud, FinCloud.transform.position, FinCloud.transform.rotation);
		Capture.transform.SetParent(CaptureZone.transform);

		FinCloud.transform.SetParent(P_FinSBook.transform.GetChild(0).transform);
        FinCloud.transform.localPosition = new Vector3(0, 0, 0);

        I_BasicDecoCloud = I_targetCloud;
        I_targetCloud = FinCloud;


        FinCloud.GetComponent<Transform>().localScale = new Vector3(0.8f, 0.8f, 0.8f);
        P_FinSBook.SetActive(true);

        TutorialManager mTutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
        if (mTutorialManager.isFinishedTutorial[6] == false) { mTutorialManager.InstantiateArrowUIObject(GameObject.Find("B_Complete").transform.position, 150f); }

        yield break;

    }
   
    
    private void FixedUpdate()
    {        
        Update_PartsMoving();

        if (checkIsWorkComplete())
            createFinCloud();
    }

    
    //Gizmo//
    private void DrawRectange(Vector2 top_right_corner, Vector2 bottom_left_corner)
    {
        Vector2 center_offset = (top_right_corner + bottom_left_corner) * 0.5f;
        Vector2 displacement_vector = top_right_corner - bottom_left_corner;
        float x_projection = Vector2.Dot(displacement_vector, Vector2.right);
        float y_projection = Vector2.Dot(displacement_vector, Vector2.up);

        Vector2 top_left_corner = new Vector2(-x_projection * 0.5f, y_projection * 0.5f) + center_offset;
        Vector2 bottom_right_corner = new Vector2(x_projection * 0.5f, -y_projection * 0.5f) + center_offset;

        Gizmos.DrawLine(top_right_corner, top_left_corner);
        Gizmos.DrawLine(top_left_corner, bottom_left_corner);
        Gizmos.DrawLine(bottom_left_corner, bottom_right_corner);
        Gizmos.DrawLine(bottom_right_corner, top_right_corner);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        DrawRectange(top_right_corner, bottom_left_corner);

    }


}
