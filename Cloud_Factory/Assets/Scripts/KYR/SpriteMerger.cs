using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteMerger : MonoBehaviour
{
    [SerializeField]
    private List<Transform> mRawList; //��������Ʈ �и� �ȵ�, ���ӿ�����Ʈ ����Ʈ

    [SerializeField]
    private List<Sprite> mspriteToMerge;
    // Start is called before the first frame update
    void Start()
    {
        mRawList = new List<Transform>();
    }

    public void initMergeList(GameObject _Fincloud)
    {
        //List �ʱ�ȭ
        mRawList.Add(_Fincloud.transform);

        for(int i = 0; i <_Fincloud.transform.childCount; i++)
        {
            mRawList.Add(_Fincloud.transform.GetChild(i));
        }

        for(int i = 0; i < mRawList.Count; i++)
        {
            mspriteToMerge.Add(mRawList[i].GetComponent<Image>().sprite);
        }
    }
   
    private void Merge()
    {
       // var newText = new Texture2D();
    }
}
