using Micosmo.SensorToolkit;
using UnityEngine;

public class MovableItem : MonoBehaviour
{
    public RaySensor rightRaySensor, leftRaySensor, forwardRaySensor, backRaySensor;
    [HideInInspector] public bool canDrag, isLocked;
    public Color color;
    public MeshRenderer meshRenderer;

    public void SetMovableItem(Color _color)
    {
        color = _color;
        meshRenderer.material.color = color;
    }

    public bool CanMove(string dir)
    {
        if (dir == "Right")
        {
            return CheckRaySensor(backRaySensor);
        }
        else if (dir == "Left")
        {
            return CheckRaySensor(forwardRaySensor);
        }
        else if (dir == "Forward")
        {
            return CheckRaySensor(rightRaySensor);
        }
        else if (dir == "Back")
        {
            return CheckRaySensor(leftRaySensor);
        }

        canDrag = true;
        return false;
    }

    private bool CheckRaySensor(RaySensor raySensor)
    {
        if (raySensor.GetDetections().Count > 0)
        {
            Cutter cutter = raySensor.GetNearestDetection().GetComponent<Cutter>();

            if (cutter && cutter.cutterColors.Contains(color))
            {
                cutter.SetCutterParticleColor(color);
                Debug.Log("Cutter");
                canDrag = false;
                return true;
            }
            else
            {
                canDrag = true;
                return false;
            }
        }
        else
        {
            Debug.Log("No detection");
            canDrag = false;
            return true;
        }
    }

    public void Unlock()
    {
        isLocked = false;
        AudioManager.Instance.Play("Unlocked");
    }
}