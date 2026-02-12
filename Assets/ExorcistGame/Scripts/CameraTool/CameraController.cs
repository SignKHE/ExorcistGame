using UnityEngine;

namespace ExorcistGame.CameraTool
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private float distance = 15f;

        private readonly Vector3 _camDirection = new Vector3(0f, 1f, -1f).normalized;

        private Transform _target;
        
        public static CameraController Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            cam =  GetComponent<Camera>();
        }

        private void Update()
        {
            if(!_target) return;
            
            cam.transform.position = _target.position + _camDirection * distance;
            
            cam.transform.rotation = Quaternion.LookRotation(_target.position - cam.transform.position);
        }

        public void SetTarget(Transform targetTransform)
        {
            _target = targetTransform;
        }

        private void OnDestroy()
        {
            if(Instance == this) Instance = null;
        }
    }
}
