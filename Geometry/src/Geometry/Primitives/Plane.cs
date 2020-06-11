using System.Collections.Generic;

namespace Qkmaxware.Geometry.Primitives {

/// <summary>
/// XY plane mesh
/// </summary>
public class Plane : Mesh {

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
    public Plane (double size, Vec3 centre) : base(Create(size, centre)) {}
}

}