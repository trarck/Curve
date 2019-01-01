using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using curve;
using System;

public class Test : MonoBehaviour
{
    int Benchmark(Action a, TimeSpan t, string name)
    {
        Debug.Log(String.Format("Testing {0}...", name));
        DateTime start = DateTime.Now;
        int i = 0;
        while (DateTime.Now - start < t) { a(); i++; }
        return (int)(i / t.TotalSeconds);
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector3 p0 = new Vector3(0, 0, 0);
        Vector3 p1 = new Vector3(100, 100, 100);
        Vector3 p2 = new Vector3(300, 200, 100);
        Vector3 p3 = new Vector3(500, 600, 200);

        CubicBezierCurve testCurve = new CubicBezierCurve(p0, p0+p1, p2+p3, p3);

        double l0 = 0, l1 = 0, l2 = 0;
        int s0 = 0, s1 = 0, s2 = 0;
        TimeSpan t = new TimeSpan(0, 0, 3);
        s0 = Benchmark(() => { l0 = testCurve.InterpolatedLength; }, t, "line interpolation");
        s1 = Benchmark(() => { l1 = testCurve.Length; }, t, "adaptive quadratic interpolation");
        s2 = Benchmark(() => { l2 = testCurve.QLength; }, t, "midpoint quadratic interpolation");
        Debug.Log(String.Format(
            "\r\n\t\tLine int.:\t| Adaptive:\t| Midpoint:\r\n" +
            "  Result[m]:\t{0}\t| {1}\t| {2}\r\n" +
            "Speed[op/s]:\t{3}\t\t| {4}\t| {5}",
            Math.Round(l0, 9), Math.Round(l1, 9), Math.Round(l2, 9),
            s0, s1, s2
        ));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
