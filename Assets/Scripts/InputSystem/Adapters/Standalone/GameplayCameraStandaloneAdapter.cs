using ProjectDwarf.Cameras.GameplayCamera;
using ProjectDwarf.Constants;
using ProjectDwarf.Events;
using ProjectDwarf.Services;
using UnityEngine;


namespace ProjectDwarf.InputSystem.Adapters
{
    public class GameplayCameraStandaloneAdapter : InputAdapter
    {
        private GameplayCameraController camera;

        public GameplayCameraStandaloneAdapter(bool _enabled) : base(_enabled)
        {
            EventManager.AddEventListener(ConstantEventsName.ON_GAMEPLAY_CAMERA_ENABLE, OnGameplayCameraEnableHandler);

            camera = ComponentLocator.Resolve<GameplayCameraController>();
            if (camera == null)
                SetEnable(false);
        }


        protected override void CustomUpdate()
        {
            var x = 0.0f;
            var y = 0.0f;
            var speedBoost = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ? 2.0f : 1.0f;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                y += 1.0f;

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                y += -1.0f;

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                x += -1.0f;

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                x += 1.0f;

            camera?.SetDirection(new Vector2(x, y) * speedBoost);


            var zoomDelta = -Input.GetAxis("Mouse ScrollWheel");
            camera?.SetZoomDelta(zoomDelta * speedBoost);
        }


        private void OnGameplayCameraEnableHandler(CustomEvent _event)
        {
            var state = (bool)_event.EventData;

            if (state && camera == null)
                camera = ComponentLocator.Resolve<GameplayCameraController>();

            SetEnable(state);
        }
    }
}
