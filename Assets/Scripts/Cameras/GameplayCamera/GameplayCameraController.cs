using ProjectDwarf.Constants;
using ProjectDwarf.Events;
using ProjectDwarf.Services;
using ProjectDwarf.Tools.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectDwarf.Cameras.GameplayCamera
{
    public class GameplayCameraController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Vector2 horizontalBorder;
        [SerializeField] private Vector2 verticalBorder;
        [SerializeField] private float speed;
        [SerializeField] private Vector2 zoomRange;
        [SerializeField] private float zoomSpeed;

        [Header("Links")]
        [SerializeField] new private Camera camera;

        private Vector2 direction;
        private float zoomDelta;
        private float baseOffset;


        private void OnEnable()
        {
            if (camera == null)
                camera = GetComponent<Camera>();

            baseOffset = camera?.orthographicSize ?? 0.0f;

            ComponentLocator.Register(this);

            EventManager.DispatchEvent(ConstantEventsName.ON_GAMEPLAY_CAMERA_ENABLE, true);
        }

        private void OnDisable()
        {
            ComponentLocator.Unregister(this.GetType());

            EventManager.DispatchEvent(ConstantEventsName.ON_GAMEPLAY_CAMERA_ENABLE, false);
        }

        private void LateUpdate()
        {
            MoveCamera();
            ZoomCamera();
        }


        public void SetBorders(Vector2 _horizontalBorder, Vector2 _verticalBorder)
        {
            horizontalBorder = _horizontalBorder;
            verticalBorder = _verticalBorder;
        }

        public void SetDirection(Vector2 _direction)
        {
            direction = _direction;
        }

        public void SetZoomDelta(float _zoomDelta)
        {
            zoomDelta = _zoomDelta;
        }


        private void MoveCamera()
        {
            var newPosition = transform.position + new Vector3(direction.x, direction.y, 0.0f) * speed * Time.deltaTime;
            newPosition = new Vector3(  Mathf.Clamp(newPosition.x, horizontalBorder.x + baseOffset, horizontalBorder.y - baseOffset),
                                        Mathf.Clamp(newPosition.y, verticalBorder.x + baseOffset, verticalBorder.y),
                                        newPosition.z);

            transform.position = newPosition;

            direction = Vector2.zero;
        }

        private void ZoomCamera()
        {
            if (camera == null)
                return;

            var newZoom = camera.orthographicSize + zoomDelta * zoomSpeed * Time.deltaTime;
            newZoom = Mathf.Clamp(newZoom, zoomRange.x, zoomRange.y);

            baseOffset = camera.orthographicSize = newZoom;            

            zoomDelta = 0.0f;
        }
    }
}
