using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudSpawner : MonoBehaviour
{
    SOWManager          SOWManager;         
    InventoryManager    InventoryManager;
    StoragedCloudData   CloudData;

    GameObject cloudMove;

    bool                isCloudGive;        // â���� ������ �����Ͽ��°�
        
    public GameObject   CloudObject;        // ���� ������Ʈ ������

    public int          cloudSpeed;         // ������ �̵��ϴ� �ӵ�

    private GameObject  tempCLoud;          // ���� ���� �� �������� ä��� ���� Temp ������Ʈ

    public Vector3 Cloud_ps = new Vector3(0f, 0f, 0f);

    public RuntimeAnimatorController[] animValue2;
    public RuntimeAnimatorController[] animValue3;
    public RuntimeAnimatorController[] animValue4;


    // ó�� �޾ƿ;� �ϴ� ��
    // 1) ���ư� ������ �ε���
    // 2) � ������ �����ϴ����� ���� ��

    // ���ο��� �����ؾ��� ���
    // 1) ���� ����
    // 2) ���� ���� �ʱ�ȭ
    // 3) ������ ������ ���ڷ� ������

    private void Awake()
    {
        isCloudGive = false;
        cloudSpeed = 3;
        SOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();
        InventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tempCLoud != null)
        {
            Cloud_ps = tempCLoud.transform.position;
        }
    }

    // ������ �����ϰ� �ʱ�ȭ�Ѵ�.
    public void SpawnCloud(int guestNum, StoragedCloudData storagedCloudData)
    {
        // ���� �ν��Ͻ� ����
        tempCLoud = Instantiate(CloudObject);
        tempCLoud.transform.position = this.transform.position;

        // ��ǥ ���� ��ġ ����
        tempCLoud.GetComponent<CloudObject>().SetTargetChair(guestNum);

        // �ӽ÷� �κ��丮�� ����ִ� ���� ��, �� �տ� �ִ� ������ ���� �����´�.
        CloudData = storagedCloudData;

        tempCLoud.GetComponent<CloudObject>().SetValue(CloudData);
        tempCLoud.GetComponent<CloudObject>().SetGuestNum(guestNum);

        // ������ ������ ��ġ���� ���� �ӵ��� �����Ѵ�.
        tempCLoud.GetComponent<CloudObject>().SetSpeed();

        // �����̴� ������ ����Ʈ�� ��Ÿ���� cloudMove�� ���� ����
        cloudMove = tempCLoud.transform.GetChild(0).gameObject;

        // MoveCloud ����
        {
            Cloud_movement movement = cloudMove.GetComponent<Cloud_movement>();

            // image
            // cloudMove.GetComponent<SpriteRenderer>().sprite = storagedCloudData.mVBase.mImage;

            for (int i = 0; i < storagedCloudData.mVPartsList.Count; i++)           // �����̹��� ��������Ʈ�� ����
            {
                movement.Parts_fly.GetComponent<SpriteRenderer>().sprite = storagedCloudData.mVPartsList[i].mImage;
                movement.Parts_fly_2.GetComponent<SpriteRenderer>().sprite = storagedCloudData.mVPartsList[i].mImage;
            }

            // scale
            //cloudMove.transform.localScale = new Vector3(0.11f, 0.12f, 0.5f);
            movement.Parts_fly.transform.localScale = new Vector3(0.11f, 0.11f, 0.5f);
            movement.Parts_fly_2.transform.localScale = new Vector3(0.16f, 0.16f, 0.5f);

            // TODO : MoveCloud Animator�� ������ �°� ���� -> CloudData���� �̿�
            // 1. ���� ������ ���� (���� �Ϸ�)
            // 2. ���� ��� ��޿� ���� �ִϸ����� ������

            int cloudColorNumber = storagedCloudData.GetCloudTypeNum();
            Debug.Log(cloudColorNumber);

            // TODO : ��͵��� ���� ������ ��ȭ��Ű�� �ڵ� �߰�
            int IngredientDataNum = storagedCloudData.GetIngredientDataNum();

            Debug.Log("������ ���� ���� ���� : " + IngredientDataNum);
            if (IngredientDataNum <= 2)
            {
                cloudMove.GetComponent<Animator>().runtimeAnimatorController = animValue3[cloudColorNumber];
            }
            else if (IngredientDataNum == 3)
            {
                cloudMove.GetComponent<Animator>().runtimeAnimatorController = animValue2[cloudColorNumber];
            }
            else
            {
                cloudMove.GetComponent<Animator>().runtimeAnimatorController = animValue4[cloudColorNumber];
            }
            
            if(cloudMove.GetComponent<Animator>().runtimeAnimatorController)
            {

            }
            else
            {
                Debug.Log("���� �ִϸ����� ���� ���� �����߻�! ");

                if(animValue3[cloudColorNumber])
                {
                    Debug.Log("���� �ִϸ����Ͱ� �������� �ʽ��ϴ�.");
                }
            }
        }
    }

    private Sprite ConvertTextureWithAlpha(Texture2D target)
    {
        Texture2D newText = new Texture2D(target.width, target.height, TextureFormat.RGBA32, false);

        for (int x = 0; x < newText.width; x++)
        {
            for (int y = 0; y < newText.height; y++)
            {
                newText.SetPixel(x, y, new Color(1, 1, 1, 0));
            }
        }

        for (int x = 0; x < target.width; x++)
        {
            for (int y = 0; y < target.height; y++)
            {
                var color = target.GetPixel(x, y);
                if (target.GetPixel(x, y).a == 1 && target.GetPixel(x, y).g == 1 && target.GetPixel(x, y).b == 1)
                {
                    color.a = 0;
                }

                newText.SetPixel(x, y, color);
            }
        }
        newText.Apply();

        Sprite sprite = Sprite.Create(newText, new Rect(0, 0, newText.width, newText.height), new Vector2(0.5f, 0.5f));

        return sprite;
    }
    // ���� �̵�
    public void MoveCloud()
    {
        Transform t_target = tempCLoud.GetComponent<CloudObject>().targetChairPos;
        tempCLoud.transform.position = Vector2.MoveTowards(transform.position, t_target.position, cloudSpeed * Time.deltaTime);
    }


}
