﻿using NUnit.Framework;
using System;
using System.Collections.Generic;


namespace SLAM.Models.Mapping.UnitTests.Extensions {

    using Mapping.Extensions;

    [TestFixture]
    public sealed partial class ExtensionsTests {

        [Test]
        public void RadiansToDegrees_ExtremeAngles_MaximalAccuracy() {
            Assert.AreEqual(Common.DegreesInRadian, 1.0.ToDegrees(), Common.PrecisionMax);
        }

        [Test]
        public void DegreesToRadians_ExtremeAngles_MaximalAccuracy() {
            Assert.AreEqual(Common.RadiansInDegree, 1.0.ToRadians(), Common.PrecisionMax);
        }

        [Test]
        public void AsNormalizedAngle_ExtremeAngles_EqualExpected() {

            foreach (var param in AsNormalizedAngle_ExtremeAngles_EqualExpected_Params) {

                double expectedAngle = param.Item2;
                double actualAngle = param.Item1.AsNormalizedAngle();

                Assert.AreEqual(expectedAngle, actualAngle);
            }

        }

        private static IEnumerable<Tuple<double, double>> AsNormalizedAngle_ExtremeAngles_EqualExpected_Params {
            get {

                // normal case:
                double range = 360.0 * 16.0, step = 0.1;
                for (double a = -range; a < range + step; a += step) {

                    double sourceAngle = a;                    // 541
                    double expectedAngle = a % 360.0;          // 541 % 360 = 181;

                    if (expectedAngle > 180.0) {
                        expectedAngle = expectedAngle - 360.0; // 181 - 360 = -179
                    }
                    if (expectedAngle < -180.0) {
                        expectedAngle = expectedAngle + 360.0; // -181 + 360 = 179
                    }
                    yield return new Tuple<double, double>(sourceAngle, expectedAngle);
                }

                // exceptional case: Values are multiples of 180 will be inverted!
                // But I do not think that this is a problem, because -180 And 180 - this is essentially the same.
                yield return new Tuple<double, double>(180, -180);
                yield return new Tuple<double, double>(-180, 180);
                yield return new Tuple<double, double>(-540, 180);
                yield return new Tuple<double, double>(540, -180); // etc.
            }
        }
    }
}