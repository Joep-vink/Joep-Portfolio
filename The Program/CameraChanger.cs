using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraChanger : MonoBehaviour
{
    public CinemachineVirtualCamera JumpScareCam, BedCam, DeskCam, PlayerCam;

    public void JumpScare()
    {
        JumpScareCam.Priority = 1;
        BedCam.Priority = 0;
        DeskCam.Priority = 0;
        PlayerCam.Priority = 0;
    }

    public void Bed()
    {
        JumpScareCam.Priority = 0;
        BedCam.Priority = 1;
        DeskCam.Priority = 0;
        PlayerCam.Priority = 0;
    }

    public void Desk()
    {
        if (JumpScareCam && BedCam)
        {
            JumpScareCam.Priority = 0;
            BedCam.Priority = 0;
        }
        DeskCam.Priority = 1;
        PlayerCam.Priority = 0;
    }

    public void Player()
    {
        if (JumpScareCam && BedCam)
        {
            JumpScareCam.Priority = 0;
            BedCam.Priority = 0;
        }
        DeskCam.Priority = 0;
        PlayerCam.Priority = 1;
    }
}
