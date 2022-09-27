using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (Input.GetKeyDown(KeyCode.P))
        {
            isCloudGive = true;
            Debug.Log("p");
        }

        // ���� �����Կ��� ���� �Ѿ���� ������ ����
        if (isCloudGive)
        {
            SpawnCloud();
            //MoveCloud();

            isCloudGive = false;
        }
    }

    // ���� ����
    public void SpawnCloud()
    {
        // ���� �ν��Ͻ� ����
        tempCLoud = Instantiate(CloudObject);
        tempCLoud.transform.position = this.transform.position;

        // ��ǥ ���� ��ġ ����
        tempCLoud.GetComponent<CloudObject>().SetTargetChair();

        // �ӽ÷� �κ��丮�� ����ִ� ���� ��, �� �տ� �ִ� ������ ���� �����´�.
        CloudData = InventoryManager.mCloudStorageData.mDatas[0];

        tempCLoud.GetComponent<CloudObject>().SetValue(CloudData.mFinalEmotions);


    }


    // ���� �̵�
    public void MoveCloud()
    {
        Transform t_target = tempCLoud.GetComponent<CloudObject>().targetChairPos;

        tempCLoud.transform.position = Vector2.MoveTowards(transform.position, t_target.position, cloudSpeed * Time.deltaTime);
    }


}
