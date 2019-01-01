using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace curve
{
    [CustomEditor(typeof(BezierCurveRenderer)), CanEditMultipleObjects]
    public class BezierCurveEditor : Editor
    {
       
        private void OnSceneGUI()
        {
            BezierCurveRenderer bcr = (BezierCurveRenderer)target;
            bcr.CheckLineRenderer();

            EditorGUI.BeginChangeCheck();
            Vector3 newTargetPosition = Handles.PositionHandle(bcr.transform.TransformPoint(bcr.p0), Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(bcr, "Change Look At Target Position");
                bcr.p0 = bcr.transform.InverseTransformPoint(newTargetPosition);
                bcr.UpdatePositions();
            }
        }
    }
}