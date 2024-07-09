using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] private GameObject setting, reset, quit;
    // Start is called before the first frame update
    void Start()
    {
        setting.SetActive(false);
        reset.SetActive(false);
        quit.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void settingButton()
    {
        setting.SetActive(true);
    }
    public void backButton()
    {
        setting.SetActive(false);
    }
    public void resetButton()
    {
        reset.SetActive(true);
        quit.SetActive(false);
    }
    public void noResetButton()
    {
        reset.SetActive(false);
    }
    public void quitButton()
    {
        quit.SetActive(true);
        reset.SetActive(false);
    }
    public void noQuitButton()
    {
        quit.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
