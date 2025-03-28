﻿using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Enums;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Controllers.UI
{
    public class UIPanelController: MonoBehaviour
    {
        #region Self Variables

        #region SerializeField Variables
            [SerializeField] private List<Transform> layers = new List<Transform>();
        #endregion
        
        #endregion
        
        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreUISignals.Instance.onClosePanel += OnClosePanel;
            CoreUISignals.Instance.onOpenPanel += OnOpenPanel;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        private void UnSubscribeEvents()
        {
            CoreUISignals.Instance.onClosePanel -= OnClosePanel;
            CoreUISignals.Instance.onOpenPanel -= OnOpenPanel;
        }
        
        private void OnOpenPanel(UIPanelTypes panelType, int value)
        {
            if (layers.Count < value) return;
            OnClosePanel(value);
            Instantiate(Resources.Load<GameObject>($"Screens/{panelType}Panel"), layers[value]);
        }
        
        private void OnClosePanel(int value)
        {
            if (layers.Count < value) return;
            if (layers[value].childCount <= 0) return;

#if UNITY_EDITOR
            DestroyImmediate(layers[value].GetChild(0).gameObject);
#else
                Destroy(layers[value].GetChild(0).gameObject);
#endif
        }
        
        private void OnCloseAllPanels()
        {
            foreach (var layer in layers)
            {
                if (layer.childCount <= 0) return;
#if UNITY_EDITOR
                DestroyImmediate(layer.GetChild(0).gameObject);
#else
                Destroy(layer.GetChild(0).gameObject);
#endif
            }
        }
    }
    
}