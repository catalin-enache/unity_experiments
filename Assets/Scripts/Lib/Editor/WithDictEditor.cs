

namespace Experiments.Lib.Editor
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(WithDictStringString))]
    public class WithDictEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (((WithDictStringString) target).Dict.isEditMode)
            {
                if (GUILayout.Button("Save Changes"))
                {
                    ((WithDictStringString) target).Dict.isEditMode = false;
                }
            }
            if (GUILayout.Button("Debug Print"))
            {
                ((WithDictStringString) target).Dict.DebugPrint();
            }
        }
    }
}
