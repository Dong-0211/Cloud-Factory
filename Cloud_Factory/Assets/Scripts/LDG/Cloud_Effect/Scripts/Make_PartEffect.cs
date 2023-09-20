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
    private bool[] Emotions = new bool[8];

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
        IsUsing = false;
        Check_Emotions(cloudSpawner);
    }


    void Update()
    {
        EffectCloudObj = cloudSpawner.newTempCloud.transform;
        EffectGenerate_Pos = cloudSpawner.newTempCloud.transform.position;
        destroy_time += Time.deltaTime;
        InvokeTime += Time.deltaTime;
        StoragedCloudData TempData = cloudSpawner.CloudData;
        int EmotionKey = TempData.mFinalEmotions[0].getKey2Int();

        // ���� ������ Ȯ���Ͽ� ���������ϴ� �Լ� ȣ����
        if (Emotions[0] == true)
        {
            Make_FadeEffect_Cloud(EffectCloudObj, EffectGenerate_Pos, cloudSpawner);
            Emotions[0] = false;
        }
        else if (Emotions[1] == true)
        {
            Make_TornadoEffect_Cloud(EffectCloudObj, EffectGenerate_Pos, cloudSpawner);
            Emotions[1] = false;
        }
        else if (Emotions[2] == true)
        {
            Make_BummerEffect_Cloud(EffectCloudObj, EffectGenerate_Pos, cloudSpawner);
            Emotions[2] = false;
        }
        else if (Emotions[3] == true)
        {
            Make_HeartBeatEffect_Cloud(EffectCloudObj, EffectGenerate_Pos, cloudSpawner);
            Emotions[3] = false;
        }
        else if (Emotions[4] == true)
        {
            Make_BounceEffect_Cloud(EffectCloudObj, EffectGenerate_Pos, cloudSpawner);
            Emotions[4] = false;
        }
        else if (Emotions[5] == true)
        {
            Make_TwinkleEffect_Cloud(EffectCloudObj, EffectGenerate_Pos, cloudSpawner);
            Emotions[5] = false;
        }
        else if (Emotions[6] == true)
        {
            Make_FogEffect_Cloud(EffectCloudObj, EffectGenerate_Pos, cloudSpawner);
            Emotions[6] = false;
        }
        else if (Emotions[7])
        {
            Make_DiagonalEffect_Cloud(EffectCloudObj, EffectGenerate_Pos, cloudSpawner);
            Emotions[7] = false;
        }
        else
        {
            // ���� 2���� destroy��Ű��, �ٽ� �������ִ°� Ȯ���ϴ� if��
            if (InvokeTime >= 2.0f) 
            {
                if(EmotionKey==0 || EmotionKey==6 || EmotionKey==7 || EmotionKey==18 || EmotionKey == 19)
                {
                    Emotions[0] = true;
                    InvokeTime = 0f;
                }
                if(EmotionKey==1)
                {
                    Emotions[1] = true;
                    InvokeTime = 0f;
                }
                if(EmotionKey==2 || EmotionKey==12 || EmotionKey == 16)
                {
                    Emotions[2] = true;
                    InvokeTime = 0f;
                }
                if(EmotionKey==3 || EmotionKey==11 || EmotionKey==15 || EmotionKey == 17)
                {
                    Emotions[3] = true;
                    InvokeTime = 0f;
                }
                if(EmotionKey==4 || EmotionKey==5 || EmotionKey == 8)
                {
                    Emotions[4] = true;
                    InvokeTime = 0f;
                }
                if(EmotionKey==9 || EmotionKey == 10)
                {
                    Emotions[5] = true;
                    InvokeTime = 0f;
                }
                if (EmotionKey == 13)
                {
                    Emotions[6] = true;
                    InvokeTime = 0f;
                }
                if (EmotionKey == 14)
                {
                    Emotions[7] = true;
                    InvokeTime = 0f;
                }
            }
        }

        if (destroy_time >= 2.0f)
        {
            Destroy(New_EffectCloudObjects[0]);
            Destroy(New_EffectCloudObjects[1]);
            Destroy(New_EffectCloudObjects[2]);
            Destroy(New_EffectCloudObjects[3]);
            Destroy(New_EffectCloudObjects[4]);
            Destroy(New_EffectCloudObjects[5]);
            Destroy(New_EffectCloudObjects[6]);
            Destroy(New_EffectCloudObjects[7]);
            destroy_time = 0.0f;
        }

        if (IsUsing)
        {
            this.gameObject.GetComponent<Make_PartEffect>().enabled = false;
            if(New_EffectCloudObjects[0] != null) { Destroy(New_EffectCloudObjects[0]); }
        }
    }

    void Check_Emotions(CloudSpawner tempSpawner)
    {
        if (tempSpawner == null) return;
        StoragedCloudData CData = tempSpawner.CloudData;
        int EmotionKey_Num = CData.mFinalEmotions[0].getKey2Int();

        if(EmotionKey_Num==0 || EmotionKey_Num==6 || EmotionKey_Num==7 || EmotionKey_Num==18 || EmotionKey_Num == 19) { Emotions[0] = true; }
        if (EmotionKey_Num == 1) { Emotions[1] = true; }
        if(EmotionKey_Num==2 || EmotionKey_Num==12 || EmotionKey_Num == 16) { Emotions[2] = true; }
        if(EmotionKey_Num==3 || EmotionKey_Num==11 || EmotionKey_Num==15 || EmotionKey_Num == 17) { Emotions[3] = true; }
        if(EmotionKey_Num==4 || EmotionKey_Num==5 || EmotionKey_Num == 8) { Emotions[4] = true; }
        if(EmotionKey_Num==9 || EmotionKey_Num == 10) { Emotions[5] = true; }
        if(EmotionKey_Num == 13) { Emotions[6] = true; }
        if(EmotionKey_Num == 14) { Emotions[7] = true; }

        Debug.Log("Key : " + EmotionKey_Num);
    }

    void Make_FadeEffect_Cloud(Transform TempTransform, Vector3 updatePos, CloudSpawner tempSpawner)
    {
        Debug.Log("FadeEffect ���� ����!!");
        New_EffectCloudObjects[0] = Instantiate(Make_EffectCloudObjects[0], TempTransform);
        New_EffectCloudObjects[0].transform.position = updatePos;
        StoragedCloudData CData = tempSpawner.CloudData;

        for (int i = 0; i < 3; i++)
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
        Debug.Log("TornadoEffect ���� ����!!");
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

    void Make_BummerEffect_Cloud(Transform TempTransform, Vector3 updatePos, CloudSpawner tempSpawner)
    {
        Debug.Log("BummerEffect ���� ����!!");
        New_EffectCloudObjects[2] = Instantiate(Make_EffectCloudObjects[2], TempTransform);
        New_EffectCloudObjects[2].transform.position = updatePos;
        StoragedCloudData CData = tempSpawner.CloudData;

        for(int i = 0; i < 3; i++)
        {
            PartBody[i] = New_EffectCloudObjects[2].transform.GetChild(i).transform;
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

    void Make_HeartBeatEffect_Cloud(Transform TempTransform, Vector3 updatePos, CloudSpawner tempSpawner)
    {
        Debug.Log("HeartBeatEffect ���� ����!!");
        New_EffectCloudObjects[3] = Instantiate(Make_EffectCloudObjects[3], TempTransform);
        New_EffectCloudObjects[3].transform.position = updatePos;
        StoragedCloudData CData = tempSpawner.CloudData;

        for(int i = 0; i < 3; i++)
        {
            PartBody[i] = New_EffectCloudObjects[3].transform.GetChild(i).transform;
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

    void Make_BounceEffect_Cloud(Transform TempTransform, Vector3 updatePos, CloudSpawner tempSpawner)
    {
        Debug.Log("BouncdeEffect ���� ����!!");
        New_EffectCloudObjects[4] = Instantiate(Make_EffectCloudObjects[4], TempTransform);
        New_EffectCloudObjects[4].transform.position = updatePos;
        StoragedCloudData CData = tempSpawner.CloudData;
        for(int i = 0; i < 3; i++)
        {
            PartBody[i] = New_EffectCloudObjects[4].transform.GetChild(i).transform;
            Part[i] = PartBody[i].transform.GetChild(0).transform;
        }
        for(int i = 0; i < CData.mVPartsList.Count; i++)
        {
            Part[0].GetComponent<SpriteRenderer>().sprite = CData.mVPartsList[i].mImage;
            Part[1].GetComponent<SpriteRenderer>().sprite = CData.mVPartsList[i].mImage;
            Part[2].GetComponent<SpriteRenderer>().sprite = CData.mVPartsList[i].mImage;
        }
        Part[0].transform.localScale = new Vector3(0.11f, 0.11f, 0.5f);
        Part[1].transform.localScale = new Vector3(0.11f, 0.11f, 0.5f);
        Part[2].transform.localScale = new Vector3(0.16f, 0.16f, 0.5f);
    }

    void Make_TwinkleEffect_Cloud(Transform TempTransform, Vector3 updatePos, CloudSpawner tempSpawner)
    {
        Debug.Log("TwinkleEffect ���� ����!!");
        New_EffectCloudObjects[5] = Instantiate(Make_EffectCloudObjects[5], TempTransform);
        New_EffectCloudObjects[5].transform.position = updatePos;
        StoragedCloudData CData = tempSpawner.CloudData;
        for(int i = 0; i < 3; i++)
        {
            PartBody[i] = New_EffectCloudObjects[5].transform.GetChild(i).transform;
            Part[i] = PartBody[i].transform.GetChild(0).transform;
        }
        for(int i = 0; i < CData.mVPartsList.Count; i++)
        {
            Part[0].GetComponent<SpriteRenderer>().sprite = CData.mVPartsList[i].mImage;
            Part[1].GetComponent<SpriteRenderer>().sprite = CData.mVPartsList[i].mImage;
            Part[2].GetComponent<SpriteRenderer>().sprite = CData.mVPartsList[i].mImage;
        }
        Part[0].transform.localScale = new Vector3(0.11f, 0.11f, 0.5f);
        Part[1].transform.localScale = new Vector3(0.11f, 0.11f, 0.5f);
        Part[2].transform.localScale = new Vector3(0.16f, 0.16f, 0.5f);
    }

    void Make_FogEffect_Cloud(Transform TempTransform, Vector3 updatePos, CloudSpawner tempSpawner)
    {
        Debug.Log("FogEffect ���� ����!!");
        New_EffectCloudObjects[6] = Instantiate(Make_EffectCloudObjects[6], TempTransform);
        New_EffectCloudObjects[6].transform.position = updatePos;
        StoragedCloudData CData = tempSpawner.CloudData;
        for (int i = 0; i < 3; i++)
        {
            PartBody[i] = New_EffectCloudObjects[6].transform.GetChild(i).transform;
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

    void Make_DiagonalEffect_Cloud(Transform TempTransform, Vector3 updatePos, CloudSpawner tempSpawner)
    {
        Debug.Log("DiagonalEffect ���� ����!!");
        New_EffectCloudObjects[7] = Instantiate(Make_EffectCloudObjects[7], TempTransform);
        New_EffectCloudObjects[7].transform.position = updatePos;
        StoragedCloudData CData = tempSpawner.CloudData;
        for (int i = 0; i < 3; i++)
        {
            PartBody[i] = New_EffectCloudObjects[7].transform.GetChild(i).transform;
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
