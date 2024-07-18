using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ScoreTextAnimator : MonoBehaviour
{
    TMP_Text scoreText;

    // Start is called before the first frame update
    void Awake()
    {
        scoreText = GetComponent<TMP_Text>();
        transform.localScale = Vector3.zero;

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
            transform.DOScale(1, 0.6f)
            .SetEase(Ease.OutQuart)
        );
        animation.Join(
            transform.DOPunchRotation(new Vector3(0, 0, 10), 0.6f, 20)
        );

        animation.AppendInterval(0.6f);
        /*
        animation.Append(
            transform.DOMoveY(transform.position.y + Screen.height * 2, 1f)
            .SetEase(Ease.OutQuart)
            );
        */
        animation.Play();
        animation.OnComplete(() => Destroy(gameObject));
    }
}
