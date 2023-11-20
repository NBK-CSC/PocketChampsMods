using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Abstractions.Core;
using Windows.Abstractions.Models;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Windows.Core
{
    public class WindowsPool : IDisposable
    {
        private readonly IDictionary<string, BaseWindow> _windowsPool = new Dictionary<string, BaseWindow>();
        private readonly List<string> _windowsHistory = new List<string>();
        private readonly Transform _content;
        private readonly WindowLayerManager _windowLayerManager = new WindowLayerManager();

        public WindowsPool(Transform content)
        {
            _content = content;
        }

        /// <summary>
        /// Set window in the object pool
        /// </summary>
        /// <param name="windowAsset">Asset of the window</param>
        /// <param name="closePrev">Need closing previous window</param>
        public void SetWindow(WindowAsset windowAsset, bool closePrev)
        {
            if (_windowsHistory.Count > 0)
            {
                if (string.Equals(windowAsset.ID, _windowsHistory.Last()))
                    return;
                if (closePrev && _windowsPool.TryGetValue(_windowsHistory.Last(), out _))
                    CloseWindow(_windowsHistory.Last(), false);
            }

            _windowsHistory.Add(windowAsset.ID);
            if (_windowsPool.TryGetValue(windowAsset.ID, out var window) == false)
            {
                window = Object.Instantiate(windowAsset.Window, _content);
                _windowsPool.Add(windowAsset.ID, window);
            }

            window.Open(() => window.Visit((IOrderLayerSetter)_windowLayerManager));
        }
        
        /// <summary>
        /// Closing of the window per <paramref name="id"/>
        /// </summary>
        /// <param name="id">ID of the window entity</param>
        public void CloseWindow(string id, bool willOpenLast = true)
        {
            if (string.IsNullOrEmpty(id) || _windowsPool.TryGetValue(id, out var window) == false || window.IsOpened == false || window.CanClose)
            {
                return;
            }

            _windowsPool[id].Close(() =>
            {
                window.Visit((IOrderLayerRevoker)_windowLayerManager);
                if (willOpenLast)
                    OpenPrevious();
            });
        }

        private void OpenPrevious()
        {
            if (_windowsHistory.Count <= 1)
                return;

            string prevIndex = _windowsHistory[^2];
            _windowsHistory.Add(prevIndex);
            _windowsPool[prevIndex].Open(() => _windowsPool[prevIndex].Visit((IOrderLayerSetter)_windowLayerManager));
        }
        
        /// <summary>
        /// Closing all windows and clear history
        /// </summary>
        public async void ClearHistory()
        {
            while (_windowsHistory.Count > 1)
            {
                string id = _windowsHistory.Last();
                bool isClosed = !_windowsPool[id].IsOpened;

                if (!isClosed)
                {
                    _windowsPool[id].Close(() => isClosed = true);
                }

                while (!isClosed)
                {
                    await Task.Yield();
                }
            }
        }

        /// <summary>
        /// Is the window open?
        /// </summary>
        /// <param name="windowID"></param>
        /// <returns></returns>
        public bool CheckWindow(string windowID)
        {
            return _windowsHistory.Contains(windowID);
        }

        public void Dispose()
        {
            _windowsHistory.Clear();
            _windowsPool.Clear();
        }
    }
}