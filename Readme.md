# C# Geometry

## Build Status
![](https://github.com/qkmaxware/Geometry/workflows/Build/badge.svg)

## Getting Started
The library is available as a NuGet package for any .Net implementation that supports the .Net Standard 2.0. Visit the [Packages](https://github.com/qkmaxware/Geometry/packages) page for downloads.

## Primitive Geometry
Geometry in this library is modeled as a collection of triangular faces whose vertices are points in 3d space. There are some handy utility classes which can be used to generate specific primitive geometric shapes as described below. 

| Name | Shape | Image |
|------|-------|-------|
| Plane | Flat planar face in the XY plane | <img width="128" src="docs/images/PrimitivePlane.png"/> |
| Cube | 6 sided cube | <img width="128" src="docs/images/PrimitiveCube.png"/> |
| Cylinder | Solid cylinder with configurable radii | <img width="128" src="docs/images/PrimitiveCylinder.png"/> |
| Tube | Hollowed out cylinder with an inner and outer radius | <img width="128" src="docs/images/PrimitiveTube.png"/> |
| Capsule | Cylinder capped with two hemispheres | <img width="128" src="docs/images/PrimitiveCapsule.png"/> |
| Cone | Cone with a given radius | <img width="128" src="docs/images/PrimitiveCone.png"/> |
| Sphere | UV sphere | <img width="128" src="docs/images/PrimitiveSphere.png"/> |
| Hemisphere | Half of a sphere | <img width="128" src="docs/images/PrimitiveHemisphere.png"/> |
| Torus | Torus with configurable radii | <img width="128" src="docs/images/PrimitiveTorus.png"/> |
| Frustum | Pyramidal Frustums | <img width="128" src="docs/images/PrimitiveFrustum.png"/> |
| Nosecone | Varieties of aerodynamic nosecones | <img width="128" height="256" src="docs/images/PrimitiveNoseconeBiconic.png"/> <img width="128" height="256" src="docs/images/PrimitiveNoseconeParabolic.png"/>|

## Transformations for Building Geometries
| Name | Effect | Result |
|------|--------|--------|
| Difference | Subtract one solid from another | <img width="128" src="docs/images/BooleanDifference.png"/> |
| Union | Combine 2 solids into a single solid | <img width="128" src="docs/images/BooleanUnion.png"/> |
| Intersection | Form a new block from where two blocks overlap | <img width="128" src="docs/images/BooleanIntersection.png"/> |

## Importing and Exporting Geometries
The core library supports importing and exporting geometry from several different 3d model file formats. All classes related to importing and exporting can be found within the `Qkmaxware.Geometry.IO` namespace.

| Format | Extension | Import Binary | Import Ascii | Export Binary | Export Ascii |
|--------|-----------|--------|--------|--------|--------|
| Stereolithography CAD | .stl | &#9745; | &#9745; | &#9745; | &#9745; |
| Wavefront Object | .obj | &#9744; | &#9745; | &#9744; | &#9745; |
| Extensible 3D Graphics | .x3d | &#9744; | &#9744; | &#9744; | &#9745; |

## Made With
- [.Net Standard](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)

## License
See [License](LICENSE.md) for license details.