using System.Collections.Generic;
using RogueLike.Components.Core;
using RogueLike.Components.MovingGameObject;
using RogueLike.Components.StaticObjects;
using RogueLike.Settings;
using UnityEngine;

namespace RogueLike.Components.Unity
{
    public class UnityRenderer : MonoBehaviour
    {
        public static UnityRenderer Instance;

        [HideInInspector] public List<UnityEngine.GameObject> ObjectList { get; private set; } = new();
        [HideInInspector] public List<UnityEngine.GameObject> BackgroundObjectList { get; private set; } = new();

        [Space]
        [Header("Клетки")]
        public UnityEngine.GameObject wallPrefab;
        public UnityEngine.GameObject emptyPrefab;
        public UnityEngine.GameObject firstAidKitPrefab;

        // [Space]
        // [Header("Ключевые объекты")]
        // public UnityEngine.GameObject exitPrefab;

        [Space]
        [Header("Игрок")]
        public UnityEngine.GameObject playerPrefab;

        [Space]
        [Header("Враги")]
        public UnityEngine.GameObject zombiePrefab;
        public UnityEngine.GameObject shooterPrefab;
        public UnityEngine.GameObject projectilePrefab;

        [HideInInspector] public Core.GameObject[,] CurrentField { get; private set; }

        private void CreateSingleton()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
                Destroy(gameObject);

            // DontDestroyOnLoad(gameObject);
        }
        void Awake()
        {
            CreateSingleton();
            GameTurnController.OnLevelEnd += RenderBackground;
            GameTurnController.OnTurnEnd += Render;
        }

        void Start()
        {
            Core.GameObject[,] field = Game.Instance.GetRenderData();
            if (field != null)
            {
                CurrentField = field;
            }

            Camera cam = Camera.main;
            float camHalfHeight = cam.orthographicSize / 2;
            float camHalfWidth = cam.aspect * camHalfHeight;
            cam.transform.position += new Vector3(camHalfWidth, camHalfHeight, 0);

            RenderBackground();
            Render();
        }

        void OnDestroy()
        {
            GameTurnController.OnTurnEnd -= Render;
            GameTurnController.OnLevelEnd -= RenderBackground;
        }

        void UpdateMap()
        {
            Core.GameObject[,] field = Game.Instance.GetRenderData();
            CurrentField = CurrentField != field ? field : CurrentField;
        }

        private void Render()
        {
            ClearObjects();

            UpdateMap();

            
            (float, float) rotation = (0, 0);

            for(int i = 0; i < MapSettings.Width; i++)
            {
                for(int j = 0; j < MapSettings.Height; j++)
                {   
                    switch (CurrentField[i, j])
                    {    
                        case Player player:
                            CameraMoveTo(player.Position);
                            _ = InstantiateGameObject(playerPrefab, player.Position, rotation, ObjectList);
                            break;
                        case Zombie zombie:
                            _ = InstantiateGameObject(zombiePrefab, zombie.Position, rotation, ObjectList);
                            break;
                        case Shooter shooter:
                            _ = InstantiateGameObject(shooterPrefab, shooter.Position, rotation, ObjectList);
                            break;
                        case Projectile projectile:
                            _ = InstantiateGameObject(projectilePrefab, projectile.Position, rotation, ObjectList);
                            break;
                        case FirstAidKit fak: // :)))))))))
                            _ = InstantiateGameObject(firstAidKitPrefab, fak.Position, rotation, ObjectList);
                            break;
                    }
                }
            }
        }

        private void RenderBackground()
        {
            ClearBackgroundObjects();

            UpdateMap();

            (float, float) rotation = (0, 0);

            for(int i = 0; i < MapSettings.Width; i++)
            {
                for(int j = 0; j < MapSettings.Height; j++)
                {
                    _ = CurrentField[i, j] switch
                    {
                        Wall wall => InstantiateGameObject(wallPrefab, wall.Position, rotation, BackgroundObjectList),
                        _ => InstantiateGameObject(emptyPrefab, CurrentField[i, j].Position, rotation, BackgroundObjectList),
                    };
                }
            }    
        }

        private void ClearObjects()
        {
            
            foreach(var go in ObjectList)
            {
                Destroy(go);
            }
            ObjectList.Clear();
        }

        private void ClearBackgroundObjects()
        {
            
            foreach(var go in BackgroundObjectList)
            {
                Destroy(go);
            }
            BackgroundObjectList.Clear();
        }

        private void CameraMoveTo(Position2D pos)
        {
            Camera cam = Camera.main;

            bool posChanged = cam.transform.position.x != pos.X || cam.transform.position.y != pos.Y;
            if (posChanged)
            {
                cam.transform.position = new Vector3(pos.X, pos.Y, -10);
            }
        }

        private UnityEngine.GameObject InstantiateGameObject(UnityEngine.GameObject prefab, Position2D pos, (float, float) rotation, List<UnityEngine.GameObject> store)
        {
            (float x, float y) = rotation;

            var obj = Instantiate(prefab, new Vector3(pos.X, pos.Y, 0), Quaternion.Euler(x, y, 0));
            store.Add(obj);
            return obj;
        }

        private UnityEngine.GameObject InstantiateGameObject(UnityEngine.GameObject prefab, Position2D pos)
        {
            var obj = Instantiate(prefab, new Vector3(pos.X, pos.Y, 0), Quaternion.Euler(0, 45, 0));
            return obj;
        }
    }
}