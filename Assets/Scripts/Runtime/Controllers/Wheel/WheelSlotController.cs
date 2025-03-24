using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Controllers.Wheel
{
    public class WheelSlotController: MonoBehaviour
    {
        #region Self Variables
        
        #region Serialized Field Variables
        
        [SerializeField] private List<GameObject> slotObjects;
        
        #endregion

        #region Private Variables

        private GameObject _lastTargetSlot;
        private Dictionary<int, GameObject> _slots = new Dictionary<int, GameObject>();
        
        #endregion
        
        #endregion
        
        private void Start()
        {
            SetDictionary();
        }

        internal GameObject GetSlot(int slotNumber)
        {
            if (_slots.TryGetValue(slotNumber, out var slotObject))
            {
                if (_lastTargetSlot)
                {
                    _lastTargetSlot.SetActive(false);
                }
                slotObject.SetActive(true);
                _lastTargetSlot = slotObject;
                return slotObject;
            }
            else
            {
                Debug.LogWarning("Target Slot: " + slotNumber + " not found");
                return null;
            }
        }

        private void SetDictionary()
        {
            foreach (var slot in slotObjects)
            {
                if (int.TryParse(slot.name, out int slotNumber))
                {
                    _slots[slotNumber] = slot;
                }
            }
        }
    }
}