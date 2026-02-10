using UnityEngine;

namespace ExorcistGame.CameraTool
{
    public class CameraTarget : MonoBehaviour
    {
        private void Start()
        {
            CameraController.Instance.SetTarget(transform);
        }
    }
}
