using DG.Tweening;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    public Transform curr;
    Tween shakeRotZ;
    Tween shakePosZ;

    public void ShakeGridZ(float duration, float zRot, int vibrato)
    {
        if (shakeRotZ.IsActive())
            return;
        shakeRotZ = transform.DOPunchRotation(new Vector3(0, 0, zRot), duration, vibrato)
            //.SetEase(Ease.InSine)
            .Play()
            .OnComplete(() => transform.DORotate(new Vector3(0, 0, 0), 0.2f).Play() );
    }

    public void ShakePosX(float duration, float xPos, int vibrato)
    {
        if (shakePosZ.IsActive())
            return;
        shakePosZ = transform.DOPunchPosition(new Vector3(xPos, 0, 0), duration, vibrato)
            //.SetEase(Ease.InSine)
            .Play();
    }
}
