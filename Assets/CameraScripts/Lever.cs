using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using StarterAssets;
using UnityEngine.InputSystem.XR;
using System;

public class Lever : MonoBehaviour
{
    public Transform leverPivot;
    public Vector3 endRotation;
	public KeyCode triggerKey;
	public ThirdPersonController controller;
	public Switchable switchable;
	public int cameraToSwitch;
	private CameraManager cameraManager;
	private PlayerUi playerUi;

    private bool playerOnRange;
    private bool active;

	private void Start()
	{
		cameraManager = CameraManager.instance;
		playerUi = PlayerUi.instance;
	}

	void Update()
    {
        if(playerOnRange && Input.GetKeyDown(triggerKey) && !active)
		{
			Action blendFinishCallback = () =>
			{
				switchable.Switch();
				cameraManager.SwitchToCamera(cameraManager.previousCamera, 2, ()=> controller.BlockMovement(false));
			};

			controller.BlockMovement(true);
			leverPivot.DOLocalRotate(endRotation, 0.5f).OnComplete(()=> cameraManager.SwitchToCamera(cameraToSwitch, blendFinishCallback.Invoke));
			active = true;
			ToggleRange(false);
		}
    }

	private void OnTriggerEnter(Collider other)
	{
		ToggleRange(true);
	}

	private void OnTriggerExit(Collider other)
	{
		ToggleRange(false);
	}

	private void ToggleRange(bool value)
	{
		playerOnRange = value;
		playerUi.ToggleInteractableIcon(value);
	}
}
