using UnityEngine;

public class ColorManager : MonoBehaviour
{
    private Gradient trailColor;

    public void SetColor(Color color)
    {     
        GameManager gameManager = FindFirstObjectByType<GameManager>();

        if(color == Color.red)
            trailColor = gameManager.red;
        else if (color == Color.yellow)
            trailColor = gameManager.yellow;
        else if (color == Color.cyan)
            trailColor = gameManager.blue;
        else if (color == Color.orange)
            trailColor = gameManager.green;
        else if (color == Color.violet)
            trailColor = gameManager.pink;

        if (GetComponentInChildren<TrailRenderer>())
            GetComponentInChildren<TrailRenderer>().colorGradient = trailColor;

        if (GetComponent<MovableItem>())
            GetComponent<MovableItem>().SetMovableItem(color);
        else if (GetComponent<Cutter>())
            GetComponent<Cutter>().SetCutter(color);        
    }
}
