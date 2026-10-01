using DG.Tweening;
using RDG;
using System;
using System.Collections;
using UnityEngine;

public class MouseDrag : MonoBehaviour
{
    private Vector3 mouseStart;
    private GameObject selectedObject;
    private float dragThreshold = 20f; // Minimum movement needed to register drag
    [HideInInspector] public bool isHorizontalMover;
    MovableItem movableItem;
    private GameManager gameManager;

    private void Start()
    {
        movableItem = GetComponent<MovableItem>();
        gameManager = FindFirstObjectByType<GameManager>();

        if (transform.rotation.eulerAngles.y == 90)
            isHorizontalMover = true;
        else
            isHorizontalMover = false;
    }

    private void OnMouseDown()
    {
        Debug.Log("Down");

        if(gameManager.levelNo > 10)
        {
            if (gameManager.moveLimitValue <= 0)
                return;
        }

        if (movableItem.canDrag && !movableItem.isLocked && !gameManager.isDragging)
            selectedObject = gameObject;
    }

    private void OnMouseUp()
    {
        selectedObject = null;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            mouseStart = Input.mousePosition;

        if (Input.GetMouseButton(0) && selectedObject)
        {
            Vibration.Vibrate(10);

            Vector3 mouseEnd = Input.mousePosition;
            Vector3 delta = mouseEnd - mouseStart;
            if (delta.magnitude < dragThreshold) return; // Ignore small drags
            string direction = "";

            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                direction = delta.x > 0 ? "Right" : "Left";
            else
                direction = delta.y > 0 ? "Forward" : "Back";

            Debug.Log($"Dragged {selectedObject.name} {direction}");

            StartCoroutine(Move(direction));

            selectedObject = null;
        }
    }

    public IEnumerator Move(string dir)
    {
        Debug.Log("Move");
        bool isMoved = false;

        if (isHorizontalMover && (dir == "Forward" || dir == "Back"))
        {
            Debug.Log("isHorizontalMover");
            yield break;  // Add delay if needed
        }

        if (!isHorizontalMover && (dir == "Right" || dir == "Left"))
        {
            Debug.Log("isVerticalMover");
            yield break;  // Add delay if needed
        }

        if (movableItem.CanMove(dir))
            gameManager.DecreseMoveLimitText();

        while (movableItem.CanMove(dir))
        {
            gameManager.isDragging = true;

            Vector3 moveOffset = Vector3.zero;
            isMoved = true;

            if (isHorizontalMover)
            {
                if (dir == "Right")
                {
                    moveOffset = transform.right * 1;  // Move along local X+
                }
                else if (dir == "Left")
                {
                    moveOffset = -transform.right * 1; // Move along local X-
                }
            }
            else
            {
                if (dir == "Forward")
                {
                    moveOffset = transform.right * 1; // Move along local Z+
                }
                else if (dir == "Back")
                {
                    moveOffset = -transform.right * 1; // Move along local Z-
                }
            }
            // Move the object and wait for the animation to finish
            yield return transform.DOMove(transform.position + moveOffset, 0.05f).SetEase(Ease.Linear).WaitForCompletion();
            gameManager.isDragging = false;
        }

        if (isMoved)
            AudioManager.Instance.Play("Move");
    }
}