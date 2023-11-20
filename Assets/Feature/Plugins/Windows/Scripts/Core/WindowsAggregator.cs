using System;
using System.Linq;
using UnityEngine;

namespace Windows.Core
{
    public class WindowsAggregator : MonoBehaviour
    {
        protected static event Action<string, bool> OnOpenWindow = delegate { };
        protected static event Action<string, bool> OnCloseWindow = delegate { };

        [SerializeField] private WindowAsset[] windows;
        [SerializeField] private Transform content;
        [SerializeField] private WindowAsset startWindow;

        private WindowsPool _pool;
        
        private void Awake()
        {
            _pool = new WindowsPool(content);
            
            OnOpenWindow += OnSetWindow;
            OnCloseWindow += _pool.CloseWindow;
            _pool.SetWindow(startWindow, true);
        }

        private void OnDestroy()
        {
            OnOpenWindow -= OnSetWindow;
            OnCloseWindow -= _pool.CloseWindow;
            _pool.Dispose();
        }

        /// <summary>
        /// Open of the window per <paramref name="windowId"/>
        /// </summary>
        /// <param name="windowId">ID of the window entity</param>
        /// <param name="isClosePrev">Need of closing previous window</param>
        public static void SetWindow(string windowId, bool isClosePrev = false)
        {
            OnOpenWindow.Invoke(windowId, isClosePrev);
        }

        /// <summary>
        /// Closing of the window per <paramref name="windowId"/>
        /// </summary>
        /// <param name="windowId">ID of the window entity</param>
        /// <param name="willOpenLast">Need of opening previous window</param>
        public static void CloseWindow(string windowId, bool willOpenLast)
        {
            OnCloseWindow.Invoke(windowId, willOpenLast);
        }

        private void OnSetWindow(string windowId, bool isClosePrev)
        {
            WindowAsset windowAsset = windows.FirstOrDefault(window => window.ID == windowId);
            if (!windowAsset)
            {
                throw new ArgumentNullException("Asset window with id " + windowId +
                                                " not contains in aggregator composite!");
            }
            _pool.SetWindow(windowAsset, isClosePrev);
        }
    }
}