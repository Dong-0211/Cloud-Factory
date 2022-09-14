using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public enum Emotion
{   //PLEASURE���� 0~ �� ���� ����
    PLEASURE, //��� 0
    UNREST, //�Ҿ� 1 
    SADNESS, //���� 2
    IRRITATION, //¥�� 3
    ACCEPT,//���� 4
    SUPCON, //SUPRISE+CONFUSION ���,ȥ�� 5
    DISGUST, //���� 6
    INTEXPEC, //INTERSTING+EXPECTATION ����,��� 7
    LOVE, //8
    OBED, //����. 9
    AWE,//10
    CONTRAY,//�ݴ� 11
    BLAME,//12
    DESPISE,//13
    AGGRESS,//AGGRESSION ���ݼ� 14
    OPTIMISM,//��r��, ��õ 15
    BITTER,//16
    LOVHAT, //LOVE AND HATRED 17
    FREEZE,//18
    CHAOTIC,//ȥ�������� 19
    NONE //20
}

[System.Serializable]
public class EmotionInfo
{
    [SerializeField]
    public Emotion Key;
    [SerializeField]
    public int Value;

    public EmotionInfo(Emotion _Key, int _Value)
    {
        Key = _Key;
        Value = _Value;
    }

    public int getKey2Int()
    {
        return (int)Key; //Emotion Enum �� ������(index��)���� ��ȯ�ؼ� ����
    }

    public int getValue()
    {
        return Value;
    }
}

[CreateAssetMenu(fileName = "IngredientData", menuName = "ScriptableObjects/IngredientData", order = 1)]
public class IngredientData : ScriptableObject
{


    // LJH, �������� ����ȭ
    [SerializeField]
    public string dataName; //��� �̸�

    // LJH, ��������Ʈ�� ����ȭ �Ұ����̾ JsonIgnore���ϸ� ����
    [JsonIgnore]
    public Sprite image;// �̹���

    //��͵� : ��͵��� ���� ������ ���� ���� �� ������ �޶�����.
    [SerializeField]
    public int rarity;

    [SerializeField]
    public EmotionInfo[] emotions;

    [SerializeField]
    public Dictionary<int, int> iEmotion;

    public void init()
    {
        iEmotion = new Dictionary<int, int>();
        foreach (EmotionInfo emotion in emotions)
        {
            iEmotion.Add(emotion.getKey2Int(), emotion.getValue());
        }
    }
}