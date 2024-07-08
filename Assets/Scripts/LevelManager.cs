using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}