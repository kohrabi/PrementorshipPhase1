using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class WinScreenAnimator : MonoBehaviour
{
    public void OnEnable()
    {
        transform.localPosition = new Vector3(0, Screen.height * 2, 0);

        PlayComeinAnimation();
        
    }

    public void PlayComeinAnimation()
    {
        var animation = DOTween.Sequence(this);
        animation.Append(
            transform.DOLocalMove(Vector3.zero, 0.3f)
                .SetEase(Ease.Linear)
        );
        animation.Append(
            transform.DOPunchRotation(new Vector3(0, 0, 5), 0.4f, 10)
        );
        animation.Join(
            transform.DOPunchPosition(new Vector3(20, 20, 0), 0.4f, 10)
        );
        animation.Play();
        const float delayShow = 0.2f;
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).transform;
            var ogScale = child.localScale;
            child.localScale = Vector3.zero;
            child.DOScale(ogScale, 0.2f)
                .SetDelay(0.2f * i + animation.Duration() + delayShow)
                .SetEase(Ease.OutBack)
                .Play();
        }
    }
}
