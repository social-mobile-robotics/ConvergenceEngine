﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ConvergenceEngine.Models.Mapping.Extensions {

    using Infrastructure.Extensions;
    using Segments;

    internal static class Selector { // IEnumerable<Segment> Extension class

        public static IEnumerable<(Segment current, Segment nearest)> SelectNearestTo(
            this IEnumerable<Segment> current, IEnumerable<Segment> another, double maxDistancePercent, double maxAngleDegrees) {

            foreach (var segment in current) {
                double currentMaxDistance = Math.Min(segment.A.Y, segment.B.Y) / 100.0 * maxDistancePercent;

                var nearest = another.SelectNearestTo(segment, currentMaxDistance, maxAngleDegrees);
                if (nearest != null) {
                    yield return (current: segment, nearest: nearest);
                }
            }
        }

        public static Segment SelectNearestTo(
            this IEnumerable<Segment> sequence, Segment segment, double maxDistance, double maxAngleDegrees) {
            var selection = sequence
                .Where(s => s.NearestByExtremePointsDistanceTo(segment, maxDistance))
                .Where(s => s.NearestByAngleTo(segment, maxAngleDegrees));
            if (selection.Count() > 1) {
                return selection.SelectWithNearestLengthTo(segment);
            }
            return selection.FirstOrDefault();
        }

        public static Segment SelectWithNearestLengthTo(this IEnumerable<Segment> sequence, Segment segment) {
            return sequence.MinBy(s => Math.Abs(s.Length - segment.Length));
        }
    }
}