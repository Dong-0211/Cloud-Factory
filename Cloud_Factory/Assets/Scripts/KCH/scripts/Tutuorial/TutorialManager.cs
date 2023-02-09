using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    // Ʃ�丮���� ������ ����Ǿ����� üũ
    [HideInInspector]
	public bool isTutorial;

    /*
     #Ʃ�丮�� ���൵ üũ
     ������ ���� 1
	 ������
	 ������ ���� 2 (ä��)
	 ���� ����
	 ���� ����
	 ���� ����
	 �մ� ���
    */
    [HideInInspector]
	public bool[] isFinishedTutorial;               
                  

	private static TutorialManager instance = null;
    void Awake()
    {
        if(null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            isTutorial = true;
            isFinishedTutorial = new bool[7];
            for(int num = 0; num < isFinishedTutorial.Length; num++) { isFinishedTutorial[num] = false; }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
