using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    #region Singleton
    private static LevelManager _instance;
    public static LevelManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {

        }
        else
        {
            _instance = this;
        }

    }
    #endregion Singleton
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Sprite onMusic, offMusic, onSfx, offSfx, onMusicPressed, offMusicPressed, onSfxPressed, offSfxPressed;
    [SerializeField] private Button music, sfx;
    [SerializeField] private string DATA_KEY;
    [SerializeField] private int level;
    [SerializeField] private string musicName;
    private LevelData _levelData;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayMusic(musicName);
        if (PlayerPrefs.HasKey(DATA_KEY))
        {
            //nếu có

            //JSON hóa từ string đã lưu thành class
            string savedJSON = PlayerPrefs.GetString(DATA_KEY);

            //gán nó cho _gameData
            this._levelData = JsonUtility.FromJson<LevelData>(savedJSON);
        }
        else
        {
            Init();
        }
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Init()
    {

        this._levelData = new LevelData();
        SaveData();
    }
    private void SaveData()
    {
        //JSON hóa data clas
        string dataJSON = JsonUtility.ToJson(this._levelData);
        //save JSON string
        PlayerPrefs.SetString(DATA_KEY, dataJSON);
    }
    public int getLv() { return level; }
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
            SpriteState state = music.spriteState;
            state.pressedSprite = offMusicPressed;
            music.spriteState = state;
        }
        else
        {
            music.GetComponent<Image>().sprite = onMusic;
            SpriteState state = music.spriteState;
            state.pressedSprite = onMusicPressed;
            music.spriteState = state;
        }
        if (GameManager.Instance.isMuteSfx())
        {
            sfx.GetComponent<Image>().sprite = offSfx;
            SpriteState state = sfx.spriteState;
            state.pressedSprite = offSfxPressed;
            sfx.spriteState = state;
        }
        else
        {
            sfx.GetComponent<Image>().sprite = onSfx;
            SpriteState state = sfx.spriteState;
            state.pressedSprite = onSfxPressed;
            sfx.spriteState = state;
        }
    }
}