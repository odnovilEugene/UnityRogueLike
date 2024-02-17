using System;
using UnityEngine;

namespace RogueLike.Components.Unity
{
    public class UnityInputController : MonoBehaviour
    {
        public static UnityInputController Instance;
        private Array AllKeyCodes { get; set;}
        public static event Action<KeyCode> OnKeyPressed;
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
            AllKeyCodes = Enum.GetValues(typeof(KeyCode));
        }

        void Update()
        {
            foreach(KeyCode kcode in AllKeyCodes)
            {
                if (Input.GetKeyDown(kcode))
                {
                    OnKeyPressed?.Invoke(kcode);
                }
            }
        }
    }
}