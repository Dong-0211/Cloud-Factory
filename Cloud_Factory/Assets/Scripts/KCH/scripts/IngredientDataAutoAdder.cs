using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��͵� 4�� ������ ��Ȯ ���� ���θ� �Ǵ� �� �ڵ����� ����Ʈ�� �߰��ϴ� ��ũ��Ʈ
// ���� �ٲ� ������, SeasonDateCalc.CalcDay()���� ����
// ó������ ġ���� ��Ƽ�� �����Ƿ� 1���������� ���� X
public class IngredientDataAutoAdder : MonoBehaviour
{
	int guestCount = 20;        // ���̺� ���� ���� �մ��� �� �߰� ���ɼ����� ���� ���� ����

    GuestInfos[] mGuestInfo;

    private Guest mGuestManager;                        // �մ��� ������ �޾ƿ��� ���� ����
    private InventoryManager mInverntoryManager;        // �κ��丮 �Ŵ����� ä�� ���� ��� ����Ʈ�� �ҷ����� ���� ����

    // Start is called before the first frame update
    void Start()
    {
        mGuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();
        mInverntoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
       
        mGuestInfo = mGuestManager.mGuestInfo;   // GuestManager�� �մ� ������ �޾ƿ�
	}

    // ġ���� �մ��� �ִ��� üũ�ϴ� �Լ�
    public void CheckIsCured()
    {
        for(int num = 0; num < guestCount; num++)
        {
            if (mGuestInfo[num].isCure) { AddIngredientToList(num); }
        }
    }

    // ġ���� �մ��� ���� ��, InventoryManager�� mIngredientData[3](��͵� 4�� �����ϴ� ����Ʈ)�� ġ���� �մ��� ��Ḧ �߰�
    private void AddIngredientToList(int guest_num)
    {
        // guest_num�� �ش��ϴ� �մ��� ���� ��� �޾ƿ��� ����
        IngredientData data = ScriptableObject.CreateInstance<IngredientData>();
        string tempWord = mGuestInfo[guest_num].mSeed.ToString();
        data.name = tempWord;
        data.dataName = tempWord;
        data.image = Resources.Load<Sprite>("Sprite/Ingredient/Rarity4/" + "M4_" + data.name);
        data.emotions = new EmotionInfo[1];
        data.emotions[0] = new EmotionInfo((Emotion)mGuestInfo[guest_num].mSeed, -1);
        data.init();

        // ��ᰡ �ߺ��Ǵ��� �˻�
        foreach (IngredientData tempData in mInverntoryManager.mIngredientDatas[3].mItemList)
        {
            if (tempData.dataName == data.dataName) { return; }
        }

        // ��ᰡ �ߺ����� ������ ��͵� 4 ��� ����Ʈ�� �߰�
        mInverntoryManager.mIngredientDatas[3].mItemList.Add(data);
    } 
}
