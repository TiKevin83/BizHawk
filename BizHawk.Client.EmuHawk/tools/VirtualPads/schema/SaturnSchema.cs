﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using BizHawk.Emulation.Common;
using BizHawk.Emulation.Cores.Consoles.Sega.Saturn;

namespace BizHawk.Client.EmuHawk
{
	[Schema("SAT")]
	// ReSharper disable once UnusedMember.Global
	public class SaturnSchema : IVirtualPadSchema
	{
		public IEnumerable<PadSchema> GetPadSchemas(IEmulator core)
		{
			var ss = ((Saturnus)core).GetSyncSettings();

			int totalPorts = (ss.Port1Multitap ? 6 : 1) + (ss.Port2Multitap ? 6 : 1);

			var padSchemas = new[]
			{
				ss.Port1,
				ss.Port2,
				ss.Port3,
				ss.Port4,
				ss.Port5,
				ss.Port6,
				ss.Port7,
				ss.Port8,
				ss.Port9,
				ss.Port10,
				ss.Port11,
				ss.Port12
			}.Take(totalPorts)
			.Where(p => p != SaturnusControllerDeck.Device.None)
			.Select((p, i) => GenerateSchemaForPort(p, i + 1))
			.Where(s => s != null)
			.Concat(new[] { ConsoleButtons() });

			return padSchemas;
		}

		private static PadSchema GenerateSchemaForPort(SaturnusControllerDeck.Device device, int controllerNum)
		{
			switch (device)
			{
				default:
				case SaturnusControllerDeck.Device.None:
					return null;
				case SaturnusControllerDeck.Device.Gamepad:
					return StandardController(controllerNum);
				case SaturnusControllerDeck.Device.ThreeDeePad:
					return ThreeDeeController(controllerNum);
				case SaturnusControllerDeck.Device.Mouse:
					return Mouse(controllerNum);
				case SaturnusControllerDeck.Device.Wheel:
					return Wheel(controllerNum);
				case SaturnusControllerDeck.Device.Mission:
					return MissionControl(controllerNum);
				case SaturnusControllerDeck.Device.DualMission:
					return DualMissionControl(controllerNum);
				case SaturnusControllerDeck.Device.Keyboard:
					MessageBox.Show("This peripheral is not supported yet");
					return null;
			}
		}

		private static PadSchema StandardController(int controller)
		{
			return new PadSchema
			{
				IsConsole = false,
				DefaultSize = new Size(250, 100),
				Buttons = new[]
				{
					ButtonSchema.Up($"P{controller} Up", 34, 17),
					ButtonSchema.Down($"P{controller} Down", 34, 61),
					ButtonSchema.Left($"P{controller} Left", 22, 39),
					ButtonSchema.Right($"P{controller} Right", 44, 39),
					new ButtonSchema
					{
						Name = $"P{controller} Start",
						DisplayName = "S",
						Location = new Point(78, 52)
					},
					new ButtonSchema
					{
						Name = $"P{controller} A",
						DisplayName = "A",
						Location = new Point(110, 63)
					},
					new ButtonSchema
					{
						Name = $"P{controller} B",
						DisplayName = "B",
						Location = new Point(134, 53)
					},
					new ButtonSchema
					{
						Name = $"P{controller} C",
						DisplayName = "C",
						Location = new Point(158, 43)
					},
					new ButtonSchema
					{
						Name = $"P{controller} X",
						DisplayName = "X",
						Location = new Point(110, 40)
					},
					new ButtonSchema
					{
						Name = $"P{controller} Y",
						DisplayName = "Y",
						Location = new Point(134, 30)
					},
					new ButtonSchema
					{
						Name = $"P{controller} Z",
						DisplayName = "Z",
						Location = new Point(158, 20)
					},
					new ButtonSchema
					{
						Name = $"P{controller} L",
						DisplayName = "L",
						Location = new Point(2, 10)
					},
					new ButtonSchema
					{
						Name = $"P{controller} R",
						DisplayName = "R",
						Location = new Point(184, 10)
					}
				}
			};
		}

		private static PadSchema ThreeDeeController(int controller)
		{
			var axisRanges = SaturnusControllerDeck.ThreeDeeAxisRanges;
			return new PadSchema
			{
				IsConsole = false,
				DefaultSize = new Size(458, 285),
				Buttons = new[]
				{
					ButtonSchema.Up($"P{controller} Up", 290, 77),
					ButtonSchema.Down($"P{controller} Down", 290, 121),
					ButtonSchema.Left($"P{controller} Left", 278, 99),
					ButtonSchema.Right($"P{controller} Right", 300, 99),
					new ButtonSchema
					{
						Name = $"P{controller} Start",
						DisplayName = "S",
						Location = new Point(334, 112)
					},
					new ButtonSchema
					{
						Name = $"P{controller} A",
						DisplayName = "A",
						Location = new Point(366, 123)
					},
					new ButtonSchema
					{
						Name = $"P{controller} B",
						DisplayName = "B",
						Location = new Point(390, 113)
					},
					new ButtonSchema
					{
						Name = $"P{controller} C",
						DisplayName = "C",
						Location = new Point(414, 103)
					},
					new ButtonSchema
					{
						Name = $"P{controller} X",
						DisplayName = "X",
						Location = new Point(366, 100)
					},
					new ButtonSchema
					{
						Name = $"P{controller} Y",
						DisplayName = "Y",
						Location = new Point(390, 90)
					},
					new ButtonSchema
					{
						Name = $"P{controller} Z",
						DisplayName = "Z",
						Location = new Point(414, 80)
					},
					new ButtonSchema
					{
						Name = $"P{controller} Stick Horizontal",
						SecondaryNames = new[] { $"P{controller} Stick Vertical" },
						AxisRange = axisRanges[0],
						SecondaryAxisRange = axisRanges[1],
						Location = new Point(6, 74),
						Type = PadInputType.AnalogStick
					},
					new ButtonSchema
					{
						Name = $"P{controller} Left Shoulder",
						DisplayName = "L",
						Location = new Point(8, 12),
						Type = PadInputType.FloatSingle,
						TargetSize = new Size(128, 55),
						MinValue = 0,
						MaxValue = 255
					},
					new ButtonSchema
					{
						Name = $"P{controller} Right Shoulder",
						DisplayName = "L",
						Location = new Point(328, 12),
						Type = PadInputType.FloatSingle,
						TargetSize = new Size(128, 55),
						MinValue = 0,
						MaxValue = 255
					}
				}
			};
		}

		private static PadSchema Mouse(int controller)
		{
			return new PadSchema
			{
				DisplayName = "Mouse",
				IsConsole = false,
				DefaultSize = new Size(375, 320),
				Buttons = new[]
				{
					new ButtonSchema
					{
						Name = $"P{controller} X",
						SecondaryNames = new[] { $"P{controller} Y" },
						Location = new Point(14, 17),
						Type = PadInputType.TargetedPair,
						TargetSize = new Size(256, 256)
					},
					new ButtonSchema
					{
						Name = $"P{controller} Mouse Left",
						DisplayName = "Left",
						Location = new Point(300, 17)
					},
					new ButtonSchema
					{
						Name = $"P{controller} Mouse Center",
						DisplayName = "Center",
						Location = new Point(300, 47)
					},
					new ButtonSchema
					{
						Name = $"P{controller} Mouse Right",
						DisplayName = "Right",
						Location = new Point(300, 77)
					},
					new ButtonSchema
					{
						Name = $"P{controller} Start",
						DisplayName = "Start",
						Location = new Point(300, 107)
					}
				}
			};
		}

		private static PadSchema Wheel(int controller)
		{
			return new PadSchema
			{
				DisplayName = "Wheel",
				IsConsole = false,
				DefaultSize = new Size(325, 100),
				Buttons = new[]
				{
					new ButtonSchema
					{
						Name = $"P{controller} Wheel",
						DisplayName = "Wheel",
						Location = new Point(8, 12),
						Type = PadInputType.FloatSingle,
						TargetSize = new Size(128, 55),
						MinValue = 0,
						MaxValue = 255
					},
					ButtonSchema.Up($"P{controller} Up", 150, 20),
					ButtonSchema.Down($"P{controller} Down", 150, 43),
					new ButtonSchema
					{
						Name = $"P{controller} A",
						DisplayName = "A",
						Location = new Point(180, 63)
					},
					new ButtonSchema
					{
						Name = $"P{controller} B",
						DisplayName = "B",
						Location = new Point(204, 53)
					},
					new ButtonSchema
					{
						Name = $"P{controller} C",
						DisplayName = "C",
						Location = new Point(228, 43)
					},
					new ButtonSchema
					{
						Name = $"P{controller} X",
						DisplayName = "X",
						Location = new Point(180, 40)
					},
					new ButtonSchema
					{
						Name = $"P{controller} Y",
						DisplayName = "Y",
						Location = new Point(204, 30)
					},
					new ButtonSchema
					{
						Name = $"P{controller} Z",
						DisplayName = "Z",
						Location = new Point(228, 20)
					},
					new ButtonSchema
					{
						Name = $"P{controller} Start",
						DisplayName = "Start",
						Location = new Point(268, 20)
					}
				}

			};
		}

		private static PadSchema MissionControl(int controller)
		{
			var axisRanges = SaturnusControllerDeck.MissionAxisRanges;
			return new PadSchema
			{
				DisplayName = "Mission",
				IsConsole = false,
				DefaultSize = new Size(445, 230),
				Buttons = new[]
				{
					new ButtonSchema
					{
						Name = $"P{controller} Start",
						DisplayName = "Start",
						Location = new Point(45, 15)
					},
					new ButtonSchema
					{
						Name = $"P{controller} L",
						DisplayName = "L",
						Location = new Point(5, 58)
					},
					new ButtonSchema
					{
						Name = $"P{controller} R",
						DisplayName = "R",
						Location = new Point(105, 58)
					},
					new ButtonSchema
					{
						Name = $"P{controller} X",
						DisplayName = "X",
						Location = new Point(30, 43)
					},
					new ButtonSchema
					{
						Name = $"P{controller} Y",
						DisplayName = "Y",
						Location = new Point(55, 43)
					},
					new ButtonSchema
					{
						Name = $"P{controller} Z",
						DisplayName = "Z",
						Location = new Point(80, 43)
					},
					new ButtonSchema
					{
						Name = $"P{controller} A",
						DisplayName = "A",
						Location = new Point(30, 70)
					},
					new ButtonSchema
					{
						Name = $"P{controller} B",
						DisplayName = "B",
						Location = new Point(55, 70)
					},
					new ButtonSchema
					{
						Name = $"P{controller} C",
						DisplayName = "C",
						Location = new Point(80, 70)
					},
					new ButtonSchema
					{
						Name = $"P{controller} Stick Horizontal",
						SecondaryNames = new[] { $"P{controller} Stick Vertical" },
						AxisRange = axisRanges[0],
						SecondaryAxisRange = axisRanges[1],
						Location = new Point(185, 13),
						Type = PadInputType.AnalogStick
					},
					new ButtonSchema
					{
						Name = $"P{controller} Throttle",
						DisplayName = "Throttle",
						Location = new Point(135, 13),
						Type = PadInputType.FloatSingle,
						TargetSize = new Size(64, 178),
						MinValue = 0,
						MaxValue = 255,
						Orientation = Orientation.Vertical
					}
				}
			};
		}

		private static PadSchema DualMissionControl(int controller)
		{
			var axisRanges = SaturnusControllerDeck.DualMissionAxisRanges;
			return new PadSchema
			{
				DisplayName = "Dual Mission",
				IsConsole = false,
				DefaultSize = new Size(680, 230),
				Buttons = new[]
				{
					new ButtonSchema
					{
						Name = $"P{controller} Left Stick Horizontal",
						SecondaryNames = new[] { $"P{controller} Left Stick Vertical" },
						AxisRange = axisRanges[3],
						SecondaryAxisRange = axisRanges[4],
						Location = new Point(58, 13),
						Type = PadInputType.AnalogStick
					},
					new ButtonSchema
					{
						Name = $"P{controller} Left Throttle",
						DisplayName = "Throttle",
						Location = new Point(8, 13),
						Type = PadInputType.FloatSingle,
						TargetSize = new Size(64, 178),
						MinValue = 0,
						MaxValue = 255,
						Orientation = Orientation.Vertical
					},
					new ButtonSchema
					{
						Name = $"P{controller} Right Stick Horizontal",
						SecondaryNames = new[] { $"P{controller} Right Stick Vertical" },
						AxisRange = axisRanges[0],
						SecondaryAxisRange = axisRanges[1],
						Location = new Point(400, 13),
						Type = PadInputType.AnalogStick
					},
					new ButtonSchema
					{
						Name = $"P{controller} Right Throttle",
						DisplayName = "Throttle",
						Location = new Point(350, 13),
						Type = PadInputType.FloatSingle,
						TargetSize = new Size(64, 178),
						MinValue = 0,
						MaxValue = 255,
						Orientation = Orientation.Vertical
					}
				}
			};
		}

		private static PadSchema ConsoleButtons()
		{
			return new PadSchema
			{
				DisplayName = "Console",
				IsConsole = true,
				DefaultSize = new Size(250, 50),
				Buttons = new[]
				{
					new ButtonSchema
					{
						Name = "Reset",
						Location = new Point(10, 15)
					},
					new ButtonSchema
					{
						Name = "Power",
						Location = new Point(58, 15)
					},
					new ButtonSchema
					{
						Name = "Previous Disk",
						DisplayName = "Prev Disc",
						Location = new Point(108, 15)
					},
					new ButtonSchema
					{
						Name = "Next Disk",
						DisplayName = "Next Disc",
						Location = new Point(175, 15)
					}
				}
			};
		}
	}
}
