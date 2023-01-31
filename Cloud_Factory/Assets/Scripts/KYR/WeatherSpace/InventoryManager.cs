using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json; // LJH, Json Namespace

// LJH, Data ������ �ӽ� ���� ����, Monobehaviour ��� ����
[System.Serializable]
public class InventoryData
{
    public List<IngredientData> mType;
    public List<int> mCnt;   

    public int minvenLevel = 1;

    public int mMaxStockCnt = 10; //�켱�� 10�����ϱ��� ����
    public int mMaxInvenCnt; //�켱�� 10�����ϱ��� ����
}

[System.Serializable]
public class CloudStorageData
{
    public List<StoragedCloudData> mDatas;

    public CloudStorageData()
    {
        mDatas = new List<StoragedCloudData>();
    }
}




[System.Serializable]
public class StoragedCloudData
{
    public List<EmotionInfo> mFinalEmotions; //���� �ٹ̱� ������ ���� ����.
    public List<GameObject> mPartsList; //���� �ٹ̱� ������ ���� ����.
    public int mdate;
    public GameObject mBase;
    public VirtualGameObject mVBase;
    public List<VirtualGameObject> mVPartsList; //���� �ٹ̱� ������ ���� ����.

    public StoragedCloudData(List<EmotionInfo> _FinalEmotions, GameObject _base, List<GameObject> _mPartsList)
    {
        mdate = 10; //�ϴ� �⺻���� ����
        mFinalEmotions = _FinalEmotions;
        mBase = _base;
        mPartsList = _mPartsList;


        //VirtualSetting
        mVPartsList = new List<VirtualGameObject>();
        mVBase = new VirtualGameObject(mBase.transform.position, mBase.transform.rotation, mBase.transform.GetComponent<RectTransform>().rect.width, mBase.transform.GetComponent<RectTransform>().rect.height, mBase.GetComponent<Image>().sprite);
        for (int i = 0; i < mPartsList.Count; i++)
        {
            mVPartsList.Add(new VirtualGameObject(_mPartsList[i].transform.localPosition, _mPartsList[i].transform.rotation, _mPartsList[i].transform.GetComponent<RectTransform>().rect.width, _mPartsList[i].transform.GetComponent<RectTransform>().rect.height, _mPartsList[i].GetComponent<Image>().sprite));
        }
    }


}

[System.Serializable]
public class CloudData
{
    public int mShelfLife;
    public List<EmotionInfo> mEmotions;

    private bool[] isCloudAnimProgressed;
    private bool mState; //0 = ��� 1 = ����
    private Sprite mcloudBaseImage;
    private Sprite mcloudDecoBaseImage;

    private List<Sprite> mcloudParts; //������ ���� �ʼ�!
    private List<List<Sprite>> mdecoImages; //2���� ����Ʈ: L M S ������ �ʿ�! �ִ� 2��

    private List<EmotionInfo> mFinalEmotions; //���� �ٹ̱� ������ ���� ����.
    public CloudData(List<EmotionInfo> Emotions)
    {
        mEmotions = Emotions;
        mFinalEmotions = new List<EmotionInfo>();
        isCloudAnimProgressed = new bool[3];
        setAnimProgressed();
        //�����Լ��� �ڵ����� ������ ����
        setShelfLife(mEmotions);
        setCloudImage(mEmotions);
        setDecoImage(mEmotions);
    }

    public CloudData()
    {
        mEmotions = new List<EmotionInfo>();
		isCloudAnimProgressed = new bool[3];
		setAnimProgressed();
	}

    public int getDecoPartsCount()
    {
        return mdecoImages.Count;
    }

    public List<EmotionInfo> getFinalEmotion()
    {
        return mFinalEmotions;
    }
 
    public Sprite getBaseCloudSprite()
    {
        return mcloudBaseImage;
    }

    public Sprite getForDecoCloudSprite()
    {
        return mcloudDecoBaseImage;
    }
    public List<Sprite> getSizeDifferParts(int _idx)
    {
        return mdecoImages[_idx];
    }

    public List<Sprite> getCloudParts()
    {
        return mcloudParts;
    }

    public int getBaseCloudColorIdx()
    {
        return (int)mEmotions[0].Key;
    }

    public bool getAnimProgressed(int anim_num)
    {
        if(anim_num > 2) { return false; }
        return isCloudAnimProgressed[anim_num];
    }

    public List<int> getMaxDecoPartsCount()
    {
        List<int> Lresult = new List<int>();

        for (int i = 1; i < mEmotions.Count; i++)
        {
            int value = mEmotions[i].Value;

            int iReuslt = (value % 10 >= 0 && value % 10 <= 4) ? value - (value % 10) : (value + 10) - (value % 10);
            Lresult.Add(iReuslt / 10);
        }

        return Lresult;
    }

    public void addFinalEmotion(List<int> _value)
    {
        for(int i = 0; i < mEmotions.Count; i++)
        {
            mFinalEmotions.Add(new EmotionInfo(mEmotions[i].Key, mEmotions[i].Value * _value[i]));
        }
       
    }
    private void setAnimProgressed()
    {
        for(int idx = 0; idx < 3; idx++)
        {
            isCloudAnimProgressed[idx] = false;
        }
	}

    public void setAnimProgressed(int anim_num, bool anim_bool)
    {
        isCloudAnimProgressed[anim_num] = anim_bool;
    }

    //Private method
    private void setShelfLife(List<EmotionInfo> Emotions)
    {
        //������ ���� �´� �����Ⱓ
    }
    private void setCloudImage(List<EmotionInfo> Emotions)
    {
        //������ ���� �´� base �����̹���
        string targetImgName = ((int)mEmotions[0].Key).ToString();
        if ((int)mEmotions[0].Key < 8)
            targetImgName = "0";
        mcloudBaseImage = Resources.Load<Sprite>("Sprite/CloudBase/2union/" + "OC_Cloud2_" + ((int)mEmotions[0].Key).ToString());
        mcloudDecoBaseImage = Resources.Load<Sprite>("Sprite/CloudBase/DecoSpaceVer/" + "OC_Cloud_" + ((int)mEmotions[0].Key).ToString());
    }
    private void setDecoImage(List<EmotionInfo> Emotions)
    {
        //���� ���� ����
        mcloudParts = new List<Sprite>();
        mcloudParts.Add(Resources.Load<Sprite>("Sprite/CloudDeco/CloudParts/OC_" + ((int)mEmotions[0].Key).ToString() + "_piece_" + "1"));
        mcloudParts.Add(Resources.Load<Sprite>("Sprite/CloudDeco/CloudParts/OC_" + ((int)mEmotions[0].Key).ToString() + "_piece_" + "2"));
        mcloudParts.Add(Resources.Load<Sprite>("Sprite/CloudDeco/CloudParts/OC_" + ((int)mEmotions[0].Key).ToString() + "_piece_" + "3"));

        //Assets/Resources/Sprite/CloudDeco/CloudParts/OC_0_piece_3.png

        //���� ����
        mdecoImages = new List<List<Sprite>>();
        //������ ���� �´� ���� �̹���
        for (int i = 1; i < Emotions.Count;i++)
        {
            List<Sprite> decoList = new List<Sprite>();
            decoList.Add(Resources.Load<Sprite>("Sprite/CloudDeco/L/" + "OC_L_" + ((int)mEmotions[i].Key).ToString()));
            decoList.Add(Resources.Load<Sprite>("Sprite/CloudDeco/M/" + "OC_M_" + ((int)mEmotions[i].Key).ToString()));
            decoList.Add(Resources.Load<Sprite>("Sprite/CloudDeco/S/" + "OC_S_" + ((int)mEmotions[i].Key).ToString()));
            mdecoImages.Add(decoList);
        }
    }
}

[System.Serializable]
//���� �� ��� �κ��丮 ���� �Ŵ���
public class InventoryManager : MonoBehaviour
{
    public IngredientList[] mIngredientDatas; // ��� ��� ������ ���� �ִ� ����Ʈ scriptable data

    private bool mIsSceneChange = false;

    [SerializeField]
    public GameObject mInventoryContainer;

    //���� ���� ����
    private CloudData beginningCloudData;    // ������ ���ڰ� ������ ���ư��� �� �ʱⰪ ����
    public CloudData createdCloudData = null;

    

    public void setDataList(List<IngredientList> Ltotal)
    {
        mIngredientDatas = new IngredientList[4];
        int idx = 0;
        foreach (IngredientList rarity in Ltotal)
        {
            mIngredientDatas[idx] = rarity;
            idx++;
        }
    }

    //Debug�� ���� �ӽ� Button �Լ�. ���߿� ������ ����
    public void go2CloudFacBtn()
    {
        mIsSceneChange = true;
    }

    /////////////////
    /////Singlton////
    /////////////////
    static InventoryManager _instance = null; //�̱��� ��ü
    public static InventoryManager Instance() //static �Լ�, �����ϰ��� �ϴ� �ܺο��� ����� ��.
    {
        return _instance; //�ڱ��ڽ� ����
    }

    void Start()
    {
        if (_instance == null) // ���� ���۵Ǹ� �ڱ��ڽ��� �ִ´�.
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else  //�ٸ� ������ �Ѿ�ٰ� back ���� �� ���ο� ���� ������Ʈ�� �����ϱ� ���� ���ǹ�.
        {
            if(this != _instance)
            {
                Destroy(this.gameObject);
            }
        }

        mType = new List<IngredientData>(); //����Ʈ �ʱ�ȭ
        mCnt = new List<int>(); //����Ʈ �ʱ�ȭ

        mCloudStorageData = new CloudStorageData();
        beginningCloudData = new CloudData();
    }

    /////////////////
    //���� ���� ����//
    /////////////////
    public CloudStorageData mCloudStorageData; //���� �κ��丮 ������ ����Ʈ Ŭ����.

    public List<IngredientData>  mType;
    public List<int>  mCnt;

    public int minvenLevel=3;

    public int mMaxStockCnt = 10; //�켱�� 10�����ϱ��� ����
    public int mMaxInvenCnt; //�켱�� 10�����ϱ��� ����

    //////////////////////////////
    //ä�� ���� �κ��丮 ���� �Լ�//
    //////////////////////////////
    public bool addStock(KeyValuePair<IngredientData, int> _stock)
    {        
        mMaxInvenCnt = getInvenSize(minvenLevel);
        if (mType.Count >= mMaxInvenCnt) return false; // �κ��丮 ��ü�� �ƿ� ���� �� ��� return false
        //�κ��丮�� �ڸ��� �ִ� ���
        if (!mType.Contains(_stock.Key)) //�κ��� ���� ���� ������ ���� �׳� �ְ� return true
        {
            mType.Add(_stock.Key);
            mCnt.Add(_stock.Value);
            return true;
        }

        //�κ��丮 �ȿ� ������ ��� ������ �̹� �ִ� ���
        int idx = mType.IndexOf(_stock.Key); //index �� ����.

        if (mCnt[idx] >= mMaxStockCnt) return false;//�κ��丮 ��� �� ���尡�� ���� ����
        int interver = mMaxStockCnt - (_stock.Value+ mCnt[idx]); //���尡�� ���� - (���ο�� �߰����� �� �κ��丮�� ����ɰ���) = �������� ���
        if (interver >= 0) mCnt[idx] = mMaxStockCnt; //���̰� 0���� ũ�� ������ Max Cnt
        else
            mCnt[idx] += _stock.Value; //�ش� ������ ī��Ʈ �߰�.

        return true;
    }

    private int getInvenSize(int invenLv)
    {
        int invensize = 0;
        switch (invenLv)
        {
            case 1:
                invensize = 8;
                break;
            case 2:
                invensize = 12;
                break;
            case 3:
                invensize = 24;
                break;
            default:
                break;
        }

        return invensize;
    }

    //�������ִ� 2���� ����Ʈ�� ��ųʸ� �������� ����: ��Ȱ�� ������ ���ؼ�!
    public Dictionary<IngredientData, int> mergeList2Dic()
    {
        Dictionary<IngredientData, int> results = new Dictionary<IngredientData, int>();
        foreach (IngredientData stock in mType)
            results.Add(stock, mCnt[mType.IndexOf(stock)]);

        return results;
    }


    //////////////////////////////
    //�����κ��丮 ���� �Լ�//
    //////////////////////////////
    ///
    //���� �Ǿ��� ���� ������Ʈ ����
    public void addStock(GameObject _cloudObject)
    {
        List<GameObject> parts = new List<GameObject>();
        GameObject cloudBase = Instantiate(_cloudObject.gameObject, _cloudObject.transform.position, _cloudObject.transform.rotation);

        DontDestroyOnLoad(cloudBase);
        for (int i = 0; i < cloudBase.transform.childCount; i++)
        {
            DontDestroyOnLoad(cloudBase.transform.GetChild(i).gameObject);
            parts.Add(cloudBase.transform.GetChild(i).gameObject);
        }
        mCloudStorageData.mDatas.Add(new StoragedCloudData(createdCloudData.getFinalEmotion(), cloudBase, parts));
        createdCloudData = beginningCloudData;
    }
}
