using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace CloudSystem
{
    public delegate void EventHandler(string name);//����̸� Ȥ�� Ű�� ���ڷ� �޾� �ѱ�.

    //selected ingredients list
    class S_list
    {
        private List<GameObject> UI_slct_mtrl;
        private int UI_mtrl_count;

        public Sprite default_sprite;//private���� �ٲ� ����.
        public Sprite cloud_sprite;//Data structure�� �ٲ� ����.

        public void init(Transform T_mtrl)
        {
            UI_slct_mtrl = new List<GameObject>();

            UI_mtrl_count = T_mtrl.childCount;
            for (int i = 0; i < UI_mtrl_count; i++)
            {
                UI_slct_mtrl.Add(T_mtrl.GetChild(i).gameObject);
            }
        }

        public void add(IngredientList mtrlDATA,int total,string name)
        {
            IngredientData data = mtrlDATA.mItemList.Find(item => name == item.dataName);
            UI_slct_mtrl[total - 1].GetComponent<Image>().sprite = data.image;
        }

        private List<IngredientData> mGetingredientDatas(IngredientList mtrlDATA) //Ȯ���� ����Ʈ�� IngredientData�� �����ִ� ����Ʈ�� ��ȯ�Ͽ� ����.
        {
            List<IngredientData> results = new List<IngredientData>();
            foreach(GameObject stock in UI_slct_mtrl)
            {
                string targetImgNm = stock.GetComponent<Image>().sprite.name;
                if (targetImgNm == default_sprite.name) continue;
                IngredientData data = mtrlDATA.mItemList.Find(item => targetImgNm == item.image.name);
                results.Add(data);
            }

            return results;
        }

        public List<EmotionInfo> mGetTotalEmoList(IngredientList mtrlDATA)
        {
            List<IngredientData> raw = mGetingredientDatas(mtrlDATA);
            List<EmotionInfo> results = new List<EmotionInfo>();

            //������ �������� ���ʷ� ����Ʈ�� �߰��Ѵ�.
            foreach(IngredientData data in raw)
            {
                foreach (KeyValuePair<int, int> emo in data.iEmotion)
                {
                    EmotionInfo emoDt = new EmotionInfo((Emotion)emo.Key, emo.Value);
                    results.Add(emoDt);
                }
                    
            }

            return results;

        }

        public GameObject getErsdobj(string name)
        {
            GameObject ERSD = UI_slct_mtrl.Find(item => name == item.name); //������ ��ĭ ã��.
            ERSD.GetComponent<Image>().sprite = default_sprite;

            return ERSD;
        }

        public void m_sort(GameObject ERSD,int total) //����ƮUI ����
        {
            int idx = UI_slct_mtrl.FindIndex(item => ERSD == item);
            if (total <= 0) return;

            //list reorder Algorithm
            for (int i = idx; i < total; i++)
            {
                GameObject curr = UI_slct_mtrl[i];
                GameObject next = UI_slct_mtrl[i + 1];
                curr.GetComponent<Image>().sprite = next.GetComponent<Image>().sprite;
            }

            //exception handling
            if (total == 0) return;
            UI_slct_mtrl[total].GetComponent<Image>().sprite = default_sprite;
        }
        public void u_setUIbright(int total,bool isBright = true)
        {
            if(isBright)
            {
                //�� ���
                for (int i = 0; i < total; i++)
                {
                    UI_slct_mtrl[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                }
            }
            else
            {
                //�� ��Ӱ�
                for (int i = 0; i < total; i++)
                {
                    UI_slct_mtrl[i].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                }
            }
        }
        public void u_init(int _total)
        {
            for (int i = 0; i < _total; i++)
            {
                Debug.Log("�̹��� �ʱ�ȭ");
                UI_slct_mtrl[i].GetComponent<Image>().sprite = default_sprite;
            }
        }
    }
}

public class CloudMakeSystem : MonoBehaviour
{
    ////////////////////////////////////////////////////////
    ///////            Interface Value               ///////
    ////////////////////////////////////////////////////////
    public CloudSystem.EventHandler E_Selected;  //�κ��丮���� ��ἱ�ý� �̺�Ʈ ȣ��_ �Լ� �� �����ϰ� ����...
    public CloudSystem.EventHandler E_UnSelected;  // ���õ� ��� ��� 
    public CloudSystem.EventHandler E_createCloud; //�������� ��ư


    //�����κ��丮 ����Ʈ_�ӽ�
    public List<GameObject> L_cloudsInven;//�ܺο��� 5���� �����س���
    //�ӽ�
    public Sprite default_sprite;//private���� �ٲ� ����.
    public Sprite cloud_sprite;//Data structure�� �ٲ� ����.

    ////////////////////////////////////////////////////////
    ///////            private Value                 ///////
    ////////////////////////////////////////////////////////
    private Transform T_mtrl;//parent transform of mtrl gameObject

    //Data
    [SerializeField]
    public IngredientList mtrlDATA; // ��� ��� ������ ���� �ִ� ����Ʈ scriptable data

    public EmotionsTable emoTableDATA;

    //Total Emotion Input List 
    private List<EmotionInfo> mEmotions;

    //UI
    [SerializeField]
    private CloudSystem.S_list slct_mtrl; //selected_material data class
    [SerializeField]
    private Text UI_btn_txt;

    //count
    private int total;

    //bool
    [HideInInspector]
    public bool isMakingCloud;   // ���� ���� ��, InventoryContainer.cs���� ���õ� ������ Ŭ���� �� ���� �����ϱ� ���� ���� 

    private void d_selectMtrl(string name)
    {
        if (total >= 5) return; //�ִ� 5������ ���� ����

        total++; //update total count

        slct_mtrl.add(mtrlDATA, total, name);
    }

    public bool d_selectMtrlListFull()
    {
        if(total >= 5) { return true; }
        else { return false; }
    }

    public bool d_selectMtrlListEmpty()
    {
        if(total <= 0) { return true; }
        else { return false; }
    }

    private void d_deselectMtrl(string name)
    {
        total--; //update total count
        slct_mtrl.m_sort(slct_mtrl.getErsdobj(name),total); //�������忡���� �̹��� ����
    }

    private bool isOverlapState(List<EmotionInfo> emotionList)
    {
        List<Emotion> tmpL = new List<Emotion>();

        foreach(EmotionInfo info in emotionList)
        {
            if (info.Key > Emotion.INTEXPEC) continue;

            if (tmpL.Contains(info.Key)) return true;
            else
                tmpL.Add(info.Key);

        }

        return false;
    }
    private void d_readCSV(string name)//���� ���չ� �˰���
    {
        Debug.Log("������Ḧ Ȯ���մϴ�.");

        //1.��� �� ���� ����Ʈ ����. => Base Emotion List
        List<EmotionInfo> emotionList = slct_mtrl.mGetTotalEmoList(mtrlDATA);

        //2.�ߺ����� ����Ʈ ����
        //(1) �ߺ������� 3�� �̻��� ��� ���ʿ� ��ġ�� 2���� ������ �ߺ�����Ʈ�� �ִ´�.
        Dictionary<Emotion, KeyValuePair<int, int>> overlapsEmosList = new Dictionary<Emotion, KeyValuePair<int, int>>(); //�ߺ����� Key : {percent, percent}

        Dictionary<Emotion, int> tmpList = new Dictionary<Emotion, int>();

        while (isOverlapState(emotionList))
        {
            bool possibility = false;
            for (int i = 0; i < emotionList.Count; i++)
            {
                EmotionInfo content = emotionList[i];

                if (tmpList.ContainsKey(content.Key))//-1- content�� �ߺ�����Ʈ�� ���ԵǾ�������
                {
                    //overlapsEmosList.Add(content.Key, new KeyValuePair<int, int>(tmpList.Value, content.Value));
                    //-2- �ߺ� ���� �� ���� ��ġ�� ���� ������ ���տ� ���ȴ�.
                    int mergedV = tmpList[content.Key] <= content.Value ? tmpList[content.Key] : content.Value;//���׿����ڸ� �̿��Ͽ� �� ���� �� ����.
                    Debug.Log("[(1)������ġ���]" + tmpList[content.Key] + "|" + content.Value + "����" + mergedV + "����!");
                    tmpList.Remove(content.Key);//-2-1 ���տ� ���� ������ ó�������� �ߺ����� ����Ʈ���� �����Ѵ�.

                    Debug.Log("[(2)�ߺ��߰�]�ߺ������� ����Ʈ���� ����");
                    Debug_PrintState("[�����ߺ�����Ʈ ���� ���]", tmpList);
                    //-3- "emotionList" ���� ���� ���� ���� ������ ������ ������ ���� ������ �Ͼ��.
                    //-3-1 ������ ���� ������ �켱������ ���� �����Ѵ�.
                    EmotionInfo fndItm = new EmotionInfo(content.Key, mergedV);
                    int targetIdx = emotionList.FindIndex(a => (a.Key == content.Key && a.Value == mergedV)); //����� ��������Ʈ������ ���մ�� ������ idx
                    int subTargetIdx = targetIdx - 1;
                    Debug.Log("[(3)���մ��]" + "{" + targetIdx + "}");
                    Debug.Log("[(3)���մ��]" + "{" + targetIdx + "}" + emotionList[targetIdx].Key);
                    Debug.Log("[(3)���մ��]" + "{" + subTargetIdx + "}");


                    bool commend;
                    //�տ� ä�� ���: outOfBound
                    //�ڿ� ä�� ���: outOfBound, �տ��� none ��� ���� ���Ա� ����.
                    if (subTargetIdx >= 0 ? true : false)//�켱����(1): ����� ���� ������ �����Ѵ�.
                        commend = emoTableDATA.getCombineResult(emotionList[targetIdx].Key, emotionList[subTargetIdx].Key) != Emotion.NONE ? false : true;//index�� OutOfBound�� �ƴϰ� ������ ����� �ִٸ�! command = false
                    else
                    {
                        commend = false;
                        subTargetIdx = targetIdx + 1;
                        Debug.Log("[�켱����2]ä��");
                    }


                    //�켱����(2): ������ command�� true�� ���� �켱���� (2)�� �Ѿ��.   subTargetIdx = targetIdx + 1
                    if (!commend && subTargetIdx < emotionList.Count ? true : false)
                        commend = emoTableDATA.getCombineResult(emotionList[targetIdx].Key, emotionList[subTargetIdx].Key) != Emotion.NONE ? true : false;

                    //�켱����(1)�Ǵ� (2)���� ���������� ����� ���������� ���� ���.
                    if (commend)
                    {
                        //(1) ���տ� ���� �� ���� �� ���� ��ġ�� �����´�.
                        int CEmoV = emotionList[targetIdx].Value <= emotionList[subTargetIdx].Value ? emotionList[targetIdx].Value : emotionList[subTargetIdx].Value;
                        EmotionInfo finalEmo = new EmotionInfo(emoTableDATA.getCombineResult(emotionList[targetIdx].Key, emotionList[subTargetIdx].Key), CEmoV);

                        emotionList[targetIdx] = finalEmo;//(2)targetEmotion�� ���յ� �� �������� �ٲ۴�.
                        Debug.Log("[���հ��]" + finalEmo.Key);
                        //(2)���տ� ��� �� ������ ����Ʈ���� �����Ѵ�.(for�� ���̱� ������ index ���� ����� �Ѵ�.)
                        if (finalEmo.Key == Emotion.NONE) continue;

                        emotionList.RemoveAt(subTargetIdx);
                        // ������ ���� index�� ���� Ÿ�� ����  index���� ���� ���(��� ����. ���� Ÿ������ ���� ������ �и��� ������, index�� -1 ���־�� �Ѵ�.
                        if (subTargetIdx < i) i--;
                        // ������ ���� index�� ���� Ÿ�� ���� index���� Ŭ ���.(��� ����: ����Ʈ�� ���̸� �޶�����.)

                        possibility = true;
                    }
  
                }
                else
                {
                    if (Emotion.PLEASURE <= content.Key && content.Key <= Emotion.INTEXPEC)
                        tmpList.Add(content.Key, content.Value);
                }
                Debug_PrintState("[���簨������Ʈ]", emotionList);
                Debug_PrintState("[�����ߺ�����Ʈ]", tmpList);
            }

            if (!possibility)
                break;

        }

        //���� ���� ����Ʈ ����.
        //�ߺ��� �ִٸ� ���� ���� ū ���� ä��
        Dictionary<Emotion, int> LoverlapsEmo = new Dictionary<Emotion, int>();
        
        foreach (EmotionInfo emotion in emotionList)
        {
            if(LoverlapsEmo.Count == 0)
            {
                LoverlapsEmo.Add(emotion.Key, emotion.Value);
                continue;
            }

            if(LoverlapsEmo.ContainsKey(emotion.Key))
            {
                if (LoverlapsEmo[emotion.Key] < emotion.Value)
                    LoverlapsEmo[emotion.Key] = emotion.Value;
            }
            else
                LoverlapsEmo.Add(emotion.Key, emotion.Value);

        }
        Debug_PrintState("[�ߺ� ���� �� ū���� ä��]", LoverlapsEmo);

        List<EmotionInfo> LfinalEmo = new List<EmotionInfo>();
        foreach (KeyValuePair<Emotion,int> overlap in LoverlapsEmo)
        {
            EmotionInfo tmp = new EmotionInfo(overlap.Key, overlap.Value);
            LfinalEmo.Add(tmp);
        }

        emotionList = LfinalEmo;
        Debug_PrintState("[������������Ʈ(1)]", emotionList);

        LfinalEmo = new List<EmotionInfo>();

        //2���� ���� ����(���� ū ���� + �ι�°�� ū ����)
        int roopCnt = 2;
        while(roopCnt!=0)
        {
            EmotionInfo maxValue = new EmotionInfo(emotionList[0].Key, emotionList[0].Value);
            foreach (EmotionInfo emotion in emotionList)
            {
                if (maxValue.Value < emotion.Value)
                    maxValue = emotion;
                else if(maxValue.Value == emotion.Value) //���ٸ� �� �� �������� ����.
                {
                    int i = Random.Range(0, 2);
                    maxValue = (i == 0 ? maxValue : emotion);

                }

            }
            LfinalEmo.Add(maxValue);
            emotionList.Remove(maxValue);
            roopCnt--;
        }

        emotionList = LfinalEmo;
        Debug_PrintState("[������������Ʈ]", emotionList);
        mEmotions = emotionList;
    }

    
    //Debug Function
    private void Debug_PrintState(string sTitle,List<EmotionInfo> emotionList)
    {
        string result = sTitle;
        foreach(EmotionInfo info in emotionList)
        {
            result += (info.Key.ToString()+":" + info.Value + "|");
        }

        Debug.Log(result);
    }

    private void Debug_PrintState(string sTitle, Dictionary<Emotion, int> emotionList)
    {
        string result = sTitle;
        foreach (KeyValuePair<Emotion,int> info in emotionList)
        {
            result += (info.Key.ToString() + "|");
        }

        Debug.Log(result);
    }
    private void d_createCloud(string name = null)
    {
        if (total < 1)
        {
            Debug.Log("������ �����մϴ�.");
            return;
        }

        float time = 5f;
        //�ڷ�ƾ
        UI_btn_txt.text = "����� ��";

        isMakingCloud = true;

        //making UI ó��
        StartCoroutine(isMaking(time));        
    }

    IEnumerator isMaking(float time) //UI ó��
    {

        //�� ��Ӱ�
        slct_mtrl.u_setUIbright(total, false);

        yield return new WaitForSeconds(time);


        yield return new WaitForSeconds(1);
        //�� ���
        slct_mtrl.u_setUIbright(total);

        //UI �ʱ�ȭ
        slct_mtrl.u_init(total);

        total = 0;
        UI_btn_txt.text = "�����ϱ�";

        isMakingCloud = false;

        int emotionCnt = mEmotions.Count;

        InventoryManager inventoryManager = GameObject.FindWithTag("InventoryManager").transform.GetComponent<InventoryManager>();
        inventoryManager.createdCloudData = new CloudData(mEmotions); //createdCloudData ����.
        //ū ��ġ = ������
        //���� ��ġ = ������ ���
        
        transform.Find("I_Result").gameObject.GetComponent<Image>().sprite = inventoryManager.createdCloudData.getBaseCloudSprite();
        Debug.Log("������ ����������ϴ�.");

        m_sendCloud();

        yield break;
    }

    //���� ���� �� �κ��丮�� ����
    private void m_sendCloud()
    {
        //�ش� ������ �´� ���� �̹��� ����

        GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>().createdCloudData = new CloudData(mEmotions);

        ////////////////////�Ʒ��� ����� ����. 
        //���� �κ��丮 ����Ʈ �����ͼ�
        int cnt = 0;

        //�� �κ��丮 ��ô�ؼ� ���� �ֱ�.
        //while(true)
        //{
        //    if (cnt >= 5) break; //�κ��丮 ũ�� �ʰ��ϸ� ����X
        //    if (L_cloudsInven[cnt].GetComponent<Image>().sprite != default_sprite)
        //    {
        //        cnt++;
        //        continue;
        //    }
        //    L_cloudsInven[cnt].GetComponent<Image>().sprite = cloud_sprite;
        //    L_cloudsInven[cnt].GetComponent<Button>().onClick.AddListener(DEMOcreateCloud);
        //    break;
        //}
    }

    public void DEMOcreateCloud()
    {
        Debug.Log("������ ����Ǿ����ϴ�.");
    }
   
    //�ʱ�ȭ �Լ�
    private void init()
    {     
        total = 0;
      
        slct_mtrl = new CloudSystem.S_list();
        slct_mtrl.init(this.transform.Find("Contents").transform);
        slct_mtrl.default_sprite = default_sprite;

        UI_btn_txt = this.transform.Find("B_CloudGIve").GetComponentInChildren<Text>();
        UI_btn_txt.text = "�����ϱ�";

        isMakingCloud = false;

        
        //���� �κ��丮 ����Ʈ�� ���۷����� ������ ���°� ���� �� ����.

        //event
        m_setEvent();
    }

    //eventmethod ���� �Լ�
    private void m_setEvent()
    {
        E_Selected = d_selectMtrl;

        E_UnSelected = d_deselectMtrl;

        E_createCloud = d_readCSV;
        E_createCloud += d_createCloud;
    }


    ////////////////////////////////////////////////////////
    ///////                Pipeline                  ///////
    ////////////////////////////////////////////////////////
    void Start()
    {
        mEmotions = new List<EmotionInfo>(); //��������� �� ���̴� Emotion List
        mtrlDATA = new IngredientList();

        InventoryManager inventoryManager = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();
        mtrlDATA = inventoryManager.mIngredientDatas[inventoryManager.minvenLevel - 1];
        init();
    }

}
