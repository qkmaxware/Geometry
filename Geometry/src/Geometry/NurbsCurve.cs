using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Qkmaxware.Geometry {

/// <summary>
/// Class representing a control point on a NURBS curve or surface
/// </summary>
public class NurbsControlPoint : Vec3 {
    /// <summary>
    /// Control point weight
    /// </summary>
    /// <value>weighting</value>
    public double Weight {get; set;} = 1;

    /// <summary>
    /// Create a new control point 
    /// </summary>
    /// <param name="x">x coordinate</param>
    /// <param name="y">y coordinate</param>
    /// <param name="z">z coordinate</param>
    /// <param name="w">point weight</param>
    /// <returns>control point</returns>
    public NurbsControlPoint(double x, double y, double z, double w) : base(x,y,z) {
        this.Weight = w;
    }

    /// <summary>
    /// Create a new control point
    /// </summary>
    /// <param name="position">x,y,z position</param>
    /// <param name="weight">point weight</param>
    /// <returns>control point</returns>
    public NurbsControlPoint(Vec3 position, double weight) : this(position.X, position.Y, position.Z, weight) {}
}

/// <summary>
/// Type of pinning to use when generating knot vector values
/// </summary>
public enum NurbsPinning {
    // No pinning
    None,
    // Pin the start point, but not the end point
    PinStart,
    // Pin the end point, but not the start point
    PinEnd,
    // Pin both the start and the end points
    PinBoth
}

/// <summary>
/// NURBS curve
/// </summary>
public class NurbsCurve {

    private List<NurbsControlPoint> internalControlPoints;               // List I can modify
    /// <summary>
    /// All control points defining the NURBS shape
    /// </summary>
    /// <value>control points</value>
    public IArray<NurbsControlPoint> ControlPoints {get; private set;}   // Expose public list getters and setters, but not adders
    private int _degree;
    /// <summary>
    /// Degree
    /// </summary>
    public int Degree {
        get => _degree;
        set {
            this._degree = Math.Max(0, Math.Min(value, ControlPoints.Length - 1));
        }
    }
    /// <summary>
    /// Order 
    /// </summary>
    public int Order {
        get => Degree + 1;
        set {
            Degree = value - 1;
        }
    }
    private List<double> knotVector {get; set;}
    /// <summary>
    /// Vector of all knots
    /// </summary>
    /// <value></value>
    public IReadOnlyCollection<double> KnotVector {get; private set;}
    /// <summary>
    /// Value of the first knot
    /// </summary>
    public double FirstKnot => knotVector[0];
    /// <summary>
    /// Value of the last knot
    /// </summary>
    public double LastKnot => knotVector[knotVector.Count - 1];

    /// <summary>
    /// Starting position
    /// </summary>
    Vec3 Start => this[0];
    /// <summary>
    /// Ending postion
    /// </summary>
    Vec3 End => this[1];

    private static double remap(double x, double in_min, double in_max, double out_min, double out_max) {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }

    /// <summary>
    /// Evaluate the curve at the given interpolation parametre
    /// </summary>
    /// <value>position on curve</value>
    public Vec3 this[double t] {
        get {
            // Clamp interpolation parametre
            if (t > 1) {
                t = 1;
            }
            if (t < 0) {
                t = 0;
            }
            
            // t is a parametre between [0,1] but the knot vectors are any positive integer, place these on the same scale
            t = remap(t, 0, 1, this.FirstKnot, this.LastKnot);

            double x = 0, y = 0, z = 0;
            double rationalWeight = 0;
            double[] ns = new double[this.ControlPoints.Length];

            for (int i = 0; i < this.ControlPoints.Length; i++) {
                double n = Nip(i, this.Degree, t);
                double temp = n * this.ControlPoints[i].Weight;
                ns[i] = temp;
                rationalWeight += temp;
            }

            for (int i = 0; i < this.ControlPoints.Length; i++) {
                double temp = ns[i];
                x += this.ControlPoints[i].X * temp / rationalWeight;
                y += this.ControlPoints[i].Y * temp / rationalWeight;
                z += this.ControlPoints[i].Z * temp / rationalWeight;
            }
            return new Vec3(x,y,z);
        }
    }

    public NurbsCurve (int degree, IEnumerable<NurbsControlPoint> points, IEnumerable<double> knot) {
        this.internalControlPoints = points.ToList();
        this.ControlPoints = new FixedSizeList<NurbsControlPoint>(this.internalControlPoints);

        this.Degree = degree;

        this.knotVector = knot.OrderBy(num => num).ToList();
        this.KnotVector = knotVector.AsReadOnly();
        
        validate();
    }

    public NurbsCurve (IEnumerable<NurbsControlPoint> points, NurbsPinning pinning = NurbsPinning.None) {
        this.internalControlPoints = points.ToList();
        this.ControlPoints = new FixedSizeList<NurbsControlPoint>(this.internalControlPoints);

        this.Degree = this.ControlPoints.Length - 1;

        this.knotVector = generateUniformKnotVector(pinning).ToList();
        this.KnotVector = knotVector.AsReadOnly();

        validate();
    }

    private void validate () {
        if (this.Order < 2) {
            throw new ArgumentException("The order of a curve must be at least 2");
        }
        if (knotVector.Count < this.ControlPoints.Length + this.Degree + 1) {
            throw new ArgumentException("Knot vector is not large enough for the given number of control points");
        }
    }

    private double[] generateUniformKnotVector(NurbsPinning pinned) {
        var length = this.ControlPoints.Length + this.Degree + 1;

        if (pinned == NurbsPinning.None){
            // {0,1,2,3,4,5,6,...}
            var vec = new double[length];
            for (var i = 0; i < length; i++) {
                this.knotVector[i] = i;
            }
            return vec;
        } else {
            // {0,0,0,1,2,3,...,n,n,n}
            var m = (this.Degree + 1);
            var start_count = 0;
            var end_count = 0;
            if (pinned == NurbsPinning.PinStart || pinned == NurbsPinning.PinBoth) {
                start_count = m;
            }
            if (pinned == NurbsPinning.PinEnd || pinned == NurbsPinning.PinBoth) {
                end_count = m;
            }
            var inner_count = length - start_count - end_count;

            List<double> vector = new List<double>();

            // Pin start
            for (var i = 0; i < start_count; i++) {
                vector.Add(0);
            }

            // Intermediate
            var f = 1;
            for (f = 1; f < inner_count; f++) {
                vector.Append(f);
            }
            
            // Pin end
            for (var i = 0; i < end_count; i++) {
                vector.Add(f);
            }

            return vector.ToArray();
        }
    }

    /// <summary>
    /// Create a linear NURBS curve
    /// </summary>
    /// <param name="from">starting position</param>
    /// <param name="to">ending position</param>
    /// <returns>line from start to end</returns>
    public static NurbsCurve Line (Vec3 from, Vec3 to) {
        return new NurbsCurve(new NurbsControlPoint[]{
            new NurbsControlPoint(from, 1),
            new NurbsControlPoint(to, 1)
        }, NurbsPinning.PinBoth);
    }

    /// <summary>
    /// Create a circle from a NURBS curve
    /// </summary>
    /// <param name="center">circle center</param>
    /// <param name="r">circle radius</param>
    /// <returns>circle</returns>
    public static NurbsCurve Circle (Vec3 center, double r) {
        var w = Math.Sqrt(2) / 2;
        var controls = new NurbsControlPoint[]{
            new NurbsControlPoint(center + new Vec3(r, 0, 0), 1),
            new NurbsControlPoint(center + new Vec3(r, r, 0), w),
            new NurbsControlPoint(center + new Vec3(0, r, 0), 1),
            new NurbsControlPoint(center + new Vec3(-r, r, 0), w),
            new NurbsControlPoint(center + new Vec3(-r, 0, 0), 1),
            new NurbsControlPoint(center + new Vec3(-r, -r, 0), w),
            new NurbsControlPoint(center + new Vec3(0, -r, 0), 1),
            new NurbsControlPoint(center + new Vec3(r, -r, 0), w),
            new NurbsControlPoint(center + new Vec3(r, 0, 0), 1),
        };
        var knots = new double[]{
            0, 
            0, 
            0, 
            Math.PI / 2, 
            Math.PI / 2,
            Math.PI,
            Math.PI, 
            3 * Math.PI / 2,
            3 * Math.PI / 2,
            2 * Math.PI,
            2 * Math.PI,
            2 * Math.PI
        };
        return new NurbsCurve(2, controls, knots);
    }

    private double Nip (int i, int p, double u) {
        double[] N = new double[p + 1];
        double saved, temp;

        var U = this.knotVector;

        int m = U.Count - 1;
        if ( 
               (i == 0 && u == U[0]) 
            || (i == (m - p - 1) && u == U[m])
        ) {
            return 1;
        }

        if (u < U[i] || u >= U[i + p + 1]) {
            return 0;
        }

        for (int j = 0; j <= p; j++) {
            if (u >= U[i + j] && u < U[i + j + 1]) {
                N[j] = 1d;
            } else {
                N[j] = 0d;
            }
        }

        for (int k = 1; k <= p; k++) {
            if (N[0] == 0)
                saved = 0d;
            else 
                saved = ((u - U[i]) * N[0]) / (U[i+k] - U[i]);

            for (int j = 0; j < p - k + 1; j++) {
                double Uleft = U[i + j + 1];
                double Uright = U[i + j + k + 1];

                if (N[j + 1] == 0) {
                    N[j] = saved;
                    saved = 0d;
                } else {
                    temp = N[j + 1] / (Uright - Uleft);
                    N[j] = saved + (Uright - u) * temp;
                    saved = (u - Uleft) * temp;
                }
            }
        }

        return N[0];
    }

    //public static NurbsSurface operator * (NurbsCurve a, NurbsCurve b) {
    // Create a surface as the product of 2 curves
    //}

    public void InsertKnot(double knot, NurbsControlPoint point) {
        // Compute index
        var index = 0;
        for (var i = 0; i < this.knotVector.Count; i++) {
            if (knot <= this.knotVector[i]) {
                index++;
            } else {
                break;
            }
        }

        // Insert
        this.knotVector.Insert(index, knot);
        this.internalControlPoints.Insert(index, point);
    }

    public void RemoveKnot(double knot) {
        var index = this.knotVector.IndexOf(knot);
        if (index != -1) {
            this.knotVector.RemoveAt(index);
            this.internalControlPoints.RemoveAt(index);
        }
    }

    public NurbsCurve Join (NurbsCurve other) {
        throw new NotImplementedException();
    }

}

/// <summary>
/// NURBS surface
/// </summary>
public class NurbsSurface {
    public Vec3 this[double u, double v] => throw new NotImplementedException();
}

/// <summary>
/// NURBS volume
/// </summary>
public class NurbsVolume : NurbsSurface {
    public Vec3 this[double u, double v, double w] => throw new NotImplementedException();
}

}