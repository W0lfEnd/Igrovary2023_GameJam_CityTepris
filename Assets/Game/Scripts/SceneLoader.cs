using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private int index;

        public void LoadScene()
        {
            SceneManager.LoadScene(index, LoadSceneMode.Single);
        }
    }
}