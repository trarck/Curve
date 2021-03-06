﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace curve
{
    [ExecuteInEditMode]
    public class BezierCurveRenderer : MonoBehaviour
    {
        LineRenderer m_lineRenderer = null;
        CubicBezierCurve m_Curve = null;

        [SerializeField]
        public Vector3 p0;
        [SerializeField]
        public Vector3 p1;
        [SerializeField]
        public Vector3 p2;
        [SerializeField]
        public Vector3 p3;

        [SerializeField]
        int m_segmentCount = 10;

        void Awake()
        {
            Debug.Log("Awake");
        }

        void OnEnable()
        {
            Debug.Log("OnEnable");
            CheckLineRenderer();
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if(transform.hasChanged)
                UpdatePositions();
        }

        public void UpdatePositions()
        {
            m_Curve = new CubicBezierCurve(p0, p1, p2, p3);
            Vector3[] pp = CreatePositions().ToArray();
            m_lineRenderer.positionCount = pp.Length;
            m_lineRenderer.SetPositions(CreatePositions().ToArray());
        }

        public void CheckLineRenderer()
        {
            m_lineRenderer = GetComponent<LineRenderer>();
            if (m_lineRenderer == null)
            {
                m_lineRenderer = gameObject.AddComponent<LineRenderer>();
            }
        }

        List<Vector3> CreatePositions()
        {
            List<Vector3> positions = new List<Vector3>();

            float d = 1.0f / m_segmentCount;
            Matrix4x4 matrix = transform.localToWorldMatrix;
            for(int i = 0; i < m_segmentCount; ++i)
            {
                positions.Add(matrix.MultiplyPoint(m_Curve.GetPointAt(d * i)));
            }

            return positions;
        }
    }
}