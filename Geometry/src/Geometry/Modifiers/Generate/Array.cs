using System.Collections.Generic;

namespace Qkmaxware.Geometry.Modifiers {

/// <summary>
/// Array modifier to repeat an object several times
/// </summary>
public class Array : PolygonGeneratorModifier<IMesh> {

    /// <summary>
    /// Number of times to repeat
    /// </summary>
    public int Count {get; set;} = 1;

    /// <summary>
    /// Displacement of each mesh relative to the last
    /// </summary>
    /// <value></value>
    public Vec3 Offset {get; set;}

    /// <summary>
    /// Create an array modifer 
    /// </summary>
    /// <param name="mesh">original mesh</param>
    /// <param name="count">number of repetitions</param>
    public Array (IMesh mesh, int count) : base (mesh) {
        this.Count = count;
        this.Offset = new Vec3(1, 0, 0);
    }

    /// <summary>
    /// Create an array modifer 
    /// </summary>
    /// <param name="mesh">original mesh</param>
    /// <param name="count">number of repetitions</param>
    /// <param name="offset">offset</param>
    public Array (IMesh mesh, int count, Vec3 offset) : base (mesh){
        this.Count = count;
        this.Offset = offset;
    }

    public override IEnumerator<Triangle> GetEnumerator() {
        for (var i = 0; i < this.Count; i++) {
            var matrix = Transformation.Offset(Offset * i);
            foreach  (var tri in this.Original) {
                yield return tri.Transform(matrix);
            }
        }
    }

}

}