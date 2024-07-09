using DG.Tweening;
using System.Collections;
using UnityEngine;

public enum ButtonState
{
    None,
    Hover,
    Exit, 
    Click,
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
    [Header("Animation Time")]
    [SerializeField] public float OpenCloseAnimation = 0.8f;
    [SerializeField] public float ExitAnimation = 0.4f;
    [SerializeField] public float HoverAnimation = 0.4f;
    [SerializeField] public float ShakeAnimation = 0.4f;

    ButtonState state; // Trang thai cua button
    bool flipped = false;
    public bool IsChosen = false;
    bool resetting = false;

    public void Start()
    {
        Sprites.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
        HiddenFrame.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        state = ButtonState.None;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            PlayWrongAnimation();
        if (Input.GetKeyDown(KeyCode.U))
            PlayCorrectAnimation();
    }

    public void OnPointerClick()
    {
        state = ButtonState.Click;
        PlayPointerClickAnimation();
    }

    public void OnPointerHover()
    {
        state = ButtonState.Hover;
        PlayPointerHoverAnimation();
    }

    public void OnPointerExit()
    {
        state = ButtonState.Exit;
        PlayPointerExitAnimation();
    }

    public void Correct()
    {
        state = ButtonState.Correct;
        PlayCorrectAnimation();
    }

    public void Wrong()
    {
        state = ButtonState.Wrong;
        PlayWrongAnimation();
    }

    #region Animations
    private void PlayCorrectAnimation()
    {
        if (!IsChosen) return;
        Sequence animation = DOTween.Sequence();
        animation.SetTarget(this);
        animation.Append(
            Sprites.DORotate(new Vector3(0, 0, 370), 0.8f)
            .SetEase(Ease.OutBack)
            .SetRelative());
        animation.Join(
            Sprites.DOScale(1.3f, 0.2f)
            .SetEase(Ease.OutBack)
            .OnComplete(() => Sprites.DOScale(1.2f, 0.2f).SetEase(Ease.OutBack).Play() ));
        animation.Append(
            Sprites.DOScale(1, 0.2f)
            .SetEase(Ease.OutQuint)
            );
        //animation.Append(
        //    Sprites.DORotate(new Vector3(0, 0, 20f), 0.4f)
        //    .SetEase(Ease.OutBounce));
        animation.AppendInterval(0);
        animation.SetAutoKill(true);
        animation.Play();
        StartCoroutine(WaitForLastAnimation(animation.Duration()));
    }

    private void PlayWrongAnimation()
    {
        if (!IsChosen) return;
        state = ButtonState.Wrong;
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
        PlayPointerClickAnimation();
    }

    private void PlayPointerClickAnimation()
    {
        // Flipping Animation
        Sequence animation = DOTween.Sequence();
        animation.SetTarget(this);

        if (flipped) // Play close Animation
        {
            animation.Append(
                Sprites.DOLocalRotate(new Vector3(0, 90, 0), OpenCloseAnimation / 2)
                .SetEase(Ease.OutCubic)
                .OnComplete(() => HiddenFrame.gameObject.SetActive(true)));
            //animation.Join(Sprites.DOShakeRotation(OpenCloseAnimation / 3, new Vector3(0, 0f, 40f), 20));
            animation.Append(
                HiddenFrame.DORotate(new Vector3(0, 0, 0), OpenCloseAnimation / 2)
                .SetEase(Ease.OutCubic));
        }
        else // Play open animation
        {
            animation.Append(
                HiddenFrame.DORotate(new Vector3(0, 90, 0), OpenCloseAnimation / 2)
                .SetEase(Ease.OutCubic)
                .OnComplete(() => HiddenFrame.gameObject.SetActive(false)));
            //animation.Join(HiddenFrame.DOShakeRotation(0.2f, new Vector3(0, 0f, 20f), 20));
            animation.Append(
                Sprites.DOLocalRotate(new Vector3(0, 0, 0), OpenCloseAnimation / 2)
                .SetEase(Ease.OutCubic));
        }
        StartCoroutine(WaitForLastAnimation(animation.Duration()));
        animation.AppendInterval(0);
        animation.SetAutoKill(true);
        animation.Play();
        // Setting Card state
        flipped = !flipped;
        IsChosen = !IsChosen;
        // Transitioning to the 
        if (IsChosen)
            PlayPointerHoverAnimation(); // Show as selected
        else
            PlayPointerExitAnimation(); // Show as deselected
    }

    private void PlayPointerHoverAnimation()
    {
        Sprites.DOShakeRotation(0.2f, new Vector3(10f, 0, 10f), 20)
            .Play();
        Sprites.DOScale(1.2f, HoverAnimation)
            .SetEase(Ease.OutQuint)
            .Play();
        StartCoroutine(WaitForLastAnimation(1.2f));
    }

    private void PlayPointerExitAnimation()
    {
        if (IsChosen) return;
        Sprites.DORotate(new Vector3(0, Sprites.rotation.eulerAngles.y, Sprites.rotation.eulerAngles.z), 0.2f)
            .Play();
        Sprites.DOScale(1, ExitAnimation)
            .SetEase(Ease.OutQuint)
            .Play();
        StartCoroutine(WaitForLastAnimation(1));
    }

    IEnumerator WaitForLastAnimation(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (this != null)
            state = ButtonState.None;
    }
    #endregion
}
