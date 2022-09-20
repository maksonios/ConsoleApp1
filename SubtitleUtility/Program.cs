using System;
using System.IO;
using SubtitleUtility;

var path = Environment.GetCommandLineArgs()[1];
var newPath = Environment.GetCommandLineArgs()[2];
var shiftMs = Convert.ToInt32(Environment.GetCommandLineArgs()[3]);
var subsNumering = Convert.ToBoolean(Environment.GetCommandLineArgs()[4]);

var input = File.ReadAllLines(path);
var output = SubtitleModifier.ExecuteSubtitleShift(input, shiftMs, subsNumering);
File.WriteAllText(newPath, output);