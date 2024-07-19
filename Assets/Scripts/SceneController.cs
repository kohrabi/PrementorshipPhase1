using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{
    #region Singleton
    private static SceneController _instance;
    public static SceneController Instance => _instance;

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

    [SerializeField] public Transform TransitionRect;
    Tween transitionEntry;
    Tween transition;

    private void Start()
    {
        PlayOpenAnimation();
        if (Instance == null)
            SceneManager.sceneLoaded += (scene, mode) => PlayOpenAnimation() ;
    }

    public void OnDestroy()
    {
        SceneManager.sceneLoaded -= (scene, mode) => PlayOpenAnimation();
    }

    public void PlayOpenAnimation()
    {
        if (TransitionRect == null)
        {
            TransitionRect = GameObject.FindGameObjectWithTag("TransitionRect").transform;
        }

        if (TransitionRect != null)
        {
            Debug.Log("Called");
            DOTween.Complete(TransitionRect, true);
            TransitionRect.localPosition = Vector3.zero;
            TransitionRect.GetChild(0).gameObject.SetActive(true);
            transition = TransitionRect.DOMove(new Vector3(Screen.width * 2, -Screen.height / 2, 0), 1f)
                .SetDelay(0.2f)
                .SetEase(Ease.OutQuart)
                .Play()
                .OnComplete(() =>
                {
                    TransitionRect.localPosition = new Vector3(-Screen.width * 2 * TransitionRect.localScale.x,
                                                                Screen.height * 2 * TransitionRect.localScale.y,
                                                                0);
                    TransitionRect.GetChild(0).gameObject.SetActive(false);
                });
        }

    }

    public void LoadScene(string sceneName)
    {
        if (TransitionRect == null)
        {
            TransitionRect = GameObject.FindGameObjectWithTag("TransitionRect").transform;
        }

        if (TransitionRect != null)
        {
            DOTween.Complete(TransitionRect, true);
            TransitionRect.GetChild(0).gameObject.SetActive(true);
            transition = TransitionRect.DOLocalMove(Vector3.zero, 0.8f)
                .SetDelay(0.2f)
                .SetEase(Ease.OutQuart)
                .Play()
                .SetUpdate(true)
                .OnComplete(() => Load(sceneName));
        }
        else
            Load(sceneName);
    }


    private void Load(string sceneName)
    {
        DOTween.KillAll(true);
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
    public void LoadNext()
    {
        GameManager.Instance.LevelIndex++;
        SceneController.Instance.LoadScene("GameScene");
    }
}
