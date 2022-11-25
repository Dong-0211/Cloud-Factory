using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudSpawner : MonoBehaviour
{
    SOWManager SOWManager;
    InventoryManager InventoryManager;
    StoragedCloudData CloudData;

    bool isCloudGive;

    public GameObject CloudObject;

    public int cloudSpeed;

    private GameObject tempCLoud;


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

    }

    // ���� ����
    public void SpawnCloud(int guestNum, StoragedCloudData storagedCloudData)
    {
        // ���� �ν��Ͻ� ����
        tempCLoud = Instantiate(CloudObject);
        tempCLoud.transform.position = this.transform.position;

        // ��ǥ ���� ��ġ ����
        tempCLoud.GetComponent<CloudObject>().SetTargetChair(guestNum);

        // �ӽ÷� �κ��丮�� ����ִ� ���� ��, �� �տ� �ִ� ������ ���� �����´�.
        CloudData = storagedCloudData;

        tempCLoud.GetComponent<CloudObject>().SetValue(CloudData.mFinalEmotions);
        tempCLoud.GetComponent<CloudObject>().SetGuestNum(guestNum);



        tempCLoud.GetComponent<CloudObject>().SetSprite(ConvertTextureWithAlpha(CloudData.mTexImage));

        tempCLoud.transform.localScale = new Vector3(0.11f, 0.12f, 0.5f);
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
            //Debug.Log("IMAGE LocalScale" + mspriteToMerge[i].texture.width + "///" + mspriteToMerge[i].texture.height);
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
