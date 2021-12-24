using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Experiments.Lib
{
    public class WithDict : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField] private List<string> _keys = new List<string>();
        [SerializeField] private List<string> _values = new List<string>();

        // Unity doesn't know how to serialize a Dictionary
        public Dictionary<string, string> Dict = new Dictionary<string, string>();

        public void OnBeforeSerialize()
        {
            if(!EditorApplication.isPlaying
               && !EditorApplication.isUpdating
               && !EditorApplication.isCompiling) return;

            _keys.Clear();
            _values.Clear();
            foreach (var kvp in Dict)
            {
                _keys.Add(kvp.Key);
                _values.Add(kvp.Value);
            }
            // Debug.Log("WithDict OnBeforeSerialize: " + _keys.Count + " " + _values.Count);
        }

        public void OnAfterDeserialize()
        {
            Dict.Clear();
            for (int i = 0; i != Math.Min(_keys.Count, _values.Count); i++)
            {
                if(!Dict.ContainsKey(_keys[i])) {
                    Dict.Add(_keys[i], _values[i]);
                };
            }
            // Debug.Log("WithDict OnAfterDeserialize: " + Dict.Count);
        }

        void OnGUI()
        {
            // This is for debugging.
            // foreach (var kvp in Dict)
            // {
            //     GUILayout.Label("Key: " + kvp.Key + " value: " + kvp.Value);
            // }
        }
    }
}