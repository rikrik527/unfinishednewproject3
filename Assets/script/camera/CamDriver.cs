
using UnityEngine;
using Cinemachine;
public class CamDriver
{

    private CinemachineVirtualCamera vcam;
    private CinemachineFollowZoom followZoom;

    public CamDriver(GameObject gameObject)
    {
        InitializeFromGameObject(gameObject);
    }

    private void InitializeFromGameObject(GameObject gameObject)
    {
        if (gameObject != null)
        {
            vcam = gameObject.GetComponent<CinemachineVirtualCamera>();
            followZoom = gameObject.GetComponent<CinemachineFollowZoom>();
        }
    }

    public void SetTarget(Transform trackNode)
    {
        vcam.Follow = trackNode;
        vcam.LookAt = trackNode;
    }
    public void ZoomTo(float destSize, float duration = 0)
    {
        followZoom.m_Damping = duration;
        followZoom.m_Width = destSize;
    }
}
