using DG.Tweening;
using System.Collections;
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
    [SerializeField] public float ExitAnimation = 0.4f;
    [SerializeField] public float HoverAnimation = 0.4f;
    [SerializeField] public float ShakeAnimation = 0.4f;
    
    ButtonState state; // Trang thai cua button
    public bool IsChosen = false;
    public bool Completed = false;
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

        if ((state == ButtonState.None) && isMouseOver)
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
        PlayIntroAnimation();
        state = ButtonState.Intro;

    }

    public void OnPointerClick()
    {
        if (state == ButtonState.Chosen || state == ButtonState.Intro) return;
        DOTween.Complete(HiddenFrame);
        DOTween.Complete(Sprites);
        PlayOpenAnimation();
        state = ButtonState.Chosen;
    }

    public void OnPointerHover()
    {
        isMouseOver = true;
        if (state == ButtonState.Chosen && !boxClose.IsActive())
        {
            PlayHoverShakeAnimation();
        }
        if (state == ButtonState.Hover || 
            state == ButtonState.Chosen || 
            state == ButtonState.Intro) return;
        PlayPointerHoverAnimation();
        state = ButtonState.Hover;
    }

    public void OnPointerExit()
    {
        if (isMouseOver && (state == ButtonState.None || state == ButtonState.Chosen))
        {
            HiddenFrame.DORotate(new Vector3(0, 0, 0), 0.2f)
                .Play();
        }
        isMouseOver = false;
        if (state == ButtonState.Chosen || 
            state == ButtonState.Exit || 
            state == ButtonState.Intro) return;
        PlayPointerExitAnimation();
        state = ButtonState.Exit;
    }

    public void Correct()
    {
        if (state != ButtonState.Chosen || 
            state == ButtonState.Intro) return;
        PlayCorrectAnimation();
        Completed = true;
        state = ButtonState.Correct;
    }

    public void Wrong()
    {
        if (state != ButtonState.Chosen || 
            state == ButtonState.Intro) return;
        PlayWrongAnimation();
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

        boxIntro.Append(
            HiddenFrame.DORotate(new Vector3(0, 0, 360), 1.5f)
            .SetEase(Ease.OutExpo)
            .SetRelative(true));
        boxIntro.Join(
            HiddenFrame.DOScale(0, 0.1f)
            .SetEase(Ease.OutSine)
            .OnComplete(() =>
            {
                HiddenFrame.localScale = new Vector3(-1, 1, 1);
                HiddenFrame.DOScale(1, 0.8f).SetEase(Ease.InOutBack).Play();
            }));

        boxIntro.OnComplete(SetState);
        boxIntro.Play();
    }

    private void PlayCorrectAnimation()
    {
        if (!IsChosen) return;

        boxCorrect = DOTween.Sequence(this);
        boxCorrect.Append(
            Sprites.DORotate(new Vector3(0, 0, 370), 0.8f)
            .SetEase(Ease.OutExpo)
            .SetRelative());
        boxCorrect.Join(
            Sprites.DOScale(1.3f, 0.3f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => Sprites.DOScale(1f, 0.4f).SetDelay(0.3f).SetEase(Ease.OutBack).Play()));
        //boxCorrect.Append(
        //    Sprites.DOScale(1, 0.2f)
        //    .SetEase(Ease.OutQuint)
        //    );
        //animation.Append(
        //    Sprites.DORotate(new Vector3(0, 0, 20f), 0.4f)
        //    .SetEase(Ease.OutBounce));
        boxCorrect.OnComplete(SetState);
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
        if (state == ButtonState.Chosen || boxPointerEnter.IsActive())
        {
            return;
        }
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
