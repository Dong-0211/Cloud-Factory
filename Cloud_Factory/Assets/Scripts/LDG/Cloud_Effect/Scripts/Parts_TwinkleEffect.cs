using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parts_TwinkleEffect : MonoBehaviour
{
    // ���̵���,�ƿ��� scale�� ��ȭ��Ű�� �κ�
    public GameObject[] Parts = new GameObject[3];
    // �̵���Ű�� �κ�
    public GameObject[] PartBodys = new GameObject[3];

    private float StartFade = 0.0f;
    private float[] Fade_State = new float[3];
    private bool[] FadeOutStart = new bool[3];

    private Vector3 Start_Scale;
    private Vector3 Target_Scale;
    private float Speed = 1.0f;

    void Start()
    {
        for(int i = 0; i < 3; i++) 
        { 
            Fade_State[i] = 0.0f;
            FadeOutStart[i] = false;
        }
        Start_Scale = Parts[2].transform.localScale;
        Target_Scale = new Vector3(0.3f, 0.3f, Parts[2].transform.localScale.z);
    }

    void Update()
    {
        if (!FadeOutStart[0]) { TwinkleStart_Part(0); } // part1 ����
        if(StartFade >= 1.7f) 
        { 
            FadeOutStart[0] = true;     // part1���º�ȭ
            TwinkleStart_Part(1);           // part2 ����
        }
        if (FadeOutStart[0]) { TwinkleEnd_Part(0); }        // part1 ��

        if(StartFade >= 3.0f) { FadeOutStart[1] = true; }   // part2 ���º�ȭ
        if (FadeOutStart[1]) { TwinkleEnd_Part(1); }            // part2 ��


        if(StartFade >= 2.5f) { TwinkleStart_Part3(); }     // part3 ����
        if(StartFade>= 3.5f) { FadeOutStart[2] = true; }    // part3 ���º�ȭ
        if (FadeOutStart[2]) { TwinkleEnd_Part3(); }        // part3 ��

    }

    void TwinkleStart_Part(int index)
    {
        StartFade += Time.deltaTime;
        if (!FadeOutStart[index])
        {
            Fade_State[index] += 0.003f;
            Parts[index].GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 255.0f, Fade_State[index]);
        } 
    }

    void TwinkleEnd_Part(int index)
    {
        if (FadeOutStart[index])
        {
            Fade_State[index] -= 0.003f;
            Parts[index].GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 255.0f, Fade_State[index]);
        }
    }

    void TwinkleStart_Part3()
    {
        if (!FadeOutStart[2])
        {
            Fade_State[2] += 0.003f;
            Parts[2].GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 255.0f, Fade_State[2]);
            Parts[2].transform.localScale = Vector3.Lerp(Parts[2].transform.localScale, Target_Scale, Speed * Time.deltaTime);
        }
    }

    void TwinkleEnd_Part3()
    {
        if (FadeOutStart[2])
        {
            Fade_State[2] -= 0.003f;
            Parts[2].transform.localScale = Vector3.Lerp(Parts[2].transform.localScale, Start_Scale, Speed * Time.deltaTime);
            Parts[2].GetComponent<SpriteRenderer>().color = new Color(255.0f, 255.0f, 255.0f, Fade_State[2]);
        }
    }
}
