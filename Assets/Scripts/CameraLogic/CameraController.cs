using Assets.Scripts.CameraLogic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour, ICamera
{

    private ICameraMovementController _controller;
    private void Awake()
    {
        _controller = CameraMovementFactory.Create();
        _controller.AttachCamera(this);
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

    public GameObject MakeRaycastToMousePosition()
    {
        Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            return hit.transform.gameObject;
        }
        return null;
    }

    private void OnDestroy()
    {
        _controller.DetachCamera();
    }
}
