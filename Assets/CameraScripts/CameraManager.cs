using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using System;
using UnityEditor;

public class CameraManager : MonoBehaviour
{
	public CinemachineVirtualCameraBase mainVirtualCamera;
	public List<CinemachineVirtualCameraBase> virtualCameraList;
	public ThirdPersonController playerContoller;
	public CinemachineBrain cinemachineBrain;
	public int startCamera;
	public int previousCamera { get; set; }
	public int currentCamera { get; set; }

	public static CameraManager instance;

	private const float STAY_TIME = 0.5f;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}

		AssignCameraPriority();
		SwitchToCamera(startCamera);
	}


	public void SwitchToCamera(int cameraId)
	{
		SwitchToCameraHelper(cameraId, null);
	}

	public void SwitchToCamera(int cameraId, Action blendFinishCallback)
	{
		SwitchToCameraHelper(cameraId, blendFinishCallback);
	}

	public void SwitchToCamera(int cameraId, float switchDelayTime, Action blendFinishCallback)
	{
		StartCoroutine(SwitchCameraCouroutine(cameraId, switchDelayTime, blendFinishCallback));
	}

	private void SwitchToCameraHelper(int cameraId, Action blendFinishCallback)
	{
		previousCamera = currentCamera;

		for (int i = 0; i < virtualCameraList.Count; i++)
		{
			if (cameraId == virtualCameraList[i].Priority)
			{
				virtualCameraList[i].gameObject.SetActive(true);
				currentCamera = i;
			}
			else
			{
				virtualCameraList[i].gameObject.SetActive(false);
			}
		}

		if(blendFinishCallback != null)
		{
			StartCoroutine(CheckForBlendCompletion(blendFinishCallback));
		}

	}

	private void AssignCameraPriority()
	{
		mainVirtualCamera.Priority = -1;

		for (int i = 0; i < virtualCameraList.Count; i++)
		{
			virtualCameraList[i].Priority = i;
		}
	}

	private IEnumerator CheckForBlendCompletion(Action blendFinishCallback)
	{
		yield return new WaitForEndOfFrame();

		while (cinemachineBrain.ActiveBlend != null)
		{
			Debug.Log("Blending in progress");
			yield return null;
		}

		Debug.Log("Blending finished");

		yield return new WaitForSeconds(STAY_TIME);
		blendFinishCallback?.Invoke();
	}
	private IEnumerator SwitchCameraCouroutine(int cameraId, float delayTime, Action blendFinishCallback)
	{
		yield return new WaitForSeconds(delayTime);
		SwitchToCameraHelper(cameraId, blendFinishCallback);
	}
}
