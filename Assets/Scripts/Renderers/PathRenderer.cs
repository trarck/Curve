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
        int m_SegmentCountPerCurve = 10;

        void Awake()
        {
            if (m_Path == null)
            {
                m_Path = GetComponent<Path>();
            }

            m_Path.AddPoint(new Point(Point.JoinType.Line, new Vector3(0, 0, 0)));
            m_Path.AddPoint(new Point(Point.JoinType.Line, new Vector3(5,0, 5)));
            m_Path.UpdateCurves();
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
            List<Vector3> positions = m_Path.GetPoints(m_SegmentCountPerCurve);
            TransformPositions(positions);
            lineRenderer.positionCount = positions.Count;
            lineRenderer.SetPositions(positions.ToArray());
        }


        void TransformPositions(List<Vector3> positions)
        {

            Matrix4x4 matrix = transform.localToWorldMatrix;
            for(int i = 0; i < positions.Count; ++i)
            {
                positions[i] = matrix.MultiplyPoint(positions[i]);
            }
        }

        public void CheckLineRenderer()
        {
            m_lineRenderer = GetComponent<LineRenderer>();
            if (m_lineRenderer == null)
            {
                m_lineRenderer = gameObject.AddComponent<LineRenderer>();
            }
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