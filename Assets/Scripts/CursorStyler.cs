
using UnityEngine;

public class CursorStyler : MonoBehaviour {
    public Texture2D cursorTexture;
    public Vector2 HotSpot = new Vector2(32, 32);
    void Start() {
        Cursor.SetCursor(cursorTexture, HotSpot, CursorMode.Auto);
    }
}
