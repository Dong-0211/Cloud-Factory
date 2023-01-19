using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class VirtualGameObject
{
    //Transform
    public Vector3 mPosition;
    public Quaternion mRotation;
    public Vector3 mScale;

    public Sprite mImage;

    public VirtualGameObject(Vector3 position, Quaternion rotation, Vector3 scale, Sprite image)
    {
        mPosition = position;
        mRotation = rotation;
        mScale = scale;
        mImage = image;
    }
}

//���� �ϳ��� �����ϴ� ������Ʈ
public class VirtualObjectManager : MonoBehaviour
{
    public GameObject OBPrefab; //���ӿ�����Ʈ �ȿ� ��ư�� �̹��� ������Ʈ�� �ִ� Prefab
    public GameObject convertVirtualToGameObject(VirtualGameObject VObject)
    {
        GameObject result;

        result = Instantiate(OBPrefab, VObject.mPosition, VObject.mRotation);

        result.GetComponent<Image>().sprite = VObject.mImage;

        

        return result;
    }
}
