using System.Collections;
using UnityEngine;

public class Bom : MonoBehaviour
{
    private MovableItem movableItem;
    public GameObject bombReadyEffect;
    void Start()
    {
        movableItem = gameObject.GetComponent<MovableItem>();
    }
    private void OnMouseDown()
    {
        if (BombManager.Instance == null) return;

        if (!BombManager.Instance.IsUsingBomb) return;

        Explode();
    }

    private void Explode()
    {
        if(!movableItem.isLocked)
        {
            BombManager.Instance.UseBomb();
            BombManager.Instance.DeactivateBomb();
            Debug.Log("alo");
            bombReadyEffect.SetActive(true);
            CheckKey();
            AudioManager.Instance.Play("Click");
            StartCoroutine(DetroyWait());
        }

    }

    public void CheckKey()
    {
        Key key = gameObject.GetComponentInChildren<Key>();

        if (key)
        {
            key.transform.parent = null;
            key.GotoLock();
        }
    }
IEnumerator DetroyWait()
    {
        yield return new WaitForSeconds(.8f);
        Destroy(gameObject);
    }
}