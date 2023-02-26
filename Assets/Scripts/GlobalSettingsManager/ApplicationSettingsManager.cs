using UnityEngine;

namespace GlobalSettingsManager
{
    public class ApplicationSettingsManager : MonoBehaviour
    {
        [SerializeField] private int _targetFrameRate = 120;
    
        private void Awake()
        {
            Application.targetFrameRate = _targetFrameRate;
        }
    }
}
