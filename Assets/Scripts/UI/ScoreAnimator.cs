using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class ScoreAnimator : MonoBehaviour
{
    [SerializeField] GameObject num;
    [SerializeField] public float CountDuration = 1;
    [SerializeField] public float DelayBetweenScoreNum = 0.6f;
    public float CurrentScore = 0;
    public float TargetScore = 0;
    bool shaking = false;
    Tween shake;


    void Update()
    {
        if (Mathf.CeilToInt(TargetScore) != Mathf.CeilToInt(CurrentScore))
        {
            if (shaking)
                ShakeScore();
            shaking = true;
            float rate = Mathf.Abs(TargetScore - CurrentScore) / CountDuration;
            CurrentScore = Mathf.MoveTowards(CurrentScore, TargetScore, rate * Time.deltaTime);
            string scoreString = Mathf.CeilToInt(CurrentScore).ToString();
            if (scoreString.Length != transform.childCount)
            {
                if (scoreString.Length > transform.childCount)
                {
                    for (int i = transform.childCount; i < scoreString.Length; i++)
                        Instantiate(num, transform);
                }
                else
                {
                    for (int i = scoreString.Length; i > transform.childCount; i--)
                        Instantiate(num, transform);
                }
            }
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                var scoreAnimator = child.GetComponentInChildren<ScoreNumberAnimator>();
                if (scoreAnimator != null)
                {
                    scoreAnimator.ChangeTo(scoreString[i].ToString());
                }
                var textAnimator = child.GetComponentInChildren<TextAnimator>();
                if (textAnimator != null)
                {
                    textAnimator.Delay = i * DelayBetweenScoreNum;
                }
            }
        }
        else
            shaking = false;

    }
    public void ShakeScore()
    {

        if (shake.IsActive()) return;
        shake = transform.DOShakeRotation(0.2f, new Vector3(0, 0, 10), 30)
             .SetEase(Ease.InSine)
             .Play();
    }
}
