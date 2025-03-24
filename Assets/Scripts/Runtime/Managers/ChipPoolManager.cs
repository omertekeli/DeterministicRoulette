using System;
using System.Collections.Generic;
using Runtime.Enums;
using Runtime.Keys;
using Runtime.Models;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Managers
{
    public class ChipPoolManager: MonoBehaviour
    {

        #region Self Variables

        #region Serializefield Variables
        
        [SerializeField] private GameObject chipPrefab;
        [SerializeField] private int initialPoolSize = 30;
        
        #endregion

        #region Private Variables

        private ChipTypes _currentChipType;
        private Dictionary<ChipTypes, Queue<GameObject>> _chipPools = new Dictionary<ChipTypes, Queue<GameObject>>();
        private List<GameObject> _activeChips = new List<GameObject>();

        #endregion
        
        #endregion
        
        private void Awake()
        {
            InitializePool();
        }

        private void OnEnable() => SubscribeEvents();
        
        private void OnDisable() => UnsubscribeEvents();

        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onPlaceBet -= OnPlaceBet;
            UISignals.Instance.onClearBets -= OnClearBets;
            UISignals.Instance.onChooseChip -= OnChooseChip;
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onPlaceBet += OnPlaceBet;
            UISignals.Instance.onClearBets += OnClearBets;
            UISignals.Instance.onChooseChip += OnChooseChip;
        }

        private void OnChooseChip(ChipParams arg0)
        {
            _currentChipType = arg0.ChipType;
        }

        private void OnClearBets()
        {
            foreach (var chip in _activeChips)
            {
                var chipType = chip.GetComponent<Chip>().ChipType;
                DespawnChip(chipType, chip);
            }
            _activeChips.Clear();
        }
        
        private void OnPlaceBet(BetParams betParams)
        {
            SpawnChip(_currentChipType, betParams.ColliderTransform.position, betParams.ColliderTransform.rotation);
        }

        private void InitializePool()
        {
            foreach (ChipTypes chipType in System.Enum.GetValues(typeof(ChipTypes)))
            {
                _chipPools[chipType] = new Queue<GameObject>();
                for (int i = 0; i < initialPoolSize; i++)
                {
                    GameObject chip = Instantiate(chipPrefab, transform);
                    chip.SetActive(false);
                    _chipPools[chipType].Enqueue(chip);
                }
            }
        }

        private void SpawnChip(ChipTypes chipType, Vector3 position, Quaternion rotation)
        {
            if (_chipPools[chipType].Count > 0)
            {
                GameObject chip = _chipPools[chipType].Dequeue();
                chip.transform.position = position;
                chip.transform.rotation = rotation;
                chip.SetActive(true);
                _activeChips.Add(chip);
            }
            else
            {
                GameObject newChip = Instantiate(chipPrefab, position, rotation);
                _activeChips.Add(newChip);
            }
        }

        private void DespawnChip(ChipTypes chipType, GameObject chip)
        {
            chip.SetActive(false);
            _chipPools[chipType].Enqueue(chip);
        }   
    }
}