using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using UnityEngine.Windows;
public class CloudContainer : MonoBehaviour
{
    //Interface UI
    public List<GameObject> mUiInvenStocks;
    public GameObject[] mTxtInfoPrefab;
    public Sprite mDefaultSprite;

    public StoragedCloudData mSelecedCloud;

    public VirtualObjectManager Virtualobjectmanager;
    [SerializeField]
    InventoryManager inventoryManager;

    [SerializeField]
    private List<StoragedCloudData> mUiStocksData; //UI�� �������� StocksData

    /////////////////////
    //�κ��丮 ���� UI//
    ////////////////////
    [SerializeField]
    private Dropdown mDropDown;

    private int mSortedCnt; //�������ĵȰ���
    private List<StoragedCloudData> mSortedData; //UI�� �������� StocksData


    // Start is called before the first frame update
    void Start()
    {
        mDropDown = GameObject.Find("UIManager").GetComponent<Dropdown>();
        mDropDown.mDropdown.onValueChanged.AddListener(delegate
        {
            OnDropdownEvent();
        });
        inventoryManager = GameObject.FindWithTag("InventoryManager").GetComponent<InventoryManager>();

        inventoryManager.mInventoryContainer = this.gameObject;
        initInven(inventoryManager.mCloudStorageData.mDatas, "public");
    }

    /////////////////////
    //Button Interact///
    ////////////////////
    public void clicked() //matarial in inventory selected
    {
        if (inventoryManager == null)
            inventoryManager = GameObject.FindWithTag("InventoryManager").GetComponent<InventoryManager>();

        //�ش� ���� ���� UI ǥ��
        Transform selected = EventSystem.current.currentSelectedGameObject.transform;
        mSelecedCloud = mUiStocksData[selected.GetSiblingIndex()];
        Debug.Log("���� ����:" + mUiStocksData[selected.GetSiblingIndex()]);
    }

    public void unclicked() //matarial in cloudmaker deselected
    {
        GameObject target = EventSystem.current.currentSelectedGameObject;
        Sprite sprite = target.GetComponent<Image>().sprite;

        if (sprite.name == "Circle") return; //����ó��

        //�ش� ���� ���� UI ǥ�� ���
    }

    public void cancelDropdownEvent()
    {
        mDropDown.mDropdown.interactable = false;
        mSortedCnt = mUiStocksData.Count;
        //clearInven(mUiStocksData);
        initInven(mUiStocksData, "private");
       // updateInven(mUiStocksData);
    }

    //DropDown public Method
    public void OnDropdownEvent()
    {
        Debug.Log("[DropdownEvent] {" + mDropDown.mDropdown.value + "} clicked.");
        mDropDown.mDropdownIndex = mDropDown.mDropdown.value;
        mSortedData = new List<StoragedCloudData>(); //init
        mSortedData = sortStock(mDropDown.mDropdownIndex);
        mSortedCnt = mSortedData.Count;
        //clearInven(mSortedData);
       // initInven(mSortedData, "private");
        //updateInven(mSortedData);
    }

  
    public void initInven(List<StoragedCloudData> invenData, string order)
    {
        if (order == "public")
        {
            mUiStocksData = new List<StoragedCloudData>();
            mUiStocksData = invenData; //UI��Ͽ� ����!

            int tmp = 0;
            foreach (StoragedCloudData stock in mUiStocksData)
            {
                GameObject invenUI = transform.GetChild(tmp).gameObject;
                mUiInvenStocks.Add(invenUI); //UIInvenGameObject List �߰�.
                tmp++;
            }
        }


        setInven(invenData);
    }

    private void setInven(List<StoragedCloudData> _mData)
    {

        //invenData�� invenContainer(UI)List�� �־��ش�.
        int tmp = 0;
        foreach (StoragedCloudData stock in _mData)
        {
            GameObject invenUI = mUiInvenStocks[tmp];

            if (invenUI.transform.childCount == 0)
            {
                GameObject cntTxt = Instantiate(mTxtInfoPrefab[0]);
                cntTxt.transform.SetParent(invenUI.transform, false);
                cntTxt.transform.GetComponent<TMP_Text>().text = stock.mdate.ToString();
            }

            //��ư ������Ʈ�� ������ ������ش�.
            if (invenUI.transform.GetComponent<Button>() == null)
            {
                Button btn = invenUI.AddComponent<Button>();
                btn.onClick.AddListener(clicked);
            }

            //���� ������ ���� ���� ������Ʈ�� Convert �Ͽ� Instantiate �ϴ� ����.
            GameObject obejct;
            obejct = Virtualobjectmanager.convertVirtualToGameObject(stock.mVBase);

            obejct.transform.SetParent(invenUI.transform);
            RectTransform rectTran = obejct.GetComponent<RectTransform>();
            rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, stock.mVBase.mHeight);
            rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, stock.mVBase.mWidth);
            foreach (VirtualGameObject Vpart in stock.mVPartsList)
            {
                GameObject obejctP;
                obejctP = Virtualobjectmanager.convertVirtualToGameObject(Vpart);

                obejctP.transform.SetParent(obejct.transform);

                obejctP.transform.localPosition = obejctP.transform.position;
                rectTran = obejctP.GetComponent<RectTransform>();
                rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Vpart.mHeight);
                rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Vpart.mWidth);
            }

            obejct.transform.localPosition = Vector3.zero;
            obejct.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            //Name Upadate
            invenUI.name = stock.mdate.ToString();

            tmp++;
        }
    }
    private void clearInven(List<StoragedCloudData> _mData)
    {
        //���� �ִ� ���� ������Ʈ�� ���� ������ ���� ���Ѵ�.
        int difference = _mData.Count - mUiInvenStocks.Count;
        //���� �ִ°� �� ���� ��� : ������ �ʱ�ȭ
        if (difference < 0) //difference�� 2�̸� 
        {
            for (int i = _mData.Count; i < mUiInvenStocks.Count; i++)
            {
                mUiInvenStocks[i].name = "000";
                mUiInvenStocks[i].GetComponent<Image>().sprite = mDefaultSprite;

                if (mUiInvenStocks[i].transform.GetComponent<Button>())
                {
                    Destroy(mUiInvenStocks[i].GetComponent<Button>());
                }

                if (mUiInvenStocks[i].transform.childCount != 0)
                {
                    Destroy(mUiInvenStocks[i].transform.GetChild(1).gameObject);
                    Destroy(mUiInvenStocks[i].transform.GetChild(0).gameObject);
                }

            }
        }
        else
        {
            //���� �ִ°� �� ���� ��� : ���׸�ŭ ������Ʈ ����
            for (int i = mUiInvenStocks.Count; i < _mData.Count; i++)
            {
                GameObject invenUI = mUiInvenStocks[i];

                if (invenUI.transform.childCount == 0)
                {
                    GameObject cntTxt = Instantiate(mTxtInfoPrefab[0]);
                    cntTxt.transform.SetParent(invenUI.transform, false);
                    cntTxt.transform.GetComponent<TMP_Text>().text = "0";

                    GameObject nameTxt = Instantiate(mTxtInfoPrefab[1]);
                    nameTxt.transform.SetParent(invenUI.transform, false);
                    nameTxt.transform.GetComponent<TMP_Text>().text = "000";
                }

                //��ư ������Ʈ�� ������ ������ش�.
                if (invenUI.transform.GetComponent<Button>() == null)
                {
                    Button btn = invenUI.AddComponent<Button>();
                    btn.onClick.AddListener(clicked);
                }

                //Image Update
                invenUI.transform.GetComponent<Image>().sprite = mDefaultSprite;

                //Name Upadate
                invenUI.name = "000";
            }
        }
    }

    //�κ��丮 ����� ������ �Ŀ��� ������ �������Ͽ��� UI�� �����ִ� ��ϵ� Update���־�� �Ѵ�.

    /////////////////////
    //�κ��丮 ���� �Լ�//
    ////////////////////


    //����Ʈ ��ü�� �����Ѵ�. ���� �� ���� �� ����Ʈ�� ���� UI�� �ݿ��Ѵ�.
    public List<StoragedCloudData> sortStock(int _emotion) //�������� �з�
    {
         List<StoragedCloudData> results = new List<StoragedCloudData>();

        //�������� �з�: �Էµ��� ������ �����ִ� �͸� �̾Ƽ� tmpList�� �߰��Ѵ�.
        foreach (StoragedCloudData stock in mUiStocksData)
        {
            //�ش� ������ stock�� iEmotion����Ʈ�� �����ϴ��� Ȯ��
           // if (!stock.Key.mFinalEmotions.ContainsKey(_emotion)) continue;
            //���������� ����

            //������������ ����
           // var queryAsc = stock.Key.iEmotion.OrderByDescending(x => x.Value);// int, int

            //ù��° ���� ���� ũ�Ƿ� ���� ū ���� �Ű����ڿ� ���� �����̶�� �߰����ش�.
          //  if (_emotion != queryAsc.First().Key) continue;
         //   results.Add(stock.Key, stock.Value);
        }


        return results;
    }

    private Dictionary<IngredientData, int> sortStock(Dictionary<IngredientData, int> _target) // �������� �з�:
    {
        Dictionary<IngredientData, int> results = new Dictionary<IngredientData, int>();

        var queryAsc = _target.OrderBy(x => x.Value);

        foreach (var dictionary in queryAsc)
            results.Add(dictionary.Key, dictionary.Value);

        return results;
    }
}
