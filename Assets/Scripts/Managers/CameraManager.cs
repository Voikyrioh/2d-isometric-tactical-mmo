using UnityEngine;

namespace DefaultNamespace.Managers
{
    public class CameraManager : MonoBehaviour
    {
        private Vector3 targetPosition;
        
        private void LateUpdate()
        {
            var diff = new Vector2(
                GameManager.Instance.mainCameraInstance.transform.position.x - GameManager.Instance.playerInstance.transform.position.x, 
                GameManager.Instance.mainCameraInstance.transform.position.y - GameManager.Instance.playerInstance.transform.position.y
                );
            if (Mathf.Abs(diff.x) >= 2 || Mathf.Abs(diff.y) >= 1)
            {
                targetPosition = new Vector3(
                    GameManager.Instance.playerInstance.transform.position.x,
                    GameManager.Instance.playerInstance.transform.position.y,
                    -1
                );
            }

            if (targetPosition != GameManager.Instance.mainCameraInstance.transform.position)
            {
                GameManager.Instance.mainCameraInstance.transform.position = Vector3.MoveTowards(
                    GameManager.Instance.mainCameraInstance.transform.position,
                    targetPosition, 
                    2f * Time.deltaTime);   
            }
        }
    }
}