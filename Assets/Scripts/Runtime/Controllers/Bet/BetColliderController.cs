using System;
using Runtime.Enums;
using Runtime.Keys;
using Runtime.Signals;
using Unity.Mathematics;
using UnityEngine;

namespace Runtime.Controllers
{
    public class BetColliderController: MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private BetTypes betType;
        [SerializeField] private string betName;
        
        #endregion

        #endregion

        private void OnMouseDown()
        {   
            CoreGameSignals.Instance.onPlaceBet?.Invoke(new BetParams()
                {
                    BetType = betType,
                    BetName = betName,
                    ColliderTransform = transform,
                }
            );
        }
        
        public string GetBetName() => betName;
    }
}