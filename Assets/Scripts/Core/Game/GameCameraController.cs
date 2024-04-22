using System;
using Unity.VisualScripting;
using UnityEngine;

public class GameCameraController 
{
    // Attributes
    private Camera activeCamera;
    private CameraBehaviour behaviour;
    private CameraBehaviourType behaviourType;

    // Set-Getters
    public Transform Orientation => activeCamera.transform;

    // Constructor
    public GameCameraController(Camera camera)
    {
        activeCamera = camera;
        activeCamera.enabled = true;
    }

    // Functions
    public void SwapCamera(Camera camera)
    {
        activeCamera.enabled = false;
        activeCamera = camera;
        activeCamera.enabled = true;
    }

    public void ResetCameraBehaviour()
    {
        GameObject.Destroy(activeCamera.GetComponent<CameraBehaviour>());
        behaviourType = CameraBehaviourType.NULL;
        behaviour = null;
    }

    public void SetCameraBehaviour(CameraBehaviourType cameraBehaviourType)
    {
        GameObject.Destroy(activeCamera.GetComponent<CameraBehaviour>());

        behaviour = cameraBehaviourType switch
        {
            CameraBehaviourType.STATIC => activeCamera.AddComponent<CameraStatic>(),
            CameraBehaviourType.FOLLOW => activeCamera.AddComponent<CameraFollowObject>(),
            CameraBehaviourType.MOUSE => activeCamera.AddComponent<CameraMouse>(),
            _ => throw new Exception("Invalid cameraBehaviourType set, please refer to enum CameraBehaviourType for valid types")
        };

        behaviourType = cameraBehaviourType;
    }
}