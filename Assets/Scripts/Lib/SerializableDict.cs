using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// https://stackoverflow.com/questions/36194178/unity-serialized-dictionary-index-out-of-range-after-12-items

namespace Experiments.Lib
{
    [Serializable]
    public class SerializableDict<TKey, TValue> : ISerializationCallbackReceiver
    {
        [SerializeField] public List<TKey> _keys = new List<TKey>();
        [SerializeField] public List<TValue> _values = new List<TValue>();
        public Dictionary<TKey, TValue> Dict = new Dictionary<TKey, TValue>();
        public bool isEditMode = false;

        // https://docs.unity3d.com/2019.1/Documentation/ScriptReference/ISerializationCallbackReceiver.html
        public void OnBeforeSerialize()
        {
            #if UNITY_EDITOR
            if(
               // !EditorApplication.isUpdating
               // && !EditorApplication.isCompiling
               // && !EditorApplication.isPlaying
               // _keys.Count != _values.Count
               // _keys.Count > Math.Max(Dict.Count, 1) && _keys[_keys.Count - 1].Equals(_keys[_keys.Count - 2])
               isEditMode
            ) return;
            #endif

            _keys.Clear();
            _values.Clear();
            foreach (var kvp in Dict)
            {
                _keys.Add(kvp.Key);
                _values.Add(kvp.Value);
            }
            // Debug.Log("WithDictStringString OnBeforeSerialize: " + _keys.Count + " " + _values.Count);
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
            // Debug.Log("WithDictStringString OnAfterDeserialize: " + Dict.Count);
        }

        public void DebugPrint()
        {
            string str = "";
            foreach (var pair in Dict)
            {
                str += pair.Key + ": " + pair.Value + " | ";
            }
            Debug.Log(str);
        }
    }
}