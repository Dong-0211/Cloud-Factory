using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Make_PartEffect : MonoBehaviour
{
    CloudSpawner cloudSpawner;

    Transform[] PartBody = new Transform[3];
    Transform[] Part = new Transform[3];

    // (����Ʈ�ε�����) : (��������Ʈ�������̸�) - (����)
    // 0 : FadeEffectCloud - ���,����,����&���,������,ȥ�������� / 1 : TornadoEffectCloud - �Ҿ� / 2 : BummerEffectCloud - ����,��å,������ / 3 : HeartBeatEffectCloud - ¥��,�ݹ�,��õ,����
    // 4 : BounceEffectCloud - ����,���&ȥ��,��� / 5 : TwinkleEffectCloud - ����,��� / 6 : FogEffectCloud - ��� / 7 : DiagonalEffectCloud - ���ݼ�
    public GameObject[] Make_EffectCloudObjects = new GameObject[8];
    private GameObject[] New_EffectCloudObjects = new GameObject[8];

    private Transform EffectCloudObj;

    private Vector3 EffectGenerate_Pos;

    // 0 : ���,����,����&���,������,ȥ�������� / 1 : �Ҿ� / 2 : ����,��å,������ / 3 : ¥��,�ݹ�,��õ,����
    // 4 : ����,���&ȥ��,��� / 5 : ����,��� / 6 : ��� / 7 : ���ݼ�
    public bool[] Emotions = new bool[8];

    private bool Effect_Start;

    private float InvokeTime;
    private float destroy_time;

    public bool IsUsing;

    void Start()
    {
        cloudSpawner = GameObject.Find("CloudSpawner").GetComponent<CloudSpawner>();
        destroy_time = 0f;
        InvokeTime = 0f;
        for(int i = 0; i < 8; i++)
        {
            Emotions[i] = false;
        }
        Emotions[0] = true;
        Effect_Start = false;
        IsUsing = false;
    }


    void Update()
    {
        
        EffectCloudObj = cloudSpawner.newTempCloud.transform;
        EffectGenerate_Pos = cloudSpawner.newTempCloud.transform.position;
        destroy_time += Time.deltaTime;
        InvokeTime += Time.deltaTime;

        if (Emotions[0])
        {
            Make_FadeEffect_Cloud(EffectCloudObj, EffectGenerate_Pos, cloudSpawner);
            Emotions[0] = false;
            Effect_Start = true;
        }
        else if (Emotions[1])
        {
            Make_TornadoEffect_Cloud(EffectCloudObj, EffectGenerate_Pos, cloudSpawner);
            Emotions[1] = false;
            Effect_Start = true;
        }
        else
        {
            if (InvokeTime >= 2.0f && Effect_Start == true) 
            {
                Emotions[0] = true;
                InvokeTime = 0f;
                Effect_Start = false;
            }
        }

        if (destroy_time >= 2.0f)
        {
            Destroy(New_EffectCloudObjects[0]);
            destroy_time = 0.0f;
        }
        if (IsUsing)
        {
            this.gameObject.GetComponent<Make_PartEffect>().enabled = false;
            if(New_EffectCloudObjects[0] != null) { Destroy(New_EffectCloudObjects[0]); }
        }
    }


    void Make_FadeEffect_Cloud(Transform TempTransform, Vector3 updatePos, CloudSpawner tempSpawner)
    {
        New_EffectCloudObjects[0] = Instantiate(Make_EffectCloudObjects[0], TempTransform);
        New_EffectCloudObjects[0].transform.position = updatePos;
        StoragedCloudData CData = tempSpawner.CloudData;

        for(int i = 0; i < 3; i++)
        {
            PartBody[i] = New_EffectCloudObjects[0].transform.GetChild(i).transform;
            Part[i] = PartBody[i].transform.GetChild(0).transform;
        }
        for (int i = 0; i < CData.mVPartsList.Count; i++)
        {
            Part[0].GetComponent<SpriteRenderer>().sprite = CData.mVPartsList[i].mImage;
            Part[1].GetComponent<SpriteRenderer>().sprite = CData.mVPartsList[i].mImage;
            Part[2].GetComponent<SpriteRenderer>().sprite = CData.mVPartsList[i].mImage;
        }
        Part[0].transform.localScale = new Vector3(0.11f, 0.11f, 0.5f);
        Part[1].transform.localScale = new Vector3(0.11f, 0.11f, 0.5f);
        Part[2].transform.localScale = new Vector3(0.16f, 0.16f, 0.5f);
    }

    void Make_TornadoEffect_Cloud(Transform TempTransform, Vector3 updatePos, CloudSpawner tempSpawner)
    {
        New_EffectCloudObjects[1] = Instantiate(Make_EffectCloudObjects[1], TempTransform);
        New_EffectCloudObjects[1].transform.position = updatePos;
        StoragedCloudData CData = tempSpawner.CloudData;

        for (int i = 0; i < 3; i++)
        {
            PartBody[i] = New_EffectCloudObjects[1].transform.GetChild(i).transform;
            Part[i] = PartBody[i].transform.GetChild(0).transform;
        }
        for (int i = 0; i < CData.mVPartsList.Count; i++)
        {
            Part[0].GetComponent<SpriteRenderer>().sprite = CData.mVPartsList[i].mImage;
            Part[1].GetComponent<SpriteRenderer>().sprite = CData.mVPartsList[i].mImage;
            Part[2].GetComponent<SpriteRenderer>().sprite = CData.mVPartsList[i].mImage;
        }
        Part[0].transform.localScale = new Vector3(0.11f, 0.11f, 0.5f);
        Part[1].transform.localScale = new Vector3(0.11f, 0.11f, 0.5f);
        Part[2].transform.localScale = new Vector3(0.16f, 0.16f, 0.5f);
    }

}
