using System.Collections.Generic;
using UnityEngine;

namespace Zone
{
    [CreateAssetMenu(fileName = "New Zone Data", menuName = "Wheel/Zone Data")]
    public class ZoneDataSO : ScriptableObject
    {
        public List<ZoneItem> ZoneItems = new();
        public int ReviveCost = 200;
    }
}