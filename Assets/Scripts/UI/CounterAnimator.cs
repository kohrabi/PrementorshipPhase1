using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class CounterAnimator : MonoBehaviour
{
    [SerializeField] private GameObject minusOne;
    TMP_Text counterText;
    int currentValue = 0;

    private void Start()
    {
        counterText = GetComponent<TMP_Text>(); 
    }


    // Update is called once per frame
    void Update()
    {
        if (LevelManager.Instance != null)
        {
            if (currentValue != LevelManager.Instance.MoveCounter)
            {
                currentValue = LevelManager.Instance.MoveCounter;
                PlayCountAnimation();
                if (minusOne != null)
                    Instantiate(minusOne, transform.parent).transform.position = transform.position;
            }
        }
    }
    
    void PlayCountAnimation()
    {
        var animation = DOTween.Sequence(this);
        animation.Append(
            transform.DOScale(0.2f, 0.1f)
                .SetEase(Ease.OutExpo)
                .OnComplete(() => counterText.text = currentValue.ToString() )
        );
        animation.Append(
            transform.DOScale(1, 0.2f)
            .SetEase(Ease.OutBack));
        animation.Join(
            transform.DOPunchRotation(new Vector3(0, 0, 10), 0.3f, 10)
            .SetEase(Ease.InSine)
            );

    }
}
