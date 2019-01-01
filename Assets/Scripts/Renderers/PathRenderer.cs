using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace curve
{
    [ExecuteInEditMode]
    public class PathRenderer : MonoBehaviour
    {
        LineRenderer m_lineRenderer = null;

        [SerializeField]
        Path m_Path = null;

        [SerializeField]
        int m_segmentCount = 10;

        void Awake()
        {
            if (m_Path == null)
            {
                m_Path = GetComponent<Path>();
            }
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
            Vector3[] positions = CreatePositions().ToArray();
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

        public LineRenderer lineRenderer
        {
            get
            {
                if (m_lineRenderer == null)
                {
                    m_lineRenderer = GetComponent<LineRenderer>();
                    if (m_lineRenderer == null)
                    {
                        m_lineRenderer = gameObject.AddComponent<LineRenderer>();
                    }
                }
                return m_lineRenderer;
            }
        }
    }
}