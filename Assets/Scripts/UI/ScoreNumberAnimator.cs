using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ScoreNumberAnimator : MonoBehaviour
{
    TMP_Text scoreText;
    TextAnimator textAnimator;
    Sequence animation;

    private void Awake()
    {
        scoreText = GetComponent<TMP_Text>();
        textAnimator = GetComponent<TextAnimator>();
        PlayChangeAnimation();
    }

    public void ChangeTo(string text)
    {
        if (text == scoreText.text)
            return;
        scoreText.text = text;  
        transform.localScale = new Vector3(0.4f, 0.4f, 1);
        PlayChangeAnimation();
    }

    void PlayChangeAnimation()
    {
        textAnimator.enabled = false;
        if (animation.IsActive())
            animation.Kill(false);
        animation = DOTween.Sequence(this);
        animation.Append(
            transform.DOScale(1, 0.1f)
            .SetEase(Ease.OutCubic)
            );
        animation.OnComplete(() => textAnimator.enabled = true);
        animation.Play();
    }

}
