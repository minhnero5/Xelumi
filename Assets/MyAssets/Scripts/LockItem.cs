using System.Collections.Generic;
using UnityEngine;

public class LockItem : MonoBehaviour
{
    public List<GameObject> locks;

    public void Unlock()
    {
        Destroy(locks[0]);
        locks.RemoveAt(0);

        if(locks.Count == 0)
            GetComponentInParent<MovableItem>().Unlock();
    }
}