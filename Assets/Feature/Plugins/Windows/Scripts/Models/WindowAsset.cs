using Windows.Abstractions.Models;
using UnityEngine;

namespace Windows
{
    /// <summary>
    /// Asset describing the entity of the window
    /// </summary>
    [CreateAssetMenu(menuName = "Plugins/Windows/" + nameof(WindowAsset), fileName = nameof(WindowAsset), order = 51)]
    public class WindowAsset : ScriptableObject
    {
        [SerializeField] private BaseWindow window;

        /// <summary>
        /// ID of the window
        /// </summary>
        public string ID => window.Name;
        
        /// <summary>
        /// Entity of the window
        /// </summary>
        public BaseWindow Window => window;
    }
}