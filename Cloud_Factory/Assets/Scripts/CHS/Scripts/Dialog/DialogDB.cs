using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class DialogDB : ScriptableObject
{
	// Excel�� ������ Sheet�̸��� ������ List<DialogDBEntity> ������ ����
	public List<DialogDBEntity> DialogText1;
	public List<DialogDBEntity> DialogText2;

	// ��� DialogText�� �����ϴ� List
	public List<List<DialogDBEntity>> DialogTexts;

	// �� ���� ����� ����� �ٲ� ����	
	public List<DialogDBEntity> SetDialogByGuestNum(int guestNum)
    {
		if (guestNum == 0)
			return DialogText1;
		else if (guestNum == 1)
			return DialogText2;
		else
			return null;
    }

}
