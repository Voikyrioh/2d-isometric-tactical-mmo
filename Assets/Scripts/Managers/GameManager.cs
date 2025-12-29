using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace DefaultNamespace.Managers
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance
        {
            get => _instance;
        }
        
        [SerializeField] private Camera mainCameraPrefab;
        [SerializeField] private Light2D lightPrefab;
        [SerializeField] private MapManager mapPrefab;
        [SerializeField] private Player playerPrefab;
        [SerializeField] private Vector2Int playerPosition;
        [SerializeField] private PlayerCursor cursorPrefab;

        public GameObject cellsContainer;
        public CameraManager cameraManager;
        public Camera mainCameraInstance;
        public Light2D lightInstance;
        public MouseManager mouseManager;
        public MapManager mapInstance;
        public PlayerCursor cursorInstance;
        public Player playerInstance;

        private void Awake()
        {
            if (_instance)
            {
                DestroyImmediate(gameObject);
            }
            _instance = this;
        }
        
        private async void Start()
        {
            await StartGame();
            await InitializeMap();
            await CreateEntities();
            await initPlayer();
            Prepare();
        }

        private async Task StartGame()
        {
            mainCameraInstance = Instantiate(mainCameraPrefab, Vector3.zero, Quaternion.identity);
            mainCameraInstance.transform.position = new Vector3(mainCameraInstance.transform.position.x, mainCameraInstance.transform.position.y, -1);
            lightInstance = Instantiate(lightPrefab, Vector3.zero, Quaternion.identity);
        }

        private async Task InitializeMap()
        {
            cellsContainer = new GameObject("CellsContainer");
            mapInstance = Instantiate(mapPrefab, Vector3.zero, Quaternion.identity);
            mapInstance.CreateCells(cellsContainer);
        }

        private async Task CreateEntities()
        {
            mapInstance.CreateEntities();
        }

        private async Task initPlayer()
        {
            cursorInstance = Instantiate(cursorPrefab, Vector3.zero, Quaternion.identity);
            playerInstance = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            cameraManager = new GameObject("CameraManager").AddComponent<CameraManager>();
            mouseManager = new GameObject("MouseManager").AddComponent<MouseManager>();
        }

        private void Prepare()
        {
            playerInstance.Init(mapInstance, cursorInstance);
            playerInstance.SetPosition(playerPosition);
        }
    }
}