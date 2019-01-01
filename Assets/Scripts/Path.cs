using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace curve
{
    public class Path : MonoBehaviour
    {
        List<Point> m_Points = new List<Point>();
        List<ICurve> m_Curves = new List<ICurve>();
        // Start is called before the first frame update

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateCurves()
        {
            if (m_Points.Count >= 2)
            {
                m_Curves.Clear();

                Point from = m_Points[0];

                for(int i = 1, l = m_Points.Count; i < l; ++i)
                {
                    Point to = m_Points[i];

                    if (from.joinType==Point.JoinType.Line && to.joinType == Point.JoinType.Line)
                    {
                        m_Curves.Add(new LineCurve(from.position, to.position));
                    }
                    else if(from.joinType==Point.JoinType.Line && to.joinType != Point.JoinType.Line)
                    {
                        m_Curves.Add(new QuadraticBezierCurve(from.position, to.controlOne, to.position));
                    }
                    else if (from.joinType != Point.JoinType.Line && to.joinType == Point.JoinType.Line)
                    {
                        m_Curves.Add(new QuadraticBezierCurve(from.position, from.controlTwo, to.position));
                    }
                    else
                    {
                        m_Curves.Add(new CubicBezierCurve(from.position, from.controlTwo,to.controlOne, to.position));
                    }

                    from = to;
                }
            }
        }

        public ICurve GetCurveAt(float distance,out float t)
        {
            float searched = 0;
            float last = 0;
            for(int i = 0, l = m_Curves.Count; i < l; ++i)
            {
                last = searched;
                searched += m_Curves[i].Length;
                if (distance <= searched)
                {
                    t = distance - last;
                    return m_Curves[i];
                }
            }
            t = -1;
            return null;
        }

        public List<Vector3> GetPoints(int divisions)
        {
            List<Vector3> positions = new List<Vector3>();
            for (int i = 0, l = m_Curves.Count; i < l; ++i)
            {
                m_Curves[i].GetPoints(divisions,positions);
            }
            return positions;
        }

        public Vector3 GetPointAt(float distance)
        {
            float t = 0;
            ICurve curve = GetCurveAt(distance, out t);
            if (curve != null)
            {
                return curve.GetPointAt(t);
            }
            return Vector3.zero;
        }

        public void AddPoint(Point p)
        {
            m_Points.Add(p);
        }

        public void RemovePoint(int index)
        {
            m_Points.RemoveAt(index);
        }

        public List<ICurve> curves
        {
            get
            {
                return m_Curves;
            }
        }

        public List<Point> points
        {
            get
            {
                return m_Points;
            }
        }
    }
}