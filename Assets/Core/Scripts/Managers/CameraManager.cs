using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoSingleton<CameraManager>
{
    public Camera mainCam;
    public CinemachineVirtualCamera gameCam;
    public CinemachineVirtualCamera mergeCam;

    public Vector3 ConvertToCameraRelativeInput(Vector2 input)
    {
        // first find out basis vectors
        Vector3 forward = mainCam.transform.forward;
        Vector3 right = mainCam.transform.right;

        // project onto ground plane & normalize
        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // scale by input
        forward *= input.y;
        right *= input.x;

        return forward + right;
    }
}
