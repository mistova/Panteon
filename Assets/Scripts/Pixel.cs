using UnityEngine;

public class Pixel : MonoBehaviour
{
    Color color;
    bool colored;
    SpriteRenderer sprite;
    void Start()
    {
        colored = false;
        sprite = GetComponent<SpriteRenderer>();
        color = new Vector4(0, 0, 0, 0);
    }

    public int Paint(Color color)
    {
        this.color += color;
        sprite.color = this.color / this.color.a;
        if (colored)
            return 0;
        else
        {
            colored = true;
            return 1;
        }
    }
    public void Clean()
    {
        color = new Vector4(0, 0, 0, 0);
        sprite.color = Color.white;
        colored = false;
    }


}
