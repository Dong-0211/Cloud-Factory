using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeHeight : MonoBehaviour
{
    RectTransform     Rect;           // �ʻ�ȭ ���α���
    float    height;
    bool    isUp;           // �ʻ�ȭ ���̰� �þ�����ΰ�
    public float ChangeSpeed;
    public float minHeight;

    private void Awake()
    {
        Rect = GetComponent<RectTransform>();
        height = 450.0f;
        minHeight = 420.0f;
        ChangeSpeed = 0.04f;
        isUp = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (height < minHeight && isUp == false)
        {
            isUp = true;
        }
        else if(height > 450.0f && isUp == true)
        {
            isUp = false;
        }

        if(isUp)
        {
            height += ChangeSpeed;
            Rect.sizeDelta = new Vector2(450, height);
        }
        else
        {
            height -= ChangeSpeed;
            Rect.sizeDelta = new Vector2(450, height);
        }
    }
}
