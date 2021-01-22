using System;
using System.Collections.Generic;

namespace Qkmaxware.Geometry.Primitives {

/// <summary>
/// Torus mesh
/// </summary>
public class Torus : ParameterizedMesh {

    protected override IMesh Generate() {
        List<Triangle> tris = new List<Triangle>();
        double ringStep = 2 * Math.PI / resolution;
        double moveStep = 2 * Math.PI / segments;

        for (int i = 1; i <= segments; i++) {
            double prevAngle    = (i - 1) * moveStep;
            double angle        = (i) * moveStep;

            var previousDirection = new Vec3(
                Math.Cos(prevAngle),
                Math.Sin(prevAngle),
                0
            );
            var previousPointCentre = majorRadius * previousDirection + centre;
            var previousLeft = previousDirection;
            var previousUp = Vec3.K;

            var nextDirection = new Vec3(
                Math.Cos(angle),
                Math.Sin(angle),
                0
            );
            var nextPointCentre = majorRadius * nextDirection + centre;
            var nextLeft = nextDirection;
            var nextUp = Vec3.K;

            // Move around the ring
            for (var j = 1; j <= resolution; j++) {
                double prevCicleAngle    = (j - 1) * ringStep;
                double cicleAngle        = (j) * ringStep;

                // Previous points
                var previousStart = previousPointCentre + minorRadius * ( previousLeft * Math.Cos(prevCicleAngle) + previousUp * Math.Sin(prevCicleAngle) );
                var previousEnd   = previousPointCentre + minorRadius * ( previousLeft * Math.Cos(cicleAngle) + previousUp * Math.Sin(cicleAngle) );

                // Next points
                var nextStart = nextPointCentre + minorRadius * ( nextLeft * Math.Cos(prevCicleAngle) + nextUp * Math.Sin(prevCicleAngle) );
                var nextEnd   = nextPointCentre + minorRadius * ( nextLeft * Math.Cos(cicleAngle) + nextUp * Math.Sin(cicleAngle) );

                // Create faces
                tris.Add(new Triangle(previousStart, nextStart, nextEnd));
                tris.Add(new Triangle(previousStart, nextEnd, previousEnd));
            }
        }

        return new ListMesh(tris);
    }

    double majorRadius;
    public double MajorRadius {
        get => majorRadius;
        set { majorRadius = value; Rebuild(); }
    }
    double minorRadius;
    public double MinorRadius {
        get => minorRadius;
        set { minorRadius = value; Rebuild(); }
    }
    Vec3 centre;
    public Vec3 Centre {
        get => centre;
        set { centre = value; Rebuild(); }
    }
    int resolution = 8;
    public int Resolution {
        get => resolution;
        set { resolution = value; Rebuild(); }
    }
    int segments = 8;
    public int Segments {
        get => segments;
        set { segments = value; Rebuild(); }
    }

    /// <summary>
    /// Torus constructor
    /// </summary>
    /// <param name="majorRadius">radius of the torus</param>
    /// <param name="minorRadius">radius of the ring</param>
    /// <param name="centre">centre of the torus</param>
    /// <param name="resolution">quality of the rings</param>
    /// <param name="segments">number of subdivisions on the ring</param>
    public Torus (double majorRadius, double minorRadius, Vec3 centre, int resolution = 8, int segments = 8) {
        this.majorRadius = majorRadius;
        this.minorRadius = minorRadius;
        this.centre = centre;
        this.resolution = resolution;
        this.segments = segments;
    }

}

}