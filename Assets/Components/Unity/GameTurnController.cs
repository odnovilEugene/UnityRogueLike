using System;
using RogueLike.Components.Core;
using UnityEngine;

namespace RogueLike.Components.Unity
{
    public class GameTurnController : MonoBehaviour
    {
        public static GameTurnController Instance;

        public static event Action OnTurnEnd;
        public static event Action OnLevelEnd;

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
            _ = Game.Instance;
            UnityInputController.OnKeyPressed += ProceedeTurn;
        }

        void OnDestroy()
        {
            UnityInputController.OnKeyPressed -= ProceedeTurn;
        }

        void ProceedeTurn(KeyCode key)
        {
            Game.Instance.MakeTurn(PlayerInput.InputToDirection(key));
            if (Game.Instance.IsGameOver)
            {
                MainMenu.Quit();
            }
            if (Game.Instance.LevelDone)
            {
                Game.Instance.Initialize(Game.Instance.Level % 2 == 0);
                OnLevelEnd?.Invoke();
            }
            OnTurnEnd?.Invoke();
            Debug.Log($"{Game.Instance.Player.Hp} / {Game.Instance.Player.MaxHp}");
        }
    }
}