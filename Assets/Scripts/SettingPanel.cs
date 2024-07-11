using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] private GameObject setting, reset, quit,credits;
    // Start is called before the first frame update
    void Start()
    {
        setting.SetActive(false);
        reset.SetActive(false);
        quit.SetActive(false);
        credits.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void settingButton()
    {
        setting.SetActive(true);
        credits.SetActive(true);
    }
    public void backButton()
    {
        setting.SetActive(false);
        
    }
    public void resetButton()
    {
        reset.SetActive(true);
        quit.SetActive(false);
        credits.SetActive(false) ;
    }
    public void noResetButton()
    {
        reset.SetActive(false);
        credits.SetActive(true) ;
    }
    public void quitButton()
    {
        quit.SetActive(true);
        reset.SetActive(false);
        credits.SetActive(false);
    }
    public void noQuitButton()
    {
        quit.SetActive(false);
        credits.SetActive(true);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
