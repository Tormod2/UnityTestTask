using System;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Segment
{
    public Vector2 First { get; set; }
    public Vector2 Second { get; set; }
}

public static class PathGeneratorScript
{
    public static ICollection<Vector2> GeneratePoints(int number)
    {
        var segments = new List<Segment>()
        {
            new Segment()
            {
                First = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100)),
                Second = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100))
            }
        };

        var numberOfGeneratedDots = 2;
        while (numberOfGeneratedDots < number)
        {
            var nextDotX = Random.Range(-100, 100);
            var nextDotY = Random.Range(-100, 100);

            var newSegment = new Segment()
            {
                First = segments.Last().Second,
                Second = new Vector2(nextDotX, nextDotY)
            };

            bool hasCurrentSegmentIntersection = false;

            foreach (var segment in segments)
            {
                if (segment == segments.Last())
                {
                    break;
                }

                if (HaveSegmentsIntersection(segment, newSegment))
                {
                    hasCurrentSegmentIntersection = true;
                    break;
                }
            }

            if (hasCurrentSegmentIntersection)
            {
                continue;
            }

            segments.Add(newSegment);
            numberOfGeneratedDots++;
        }

        var segmentsDots = new List<Vector2>()
        {
            segments.First().First,
            segments.First().Second
        };

        segmentsDots.AddRange(segments.Skip(1).Select(segment => segment.Second));
        return segmentsDots;
    }

    private static bool HaveSegmentsIntersection(Segment firstSegment, Segment secondSegment)
    {
        var (coefficient1, offset1) = GetLineEquation(firstSegment);
        var (coefficient2, offset2) = GetLineEquation(secondSegment);

        var intersectionX = (offset2 - offset1) / (coefficient1 - coefficient2);
        var intersectionY = coefficient1 * intersectionX + offset1;

        var intersectionPoint = new Vector2(intersectionX, intersectionY);

        return IsPointInBoundingArea(intersectionPoint, firstSegment)
               && IsPointInBoundingArea(intersectionPoint, secondSegment);
    }

    private static (float Coefficient, float Offset) GetLineEquation(Segment segment)
    {
        var x1 = segment.First.x;
        var y1 = segment.First.y;

        var x2 = segment.Second.x;
        var y2 = segment.Second.y;

        var divider = x2 - x1 == 0 ? 0.00001f : x2 - x1;

        var coefficient = (y2 - y1) / divider;
        var offset = -x1 * (y2 - y1) / divider + y1;

        return (coefficient, offset);
    }

    private static bool IsPointInBoundingArea(Vector2 point, Segment segment)
    {
        const float precision = 0.00001f;

        var leftCornerX = Math.Min(segment.First.x, segment.Second.x);
        var bottomCornerY = Math.Min(segment.First.y, segment.Second.y);

        var rightCornerX = Math.Max(segment.First.x, segment.Second.x);
        var topCornerY = Math.Max(segment.First.y, segment.Second.y);

        return (point.x > leftCornerX || Math.Abs(point.x - leftCornerX) < precision)
               && (point.x < rightCornerX || Math.Abs(point.x - rightCornerX) < precision)
               && (point.y > bottomCornerY || Math.Abs(point.y - bottomCornerY) < precision)
               && (point.y < topCornerY || Math.Abs(point.y - topCornerY) < precision);
    }

    /// <summary>
    /// Generates the sequence of movements for ball based on a given trajectory to be completed in a given time
    /// </summary>
    /// <param name="trajectory"></param>
    /// <param name="transform"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public static Sequence GenerateSequence(List<Vector3> trajectory, Transform transform, float time)
    {
        Sequence sequence = DOTween.Sequence();
        CalculateDistances(trajectory, out List<float> distances, out float sumDistance);

        for (int i = 0; i < trajectory.Count; i++)
        {
            sequence.Append(transform.DOMove(trajectory[i], time * distances[i] / sumDistance).SetSpeedBased().SetEase(Ease.Linear));
            if (i < trajectory.Count - 1)
            {
                sequence.Append(transform.DOLookAt(trajectory[i+1], 0.001f));
            }
        }

        return sequence;
    }

    /// <summary>
    /// Calculates the lenght of the trajectory and the lenght of each part of the sequence of movements
    /// </summary>
    /// <param name="trajectory"></param>
    /// <param name="distances"></param>
    /// <param name="sumDistance"></param>
    private static void CalculateDistances(List<Vector3> trajectory, out List<float> distances, out float sumDistance)
    {
        distances = new List<float>();
        sumDistance = 0;
        for (int i = 0; i < trajectory.Count - 1; i++)
        {
            distances.Add(Vector3.Distance(trajectory[i], trajectory[i + 1]));
            sumDistance += Vector3.Distance(trajectory[i], trajectory[i + 1]);
        }

        //Removing the first point from the trajectory here, because
        //the object is already there, but we needed that point to calculate the distance of the first movement
        trajectory.RemoveAt(0);
    }
}
