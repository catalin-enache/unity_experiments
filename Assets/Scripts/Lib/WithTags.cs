using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Experiments.Lib
{
    [Serializable]
    public class StringSet : SerializableSet<string> { };
    public class WithTags : MonoBehaviour
    {
        public StringSet Tags = new StringSet();
        void OnGUI()
        {
            // This is for debugging.
            foreach (var _tag in Tags.Set)
            {
                // GUILayout.Label("Tag: " + _tag);
            }
        }
    }
}
