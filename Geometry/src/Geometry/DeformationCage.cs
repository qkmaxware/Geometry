using System;

namespace Qkmaxware.Geometry {

public class DeformationCage {

    public Vec3[,,] BoundaryVertices {get; private set;}

    public DeformationCage() : this(1, 2, 2, 2) {}

    public DeformationCage(int size, int ru, int rv, int rw) {
        ru = Math.Max(ru, 2);
        rv = Math.Max(rv, 2);
        rw = Math.Max(rw, 2);

        this.BoundaryVertices = new Vec3[ru, rv, rw];

        var start = -size;
        var end = size;

        // For all Z
        for (var w = 0; w < rw; w++) {
            var zPos = lerp(start, end, w, rw-1);

            // For all Y
            for (var v = 0; v < rv; v++) {
                var yPos = lerp(start, end, v, rv-1);

                // For all X
                for (var u = 0; u < ru; u++) {
                    var xPos = lerp(start, end, u, ru-1);
                    this.BoundaryVertices[u,v,w] = new Vec3(xPos, yPos, zPos);
                }
            }
        }
    }

    private static float lerp(float from, float to, float current, float max) {
        var t = current / max;
        return (1 - t) * from + t * to;
    }

}

}