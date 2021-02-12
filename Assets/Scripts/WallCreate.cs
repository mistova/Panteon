using UnityEngine;
using UnityEngine.UI;

public class WallCreate : MonoBehaviour
{
    private static WallCreate instance;

    public GameObject pixelPrefap;

    Pixel[] pixels;
    public int x, y, painted;
    public float staticWidth, staticHeight;

    public Text percantegeText;

    bool canDraw;
    public GameObject uiTemp;
    Color color;

    public static WallCreate Instance { get => instance; set => instance = value; }

    void Start()
    {
        painted = 0;
        instance = this;
        canDraw = false;
        BuildWall();
    }

    private void BuildWall()
    {
        pixels = new Pixel[x * y];
        for (int i = 0; i < y; i++)
            for (int j = 0; j < x; j++)
                pixels[i * x + j] = Instantiate(pixelPrefap, transform.position + (new Vector3(0, (i - y / 2) * pixelPrefap.transform.localScale.x * 20 + 0.5f, (x / 2 - j) * pixelPrefap.transform.localScale.x * 20 - 0.5f)), new Quaternion(), transform).GetComponent<Pixel>();
        transform.localScale *= (Screen.width * staticHeight) / (staticWidth * Screen.height);//Screen fit according to size.
    }

    void Update()
    {
        if (canDraw)
            DrawByClick();
    }

    private void DrawByClick()
    {
        bool screenClick = false;
        if (Input.GetMouseButton(0) && IsMouseInScreen())
            screenClick = true;
        float x, y;
        x = Input.mousePosition.x;
        y = Input.mousePosition.y;
        if (screenClick)
        {
            x = (int) (this.x * x / Screen.width);
            y = (int) (this.y * (y - (Screen.height - Screen.width) / 2) / Screen.width);
            int i = (int) (y * this.x + x);
            painted += pixels[i].Paint(color);
            percantegeText.text = "" + 100 * painted / (this.x * this.y) + " %";
        }
    }

    private bool IsMouseInScreen()
    {
        if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width)
            return false;
        if (Input.mousePosition.y < (Screen.height - Screen.width) / 2 || Input.mousePosition.y > (Screen.height + Screen.width) / 2)
            return false;
        return true;
    }

    internal void CanDraw()
    {
        GameControl.Instance.ToWall();
    }

    internal void ColorButton(Color color)
    {
        this.color = color;
        uiTemp.SetActive(false);
        canDraw = true;
    }
    public void Clean()
    {
        for (int i = 0; i < y; i++)
            for (int j = 0; j < x; j++)
                pixels[i * x + j].Clean();
        painted = 0;
        percantegeText.text = "" + 0 + " %";
    }
}
