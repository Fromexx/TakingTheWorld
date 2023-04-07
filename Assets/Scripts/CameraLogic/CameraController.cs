using Assets.Scripts.CameraLogic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private ICameraMovementController _controller;
    private void Awake()
    {
        _controller = CameraMovementFactory.Create();
    }

    // Update is called once per frame
    void Update()
    {
        _controller.UpdateCameraPosition();
        var actualPosition = _controller.GetCameraPosition();
        Debug.Log($"Swipe: {actualPosition} \nReal:{transform.position}");
        Debug.Log(actualPosition - transform.position);
        transform.position = actualPosition;
    }
}
