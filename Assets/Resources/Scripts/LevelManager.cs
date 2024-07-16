﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
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
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion Singleton

    [Header("Score")] [SerializeField] private ScoreAnimator scoreAnimator;
    [SerializeField] private ScoreData scoreData;

    //UI
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Sprite onMusic, offMusic, onSfx, offSfx;
    [SerializeField] private Button music, sfx;
    [SerializeField] private string DATA_KEY;
    [SerializeField] private string musicName;
    [SerializeField] public int MoveCounter;
    [SerializeField] private TMP_Text Current, YouWin;
    private float time;
    private LevelData _levelData;
    public int CurrentLevel { get; set; }

    [SerializeField] private BoxClassification _boxSO; //Scriptable Object chua cac box, moi box co type va sprite rieng
    [SerializeField] public List<BoxClass> _boxtypelist = new List<BoxClass>(); //Danh sach cac box khi duoc nhan doi
    public List<ButtonScript> _board = new List<ButtonScript>(); //Danh sach cac button
    [SerializeField] private GameObject Board_GameObject;

    [SerializeField] private GameObject _youwonmenu;
    [SerializeField] private GameObject _youlosemenu;

    void Start()
    {
        CurrentLevel = 1;
        InitLevel();
        if (AudioManager.Instance != null) AudioManager.Instance.PlayMusic(musicName);
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
        //TimeManager.Instance.StartTracking();
        time = 0;

        InitBoard();
        UpdateUI();
    }

    void FixedUpdate()
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

    public void pauseButton()
    {
        Current.text = "Time: " + TimeManager.Instance.CurrentTimeString + " Move: " + MoveCounter.ToString() +
                       " Score: " + scoreAnimator.TargetScore.ToString();
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


    #region Music

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

    #endregion Music

    #region Select And Compare Box

    private ButtonScript _box1;
    private ButtonScript _box2;

    public bool GetClick(ButtonScript button)
    {
        //Khong duoc chon lai box cu
        if (_box1 == button)
            return false;

        //Vua chon 2 box thi ko the chon them box khac
        if (_box1 != null && _box2 != null)
            return false;

        //Chưa chọn đủ
        if (_box1 == null)
        {
            _box1 = button;
            return true;
        }

        _box2 = button;

        StartCoroutine(XuLi2Box());
        return true;
    }

    public IEnumerator XuLi2Box()
    {
        yield return new WaitForSeconds(0.7f);
        MoveCounter++;
        //Chọn sai
        if (_box2.buttonType.type != _box1.buttonType.type)
        {
            //Đóng box1 và box2 
            _box2.Wrong();
            _box1.Wrong();

            _box1 = null;
            _box2 = null;
        }
        //Chon dung
        else
        {
            //Animation chon dung
            _box1.Correct();
            _box2.Correct();
            scoreAnimator.TargetScore +=
                scoreData.GetScore(Mathf.Min(_box1.OpenedCounter, _box2.OpenedCounter) - 1).Score;

            //Xoa box1 va box2 khoi list
            for (int i = 0; i < _board.Count; i++)
            {
                if (_board[i] == _box1 || _board[i] == _box2)
                {
                    _board.RemoveAt(i);
                    i--; //List rut phan tu xuong
                }
            }

            _box1 = null;
            _box2 = null;

            if (CheckWinCondition())
                PlayerWin();
        }
    }

    #endregion Select And Compare Box

    #region Khoi tao level

    [SerializeField] private Level_SO levelSO;

    public void InitLevel()
    {
        GridLayoutGroup grid = Board_GameObject.GetComponent<GridLayoutGroup>();
        Level_Info lv_info = levelSO.lv_InfoList[CurrentLevel - 1];
        switch (lv_info.Size)
        {
            case 4:
                grid.constraintCount = 4;
                grid.cellSize = new Vector2(100f, 100f);
                grid.spacing = new Vector2(30f, 30f);
                break;
            case 6:
                grid.constraintCount = 6;
                grid.cellSize = new Vector2(80f, 80f);
                grid.spacing = new Vector2(15f, 15f);
                break;
        }
        Debug.Log(lv_info.Time);
        TimeManager.Instance._limittime = lv_info.Time;
    }

    #endregion Khoi tao level

    #region Khoi tao board

    void Shuffle(List<BoxClass> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            BoxClass temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    void InitBoard()
    {
        //Them cac button vao _board
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PuzzleButton");
        for (int i = 0; i < objects.Length; i++)
        {
            _board.Add(objects[i].GetComponent<ButtonScript>());
        }

        //Chuyen box trong scriptable object vao list va random
        for (int i = 0; i < objects.Length / 2; i++)
        {
            _boxtypelist.Add(_boxSO.BoxTypeList[i]);
            _boxtypelist.Add(_boxSO.BoxTypeList[i]); //Nhan doi so box
        }

        Shuffle(_boxtypelist); //Random cac box

        //Gan box vao cac button
        for (int i = 0; i < objects.Length; i++)
        {
            _board[i].buttonType = _boxtypelist[i];

            //Tim Sprites la con cua button
            Transform spritesTransform = _board[i].transform.Find("Sprites");
            GameObject child = spritesTransform.gameObject;

            //Tim ButtonFrame la con Sprites
            Transform buttonFrameTransform = child.transform.Find("ButtonFrame");
            GameObject grandchild = buttonFrameTransform.gameObject;
            Image grandchildImage = grandchild.GetComponent<Image>();

            //Thay doi ButtonFrame image thanh sprite tuong ung trong box (hinh anh phia sau button)
            grandchildImage.sprite = _board[i].buttonType.icon;
        }
    }

    #endregion

    #region End Game

    public bool CheckWinCondition()
    {
        return _board.Count == 0;
    }

    public void PlayerWin()
    {
        YouWin.text = "Time: " + TimeManager.Instance.CurrentTimeString + " Move: " + MoveCounter.ToString() +
                      " Score: " + scoreAnimator.TargetScore.ToString();
        //Hien thong bao chien thang, hien so sao cua player, cho phep chuyen level...
        _youwonmenu.SetActive(true);
        Debug.Log("You win");
        return;
    }

    public void PlayerLose()
    {
        if (!CheckWinCondition())
            _youlosemenu.SetActive(true);
    }

    #endregion EndGame
}