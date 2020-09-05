using System;
using System.Linq;
using System.Collections.Generic;

namespace Qkmaxware.Geometry.Primitives {

/// <summary>
/// Arrow pointing in the +Z axis
/// </summary>
public class Arrow : Mesh {
    /// <summary>
    /// Create a new arrow with the given length
    /// </summary>
    /// <param name="length">length of the arrow</param>
    /// <param name="radius">radius of the arrow head</param>
    /// <param name="resolution">subdivision level</param>
    public Arrow(double length = 1, double radius = 0.08, int resolution = 8) {
        var innerRadius = 0.6 * radius;
        var lineLength = 0.8 * length;
        var pointLength = 0.2 * length;
        this.AppendRange(new Cylinder(
            upperRadius: innerRadius,
            lowerRadius: innerRadius, 
            height: lineLength,
            centre: Vec3.K * (lineLength / 2),
            resolution: resolution
        ));
        this.AppendRange(new Cone(
            radius: radius,
            height: pointLength, 
            centre: Vec3.K * lineLength,
            resolution: resolution
        ));
    }
}

}