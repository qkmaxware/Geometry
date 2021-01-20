using System.Collections;
using System.Collections.Generic;

namespace Qkmaxware.Geometry.Modifiers {

/// <summary>
/// Base class for mesh modification decorators
/// </summary>
public abstract class BaseModifier<From> {

    /// <summary>
    /// The decorated object
    /// </summary>
    public From Original {get; private set;}

    /// <summary>
    /// Add a new modifier to the mesh
    /// </summary>
    /// <param name="originalMesh">original mesh</param>
    public BaseModifier(From original) {
        this.Original = original;
    }

}

}