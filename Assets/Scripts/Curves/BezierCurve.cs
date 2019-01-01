
using System;
using System.Collections.Generic;
using UnityEngine;

namespace curve
{
    public class QuadraticBezierCurve : ICurve
    {
        protected const double InterpolationPrecision = 0.001;

        //Start 
        Vector3 p0;
        //Control
        Vector3 p1;
        //End
        Vector3 p2;

        public QuadraticBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2)
        {
            this.p0 = p0;
            this.p1 = p1;
            this.p2 = p2;
        }

        public float Length
        {
            get
            {
                if (p0 == p1)
                {
                    if (p0 == p1) return 0.0f;
                    return (p0 - p1).magnitude;
                }
                if (p1 == p0 || p1 == p2) return (p0 - p2).magnitude;
                Vector3 A0 = p1 - p0;
                Vector3 A1 = p0 - 2.0f * p1 + p2;
                if (A1 != Vector3.zero)
                {
                    double c = 4.0 * Vector3.Dot(A1, A1);
                    double b = 8.0 * Vector3.Dot(A0, A1);
                    double a = 4.0 * Vector3.Dot(A0, A0);
                    double q = 4.0 * a * c - b * b;
                    double twoCpB = 2.0 * c + b;
                    double sumCBA = c + b + a;
                    var l0 = (0.25 / c) * (twoCpB * Math.Sqrt(sumCBA) - b * Math.Sqrt(a));
                    if (q == 0.0) return (float)l0;
                    var l1 = (q / (8.0 * Math.Pow(c, 1.5))) * (Math.Log(2.0 * Math.Sqrt(c * sumCBA) + twoCpB) - Math.Log(2.0 * Math.Sqrt(c * a) + b));
                    return (float)(l0 + l1);
                }
                else return (float)(2.0 * A0.magnitude);
            }
        }

        public float InterpolatedLength
        {
            get
            {
                if (p0 == p2)
                {
                    if (p0 == p1) return 0;
                    return (p0 - p1).magnitude;
                }
                if (p1 == p0 || p1 == p2) return (p0 - p2).magnitude;
                double dt = InterpolationPrecision / (p2 - p0).magnitude, length = 0.0f;
                for (double t = dt; t < 1.0; t += dt) length += (GetPointAt((float)(t - dt)) - GetPointAt((float)t)).magnitude;
                return (float)length;
            }
        }


        public Vector3 GetPointAt(float t)
        {
            float tr = 1 - t;
            return Mathf.Pow(tr, 2) * p0 + 2 * t * tr * p1 + Mathf.Pow(t, 2) * p2;
        }

        // calculate  tangent
        public Vector3 Derivative(float t)
        {
            var tr = 1 - t;

            return 2 * tr * (p1 - p0) + 2 * t * (p2 - p1);
        }

        public List<Vector3> GetPoints(int divisions)
        {
            //TODO divisions*=2
            List<Vector3> points = new List<Vector3>();

            for(int i = 0; i <= divisions; ++i)
            {
                points.Add(GetPointAt((float)i / divisions));
            }

            return points;
        }

        public void GetPoints(int divisions, List<Vector3> points)
        {
            for (int i = 0; i <= divisions; ++i)
            {
                points.Add(GetPointAt((float)i / divisions));
            }
        }
    }

    public class CubicBezierCurve : ICurve
    {
        public static double InterpolationPrecision = 0.001;
        public static double LineInterpolationPrecision = 0.05;

        protected static double Sqrt3 = Math.Sqrt(3d);
        protected static double Div18Sqrt3 = 18d / Sqrt3;
        protected static double OneThird = 1d / 3d;
        protected static double Sqrt3Div36 = Sqrt3 / 36d;

        //Start 
        Vector3 p0;
        //Control 1
        Vector3 p1;
        //Control 2
        Vector3 p2;
        //End
        Vector3 p3;

        public CubicBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            this.p0 = p0;
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
        }

        public CubicBezierCurve[] SplitAt(float t)
        {
            Vector3 a = Vector3.Lerp(p0, p1, t);
            Vector3 b = Vector3.Lerp(p1, p2, t);
            Vector3 c = Vector3.Lerp(p2, p3, t);
            Vector3 m = Vector3.Lerp(a, b, t);
            Vector3 n = Vector3.Lerp(b, c, t);
            Vector3 p = GetPointAt(t);
            return new[] { new CubicBezierCurve(p0, a, m, p), new CubicBezierCurve(p, n, c, p3) };
        }

        private double Tmax
        {
            get
            {

                return Math.Pow(Div18Sqrt3 * InterpolationPrecision / ((p3 - 3.0f * p2 + 3.0f * p1 - p0).magnitude / 2.0), OneThird);
            }
        }

        public float Length
        {
            get
            {
                float tmax = 0.0f;
                CubicBezierCurve segment = this;
                List<CubicBezierCurve> segments = new List<CubicBezierCurve>();
                while ((tmax = (float)segment.Tmax) < 1.0)
                {
                    var split = segment.SplitAt(tmax);
                    segments.Add(split[0]);
                    segment = split[1];
                }
                segments.Add(segment);

                float total = 0.0f;
                for (int i = 0, l = segments.Count; i < l; ++i)
                {
                    total += segments[i].QLength;
                }
                return total;
            }
        }

        public float InterpolatedLength
        {
            get
            {
                double dt = LineInterpolationPrecision / (p3 - p0).magnitude, length = 0.0f;
                for (double t = dt; t < 1.0; t += dt) length += (GetPointAt((float)(t - dt)) - GetPointAt((float)t)).magnitude;
                return (float)length;
            }
        }

        /// <summary>
        /// Gets the calculated length of the mid-point quadratic approximation
        /// </summary>
        public float QLength
        {
            get
            {
                Vector3 q = (3.0f * p2 - p3 + 3.0f * p1 - p0) / 4.0f;
                return new QuadraticBezierCurve(p0, q, p3).Length;
            }
        }

        public Vector3 GetPointAt(float t)
        {
            float tr = 1 - t;
            return Mathf.Pow(tr, 3) * p0 + 3 * t * Mathf.Pow(tr, 2) * p1 + 3 * (tr) * Mathf.Pow(t, 2) * p2 + Mathf.Pow(t, 3) * p3;
        }

        // calculate  tangent
        public Vector3 Derivative(float t)
        {
            var tr = 1 - t;

            return 3 * (tr * tr) * (p1 - p0) + 6 * tr * t * (p2 - p1) + 3 * (t * t) * (p3 - p2);
        }

        public List<Vector3> GetPoints(int divisions)
        {
            //TODO divisions*=3
            List<Vector3> points = new List<Vector3>();

            for (int i = 0; i <= divisions; ++i)
            {
                points.Add(GetPointAt((float)i / divisions));
            }

            return points;
        }

        public void GetPoints(int divisions, List<Vector3> points)
        {
            for (int i = 0; i <= divisions; ++i)
            {
                points.Add(GetPointAt((float)i / divisions));
            }
        }
    }
}