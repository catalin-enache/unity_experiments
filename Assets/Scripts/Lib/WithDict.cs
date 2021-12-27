using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Experiments.Lib
{
    [Serializable] public class DictionaryOfStringAndString : SerializableDict<string, string> {}
    public class WithDict : MonoBehaviour
    {
        public DictionaryOfStringAndString Dict = new DictionaryOfStringAndString();
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