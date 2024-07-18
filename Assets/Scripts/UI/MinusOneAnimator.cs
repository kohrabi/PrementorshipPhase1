using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinusOneAnimator : MonoBehaviour
{
    RectTransform rectTransform;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        PlayAnimation();
    }

    public void OnDestroy()
    {
        DOTween.Kill(transform);    
        DOTween.Kill(this);    
    }

    void PlayAnimation()
    {
        var animation = DOTween.Sequence(this);
        animation.Append(
            transform.DOLocalMoveY(rectTransform.rect.height, 0.3f)
            .SetEase(Ease.OutBack)
        );
        animation.Join(
            transform.DOPunchRotation(new Vector3(0, 0, 10), 0.3f)
            .SetEase(Ease.InSine)
        );
        animation.AppendInterval(0.8f);
        animation.Append(
            transform.DOLocalMoveY(transform.position.y + Screen.height * 2.0f, 1f)
            .SetEase(Ease.InSine)
            );
        animation.Join(
            transform.DOScaleY(1.5f, 0.2f)
            .SetEase(Ease.InQuart)
            );
        animation.OnComplete(() => Destroy(gameObject));
    }

}
