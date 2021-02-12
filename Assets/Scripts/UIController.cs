using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{

    bool timeControl = false;

    private void Start()
    {
        Time.timeScale = 0;
    }

    public void ColorButton(Image image)
    {
        WallCreate.Instance.ColorButton(image.color);
    }
    public void Clean()
    {
        WallCreate.Instance.Clean();
    }
    public void Pause()
    {
        if (timeControl)
        {
            Time.timeScale = 1;
            timeControl = false;
        }
        else
        {
            Time.timeScale = 0;
            timeControl = true;
        }
    }
    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Play(GameObject playButton)
    {
        Time.timeScale = 1;
        playButton.SetActive(false);
    }
}
