using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Abstractions.Core;
using Windows.Abstractions.Models;
using UnityEngine;

namespace Windows.Core
{
    public class WindowLayerManager : IOrderLayerSetter, IOrderLayerRevoker
    {
        private readonly Dictionary<Type, List<int>> _orderWindows;
        private readonly IReadOnlyDictionary<Type, int> _layers = new Dictionary<Type, int>()
        {
            { typeof(Window), 100 },
        };

        public WindowLayerManager()
        {
            _orderWindows = new Dictionary<Type, List<int>>();
        }

        private bool Contains<T>()
        {
            return _orderWindows.ContainsKey(typeof(T));
        }
        
        public void Set<T>(T window) where T : BaseWindow
        {
            if (Contains<T>() == false)
                _orderWindows.Add(typeof(T), new List<int> { _layers[typeof(T)] });
            
            window.Layer = _orderWindows[typeof(T)].Last() + 1;
            _orderWindows[typeof(T)].Add(window.Layer);
        }

        public void Revoke<T>(T window) where T : BaseWindow
        {
            if (Contains<T>() == false)
            {
#if UNITY_EDITOR
                Debug.LogWarning("An attempt to cancel a window even though there was no window");
#endif
                return;
            }
            
            _orderWindows[typeof(T)].Remove(window.Layer);
            window.Layer = 0;
        }
    }
}