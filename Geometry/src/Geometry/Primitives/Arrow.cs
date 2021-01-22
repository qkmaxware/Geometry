using System;
using System.Linq;
using System.Collections.Generic;

namespace Qkmaxware.Geometry.Primitives {

/// <summary>
/// Arrow pointing in the +Z axis
/// </summary>
public class Arrow : ParameterizedMesh {

    private double length;
    public double Length {
        get => length;
        set { length = value; Rebuild(); }
    }

    private double radius;
    public double Radius {
        get => radius;
        set { radius = value; Rebuild(); }
    }

    private int resolution;
    public int Resolution {
        get => resolution;
        set { resolution = value; Rebuild(); }
    }

    /// <summary>
    /// Create a new arrow with the given length
    /// </summary>
    /// <param name="length">length of the arrow</param>
    /// <param name="radius">radius of the arrow head</param>
    /// <param name="resolution">subdivision level</param>
    public Arrow(double length = 1, double radius = 0.08, int resolution = 8) {
        this.length = length;
        this.radius = radius;
        this.resolution = resolution;
        Rebuild();
    }

    protected override IMesh Generate() {
        List<Triangle> meshdata = new List<Triangle>();
        var innerRadius = 0.6 * radius;
        var lineLength = 0.8 * length;
        var pointLength = 0.2 * length;
        meshdata.AddRange(new Cylinder(
            upperRadius: innerRadius,
            lowerRadius: innerRadius, 
            height: lineLength,
            centre: Vec3.K * (lineLength / 2),
            resolution: resolution
        ));
        meshdata.AddRange(new Cone(
            radius: radius,
            height: pointLength, 
            centre: Vec3.K * lineLength,
            resolution: resolution
        ));
        return new ListMesh(meshdata);
    }
}

}