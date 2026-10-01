using DG.Tweening;
using UnityEngine;

public class Key : MonoBehaviour
{
    public Vector3 axis;
    public float speed;
    private Transform model;

    private void Start()
    {
        model = transform.GetChild(0);
    }
    void Update()
    {
        model.Rotate(axis * speed * Time.deltaTime);
    }

    public void GotoLock()
    {
        AudioManager.Instance.Play("Key");
        LockItem lockItem = FindFirstObjectByType<LockItem>();
        Vector3 lockPos = lockItem.transform.position;

        transform.DOMove(lockPos, 1).OnComplete
        (() =>
        {
            AudioManager.Instance.Play("KeyUnlock");
            lockItem.Unlock();
            Destroy(gameObject);
        });
    }
}