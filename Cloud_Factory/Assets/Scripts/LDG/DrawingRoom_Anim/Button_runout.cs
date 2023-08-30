using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_runout : MonoBehaviour
{
    // OK��ư ����
    public GameObject OkNoGroup;
    private Transform B_Ok;
    private RectTransform B_OK_Rect;

    // NO��ư ����
    private Transform B_NO;
    private RectTransform B_NO_Rect;

    // ProfileBG����
    private Transform ProfileBG;
    private RectTransform Profile_Rect;

    // I_Portrait����
    private Transform I_Portrait;

    // B_OKȸ����
    private Quaternion OK_StartRot;
    private Quaternion OK_MiddleRot;
    private Quaternion OK_EndRot;

    // OK��ư �̵��� ��ġ����
    private Vector3 Start_Pos;
    private Vector3 Middle_Pos;
    private Vector3 End_Pos;

    // NO��ư �̵��� ��ġ����
    private Vector3 B_NO_Start_Pos;
    private Vector3 B_NO_Middle_Pos;
    private Vector3 B_NO_End_Pos;

    // ProfileBG �̵��� ��ġ����
    private Vector3 Profile_Start_Pos;
    private Vector3 Profile_Middle_Pos;
    private Vector3 Profile_End_Pos;

    // OK��ư ȸ�������ϴ� ����
    private float[] OK_RotationRelation = new float[3];

    // OK��ư �ִϸ��̼��� �÷��̽�Ű�µ� �����ϴ� ����
    private bool OKButton_AnimPlay;

    // OK��ư�� ��,�Ʒ��� ��������� �̵��ϴ��� �����ϴ� ����
    private bool UpRot;
    private bool DownRot;

    // OK��ư �ִϸ��̼��� ���Ʒ��� �Դٰ��ٰŸ��µ� �����ϴ� ����
    private int OK_AnimFinished;

    // OK,NO��ư �� ProfileBG �����̴µ� ����ϴ� ����
    private bool NextMove_Ok;
    private bool LastMove;
    private float MoveTimer;
    private float MoveTimer_2;
    private float MoveDuration;

    // ����
    private float transparency;

    private Vector3 First_Scale;
    private Vector3 Target_Scale;

    void Start()
    {
        for (int i = 0; i < 2; i++)
        {
            UpRot = true;
            DownRot = false;
        }

        OK_RotationRelation[0] = 0.3f;
        OK_RotationRelation[1] = 0f;
        OK_RotationRelation[2] = 0f;

        B_Ok = OkNoGroup.transform.GetChild(2);
        B_OK_Rect = B_Ok.GetComponent<RectTransform>();

        B_NO = OkNoGroup.transform.GetChild(3);
        B_NO_Rect = B_NO.GetComponent<RectTransform>();

        ProfileBG = OkNoGroup.transform.GetChild(1);
        Profile_Rect = ProfileBG.GetComponent<RectTransform>();

        I_Portrait = ProfileBG.transform.GetChild(0);

        OK_StartRot = Quaternion.Euler(0f, 0f, -20f);
        OK_MiddleRot = Quaternion.Euler(0f, 0f, 10f);
        OK_EndRot = Quaternion.Euler(0f, 0f, 0f);

        OKButton_AnimPlay = false;

        OK_AnimFinished = 0;
        NextMove_Ok = false;

        LastMove = false;

        MoveTimer = 0f;
        MoveTimer_2 = 0f;
        MoveDuration = 0.5f;

        Start_Pos = new Vector3(300.0f, B_OK_Rect.localPosition.y, B_OK_Rect.localPosition.z);
        Middle_Pos = new Vector3(100.0f, B_OK_Rect.localPosition.y, B_OK_Rect.localPosition.z);
        End_Pos = new Vector3(1200.0f, B_OK_Rect.localPosition.y, B_OK_Rect.localPosition.z);

        B_NO_Start_Pos = new Vector3(350.0f, B_NO_Rect.localPosition.y, B_NO_Rect.localPosition.z);
        B_NO_Middle_Pos = new Vector3(180.0f, B_NO_Rect.localPosition.y, B_NO_Rect.localPosition.z);
        B_NO_End_Pos = new Vector3(1200.0f, B_NO_Rect.localPosition.y, B_NO_Rect.localPosition.z);

        Profile_Start_Pos = new Vector3(Profile_Rect.localPosition.x, -50f, Profile_Rect.localPosition.z);
        Profile_Middle_Pos = new Vector3(Profile_Rect.localPosition.x, -30f, Profile_Rect.localPosition.z);
        Profile_End_Pos = new Vector3(Profile_Rect.localPosition.x, -900f, Profile_Rect.localPosition.z);

        transparency = 1f;

        First_Scale = B_OK_Rect.localScale;
        Target_Scale = new Vector3(1.3f, 1.3f, 1f);
    }

    void Update()
    {
        if (OKButton_AnimPlay)
        {
            Runout_OKButton();
        }
        if (NextMove_Ok)
        {
            OkNoGroup.GetComponent<OkNoGroup_Anim>().enabled = false;
            NextMove_Button();
        }
    }

    // ������ư ���ȴ��� Ȯ���ϴ� �Լ�
    public void B_Ok_Clicked()
    {
        if (!OKButton_AnimPlay) { OKButton_AnimPlay = true; }
    }

    // ������ư ��鸮�� �ִ� ó���ϴ� �Լ�
    private void Runout_OKButton()
    {
        if (UpRot && OK_AnimFinished != 2)
        {
            OK_RotationRelation[1] += Time.deltaTime;
            float Speed = OK_RotationRelation[1] / OK_RotationRelation[0];
            B_OK_Rect.rotation = Quaternion.Lerp(B_Ok.GetComponent<RectTransform>().rotation, OK_StartRot, Speed);
            B_OK_Rect.localScale = Vector3.Lerp(B_OK_Rect.localScale, Target_Scale, Speed);
            if (Speed >= 0.6f)
            {
                UpRot = false;
                DownRot = true;
                OK_AnimFinished += 1;
                OK_RotationRelation[2] = 0f;
            }
        }
        if (DownRot && OK_AnimFinished != 2)
        {
            OK_RotationRelation[2] += Time.deltaTime;
            float speed = OK_RotationRelation[2] / OK_RotationRelation[0];
            B_OK_Rect.rotation = Quaternion.Lerp(B_Ok.GetComponent<RectTransform>().rotation, OK_MiddleRot, speed);
            B_OK_Rect.localScale = Vector3.Lerp(B_OK_Rect.localScale, First_Scale, speed);
            if (speed >= 0.6f)
            {
                UpRot = true;
                DownRot = false;
                OK_AnimFinished += 1;
                OK_RotationRelation[1] = 0f;
            }
        }
        else
        {
            OK_RotationRelation[1] += Time.deltaTime;
            float speed = OK_RotationRelation[1] / OK_RotationRelation[0];
            B_OK_Rect.rotation = Quaternion.Lerp(B_Ok.GetComponent<RectTransform>().rotation, OK_EndRot, speed);
            B_OK_Rect.localScale = First_Scale;
            if (speed >= 1.0f) { NextMove_Ok = true; }
        }
    }

    private void NextMove_Button()
    {
        if (MoveTimer < MoveDuration) 
        {
            MoveTimer += Time.deltaTime;
            float Speed = MoveTimer / MoveDuration;

            transparency -= 0.005f;
            B_Ok.GetComponent<Image>().color = new Color(255.0f, 255.0f, 255.0f, transparency);
            B_NO.GetComponent<Image>().color = new Color(255.0f, 255.0f, 255.0f, transparency);
            ProfileBG.GetComponent<Image>().color = new Color(255.0f, 255.0f, 255.0f, transparency);
            I_Portrait.GetComponent<Image>().color = new Color(255.0f, 255.0f, 255.0f, transparency);
            B_OK_Rect.localPosition = Vector3.Lerp(Start_Pos, Middle_Pos, Speed);
            B_NO_Rect.localPosition = Vector3.Lerp(B_NO_Start_Pos, B_NO_Middle_Pos, Speed);
            Profile_Rect.localPosition = Vector3.Lerp(Profile_Start_Pos, Profile_Middle_Pos, Speed);
            if(Speed >= 1.0f) { LastMove = !LastMove; }
        }
        if (LastMove)
        {
            MoveTimer_2 += Time.deltaTime;
            float Speed = MoveTimer_2 / MoveDuration;

            transparency -= 0.005f;
            B_Ok.GetComponent<Image>().color = new Color(255.0f, 255.0f, 255.0f, transparency);
            B_NO.GetComponent<Image>().color = new Color(255.0f, 255.0f, 255.0f, transparency);
            ProfileBG.GetComponent<Image>().color = new Color(255.0f, 255.0f, 255.0f, transparency);
            I_Portrait.GetComponent<Image>().color = new Color(255.0f, 255.0f, 255.0f, transparency);
            B_OK_Rect.localPosition = Vector3.Lerp(B_OK_Rect.localPosition, End_Pos, Speed);
            B_NO_Rect.localPosition = Vector3.Lerp(B_NO_Rect.localPosition, B_NO_End_Pos, Speed);
            Profile_Rect.localPosition = Vector3.Lerp(Profile_Rect.localPosition, Profile_End_Pos, Speed);
        }
    }
}
