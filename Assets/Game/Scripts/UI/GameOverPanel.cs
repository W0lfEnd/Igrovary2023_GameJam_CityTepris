using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class GameOverPanel : MonoBehaviour
    {
        public UnityEvent onRestartButtonClick;

        [SerializeField] private Canvas _parentCanvas;
        [SerializeField] private TMP_Text _enemiesKilledTextLabel;
        [SerializeField] private TMP_Text _timeSurvivedTimeTextLabel;
        [SerializeField] private Button _restartButton;

        private const string _enemiesKilledTextPrefix = "Знищено ворогів: ";
        private const string _timeSurvivedTimeTextPrefix = "Часу пережито: {0}c.";
        private const string _totalScoreTextPrefix = "Total score: ";

        private void Awake() => Initialize();


        private void Initialize()
        {
            InitializeRestartButton();


            _enemiesKilledTextLabel.text = _enemiesKilledTextPrefix + GameManager.Instance.GetEnemiesDied();
            _timeSurvivedTimeTextLabel.text = _timeSurvivedTimeTextPrefix +
                                              string.Format(_timeSurvivedTimeTextPrefix,
                                                  GameManager.Instance.GetPlayTime());
        }

        private void InitializeRestartButton()
        {
            _restartButton.onClick = new Button.ButtonClickedEvent();
            _restartButton.onClick.AddListener((() =>
            {
                var scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name, LoadSceneMode.Single);
            }));
        }
    }
}