using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudSpawner : MonoBehaviour
{
    SOWManager          SOWManager;         
    InventoryManager    InventoryManager;
    public StoragedCloudData   CloudData;

    GameObject cloudMove;

    bool                isCloudGive;        // â���� ������ �����Ͽ��°�

    public int          cloudSpeed;         // ������ �̵��ϴ� �ӵ�

    public Vector3 Cloud_ps;

    public RuntimeAnimatorController[] animValue2;
    public RuntimeAnimatorController[] animValue3;
    public RuntimeAnimatorController[] animValue4;


    public GameObject EffectCloudObj;   // ���ο� ���� ������Ʈ ������

    Make_PartEffect make_PartEffect;

    public GameObject newTempCloud;

    public bool IsUsing;                   // ������ ��������� üũ

    GameObject MainEffectCloudMove;

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

    void Start()
    {
        IsUsing = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // ������ �����ϰ� �ʱ�ȭ�Ѵ�.
    public void SpawnCloud(int guestNum, StoragedCloudData storagedCloudData /*QA��*/, int sat)
    {
        // ���� �ν��Ͻ� ����
        newTempCloud = Instantiate(EffectCloudObj);
        newTempCloud.transform.GetChild(0).gameObject.SetActive(true);

        make_PartEffect = newTempCloud.GetComponent<Make_PartEffect>();

        SOWManager SOWManager = GameObject.Find("SOWManager").GetComponent<SOWManager>();
        if(SOWManager != null)
        {
            SOWManager.mCloudObjectList.Add(newTempCloud);
        }

        Debug.Log("Instantiate");
        newTempCloud.transform.position = this.transform.position;
        Cloud_ps = newTempCloud.transform.position;

        // ��ǥ ���� ��ġ ����
        newTempCloud.GetComponent<CloudObject>().SetTargetChair(guestNum);
        
        Debug.Log("SetTargetChair");

        // ������ �����޴� �մ��� isGettingCloud ���¸� �����Ѵ�.
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Guest");
            if (gameObjects != null)
            {
                foreach (GameObject gameObject in gameObjects)
                {
                    if(gameObject == null)
                    {
                        continue;
                    }

                    GuestObject guestObject = gameObject.GetComponent<GuestObject>();
                    if (guestObject != null && guestObject.mGuestNum == guestNum)
                    {
                        bool isGettingCloud = gameObject.GetComponent<GuestObject>().isGettingCloud;
                        gameObject.GetComponent<GuestObject>().isGettingCloud = true;
                    }
                }
            }
        }

        // �ӽ÷� �κ��丮�� ����ִ� ���� ��, �� �տ� �ִ� ������ ���� �����´�.
        CloudData = storagedCloudData;

        CloudObject cloudObject = newTempCloud.GetComponent<CloudObject>(); 
        if (cloudObject != null)
        {
            cloudObject.SetValue(CloudData);
            cloudObject.SetGuestNum(guestNum);

            // ������ ������ ��ġ���� ���� �ӵ��� �����Ѵ�.
            cloudObject.SetSpeed();

            // QA��
            cloudObject.sat = sat;
        }

        // �����̴� ������ ����Ʈ�� ��Ÿ���� MaincloudMove�� ���� ����
        MainEffectCloudMove = newTempCloud.transform.GetChild(0).gameObject;

        // MoveCloud ����
        {
            Make_PartEffect make_PartEffect = MainEffectCloudMove.GetComponent<Make_PartEffect>();
            if(make_PartEffect == null)
            {
                return;
            }

            // TODO : MoveCloud Animator�� ������ �°� ���� -> CloudData���� �̿�
            // 1. ���� ������ ���� (���� �Ϸ�)
            // 2. ���� ��� ��޿� ���� �ִϸ����� ������

            int cloudColorNumber = storagedCloudData.GetCloudTypeNum();

            // TODO : ��͵��� ���� ������ ��ȭ��Ű�� �ڵ� �߰�
            int IngredientDataNum = storagedCloudData.GetIngredientDataNum();

            Debug.Log("������ ���� ���� ���� : " + IngredientDataNum);

            // Prefab������ �ִϸ��̼� ������ �κ��Դϴ� - ���� -
            if (IngredientDataNum <= 2)
            {
                //cloudMove.GetComponent<Animator>().runtimeAnimatorController = animValue3[cloudColorNumber];
                MainEffectCloudMove.GetComponent<Animator>().runtimeAnimatorController = animValue3[cloudColorNumber];
            }
            else if (IngredientDataNum == 3)
            {
                //cloudMove.GetComponent<Animator>().runtimeAnimatorController = animValue2[cloudColorNumber];
                MainEffectCloudMove.GetComponent<Animator>().runtimeAnimatorController = animValue2[cloudColorNumber];
            }
            else
            {
                //cloudMove.GetComponent<Animator>().runtimeAnimatorController = animValue4[cloudColorNumber];
                MainEffectCloudMove.GetComponent<Animator>().runtimeAnimatorController = animValue4[cloudColorNumber];
            }

            //if(cloudMove.GetComponent<Animator>().runtimeAnimatorController)
            //{

            //}
            if (MainEffectCloudMove.GetComponent<Animator>().runtimeAnimatorController)
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
        Transform New_Target = newTempCloud.GetComponent<CloudObject>().targetChairPos;
        newTempCloud.transform.position = Vector2.MoveTowards(transform.position, New_Target.position, cloudSpeed * Time.deltaTime);
    }
}
