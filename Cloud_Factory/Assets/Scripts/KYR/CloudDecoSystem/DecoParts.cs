using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecoParts : MonoBehaviour
{
    // Start is called before the first frame update
    
    public bool canAttached; //������ �� �ִ°�.
    public bool isEditActive; //�̵� ȸ�� ��尡 Ȱ��ȭ �Ǿ��°�.
    public bool canEdit; //�̵� �� ȸ���� �����Ѱ�.UI On(�����Ӱ� ��ư)


    public Vector2 mouseWorld;
    private Vector2 top_right_corner;
    private Vector2 bottom_left_corner;
    public void init(Vector2 _top_right_corner, Vector2 _bottom_left_corner)
    {
        canAttached = true;// ���߿� false�� �ٲ������.
        isEditActive = false;
        canEdit = false;
       // top_right_corner = _top_right_corner;
        top_right_corner =new Vector2(2,1);
        bottom_left_corner = new Vector2(-6, -3);
        // bottom_left_corner = _bottom_left_corner;
    }

    private void Update()
    {
        if(!isEditActive)
        {
            CheckIsInValidPlace();
        }

    }

    public bool CheckIsInValidPlace()
    {
        mouseWorld = Camera.main.ScreenToWorldPoint(gameObject.transform.position);
        if (mouseWorld.x < top_right_corner.x && mouseWorld.y < top_right_corner.y &&
            mouseWorld.x > bottom_left_corner.x && mouseWorld.y > bottom_left_corner.y)
        {
            //Debug.Log("Crash");
            canAttached = true;
        }
        else
            canAttached = false;

        return canAttached;
    }

    public void UpdateEditGuidLineUI(bool isClicked)
    {
        canEdit = isClicked;
        transform.GetChild(0).gameObject.SetActive(canEdit);
        transform.GetChild(1).gameObject.SetActive(canEdit);
    }
    public void ChangeDecoPartsPosition(Vector3 mousePos)
    {
        if(!canEdit)
        {
            transform.position = mousePos;
            return;
        }
    }

    public void ReSettingDecoParts()
    {
        if (canAttached)
            isEditActive = true;
        else
            isEditActive = false;
    }

    public void ActiveCanEdit()
    {
        canEdit = true;
    }

    public void UnactiveCanEdit()
    {
        canEdit = false;
    }
}
