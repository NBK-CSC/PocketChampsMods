using Windows.Abstractions.Models;

namespace Windows.Abstractions.Core
{
    public interface IOrderLayerSetter
    {
        public void Set<T>(T window) where T : BaseWindow;
    }
}