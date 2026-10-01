using Unity.Cinemachine;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    CinemachineTargetGroup cinemachineTargetGroup;

    private void Start()
    {
        cinemachineTargetGroup = FindFirstObjectByType<CinemachineTargetGroup>();
    }

    public void AddTarget(Transform raftTra)
    {
        cinemachineTargetGroup.AddMember(raftTra, 1, 1);
    }
}