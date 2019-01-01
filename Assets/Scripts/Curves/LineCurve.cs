
using System;
using System.Collections.Generic;
using UnityEngine;

namespace curve
{
    public class LineCurve : ICurve
    {
        protected const double InterpolationPrecision = 0.001;

        //Start 
        Vector3 p0;
        //End
        Vector3 p1;

        public LineCurve(Vector3 p0, Vector3 p1)
        {
            this.p0 = p0;
            this.p1 = p1;
        }

        public float Length
        {
            get
            {
                return Vector3.Distance(p0, p1);
            }
        }



        public Vector3 GetPointAt(float t)
        {
            return Vector3.Lerp(p0, p1, t);
        }

        // calculate  tangent
        public Vector3 Derivative(float t)
        {
            return (p1-p0).normalized;
        }

        public List<Vector3> GetPoints(int divisions)
        {
            List<Vector3> points = new List<Vector3>();
            points.Add(p0);
            points.Add(p1);
            return points;
        }

        public void GetPoints(int divisions, List<Vector3> points)
        {
            points.Add(p0);
            points.Add(p1);
        }
    }
}