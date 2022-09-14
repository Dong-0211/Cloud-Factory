using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class CloudDecoManager : MonoBehaviour
{

    //���ڿ� ��ư �׷�
    public GameObject B_decoParts;
    public GameObject B_PosNeg;
    public GameObject[] B_Edits; //frame : �̵�, Rotate: ȸ��. �ܺο��� ����.

    public CloudData mtargetCloud;// �������忡�� ���� ������ ����.

    private bool cursorChasing;
    
    private DecoParts selectedParts;
    private List<List<GameObject>> mUIDecoParts;
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
        //inventoryManager = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();


        initParam();
        init();
    }

    private void initParam()
    {
        cursorChasing = false;
        //UI ����
        for (int i = 0; i < 3; i++)
        {
            Transform tmp = B_decoParts.transform.GetChild(i);
            List<GameObject> Ltmp = new List<GameObject>();
            for (int j = 0; j < 3; j++)
                Ltmp.Add(tmp.GetChild(j).gameObject);


            mUIDecoParts.Add(Ltmp);
        }

    }
    private void init()
    {
        //Cloud data ��������.(���̵��ÿ�������)
       // mtargetCloud = inventoryManager.createdCloudData;

       
        //Ŭ���� �����Ϳ� ���� UI�� �̹��� ����.

    }

    private CloudData getTargetCloudData()
    {
        return GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>().createdCloudData;
    }


    //UI Button Function
    public void EClickedDecoParts()
    {
        GameObject target = EventSystem.current.currentSelectedGameObject;
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

    private void FixedUpdate()
    {
        
        Update_PartsMoving();
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
