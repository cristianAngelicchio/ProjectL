using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Reflector))]
public class ReflectorInspector : Editor
{
    public override void OnInspectorGUI()
    {
        Reflector reflector = (Reflector)target;
        reflector.Rotate(reflector.Orientation.RotateClockwise());
    }
}
