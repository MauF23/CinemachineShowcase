using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerUi : MonoBehaviour
{
    public CanvasGroup mainCanvasGroup;
    public CanvasGroup interactableIconCanvas;
    private const float FADE_TIME = 0.15F;

	public static PlayerUi instance;

	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
	}

	private void Start()
	{
		mainCanvasGroup.alpha = 0;
		interactableIconCanvas.alpha = 0;
	}

	public void ToggleUI(bool toggle)
    {
        float endValue = toggle ? 1f : 0f;
        mainCanvasGroup.DOFade(endValue, FADE_TIME);
    }

	public void ToggleInteractableIcon(bool toggle)
	{
		float endValue = toggle ? 1f : 0f;
		interactableIconCanvas.DOFade(endValue, FADE_TIME);
	}
}
