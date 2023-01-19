using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

// ���� �κ��丮 UI
public class StorageUIManager : MonoBehaviour
{
    public Sprite       mDropBoxDown; // ȭ��ǥ �Ʒ�
    public Sprite       mDropBoxUp;   // ȭ��ǥ ��

    public Image        mArrow;       // ��� �ڽ� ȭ��ǥ

    public GameObject   mTemplate;    // ��� �ڽ� ����
    public GameObject[] mGiveCloudCheckBox = new GameObject[3]; // ���� ����ȭ�� üũ �ڽ� 2��

    public TMP_Dropdown mSortDropBox; // ��ӹڽ�

    public CloudMakeSystem cloudMakeSystem;
    private Dropdown    mDropdown;    // ��Ӵٿ� Ŭ����

    private InventoryContainer inventoryContainer; //yeram

    void Start()
    {
        mDropdown = GetComponent<Dropdown>();
        inventoryContainer = GameObject.Find("I_Scroll View Inventory").transform.Find("Viewport").transform.Find("Content").GetChild(0).GetComponent<InventoryContainer>();

    }
    void Update()
    {
        // ȭ��ǥ �ø�
        if (mTemplate.activeSelf) mArrow.sprite = mDropBoxUp;
        else if (!mTemplate.activeSelf) mArrow.sprite = mDropBoxDown;
    }

    // �������� ����
    public void SortEmotion()
    {
        if (!mGiveCloudCheckBox[(int)ECheckBox.Emotion].activeSelf)
        {
            mSortDropBox.interactable = true;
            mGiveCloudCheckBox[(int)ECheckBox.Emotion].SetActive(true);

            Debug.Log("���� �ε��� : " + mDropdown.mDropdownIndex);
            Debug.Log("�ε��� �޾ƿͼ� ���� ����Ǿ� �ִ� �ε����� ������ ���� �޼ҵ� ȣ��");
            inventoryContainer.OnDropdownEvent();
        }
        else
        {
            mSortDropBox.interactable = false;
            mGiveCloudCheckBox[(int)ECheckBox.Emotion].SetActive(false);
            inventoryContainer.cancelDropdownEvent();
        }
    }

    // ���ư��� ��ư
    public void GoToCloudFactory()
    {
        LoadingSceneController.Instance.LoadScene("Cloud Factory");
    }
    public void MakeCloud()
    {
		Debug.Log("���� ���� �޼ҵ� ȣ��");

		bool isMakingCloud = GameObject.Find("I_CloudeGen").GetComponent<CloudMakeSystem>().isMakingCloud;
		bool isMtrlListEmpty = GameObject.Find("I_CloudeGen").GetComponent<CloudMakeSystem>().d_selectMtrlListEmpty();

		// �̹� ������ ���� ���̰ų�, ����ĭ�� ��ᰡ ���� �� ��ư�� ������ �ƹ� �ϵ� �Ͼ�� �ʵ��� �Ѵ�.
        // ������ �̹� ������� ���´� ���ʿ� ���̻� ��Ḧ ����ĭ�� ���� �� ���� ������ ���� return ó�� ���� ����
		if (isMakingCloud || isMtrlListEmpty) { Debug.Log("������ �Ұ����մϴ�."); return; }     
		cloudMakeSystem.E_createCloud(EventSystem.current.currentSelectedGameObject.name);
    }
}
