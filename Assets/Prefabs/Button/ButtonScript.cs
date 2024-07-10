using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum ButtonState
{
    None,
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
    [SerializeField] public Image ButtonSprite;
    [Header("Animation Time")]
    [SerializeField] public float OpenCloseAnimation = 0.8f;
    [SerializeField] public float ExitAnimation = 0.4f;
    [SerializeField] public float HoverAnimation = 0.4f;
    [SerializeField] public float ShakeAnimation = 0.4f;
    
    ButtonState state; // Trang thai cua button
    public bool IsChosen = false;
    public bool Completed = false;
    bool resetting = false;

    Sequence boxPointerEnter;
    Sequence boxPointerExit;
    Sequence boxOpen;
    Sequence boxClose;
    Sequence boxCorrect;
    Sequence boxWrong;

    public void Start()
    {
        Sprites.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
        HiddenFrame.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        state = ButtonState.None;
    }

    public void SetSprite(Sprite sprite)
    {
        ButtonSprite.sprite = sprite;   
    }

    public void Update()
    {
        if (IsChosen) state = ButtonState.Chosen;

        if (Input.GetKeyDown(KeyCode.Space))
            Wrong();
        if (Input.GetKeyDown(KeyCode.U))
            Correct();
    }

    public void OnPointerClick()
    {
        if (state == ButtonState.Chosen) return;
        PlayOpenAnimation();
        state = ButtonState.Chosen;
    }

    public void OnPointerHover()
    {
        if (state == ButtonState.Chosen)
        {
            PlayHoverShakeAnimation();
        }
        if (state == ButtonState.Hover || 
            state == ButtonState.Chosen) return;
        PlayPointerHoverAnimation();
        state = ButtonState.Hover;
    }

    public void OnPointerExit()
    {
        if (state == ButtonState.Chosen || 
            state == ButtonState.Exit) return;
        PlayPointerExitAnimation();
        state = ButtonState.Exit;
    }

    public void Correct()
    {
        if (state != ButtonState.Chosen) return;
        PlayCorrectAnimation();
        Completed = true;
        state = ButtonState.Correct;
    }

    public void Wrong()
    {
        if (state != ButtonState.Chosen) return;
        state = ButtonState.Wrong;
        PlayWrongAnimation();
    }

    #region Animations
    private void PlayCorrectAnimation()
    {
        if (!IsChosen) return;

        boxCorrect = DOTween.Sequence(this);
        boxCorrect.Append(
            Sprites.DORotate(new Vector3(0, 0, 370), 0.8f)
            .SetEase(Ease.OutBack)
            .SetRelative());
        boxCorrect.Join(
            Sprites.DOScale(1.3f, 0.2f)
            .SetEase(Ease.OutBack)
            .OnComplete(() => Sprites.DOScale(1.2f, 0.2f).SetEase(Ease.OutBack).Play()));
        boxCorrect.Append(
            Sprites.DOScale(1, 0.2f)
            .SetEase(Ease.OutQuint)
            );
        //animation.Append(
        //    Sprites.DORotate(new Vector3(0, 0, 20f), 0.4f)
        //    .SetEase(Ease.OutBounce));
        boxCorrect.OnComplete(SetState);
        boxCorrect.Play();
    }

    private void PlayWrongAnimation()
    {
        resetting = true;
        Sprites.DOShakePosition(ShakeAnimation, new Vector3(25, 0), 20)
            .Play()
            .OnComplete(() =>
            {
                resetting = false;
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
