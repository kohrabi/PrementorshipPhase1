using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private void Awake()
    {

        if (_instance != null)
        {

        }
        else
        {
            _instance = this;

        }
        if (PlayerPrefs.HasKey(DATA_KEY))
        {
            //nếu có

            //JSON hóa từ string đã lưu thành class
            string savedJSON = PlayerPrefs.GetString(DATA_KEY);

            //gán nó cho _gameData
            this._gameData = JsonUtility.FromJson<GameData>(savedJSON);

        }
        else
        {
            Init();
        }
    }

    #endregion Singleton
    // Start is called before the first frame update
    private GameData _gameData;
    public const string DATA_KEY = "DATA_KEY";
    void Start()
    {

        if (PlayerPrefs.HasKey(DATA_KEY))
        {
            //nếu có

            //JSON hóa từ string đã lưu thành class
            string savedJSON = PlayerPrefs.GetString(DATA_KEY);

            //gán nó cho _gameData
            this._gameData = JsonUtility.FromJson<GameData>(savedJSON);
            
        }
        else
        {
            Init();
        }
    }
    void Update()
    {

    }
    public int level() {  return _gameData.level; }
    public void Load(string name)
    {
        SceneController.Instance.LoadScene(name);
    }
    public void Init()
    {
        this._gameData = new GameData();
        SaveData();

    }
    private void SaveData()
    {
        //JSON hóa data clas
        string dataJSON = JsonUtility.ToJson(this._gameData);
        //save JSON string

        PlayerPrefs.SetString(DATA_KEY, dataJSON);
    }
    public void Detele()
    {
        PlayerPrefs.DeleteAll();
        _instance._gameData = new GameData();
    }
    public bool ToggleMusic()
    {
        _gameData.isMuteMusic = !_gameData.isMuteMusic;
        SaveData();
        return _gameData.isMuteMusic;
    }
    public bool ToggleSfx()
    {
        _gameData.isMuteSfx = !_gameData.isMuteSfx;
        SaveData();
        return _gameData.isMuteSfx;
    }
    public bool isMuteMusic() { return _gameData.isMuteMusic; }
    public bool isMuteSfx() { return _gameData.isMuteSfx; }
    public void Wait(float t)
    {
        StartCoroutine(wait(t));
    }
    IEnumerator wait(float t)
    {
        yield return new WaitForSeconds(t);
    }
}
