using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Sprite onMusic, offMusic, onSfx, offSfx;
    [SerializeField] private Button music, sfx;
    [SerializeField] private string musicName;

    public List<GameObject> list;
    // Start is called before the first frame update
    void Start()
    {
        int n = GameManager.Instance.level();
        if(n>=list.Count) n=list.Count-1;
        for (int i = 0; i <=n; i++)
        {
            list[i].SetActive(true);
        }

            if (AudioManager.Instance!=null) AudioManager.Instance.PlayMusic(musicName);
    
        Time.timeScale = 1;
            pauseMenu.SetActive(false);
            UpdateUI();
    }

    public void pauseButton()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }
    public void resumeButton()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    public void Load(string name)
    {
        GameManager.Instance.LevelIndex = Int32.Parse(name);
        SceneController.Instance.LoadScene("GameScene");
    }
    public void LoadScene(string name)
    {
        SceneController.Instance.LoadScene(name);
    }
    public void PlayMusic(string name)
    {
        AudioManager.Instance.PlayMusic(name);
    }
    public void PlaySfx(string name)
    {
        AudioManager.Instance.PlaySFX(name);
    }
    public void ToggleMusic()
    {
        AudioManager.Instance.ToggleMusic();
        UpdateUI();
    }
    public void ToggleSfx()
    {
        AudioManager.Instance.ToggleSfx();
        UpdateUI();
    }
    public void UpdateUI()
    {
        if (GameManager.Instance.isMuteMusic())
        {
            music.GetComponent<Image>().sprite = offMusic;

        }
        else
        {
            music.GetComponent<Image>().sprite = onMusic;

        }
        if (GameManager.Instance.isMuteSfx())
        {
            sfx.GetComponent<Image>().sprite = offSfx;

        }
        else
        {
            sfx.GetComponent<Image>().sprite = onSfx;

        }
    }

}
