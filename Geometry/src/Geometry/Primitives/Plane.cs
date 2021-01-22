using System.Collections.Generic;

namespace Qkmaxware.Geometry.Primitives {

/// <summary>
/// XY plane mesh
/// </summary>
public class Plane : ParameterizedMesh {

    private static Vec3[] planeCoordinates = new Vec3[]{
        new Vec3(-0.5, -0.5,  0), // Bottom Left
        new Vec3(-0.5,  0.5,  0), // Top Left
        new Vec3( 0.5,  0.5,  0), // Top Right
        new Vec3( 0.5, -0.5,  0), // Bottom Right
    };

    private static int[] planeFaces = new int[]{
        0, 1, 2,
        0, 2, 3
    };

    protected override IMesh Generate() {
        return new ListMesh(Create(size, centre));
    }

    public static List<Triangle> Create(double size, Vec3 centre) {
        List<Triangle> tris = new List<Triangle>();
        for(int i = 0; i < planeFaces.Length; i+=3) {
            tris.Add(
                new Triangle(
                    size * planeCoordinates[planeFaces[i]] + centre,      
                    size * planeCoordinates[planeFaces[i + 1]] + centre,
                    size * planeCoordinates[planeFaces[i + 2]] + centre
                )
            );
        }
        return tris;
    }

    /// <summary>
    /// Create a plane
    /// </summary>
    /// <param name="size">plane size</param>
    /// <param name="centre">centre</param>
    public Plane (double size, Vec3 centre) {
        this.size = size;
        this.centre = centre;
        Rebuild();
    }

    double size;
    public double Size {
        get => size;
        set { size = value; Rebuild(); }
    }
    Vec3 centre;
    public Vec3 Centre {
        get => centre;
        set { centre = value; Rebuild(); }
    }
}

}