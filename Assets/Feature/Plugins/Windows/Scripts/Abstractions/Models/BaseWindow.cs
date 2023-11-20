using System;
using Windows.Abstractions.Core;
using UnityEngine;

namespace Windows.Abstractions.Models
{
    public abstract class BaseWindow : MonoBehaviour
    {
        [SerializeField] private bool canClose;

        public abstract int Layer { get; set; }
        public bool IsOpened { get; protected set; }

        public bool CanClose => canClose;
        public string Name => name + "_window";

        public abstract void Open(Action onOpenCallback = null);
        
        public abstract void Close(Action onCloseCallback = null);

        public abstract void Visit(IOrderLayerSetter orderLayerSetter);

        public abstract void Visit(IOrderLayerRevoker orderLayerSetter);
    }
}