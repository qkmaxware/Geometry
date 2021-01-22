namespace Qkmaxware.Geometry.Modifiers {

/// <summary>
/// Skin a surface between two NURBS curves
/// </summary>
public class NurbsSkin : BaseModifier<NurbsCurve> {
    
    public NurbsCurve Original2 {get; set;}

    public NurbsSkin (NurbsCurve curve1, NurbsCurve curve2) : base(curve1) {
        this.Original2 = curve2;
    }

}

}