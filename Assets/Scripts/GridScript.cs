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
        shakeRotZ = transform.DOShakeRotation(duration, new Vector3(0, 0, zRot), vibrato)
            .SetEase(Ease.OutQuad)
            .Play()
            .OnComplete(() => transform.DORotate(new Vector3(0, 0, 0), 0.2f).SetEase(Ease.OutQuad).Play() );
    }

    public void ShakePosX(float duration, float xPos, int vibrato)
    {
        if (shakePosZ.IsActive())
            return;
        shakePosZ = transform.DOShakePosition(duration, new Vector3(xPos, 0, 0), vibrato)
            .SetEase(Ease.OutQuad)
            .Play();
    }
}
