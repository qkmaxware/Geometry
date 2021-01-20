using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

namespace Qkmaxware.Geometry.IO {

/// <summary>
/// Description of a partiular G code as described at https://reprap.org/wiki/G-code#G-commands
/// </summary>
public class GCode {
    /// <summary>
    /// Command type G, M
    /// </summary>
    public char Type {get; set;}
    /// <summary>
    /// Code number
    /// </summary>
    public int Number {get; set;}
    /// <summary>
    /// Select tool nnn. In RepRap, a tool is typically associated with a nozzle, which may be fed by one or more extruders.
    /// </summary>
    public int? Tool {get; set;}

    /// <summary>
    /// Command parameter, such as time in seconds; temperatures; voltage to send to a motor
    /// </summary>
    public object? S {get; set;} = null;
    /// <summary>
    /// Command parameter, such as time in milliseconds; proportional (Kp) in PID Tuning
    /// </summary>
    public object? P {get; set;} = null;
    /// <summary>
    /// A X coordinate, usually to move to. This can be an Integer or Fractional number.
    /// </summary>
    public float? X {get; set;}
    /// <summary>
    /// A Y coordinate, usually to move to. This can be an Integer or Fractional number.
    /// </summary>
    public float? Y {get; set;}
    /// <summary>
    /// A Z coordinate, usually to move to. This can be an Integer or Fractional number.
    /// </summary>
    public float? Z {get; set;}
    /// <summary>
    /// Additional axis coordinates (RepRapFirmware)
    /// </summary>
    public float? U {get; set;}
    /// <summary>
    /// Additional axis coordinates (RepRapFirmware)
    /// </summary>
    public float? V {get; set;}
    /// <summary>
    /// Additional axis coordinates (RepRapFirmware)
    /// </summary>
    public float? W {get; set;}
    /// <summary>
    /// Parameter - X-offset in arc move; integral (Ki) in PID Tuning
    /// </summary>
    public float? I {get; set;}
    /// <summary>
    /// Parameter - Y-offset in arc move
    /// </summary>
    public float? J {get; set;}
    /// <summary>
    /// Parameter - used for diameter; derivative (Kd) in PID Tuning
    /// </summary>
    public float? D {get; set;}
    /// <summary>
    /// Parameter - used for heater number in PID Tuning
    /// </summary>
    public float? H {get; set;}
    /// <summary>
    /// Feedrate in mm per minute. (Speed of print head movement)
    /// </summary>
    public float? F {get; set;}
    /// <summary>
    /// Parameter - used for temperatures
    /// </summary>
    public float? R {get; set;}
    /// <summary>
    /// Parameter - not currently used
    /// </summary>
    public float? Q {get; set;}
    /// <summary>
    /// Length of extrude. This is exactly like X, Y and Z, but for the length of filament to consume.
    /// </summary>
    public float? E {get; set;}

    private string encode<T> (char name, T? value) where T:class {
        if (value != null) {
            return ' ' + (name + value.ToString());
        } else {
            return string.Empty;
        }
    }

    private string encode<T> (char name, T? value) where T:struct {
        if(value.HasValue) {
            return ' ' + (name + value.ToString());
        } else {
            return string.Empty;
        }
    }

    public override string ToString() {
        return $"{Type}{Number}{encode('S', S)}{encode('P', P)}{encode('T', Tool)}{encode('X', X)}{encode('Y', Y)}{encode('Z', Z)}{encode('U', U)}{encode('V', V)}{encode('W', W)}{encode('I', I)}{encode('J', J)}{encode('D', D)}{encode('H', H)}{encode('F', F)}{encode('R', R)}{encode('Q', Q)}{encode('E', E)}";
    }
    
}

/// <summary>
/// Serialize and deserialize GCode
/// </summary>
public class GCodeSerializer {
    /// <summary>
    /// MIME type for OBJ files
    /// </summary>
    public static readonly string AsciiMIME = "text/plain";

    public string Serialize(IEnumerable<GCode> commands) {
        int lineNumber = 1;
        StringBuilder sb = new StringBuilder();
        foreach (var code in commands) {
            if (code == null)
                continue;

            sb.Append('N'); sb.Append(lineNumber++); sb.Append(' ');
            sb.AppendLine(code.ToString());
        }
        return sb.ToString();
    }

    private static Regex gcodeRegex = new Regex(@"\s*(?:(?<Letter>(?:\w|\*))(?<Value>(?:\d+(?:\.\d+)?)))");

    public IEnumerable<GCode> Deserialize (TextReader reader) {
        string line;
        while ((line = reader.ReadLine()) != null) {
            // Foreach line
            // Decode line
            var parts = line.Split(';', 2);
            var code = parts[0];
            var comment = parts.Length > 1 ? parts[1] : null;

            var matches = gcodeRegex.Matches(code);
            GCode? g = null;
            foreach (Match match in matches) {
                g = g ?? new GCode();
                var letter = match.Groups["Letter"].Value[0];
                var argument = match.Groups["Value"].Value;

                switch (letter) {
                    case 'G':
                        g.Type = 'G';
                        g.Number = int.Parse(argument);
                        break;
                    case 'M':
                        g.Type = 'M';
                        g.Number = int.Parse(argument);
                        break;
                    case 'T':
                        g.Tool = int.Parse(argument);
                        break;
                    case 'S':
                        g.S = argument;
                        break;
                    case 'P':
                        g.P = argument;
                        break;
                    case 'X':
                        g.X = float.Parse(argument);
                        break;
                    case 'Y':
                        g.Y = float.Parse(argument);
                        break;
                    case 'Z':
                        g.Z = float.Parse(argument);
                        break;
                    case 'U':
                        g.U = float.Parse(argument);
                        break;
                    case 'V':
                        g.V = float.Parse(argument);
                        break;
                    case 'W':
                        g.W = float.Parse(argument);
                        break;
                    case 'I':
                        g.I = float.Parse(argument);
                        break;
                    case 'J':
                        g.J = float.Parse(argument);
                        break;
                    case 'D':
                        g.D = float.Parse(argument);
                        break;
                    case 'H':
                        g.H = float.Parse(argument);
                        break;
                    case 'F':
                        g.F = float.Parse(argument);
                        break;
                    case 'R':
                        g.R = float.Parse(argument);
                        break;
                    case 'Q':
                        g.Q = float.Parse(argument);
                        break;
                    case 'E':
                        g.E = float.Parse(argument);
                        break;
                }
            }
            if (g != null)
                yield return g;
        }
    }
}

}