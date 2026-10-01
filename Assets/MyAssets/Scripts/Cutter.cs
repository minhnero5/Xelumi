using DG.Tweening.Core.Easing;
using System;
using System.Collections.Generic;
using UnityEngine;
using RDG;

public class Cutter : MonoBehaviour
{
    public ParticleSystem cuttingParticle;
    public List<Color> cutterColors;
    public MeshRenderer containerR, containerL, gateR, gateL;
    public GameObject arrowM, arrowR, arrowL;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    void Update()
    {
  
    }
    public void SetCutter(Color color)
    {
        cutterColors.Add(color);

        containerR.material.color = cutterColors[0];
        containerL.material.color = cutterColors[0];

        gateR.material.color = cutterColors[0];
        gateL.material.color = cutterColors[0];

        if (cutterColors.Count == 2)
        {
            arrowM.SetActive(false);
            arrowR.SetActive(true);
            arrowL.SetActive(true);
            containerL.material.color = cutterColors[1];
            gateL.material.color = cutterColors[1];
        }
    }

    public void SetCutterParticleColor(Color color)
    {
        cuttingParticle.GetComponent<ParticleSystemRenderer>().material.color = color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 10) // Movable Item
        {
            Key key = other.GetComponentInChildren<Key>();

            if (key)
            {
                key.transform.parent = null;
                key.GotoLock();
            }

            Vibration.Vibrate(50);

            AudioManager.Instance.Play("Cut");
            cuttingParticle.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 10) // Movable Item
        {
            gameManager.isDragging = false;

            cuttingParticle.Stop();
            Destroy(other.gameObject);

            Invoke(nameof(CheckWin), .5f);
        }
    }

    private void CheckWin()
    {
        GameManager gameManager = FindFirstObjectByType<GameManager>();

        if (FindFirstObjectByType<MovableItem>())
            Debug.Log("NotYet");
        else
            gameManager.Win();
    }

}
