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
        
    }


    // ���� �̵�
    public void MoveCloud()
    {
        Transform t_target = tempCLoud.GetComponent<CloudObject>().targetChairPos;

        tempCLoud.transform.position = Vector2.MoveTowards(transform.position, t_target.position, cloudSpeed * Time.deltaTime);
    }


}
