using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �����ǿ� ���� �湮���� �� ������ UI

public class DrawingRoomAnim : MonoBehaviour
{
    private Guest       mGuestManager;
    public  GameObject  mExM;

    void Awake()
    {
        mGuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();

    }
    void Update()
    {
        if (mGuestManager.isGuestInLivingRoom)
        {
            mExM.SetActive(true);
        }
        else
        {
            mExM.SetActive(false);
        }
    }
}
