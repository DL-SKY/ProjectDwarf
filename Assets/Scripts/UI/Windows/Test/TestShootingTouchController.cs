using Lean.Touch;
using UnityEngine;

namespace ProjectDwarf.UI.Windows.Test
{
    public class TestShootingTouchController : MonoBehaviour
    {
        [SerializeField] private TestShootingProgressBar progress;

        private float progressMaxValue;


        private void Start()
        {
            progressMaxValue = Screen.height / 1.5f;
        }

        private void Update()
        {
            var fingers = LeanTouch.Fingers;

            if (fingers.Count == 1 && fingers[0].ScreenDelta.y > 0.0f)
            {
                var delta = Mathf.InverseLerp(0.0f, progressMaxValue, fingers[0].ScreenDelta.y);
                progress.AddFillAmount(delta);
            }
        }


        public void ResetProgress()
        {
            progress.FillAmount = 0.0f;
        }

        public float GetProgress()
        {
            return progress.FillAmount;
        }
    }
}
