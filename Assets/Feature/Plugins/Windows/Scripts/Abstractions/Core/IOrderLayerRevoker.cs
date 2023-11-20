using Windows.Abstractions.Models;

namespace Windows.Abstractions.Core
{
    public interface IOrderLayerRevoker
    {
        public void Revoke<T>(T window) where T : BaseWindow;
    }
}