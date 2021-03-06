using System.Collections.Generic;

namespace Qkmaxware.Geometry.Primitives {

/// <summary>
/// Cubic mesh
/// </summary>
public class Cube : ParameterizedMesh {
    private static Vec3[] cubeCoordinates = new Vec3[]{
        new Vec3(-0.5,-0.5,0.5),
        new Vec3(-0.5,0.5,0.5),
        new Vec3(-0.5,-0.5,-0.5),
        new Vec3(-0.5,0.5,-0.5),
        new Vec3(0.5,-0.5,0.5),
        new Vec3(0.5,0.5,0.5),
        new Vec3(0.5,-0.5,-0.5),
        new Vec3(0.5,0.5,-0.5),
    };

    private static int[] cubeFaces = new int[]{
        3,2,0,
        7,6,2,
        
        5,4,6,
        1,0,4,
        
        2,6,4,
        7,3,1,
        
        1,3,0,
        3,7,2,
        
        7,5,6,
        5,1,4,
        
        0,2,4,
        5,7,1 
    };

    protected override IMesh Generate() {
        List<Triangle> tris = new List<Triangle>();
        for(int i = 0; i < cubeFaces.Length; i+=3) {
            tris.Add(
                new Triangle(
                    size * cubeCoordinates[cubeFaces[i]] + centre,      
                    size * cubeCoordinates[cubeFaces[i + 1]] + centre,
                    size * cubeCoordinates[cubeFaces[i + 2]] + centre
                )
            );
        }
        return new ListMesh(tris);
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

    /// <summary>
    /// Create a cube
    /// </summary>
    /// <param name="size">size of the cube</param>
    /// <param name="centre">centre of the cube</param>
    public Cube (double size, Vec3 centre) {
        this.size = size;
        this.centre = centre;
        Rebuild();
    }
}

}