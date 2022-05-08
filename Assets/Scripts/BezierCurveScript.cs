using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public static class BezierCurveScript
{
    /// <summary>
    /// Takes the list of points that needs to be smoothed with a given step.
    /// It divides the initial trajectory in groups of three,
    /// where the last point of the privious group is the first point of the next one.
    /// </summary>
    /// <param name="step"></param>
    /// <param name="trajectory"></param>
    /// <returns></returns>
    public static List<Vector3> MakeBezierCurve(float step, List<Vector3> trajectory)
    {
        var newTrajectory = new List<Vector3>();
        var sharpPoints = new List<Vector3>();

        for (int j = 0; j < trajectory.Count - 2; j += 2)
        {
            var bezierPoints = new List<Vector3>();
            var points = new List<Vector3>();
            points.Add(trajectory[j]);
            points.Add(trajectory[j+1]);
            points.Add(trajectory[j+2]);

            GenerateBezierPoints(step, bezierPoints, points);

            sharpPoints.Add(points[2]);

            newTrajectory.AddRange(bezierPoints);
        }

        for (int i = 0; i < sharpPoints.Count-1; i++)
        {
            var points = new List<Vector3>();
            var bezierPoints = new List<Vector3>();

            var index = newTrajectory.IndexOf(sharpPoints[i]);

            points.Add(newTrajectory[index - 3]);
            points.Add(sharpPoints[i]);
            points.Add(newTrajectory[index + 4]);

            GenerateBezierPoints(step, bezierPoints, points);

            newTrajectory.RemoveRange(index-3,8);
            newTrajectory.InsertRange(index-3, bezierPoints);
        }

        //In this algorithm, if the amount of points is even,
        //the last point should be added manually, because it's not fitting in.
        if (trajectory.Count % 2 == 0)
        {
            newTrajectory.Add(trajectory.Last());
        }

        return newTrajectory;
    }

    private static void GenerateBezierPoints(float step, List<Vector3> bezierPoints, List<Vector3> points)
    {
        for (var t = 0f; t < 1 + step; t += step)
        {
            if (t > 1)
            {
                t = 1;
            }

            var index = bezierPoints.Count;
            bezierPoints.Add(new Vector3(0,2,0));

            for (var i = 0; i < points.Count; i++)
            {
                var newVector = bezierPoints[index];
                var polinom = Polinom(points.Count - 1, t, i);

                newVector.x += points[i].x * polinom;
                newVector.z += points[i].z * polinom;
                bezierPoints[index] = newVector;
            }
        }
    }

    /// <summary>
    /// Calculates factorial for given number
    /// </summary>
    /// <param name="n">Number</param>
    /// <returns></returns>
    public static int Factorial(int n)
    {
        return (n <= 1) ? 1 : n * Factorial(n - 1);
    }

    /// <summary>
    /// Calculates Bernstein polinom
    /// </summary>
    /// <param name="n">Polinom power</param>
    /// <param name="t">Step</param>
    /// <param name="i">Point number</param>
    /// <returns></returns>
    public static float Polinom(int n, float t, int i)
    {
        return (float)(Factorial(n) / (Factorial(i) * Factorial(n - i))
            * Math.Pow(t, i)
            * Math.Pow(1 - t, n - i));
    }
}
