namespace Qkmaxware.Geometry.Modifiers {

// https://www.ebalstudios.com/blog/introduction-nurbs?fbclid=IwAR3KwpxaDlx4NAOQ_MY_SzpmUXWEwLJ1AMEzsICbuEFQpaapGtkFfaFyfp4

/// <summary>
/// Revolve a curve to create a curve or volume
/// </summary>
public class NurbsRevolve : BaseModifier<NurbsCurve> {
    
    public Vec3 RotationAxis {get; set;}
    public float RotationAngle {get; set;}

    public NurbsRevolve (NurbsCurve curve1, Vec3 axis, float angle) : base(curve1) {
        this.RotationAngle = angle;
        this.RotationAxis = axis;
    }

}

}