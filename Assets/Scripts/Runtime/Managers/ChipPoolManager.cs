using System.Collections.Generic;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Managers
{
    public class ChipPoolManager: MonoBehaviour
    {
        [SerializeField] private GameObject chipPrefab;
        [SerializeField] private int initialPoolSize = 20;

        private Dictionary<ChipTypes, Queue<GameObject>> chipPools = new Dictionary<ChipTypes, Queue<GameObject>>();

        private void Awake()
        {
            InitializePool();
        }

        private void InitializePool()
        {
            foreach (ChipTypes chipType in System.Enum.GetValues(typeof(ChipTypes)))
            {
                chipPools[chipType] = new Queue<GameObject>();
                for (int i = 0; i < initialPoolSize; i++)
                {
                    GameObject chip = Instantiate(chipPrefab, transform);
                    chip.SetActive(false);
                    chipPools[chipType].Enqueue(chip);
                }
            }
        }

        public GameObject GetChip(ChipTypes chipType, Vector3 position, Quaternion rotation)
        {
            if (chipPools[chipType].Count > 0)
            {
                GameObject chip = chipPools[chipType].Dequeue();
                chip.transform.position = position;
                chip.transform.rotation = rotation;
                chip.SetActive(true);
                return chip;
            }
            else
            {
                GameObject newChip = Instantiate(chipPrefab, position, rotation);
                return newChip;
            }
        }

        public void ReturnChip(ChipTypes chipType, GameObject chip)
        {
            chip.SetActive(false);
            chipPools[chipType].Enqueue(chip);
        }   
    }
}