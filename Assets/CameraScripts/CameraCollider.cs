using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraCollider : MonoBehaviour
{
	public bool changeOnStart = false;
	public bool changeOnEnter = true;
	public bool changeOnExit = false;

	public CameraId cameraToSwitchStart, cameraToSwitchEnter, cameraToSwitchExit;
	private CameraManager cameraManager;

	private void Start()
	{
		if (CameraManager.instance != null)
		{
			cameraManager = CameraManager.instance;

		}
		if (changeOnStart)
		{
			cameraManager.SwitchToCamera(cameraToSwitchStart.id, cameraToSwitchStart.delay, cameraToSwitchStart.onCameraSwitchEvent.Invoke);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (changeOnEnter)
		{
			cameraManager.SwitchToCamera(cameraToSwitchEnter.id, cameraToSwitchEnter.delay, cameraToSwitchEnter.onCameraSwitchEvent.Invoke);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (changeOnExit)
		{
			cameraManager.SwitchToCamera(cameraToSwitchExit.id, cameraToSwitchExit.delay, cameraToSwitchExit.onCameraSwitchEvent.Invoke);
		}
	}

	[Serializable]
	public class CameraId
	{
		public int id;
		public float delay;
		public UnityEvent onCameraSwitchEvent;
	}
}
