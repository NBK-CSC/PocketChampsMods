using System;
using Windows.Abstractions.Core;
using Windows.Abstractions.Models;
using UnityEngine;

namespace Windows
{
    [RequireComponent(typeof(Canvas))]
    public class Window : BaseWindow
    {
        private Canvas _canvas;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }
        
        public override int Layer 
        {
            get => _canvas.sortingOrder;
            set => _canvas.sortingOrder = value;
        }
        
        public override void Open(Action onOpenCallback = null)
        {
            gameObject.SetActive(true);
            IsOpened = true;
            onOpenCallback?.Invoke();
        }
        
        public override void Close(Action onCloseCallback = null)
        {
            gameObject.SetActive(false);
            IsOpened = false;
            onCloseCallback?.Invoke();
        }

        public override void Visit(IOrderLayerSetter orderLayerSetter)
        {
            orderLayerSetter.Set(this);
        }
        
        public override void Visit(IOrderLayerRevoker orderLayerSetter)
        {
            orderLayerSetter.Revoke(this);
        }
    }
}