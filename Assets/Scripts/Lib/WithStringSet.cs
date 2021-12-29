using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Experiments.Lib
{
    [Serializable]
    public class StringSet : SerializableSet<string> { };
    public class WithStringSet : MonoBehaviour
    {
        public StringSet Set = new StringSet();
        void OnGUI()
        {
            // This is for debugging.
            foreach (var _entry in Set.Set)
            {
                // GUILayout.Label("Entry: " + _entry);
            }
        }
    }
}
