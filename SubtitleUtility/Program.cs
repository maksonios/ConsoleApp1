using Microsoft.Extensions.DependencyInjection;
using SubtitleUtility.Interfaces;
using SubtitleUtility.Services;

var path = Environment.GetCommandLineArgs()[1];
var newPath = Environment.GetCommandLineArgs()[2];
var shiftMs = Convert.ToInt32(Environment.GetCommandLineArgs()[3]);
var subsNumbering = Convert.ToBoolean(Environment.GetCommandLineArgs()[4]);

var services = new ServiceCollection();
services.AddTransient<ISubtitleManager, SubtitleModifier>();
var provider = services.BuildServiceProvider();

var subtitleManager = provider.GetRequiredService<ISubtitleManager>();

var input = File.ReadAllText(path);
var output = subtitleManager.ExecuteSubtitleShift(input, shiftMs, isSubtitleNumberingEnabled: subsNumbering);
File.WriteAllText(newPath, output);

