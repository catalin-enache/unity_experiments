using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// https://stackoverflow.com/questions/36194178/unity-serialized-dictionary-index-out-of-range-after-12-items

namespace Experiments.Lib
{
    public class SerializableSet<TValue> : ISerializationCallbackReceiver
    {
        [SerializeField] private List<TValue> _set = new List<TValue>();
        public HashSet<TValue> Set = new HashSet<TValue>();


        // https://docs.unity3d.com/2019.1/Documentation/ScriptReference/ISerializationCallbackReceiver.html
        public void OnBeforeSerialize()
        {
            #if UNITY_EDITOR
            if(
            // !EditorApplication.isUpdating
            // && !EditorApplication.isCompiling
            // && !EditorApplication.isPlaying
            // if new item has been added in inspector (we know that new item is a clone of last item)
            _set.Count > Math.Max(Set.Count, 1) && _set[_set.Count - 1].Equals(_set[_set.Count - 2])
            ) return;
            #endif

            _set.Clear();
            foreach(var aTag in Set)
            {
                _set.Add(aTag);
            }
            // Debug.Log("OnBeforeSerialize: " + _set.Count);
        }
        
        public void OnAfterDeserialize()
        {
            Set.Clear();
            foreach(var aTag in _set)
            {
                Set.Add(aTag);
            }
            // Debug.Log("OnAfterDeserialize: " + Set.Count);
        }
        
    }
}