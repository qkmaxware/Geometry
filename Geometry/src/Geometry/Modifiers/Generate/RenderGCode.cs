using System;
using System.Collections.Generic;
using Qkmaxware.Geometry.IO;

namespace Qkmaxware.Geometry.Modifiers {

/// <summary>
/// RenderGCode modifier to convert GCode back into a 3d model
/// </summary>
public class RenderGCode : GeneratorModifier<IEnumerable<GCode>> {

    /// <summary>
    /// Radius of the path tube
    /// </summary>
    public float TubeRadius {get; set;}

    /// <summary>
    /// Tube quality
    /// </summary>
    public int Resolution {get; set;}

    /// <summary>
    /// Create a new RenderGCode modifier
    /// </summary>
    /// <param name="code">gcode to render</param>
    /// <param name="radius">filament radius</param>
    /// <param name="resolution">tube resolution</param>
    public RenderGCode (IEnumerable<GCode> code, float radius = 0.15f, int resolution = 8) : base (code) {
        this.TubeRadius = radius;
        this.Resolution = resolution;
    }

    public override IEnumerator<Triangle> GetEnumerator() {
        var extruding = false;
        var id = Transformation.Identity();
        double angularStep = 2 * Math.PI / Resolution;
        var basis = new Basis();
        
        // Print head
        var x = 0.0;
        var y = 0.0;
        var z = 0.0;
        bool relative = false;

        foreach (var command in this.Original) {
            if (command.Type != 'G')
                continue;

            switch (command.Number) {
                case 90:
                    // Behaviour is MoveTo
                    relative = false;
                    break;
                case 91:
                    // Behaviour is MoveBy
                    relative = true;
                    break;
                case 0: 
                    if (extruding) {
                        // Create end cap at the previous position
                    }
                    
                    if (command.X.HasValue) {
                        x = command.X.Value + (relative ? x : 0);
                    }
                    if (command.Y.HasValue) {
                        y = command.Y.Value + (relative ? y : 0);
                    }
                    if (command.Z.HasValue) {
                        z = command.Z.Value + (relative ? z : 0);
                    }

                    extruding = false;
                    break;
                case 1:
                    if (!extruding) {
                        // Create start cap at the new position
                    }
                    var start = new Vec3(x, y, z);
                    bool changed = false;

                    if (command.X.HasValue) {
                        x = command.X.Value + (relative ? x : 0);
                        changed = true;
                    }
                    if (command.Y.HasValue) {
                        y = command.Y.Value + (relative ? y : 0);
                        changed = true;
                    }
                    if (command.Z.HasValue) {
                        z = command.Z.Value + (relative ? z : 0);
                        changed = true;
                    }
                    var end = new Vec3(x, y, z);

                    // Didn't actually move or not actually extruding
                    if (!changed || start == end || !command.E.HasValue)
                        continue;

                    // New extrusion, new starting basis, otherwise if continuing, reuse last basis
                    if (!extruding) {
                        basis.Transform = Quat.FromToRotation(Vec3.K, end - start) * id;
                    }

                    for (int i = 1; i <= Resolution; i++) {
                        double previousAngle = (i - 1) * angularStep;
                        double xi = TubeRadius * Math.Cos(previousAngle);
                        double yi = TubeRadius * Math.Sin(previousAngle);

                        double nextAngle = (i) * angularStep;
                        double xe = TubeRadius * Math.Cos(nextAngle);
                        double ye = TubeRadius * Math.Sin(nextAngle);

                        Vec3 be = start         + basis.Y * ye     + basis.X * xe;
                        Vec3 bi = start         + basis.Y * yi     + basis.X * xi;

                        basis.Transform = Quat.FromToRotation(Vec3.K, end - start) * id;
                        Vec3 te = end           + basis.Y * ye     + basis.X * xe;
                        Vec3 ti = end           + basis.Y * yi     + basis.X * xi;

                        yield return new Triangle(be, te, ti);
                        yield return new Triangle(be, ti, bi);
                    }

                    extruding = true;
                    break;
                default:
                    continue;
            }
        }
    }
}

}