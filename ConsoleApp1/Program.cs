using ConsoleApp1;

var path = Environment.GetCommandLineArgs()[1];
var newPath = Environment.GetCommandLineArgs()[2];
var shiftMs = Convert.ToInt32(Environment.GetCommandLineArgs()[3]);

SubtitleModifier.ExecuteSubtitleShift(path, newPath, shiftMs);