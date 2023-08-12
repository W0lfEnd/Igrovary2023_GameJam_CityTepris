using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class GameOverPanel : MonoBehaviour
    {
        public UnityEvent onRestartButtonClick;

        [SerializeField] private Canvas _parentCanvas;
        [SerializeField] private TMP_Text _enemiesKilledTextLabel;
        [SerializeField] private TMP_Text _timeSurvivedTimeTextLabel;
        [SerializeField] private TMP_Text _totalScoreTextLabel;
        [SerializeField] private Button _restartButton;

        private const string _enemiesKilledTextPrefix = "Enemies killed: ";
        private const string _timeSurvivedTimeTextPrefix = "Time survived: ";
        private const string _totalScoreTextPrefix = "Total score: ";

        private void Awake() => Initialize();

        public void Show()
        {
            Configurate();
            _parentCanvas.gameObject.SetActive(true);
        }

        private void Hide()
        {
            _parentCanvas.gameObject.SetActive(false);
            Clear();
        }

        private void Initialize()
        {
            Hide();
            InitializeRestartButton();
        }

        private void Configurate()
        {
            //parse data from static GameManager

            //_enemiesKilledTextLabel.text = _enemiesKilledTextPrefix + ;
            //_timeSurvivedTimeTextLabel.text = _timeSurvivedTimeTextPrefix + ;
            //_totalScoreTextLabel.text = _totalScoreTextPrefix + ;
        }

        private void Clear()
        {
            _enemiesKilledTextLabel.text = _enemiesKilledTextPrefix;
            _timeSurvivedTimeTextLabel.text = _timeSurvivedTimeTextPrefix;
            _totalScoreTextLabel.text = _totalScoreTextPrefix;
        }

        private void InitializeRestartButton()
        {
            _restartButton.onClick = new Button.ButtonClickedEvent();
            _restartButton.onClick.AddListener(onRestartButtonClick.Invoke);
            _restartButton.onClick.AddListener(Hide);
        }
    }
}
