using DG.Tweening;
using System.Collections;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public enum ButtonState
{
    None,
    Intro,
    Hover,
    Exit, 
    Chosen,
    Correct,
    Wrong
}

public class ButtonScript : MonoBehaviour
{
    //De phan loai button//Khoi
    public BoxClass buttonType;

    [SerializeField] public Transform Text;
    [SerializeField] public Transform Sprites;
    [SerializeField] public Transform HiddenFrame;
    [SerializeField] public RectTransform Collider;
    [SerializeField] public Image ButtonSprite;
    [Header("Animation Time")]
    [SerializeField] public float OpenCloseAnimation = 0.4f;
    [SerializeField] public float WaitOpen = 0.5f;
    [SerializeField] public float ExitAnimation = 0.4f;
    [SerializeField] public float HoverAnimation = 0.4f;
    [SerializeField] public float ShakeAnimation = 0.4f;
    
    ButtonState state; // Trang thai cua button
    [HideInInspector] public int OpenedCounter = 0;
    [HideInInspector] public bool Completed = false;
    [HideInInspector] public bool IsChosen = false;
    bool isMouseOver = false;

    Sequence boxPointerEnter;
    Sequence boxPointerExit;
    Sequence boxOpen;
    Sequence boxClose;
    Sequence boxCorrect;
    Sequence boxWrong;
    Sequence boxIntro;

    public void Start()
    {
        Sprites.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
        HiddenFrame.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        state = ButtonState.None;
        Intro();
    }

    public void SetSprite(Sprite sprite)
    {
        ButtonSprite.sprite = sprite;   
    }
    public void Update()
    {
        if (IsChosen) state = ButtonState.Chosen;

        if (false)// && (state == ButtonState.None) && isMouseOver)
        {
            var mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            mousePos.x = Mathf.Clamp01(mousePos.x);
            mousePos.y = Mathf.Clamp01(mousePos.y);
            mousePos.z = 0;
            //mousePos = mousePos * 2 - Vector3.one;
            var boxPos = Camera.main.ScreenToViewportPoint(Collider.position);
            var pos = boxPos - mousePos;
            pos /= GetWidth(Collider) / (Screen.width / 1f);
            pos *= 30;

            HiddenFrame.DORotate(new Vector3(-pos.y, pos.x, 0), 0.2f)
                .SetEase(Ease.OutQuint)
                .Play()
                .OnComplete(() =>
                {
                    if (!isMouseOver)
                    {
                        HiddenFrame.DORotate(new Vector3(0, 0, 0), 0.2f)
                            .Play();
                    }
                });
        }
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
            Wrong();
        if (Input.GetKeyDown(KeyCode.U))
            Correct();
        if (Input.GetKeyDown(KeyCode.I))
            Intro();
#endif
    }

    public void Intro()
    {
        if (state == ButtonState.Intro) return;
        //float yRand = Random.Range(Screen.height / 1.5f, Screen.height / 1);
        //if (Random.Range(0, 2) == 0)
        //    yRand *= -1;
        //Sprites.localPosition = new Vector3(Sprites.localPosition.x + Random.Range(-Screen.width / 2, Screen.width / 2), 
        //                                    Sprites.localPosition.y + yRand, 
        //                                    Sprites.localPosition.z);
        Sprites.localPosition = new Vector3(Sprites.localPosition.x,// + Random.Range(-Screen.width / 2, Screen.width / 2),
                                           Sprites.localPosition.y - Screen.height * 2, 
                                           Sprites.localPosition.z);
        PlayIntroAnimation();
        state = ButtonState.Intro;

    }

    public void OnPointerClick()
    {
        if (state == ButtonState.Chosen || state == ButtonState.Intro) return;
        if (!LevelManager.Instance.GetClick(this)) return;
        PlayOpenAnimation();
        OpenedCounter++;
        state = ButtonState.Chosen;
    }

    public void OnPointerHover()
    {

        isMouseOver = true;
        if (state == ButtonState.Chosen && !shakingPlaying)// && !boxOpen.IsActive() && !boxClose.IsActive())
        {
            PlayHoverShakeAnimation();
        }
        if (state == ButtonState.Hover || 
            state == ButtonState.Chosen || 
            state == ButtonState.Intro || 
            state == ButtonState.Correct) 
            return;
        PlayPointerHoverAnimation();
        state = ButtonState.Hover;
    }

    public void OnPointerExit()
    {
        /*
        if (isMouseOver && (state == ButtonState.None || state == ButtonState.Chosen))
        {
            HiddenFrame.DORotate(new Vector3(0, 0, 0), 0.2f)
                .Play();
        }
        */
        isMouseOver = false;
        if (state == ButtonState.Chosen || 
            state == ButtonState.Exit || 
            state == ButtonState.Intro ||
            state == ButtonState.Correct) return;
        PlayPointerExitAnimation();
        state = ButtonState.Exit;
    }
    
    void CorrectComplete()
    {
        Sprites.gameObject.SetActive(false);
        this.enabled = false;
    }
    public void Correct()
    {
        if (state != ButtonState.Chosen || 
            state == ButtonState.Intro) return;
        PlayCorrectAnimation();
        if (transform.parent != null)
        {
            if (transform.parent.TryGetComponent<GridScript>(out var grid))
            {
                grid.ShakeGridZ(0.4f, 3, 20);
            }
        }
        Completed = true;
        state = ButtonState.Correct;
    }

    public void Wrong()
    {
        if (state != ButtonState.Chosen || 
            state == ButtonState.Intro) return;
        PlayWrongAnimation(); 
        if (transform.parent != null)
        {
            if (transform.parent.TryGetComponent<GridScript>(out var grid))
            {
                grid.ShakePosX(0.4f, 6, 10);
            }
        }
        state = ButtonState.Wrong;
    }

    float GetWidth(RectTransform rt)
    {
        var w = (rt.anchorMax.x - rt.anchorMin.x) * Screen.width + rt.sizeDelta.x * rt.gameObject.GetComponentInParent<Canvas>().scaleFactor;
        return w * transform.localScale.x;
    }

    #region Animations

    private void PlayIntroAnimation()
    {
        HiddenFrame.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        boxIntro = DOTween.Sequence(this);

        const float timeMul = 1f;
        const float childDelay = 15f; // magic number

        boxIntro.SetDelay(transform.GetSiblingIndex() / childDelay);
        boxIntro.Append(
            Sprites.DOLocalMove(Vector3.zero, 0.6f * timeMul)
            .SetEase(Ease.OutQuad)
            );
        boxIntro.Join(
            HiddenFrame.DORotate(new Vector3(0, 0, 360), 1.5f * timeMul)
            .SetEase(Ease.OutExpo)
            .SetRelative(true));
        boxIntro.Join(
            HiddenFrame.DOScale(0, 0.1f * timeMul)
            .SetEase(Ease.OutSine)
            .OnComplete(() =>
            {
                HiddenFrame.localScale = new Vector3(-1, 1, 1);
                HiddenFrame.DOScale(1, 0.8f * timeMul).SetEase(Ease.InOutBack).Play();
            }));
        boxIntro.AppendInterval(transform.parent.childCount / childDelay);

        // Play open Animation
        boxIntro.Append(
            HiddenFrame.DORotate(new Vector3(0, 90, 0), OpenCloseAnimation / 2)
            .SetEase(Ease.OutCubic)
            .OnComplete(() => HiddenFrame.gameObject.SetActive(false)));
        //animation.Join(HiddenFrame.DOShakeRotation(0.2f, new Vector3(0, 0f, 20f), 20));
        boxIntro.Append(
            Sprites.DOLocalRotate(new Vector3(0, 0, 0), OpenCloseAnimation / 2)
            .SetEase(Ease.OutCubic));
        boxIntro.Join(
            Sprites.DOScale(1, ExitAnimation)
                .SetEase(Ease.OutQuint)
        );
        boxIntro.AppendInterval(transform.parent.childCount / childDelay);
        boxIntro.Append(
            Sprites.DOLocalRotate(new Vector3(0, 90, 0), OpenCloseAnimation / 2)
            .SetEase(Ease.OutCubic)
            .OnComplete(() => HiddenFrame.gameObject.SetActive(true)));
        //animation.Join(Sprites.DOShakeRotation(OpenCloseAnimation / 3, new Vector3(0, 0f, 40f), 20));
        boxIntro.Append(
            HiddenFrame.DORotate(new Vector3(0, 0, 0), OpenCloseAnimation / 2)
            .SetEase(Ease.OutCubic));

        boxIntro.OnComplete(SetState);
        boxIntro.Play();
    }

    private void PlayCorrectAnimation()
    {
        if (!IsChosen) return;

        boxCorrect = DOTween.Sequence(this);
        boxCorrect.Append(
            Sprites.DOShakeRotation(0.3f, new Vector3(0f, 0, 20), 32)
            .SetEase(Ease.InSine)
            );
        boxCorrect.Join(
            Sprites.DOScale(1.4f, 0.2f)
            .SetEase(Ease.OutBack)
            );
        boxCorrect.AppendInterval(0.2f);
        boxCorrect.Append(
            Sprites.DOLocalMoveY(Sprites.position.y + Screen.height * 2, 1f)
            .SetEase(Ease.InOutBack)
            );
        boxCorrect.Join(
            Sprites.DOScale(new Vector3(0.5f, 3f, 1f), 0.6f).SetDelay(0.3f)
            .SetEase(Ease.OutQuart)
            );
        //boxCorrect.Append(
        //    Sprites.DOScale(1, 0.2f)
        //    .SetEase(Ease.OutQuint)
        //    );
        //animation.Append(
        //    Sprites.DORotate(new Vector3(0, 0, 20f), 0.4f)
        //    .SetEase(Ease.OutBounce));
        boxCorrect.OnComplete(CorrectComplete);
        boxCorrect.Play();
    }

    private void PlayWrongAnimation()
    {
        Sprites.DOShakePosition(ShakeAnimation, new Vector3(25, 0), 20)
            .Play()
            .OnComplete(() =>
            {
                PlayResetAnimation();
            });
    }

    private void PlayResetAnimation()
    {
        PlayCloseAnimation();
    }

    private void PlayOpenAnimation()
    {
        // Flipping Animation

        boxOpen = DOTween.Sequence(this);
        boxOpen.Append(
            HiddenFrame.DORotate(new Vector3(0, 90, 0), OpenCloseAnimation / 2)
            .SetEase(Ease.OutCubic)
            .OnComplete(() => HiddenFrame.gameObject.SetActive(false)));
        //animation.Join(HiddenFrame.DOShakeRotation(0.2f, new Vector3(0, 0f, 20f), 20));
        boxOpen.Append(
            Sprites.DOLocalRotate(new Vector3(0, 0, 0), OpenCloseAnimation / 2)
            .SetEase(Ease.OutCubic));
        boxOpen.Join(
            Sprites.DOScale(1, ExitAnimation)
                .SetEase(Ease.OutQuint)
        );
        boxOpen.Play();

        // Setting Card state
        IsChosen = true;
    }

    private void PlayCloseAnimation()
    {
        boxClose = DOTween.Sequence(this);

        boxClose.Append(
                Sprites.DOLocalRotate(new Vector3(0, 90, 0), OpenCloseAnimation / 2)
                .SetEase(Ease.OutCubic)
                .OnComplete(() => HiddenFrame.gameObject.SetActive(true)));
        //animation.Join(Sprites.DOShakeRotation(OpenCloseAnimation / 3, new Vector3(0, 0f, 40f), 20));
        boxClose.Append(
            HiddenFrame.DORotate(new Vector3(0, 0, 0), OpenCloseAnimation / 2)
            .SetEase(Ease.OutCubic));

        boxClose.OnComplete(SetState);
        boxClose.Play();
        IsChosen = false;
    }

    private bool shakingPlaying = false;
    private void PlayHoverShakeAnimation()
    {
        if (!shakingPlaying)
        {
            if (state == ButtonState.Chosen)
            {
                shakingPlaying = true;
                Sprites.DOShakeRotation(0.2f, new Vector3(0f, 0, 10), 20)
                    .Play()
                    .OnComplete(() => shakingPlaying = false);
            }
            else
            {
                shakingPlaying = true;
                Sprites.DOShakeRotation(0.2f, new Vector3(10f, 0, 0), 20)
                    .Play()
                    .OnComplete(() => shakingPlaying = false);
            }
        }
    }

    private void PlayPointerHoverAnimation()
    {
        // Shaking
        PlayHoverShakeAnimation();
        boxPointerEnter = DOTween.Sequence(this);
        boxPointerEnter.Append(
            Sprites.DOScale(1.2f, HoverAnimation)
                .SetEase(Ease.OutQuint)
        );
        boxPointerEnter.Append(
            Collider.DOScale(1.25f, HoverAnimation)
                .SetEase(Ease.OutQuint)
        );
        boxPointerEnter.OnComplete(SetState);
        boxPointerEnter.Play();
    }

    private void PlayPointerExitAnimation()
    {
        if (state == ButtonState.Chosen) return;

        boxPointerExit = DOTween.Sequence(this);

        boxPointerExit.Join(
            Sprites.DOScale(1, ExitAnimation)
                .SetEase(Ease.OutQuint)
        );
        boxPointerExit.Join(
            Collider.DOScale(1, ExitAnimation)
                .SetEase(Ease.OutQuint)
        );
        boxPointerExit.OnComplete(SetState);
        boxPointerExit.Play();
    }

    private void SetState()
    {
        if (IsChosen)
            state = ButtonState.Chosen;
        else
            state = ButtonState.None;
    }
    IEnumerator WaitForLastAnimation(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (this != null)
            state = ButtonState.None;
    }
    #endregion
}
