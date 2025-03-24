using System;
using System.Collections.Generic;
using Runtime.Enums;
using Runtime.Keys;
using Runtime.Models;
using Runtime.Signals;
using Unity.VisualScripting;
using UnityEngine;

namespace Runtime.Managers
{
    public class ChipPoolManager: MonoBehaviour
    {

        #region Self Variables

        #region Serializefield Variables
        
        [SerializeField] private GameObject whiteChipPrefab;
        [SerializeField] private GameObject blueChipPrefab;
        [SerializeField] private GameObject greenChipPrefab;
        [SerializeField] private GameObject orangeChipPrefab;
        [SerializeField] private GameObject purpleChipPrefab;
        [SerializeField] private int initialPoolSize = 30;
        
        #endregion

        #region Private Variables

        private int _betAmount;
        private float _chipHeight;
        private float _chipHeightMultiplier = 1.5f;
        private ChipTypes _currentChipType;
        private Dictionary<ChipTypes, Queue<GameObject>> _chipPools = new Dictionary<ChipTypes, Queue<GameObject>>();
        private Dictionary<string, int> _activeChipAmountOnBet = new Dictionary<string, int>();

        #endregion
        
        #endregion
        
        private void Awake()
        {
            InitializePool();
        }
        
        private void OnEnable() => SubscribeEvents();
        
        private void OnDisable() => UnsubscribeEvents();

        private void Start()
        {
            _chipHeight = whiteChipPrefab.transform.Find("Mesh").transform.localScale.y * _chipHeightMultiplier;
            _betAmount = 0;
        }

        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onTurnResult -= OnTurnResult;
            CoreGameSignals.Instance.onPlaceBet -= OnPlaceBet;
            UISignals.Instance.onClearBets -= OnClearBets;
            UISignals.Instance.onChooseChip -= OnChooseChip;
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onPlaceBet += OnPlaceBet;
            UISignals.Instance.onClearBets += OnClearBets;
            UISignals.Instance.onChooseChip += OnChooseChip;
            CoreGameSignals.Instance.onTurnResult += OnTurnResult;
        }

        private void OnTurnResult(TurnResultParams arg0)
        {
            DespawnChips();
        }

        private void OnChooseChip(ChipParams arg0)
        {
            _betAmount = arg0.Amount;
            _currentChipType = arg0.ChipType;
        }

        private void OnClearBets()
        {
            _betAmount = 0;
            DespawnChips();
        }

        private void DespawnChips()
        {
            foreach (Transform chip in transform)
            {
                var chipType = chip.GetComponent<Chip>().ChipType;
                DespawnChip(chipType, chip.gameObject);
            }

            _activeChipAmountOnBet.Clear();
        }

        private void OnPlaceBet(BetParams betParams)
        {
            if (_betAmount > 0)
            {
                SpawnChip(_currentChipType, betParams);
            }
        }

        private void InitializePool()
        {
            foreach (ChipTypes chipType in System.Enum.GetValues(typeof(ChipTypes)))
            {
                _chipPools[chipType] = new Queue<GameObject>();
                GameObject prefab = GetPrefabByType(chipType);
                for (int i = 0; i < initialPoolSize; i++)
                {
                    GameObject chip = Instantiate(prefab, transform);
                    chip.SetActive(false);
                    _chipPools[chipType].Enqueue(chip);
                }
            }
        }

        private GameObject GetPrefabByType(ChipTypes chipType)
        {
            return chipType switch
            {
                ChipTypes.White => whiteChipPrefab,
                ChipTypes.Blue => blueChipPrefab,
                ChipTypes.Green => greenChipPrefab,
                ChipTypes.Orange => orangeChipPrefab,
                ChipTypes.Purple => purpleChipPrefab,
                _ => null
            };
        }

        private Vector3 GetSpawnOffset(BetParams betParams)
        {
            if (!_activeChipAmountOnBet.ContainsKey(betParams.BetName))
            {
               return Vector3.zero;
            }
            else
            {
                var yOffset = _chipHeight / 2 + _chipHeight * (_activeChipAmountOnBet[betParams.BetName]);
                return new Vector3(0, yOffset, 0);
            }
        }
        
        private void SpawnChip(ChipTypes chipType, BetParams betParams)
        {
            var offset = GetSpawnOffset(betParams);
            var position = betParams.ColliderTransform.position + offset;
            var rotation = betParams.ColliderTransform.rotation;
            if (_chipPools[chipType].Count > 0)
            {
                GameObject chip = _chipPools[chipType].Dequeue();
                AddActiveChipOnBet(betParams);
                chip.transform.position = position;
                chip.transform.rotation = rotation;
                chip.SetActive(true);
            }
            else
            {
                GameObject chipPrefab = GetPrefabByType(chipType);
                GameObject newChip = Instantiate(chipPrefab, position, rotation);
                AddActiveChipOnBet(betParams);
            }
        }

        private void AddActiveChipOnBet(BetParams betParams)
        {
            if (!_activeChipAmountOnBet.ContainsKey(betParams.BetName))
            {
                _activeChipAmountOnBet[betParams.BetName] = 1;

            }
            else
            {
                _activeChipAmountOnBet[betParams.BetName] += 1;
            }
        }

        private void DespawnChip(ChipTypes chipType, GameObject chip)
        {
            chip.SetActive(false);
            _chipPools[chipType].Enqueue(chip);
        }   
    }
}