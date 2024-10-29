using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraCollider : MonoBehaviour
{
	[Tooltip("si la cámara cambia en Start")]
	public bool changeOnStart = false;

	[Tooltip("si la cámara cambia en TriggerEnter")]
	public bool changeOnEnter = true;

	[Tooltip("si la cámara cambia en TriggerExit")]
	public bool changeOnExit = false;


	[Tooltip("el id/prioridad de la cámara a cambiar, con la posibilidad de especificar delay y evento al terminar el cambio")]
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
