using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ UI
public class DrawingUIManager : MonoBehaviour
{
    [SerializeField] bool       mTextEnd;      // �ؽ�Ʈ ����� ������ ��?
    [SerializeField] GameObject gSpeechBubble; // ��ǳ�� ������Ʈ
    [SerializeField] GameObject gOkNoGroup;    // ���� ���� ������Ʈ

    void Update()
    {
        if (mTextEnd == true) // �ؽ�Ʈ ����� �����ٸ�
        {
            gSpeechBubble.SetActive(false);
            gOkNoGroup.SetActive(true);
        }        
    }

    public void ActiveOk()
    {
        gSpeechBubble.SetActive(true);
        gOkNoGroup.SetActive(false);
        mTextEnd = false;

        // �������� �� �޼ҵ� ȣ��
        Debug.Log("������ ���� �޼ҵ� ȣ��");
    }

    public void ActiveNo()
    {
        TutorialManager mTutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
        if (!mTutorialManager.isFinishedTutorial[1]) { return; }

        gSpeechBubble.SetActive(true);
        gOkNoGroup.SetActive(false);
        mTextEnd = false;

        // �������� �� �޼ҵ� ȣ��
        Debug.Log("������ ���� �޼ҵ� ȣ��");
    }
}
