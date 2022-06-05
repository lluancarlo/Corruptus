using UnityEngine;

public class MapManager : MonoBehaviour
{
    
    void Start()
    {
        var screenHeight = Camera.main.orthographicSize*2f;
        var screenWidth = screenHeight/Screen.height*Screen.width;
        this.CreateMapLimits(screenWidth, screenHeight);
    }

    void CreateMapLimits(float screenWidth, float screenHeight)
    {
        var leftCollider = this.gameObject.AddComponent<BoxCollider2D>();
        leftCollider.offset = new Vector3(-screenWidth/2, 0, 0);
        leftCollider.size = new Vector2(0.1f, screenHeight);

        var rightCollider = this.gameObject.AddComponent<BoxCollider2D>();
        rightCollider.offset = new Vector3(screenWidth/2, 0, 0);
        rightCollider.size = new Vector2(0.1f, screenHeight);
    }
}
