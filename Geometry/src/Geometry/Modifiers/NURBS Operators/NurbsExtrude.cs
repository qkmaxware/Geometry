namespace Qkmaxware.Geometry.Modifiers {

/// <summary>
/// Extrude a surface from a NURBS curve following another curve as a rail
/// </summary>
public class NurbsExtrude : BaseModifier<NurbsCurve> {
    
    public NurbsCurve Rail {get; set;}

    public NurbsExtrude (NurbsCurve curve, NurbsCurve rail) : base(curve) {
        this.Rail = rail;
    }

}

public class NurbsExtrudedSurface {

    public NurbsCurve ProfileCurve {get; set;}
    public NurbsCurve ExtrusionRail {get; set;}

    public NurbsExtrudedSurface (NurbsCurve profile, NurbsCurve rail) {
        this.ProfileCurve = profile;
        this.ExtrusionRail = rail;
    }

    public Vec3 this[double u, double v] {
        get {
            var uPos = this.ProfileCurve[u];
            var vPos = this.ExtrusionRail[v];
            return Vec3.Cross(uPos, vPos);
        }
    }

}

}