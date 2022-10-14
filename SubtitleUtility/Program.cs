using SubtitleUtility;

var path = Environment.GetCommandLineArgs()[1];
var newPath = Environment.GetCommandLineArgs()[2];
var shiftMs = Convert.ToInt32(Environment.GetCommandLineArgs()[3]);
var subsNumbering = Convert.ToBoolean(Environment.GetCommandLineArgs()[4]);

var input = File.ReadAllText(path);
var output = SubtitleModifier.ExecuteSubtitleShift(input, shiftMs, isSubtitleNumberingEnabled: subsNumbering);
File.WriteAllText(newPath, output);

