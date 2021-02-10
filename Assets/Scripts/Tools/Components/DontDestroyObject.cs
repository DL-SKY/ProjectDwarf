using UnityEngine;

namespace ProjectDwarf.Tools.Components
{
    public class DontDestroyObject : MonoBehaviour
    {
        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
