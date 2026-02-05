using System.Collections.Generic;
using UnityEngine;

namespace Wheel
{
    [CreateAssetMenu(fileName = "New Zone Data", menuName = "Wheel/Zone Data")]
    public class ZoneDataSO : ScriptableObject
    {
        public List<ZoneItem> ZoneItems = new List<ZoneItem>();
    }
}