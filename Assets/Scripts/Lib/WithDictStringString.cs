using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Experiments.Lib
{
    [Serializable] public class DictStringString : SerializableDict<string, string> {}
    public class WithDictStringString : MonoBehaviour
    {
        public DictStringString Dict = new DictStringString();
        void OnGUI()
        {
            // This is for debugging.
            foreach (var kvp in Dict.Dict)
            {
                // GUILayout.Label("Key: " + kvp.Key + " value: " + kvp.Value);
            }
        }
    }
}