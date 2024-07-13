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
    public float CurrentScore = 0;
    public float TargetScore = 0;


    void Update()
    {
        if (Mathf.CeilToInt(TargetScore) != Mathf.CeilToInt(CurrentScore))
        {
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
                    textAnimator.Delay = i / 5f;
                }
            }
        }
        
    }
}
