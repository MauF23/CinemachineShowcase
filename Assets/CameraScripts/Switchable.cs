using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Switchable : MonoBehaviour
{
    private const float TWEEN_TIME = 0.5f;
	public Transform target, tweenEndPosition;

    public void Switch()
    {
        target.DOMove(tweenEndPosition.position, TWEEN_TIME);
    }
}
