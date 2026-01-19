using UnityEngine;

namespace DefaultNamespace.Managers
{
    public class CameraManager : MonoBehaviour
    {
        private bool shouldFollow(Vector3 target)
        {
            return true;
        }

        private bool inLimits(Vector3 position, Vector3 target, float margin)
        {
            var relativePosition = position - target;
            var orthographicSize = GameManager.Instance.mainCameraInstance.orthographicSize * margin;
            var inLimitsY = relativePosition.y > -orthographicSize && relativePosition.y < orthographicSize;
            var inLimitsX = relativePosition.x > -orthographicSize * GameManager.Instance.mainCameraInstance.aspect && relativePosition.x < orthographicSize * GameManager.Instance.mainCameraInstance.aspect;
            return inLimitsY && inLimitsX;
        }

        private Vector3 getTargetCameraPosition(Vector3 target)
        {
            return new Vector3(
                target.x,
                target.y,
                -1
            );
        }

        private void Start()
        {
            GameManager.Instance.mainCameraInstance.transform.position = getTargetCameraPosition(GameManager.Instance.playerInstance.transform.position);
        }
        
        private void followMouse(float deltatime)
        {
            var target = GameManager.Instance.mainCameraInstance.ScreenToWorldPoint(Input.mousePosition);
            var newPosition = Vector3.MoveTowards(
                GameManager.Instance.mainCameraInstance.transform.position,
                getTargetCameraPosition(target),
                deltatime * 1.2f
            );
            if (!inLimits(newPosition, GameManager.Instance.playerInstance.transform.position, 0.65f) || inLimits(newPosition, target, 0.75f)) return;
            GameManager.Instance.mainCameraInstance.transform.position = newPosition;

        }
        
        private void followPlayer(float deltatime)
        {
            var target = GameManager.Instance.playerInstance.transform.position;
            var newPosition = Vector3.MoveTowards(
                GameManager.Instance.mainCameraInstance.transform.position,
                getTargetCameraPosition(target),
                deltatime * GameManager.Instance.playerInstance.speed
            );
            GameManager.Instance.mainCameraInstance.transform.position = newPosition;

        }
        
        private void LateUpdate()
        {
            if (GameManager.Instance.playerInstance.isMoving)
            {
                followPlayer(Time.deltaTime);
            }
            else
            {
                followMouse(Time.deltaTime);
            }
        }
    }
}