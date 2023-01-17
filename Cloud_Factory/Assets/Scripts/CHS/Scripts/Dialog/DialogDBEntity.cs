using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogDBEntity
{
    // �ش� Ʋ�� ���缭 ���������� ä���.
    public int      GuestID;            // �մ� ��ȣ
    public int      Sat;                // �մ� ������
    public int      SatVariation;       // �մ� ������ ������
    public int      DialogIndex;        // ��ȭ ����
    public int      DialogImageNumber;  // �ش� ��ȭ�� �´� ǥ�� 
    public string   Text;               // ��ȭ ����
    public int      isGuest;            // �մ�/�÷��̾� �� ���� ���ϴ��� (0/1 �� ����)
    public int      VisitCount;         // �մ��� ���� �湮 Ƚ��
    public int      Emotion;            // �մ��� ǥ���ϰ� ���� ����         
}
