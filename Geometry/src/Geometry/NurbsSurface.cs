namespace Qkmaxware.Geometry {

/// <summary>
/// Interface describing a mutable NURBS control net
/// </summary>
public interface INurbsControlNet {
    NurbsCurve UBasis {get;}
    NurbsCurve VBasis {get;}

    NurbsControlPoint this[int u, int v] {get;}
}

/// <summary>
/// Control net representing the summation of basis control points
/// </summary>
public class BasisSumControlNet : INurbsControlNet {
    public NurbsCurve UBasis {get; private set;}
    public NurbsCurve VBasis {get; private set;}

    public NurbsControlPoint this[int u, int v] {
        get {
            var up = UBasis.ControlPoints[u];
            var vp = VBasis.ControlPoints[v];

            return new NurbsControlPoint(
                up + vp,
                up.Weight * vp.Weight
            );
        }
    }

    public BasisSumControlNet(NurbsCurve u, NurbsCurve v) {
        this.UBasis = u;
        this.VBasis = v;
    }
}

/// <summary>
/// NURBS surface
/// </summary>
public class NurbsSurface {
    /// <summary>
    /// Control net defining the surface
    /// </summary>
    /// <value>control net</value>
    public INurbsControlNet ControlNet {get; private set;}

    public NurbsSurface (INurbsControlNet controlNet) {
        this.ControlNet = controlNet;
    }

    /// <summary>
    /// Create a 2d NURBS plane
    /// </summary>
    /// <param name="size">plane size</param>
    /// <returns>NURBS surface representing the plane</returns>
    public static NurbsSurface Plane (int size) {
        var i = NurbsCurve.Line(Vec3.Zero, Vec3.I * size);
        var j = NurbsCurve.Line(Vec3.Zero, Vec3.J * size);

        return new NurbsSurface(new BasisSumControlNet(i, j));
    }

    public Vec3 this[double u, double v] {
        get {
            double x = 0, y = 0, z = 0;
            var rationalWeight = 0.0;
            var p = this.ControlNet.UBasis.Degree;
            var n = this.ControlNet.UBasis.ControlPoints.Length;
            var q = this.ControlNet.VBasis.Degree;
            var m = this.ControlNet.VBasis.ControlPoints.Length;

            for (var i = 0; i < n; i++) {
                for (var j = 0; j < m; j++) {
                    var CPij = this.ControlNet[i, j];
                    var Pij = CPij;
                    var wij = CPij.Weight;

                    var temp = this.ControlNet.UBasis.Nip(i, p, u) * this.ControlNet.VBasis.Nip(j, q, v) * wij;
                    rationalWeight += temp;

                    x += temp * Pij.X;
                    y += temp * Pij.Y;
                    z += temp * Pij.Z;
                }
            }

            return new Vec3(x / rationalWeight, y / rationalWeight, z / rationalWeight);
        }
    }
}

}