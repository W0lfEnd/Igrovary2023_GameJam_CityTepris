using Enemies;
using Game.Scripts.World;
using UnityEngine;

namespace Game.Scripts
{
    public class WorldSwapper : MonoBehaviour
    {
        [SerializeField] private GameObject _swapBtn;
        [SerializeField] private WorldAnimSwapper _worldAnimSwapper;
        public Dimension DimensionType { get; private set; }

        private void Start()
        {
            DimensionType = Dimension.TopDimesion;
            _worldAnimSwapper.OnStartSwap += StartSwap;
            _worldAnimSwapper.OnCompleteSwap += EndSwap;
        }

        public void SwapWorld()
        {
            if (DimensionType == Dimension.TopDimesion)
            {
                DimensionType = Dimension.BottomDimension;
                _worldAnimSwapper.SwapToDown();
            }
            else if(DimensionType==Dimension.BottomDimension)
            {
                DimensionType = Dimension.TopDimesion;
                _worldAnimSwapper.SwapToUp();
            }
        }

        private void StartSwap()
        {
            _swapBtn.SetActive(false);
        }

        private void EndSwap()
        {
            _swapBtn.SetActive(true);
        }
    }
}