using UnityEngine;

public class CellView : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetupView(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }
}
