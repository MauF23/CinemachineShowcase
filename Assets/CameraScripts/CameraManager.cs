using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using System;
using UnityEditor;

public class CameraManager : MonoBehaviour
{
	[Tooltip("la cámara principal")]
	public CinemachineVirtualCameraBase mainVirtualCamera;

	[Tooltip("lista de cámaras virtuales de la escena")]
	public List<CinemachineVirtualCameraBase> virtualCameraList;

	[Tooltip("el controlador del jugador")]
	public ThirdPersonController playerContoller;

	[Tooltip("el cinemachineBrain, se usa para verficar si hay un blending de cámaras")]
	public CinemachineBrain cinemachineBrain;

	[Tooltip("la cámara inicial, al ejecutar el juego será la primera cámara activa")]
	public int startCamera;

	[Tooltip("la prioridad de la cámara anterior")]
	public int previousCamera { get; set; }

	[Tooltip("la prioridad de la cámara activa")]
	public int currentCamera { get; set; }

	public static CameraManager instance;

	[Tooltip("tiempo de espera para ejecutar eventos en la corutina CheckForBlendCompletion")]
	private const float STAY_TIME = 0.5f;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}

		//Asingar la prioridad de las cámaras
		AssignCameraPriority();

		//Asegurarse que la cámara activa sea la marcada por startCamera
		SwitchToCamera(startCamera);
	}

	/// <summary>
	/// Función que cambia a la camara indicada, usando la prioridad como índice de búqueda
	/// </summary>
	/// <param name="cameraId">si una cámara tiene la misma prioridad esa será la cámara activa</param>
	public void SwitchToCamera(int cameraId)
	{
		SwitchToCameraHelper(cameraId, null);
	}


	/// <summary>
	/// Función que cambia a la camara indicada, usando la prioridad como índice de búqueda
	/// </summary>
	/// <param name="cameraId">si una cámara tiene la misma prioridad esa será la cámara activa</param>
	/// <param name="blendFinishCallback">evento que se llama cuando la cámara termina de hacer su blend</param>
	public void SwitchToCamera(int cameraId, Action blendFinishCallback)
	{
		SwitchToCameraHelper(cameraId, blendFinishCallback);
	}

	/// <summary>
	/// Función que cambia a la camara indicada, usando la prioridad como índice de búqueda
	/// </summary>
	/// <param name="cameraId">si una cámara tiene la misma prioridad esa será la cámara activa</param>
	/// <param name="switchDelayTime">el tiempo que se tardará en ejecutar el cambio de cámara</param>
	/// <param name="blendFinishCallback">evento que se llama cuando la cámara termina de hacer su blend</param>
	public void SwitchToCamera(int cameraId, float switchDelayTime, Action blendFinishCallback)
	{
		StartCoroutine(SwitchCameraCouroutine(cameraId, switchDelayTime, blendFinishCallback));
	}

	/// <summary>
	/// Función "Núcleo" que encapsula los cambios de cámara con los parametros posibles de las demás funciones
	/// </summary>
	/// <param name="cameraId"></param>
	/// <param name="blendFinishCallback"></param>
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

	/// <summary>
	/// Asigna la prioridad a las cámaras en el orden que fueron asignadas a la lista, usaremos la propiedad priority
	/// Para buscar la cámara a la que queramos cambiar.
	/// </summary>
	private void AssignCameraPriority()
	{
		mainVirtualCamera.Priority = -1;

		for (int i = 0; i < virtualCameraList.Count; i++)
		{
			virtualCameraList[i].Priority = i;
		}
	}

	/// <summary>
	/// Corutina que revisa si una cámara esta en proceso de blending o no.
	/// </summary>
	/// <param name="blendFinishCallback">evento que se llama al terminar el blending</param>
	/// <returns></returns>
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

	/// <summary>
	/// Corutina que llama la función SwitchToCameraHelper con la posibilidad de añadirle un delay
	/// </summary>
	/// <param name="cameraId">si una cámara tiene la misma prioridad esa será la cámara activa</param>
	/// <param name="delayTime">el tiempo que se tardará en ejecutar el cambio de cámara</param>
	/// <param name="blendFinishCallback">evento que se llama cuando la cámara termina de hacer su blend</param>
	/// <returns></returns>
	private IEnumerator SwitchCameraCouroutine(int cameraId, float delayTime, Action blendFinishCallback)
	{
		yield return new WaitForSeconds(delayTime);
		SwitchToCameraHelper(cameraId, blendFinishCallback);
	}
}
