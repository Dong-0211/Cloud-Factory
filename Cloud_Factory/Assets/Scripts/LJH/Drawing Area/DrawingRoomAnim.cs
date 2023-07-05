using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �����ǿ� ���� �湮���� �� ������ UI

public class DrawingRoomAnim : MonoBehaviour
{
    private Guest mGuestManager;
    public GameObject mExM;
    private bool isOnetime;

    void Awake()
    {
        mGuestManager = GameObject.Find("GuestManager").GetComponent<Guest>();

    }
    void Update()
    {
        if (mGuestManager.isGuestInLivingRoom)
        {
            mExM.SetActive(true);
            isOnetime = true;
        }
        else
        {
            mExM.SetActive(false);
        }
    }
}
