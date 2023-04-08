using Assets.Scripts.CameraLogic;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour, ICamera
{
    [SerializeField] private GraphicRaycaster _canvasRaycaster;
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
        //Debug.Log($"Swipe: {actualPosition} \nReal:{transform.position}");
        //Debug.Log(actualPosition - transform.position);
        transform.position = actualPosition;
    }

    public GameObject MakeRaycastToMousePosition()
    {
        var eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        var result = new List<RaycastResult>();
        _canvasRaycaster.Raycast(eventData, result);
        Debug.Log(result.ToArray().ToString());
        if (result.Count > 0)
            return result[0].gameObject;
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
