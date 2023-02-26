using GameStateMachine.GameStates;
using Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class GameStateChangeButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private GameStateType _targetStateType;

        private SignalBus _signalBus;
        private OnGameStateChangeButtonClick _onButtonClickEvent;

        [Inject]
        private void Init(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        private void OnValidate()
        {
            if (_button == null)
                _button = GetComponentInChildren<Button>();
        }

        private void Awake()
        {
            _onButtonClickEvent = new OnGameStateChangeButtonClick(_targetStateType);
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            _signalBus.Fire(_onButtonClickEvent);
        }
    }
}