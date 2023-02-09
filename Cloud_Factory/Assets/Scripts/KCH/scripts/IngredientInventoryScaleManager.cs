using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientInventoryScaleManager : MonoBehaviour
{
    public Sprite[]     inventorySprite = new Sprite[2];  // �κ��丮 �̹��� (2, 3�ܰ�)
    public GameObject   inventoryCover;                   // �κ��丮 ������
    public GameObject   inventoryLayOut;                  // �κ��丮 ���̾ƿ�
    private GameObject  inventoryContent;                  // �κ��丮 content

    InventoryManager mInventoryManager;
    // Start is called before the first frame update
    void Awake()
    {
        mInventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();       // invLevel�� �޾ƿ��� ���� ����
        inventoryContent = inventoryLayOut.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        updateInventory();
    }

    void updateInventory()
    {
        int invLevel = mInventoryManager.minvenLevel;

        if(invLevel <= 2)
        {
            inventoryLayOut.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1200f);
			inventoryContent.GetComponent<Image>().sprite = inventorySprite[0];
			inventoryContent.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = inventorySprite[0];
			inventoryContent.transform.GetChild(0).gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 870f);
		}
        else
        {
			inventoryLayOut.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1275f);
			inventoryContent.GetComponent<Image>().sprite = inventorySprite[1];
			inventoryContent.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = inventorySprite[1];
            inventoryContent.transform.GetChild(0).gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1740f);
		}

        if(invLevel >= 2) { inventoryCover.SetActive(false); }
        else { inventoryCover.SetActive(true); }
    }


}
