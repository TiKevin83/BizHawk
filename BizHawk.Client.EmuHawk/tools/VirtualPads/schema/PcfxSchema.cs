﻿using System.Collections.Generic;
using System.Drawing;
using BizHawk.Emulation.Common;
using BizHawk.Emulation.Cores.Consoles.NEC.PCFX;

namespace BizHawk.Client.EmuHawk
{
	[Schema("PCFX")]
	// ReSharper disable once UnusedMember.Global
	public class PcfxSchema : IVirtualPadSchema
	{
		public IEnumerable<PadSchema> GetPadSchemas(IEmulator core)
		{
			var ss = ((Tst)core).GetSyncSettings();

			var schemas = new List<PadSchema>();
			if (ss.Port1 != ControllerType.None || ss.Port2 != ControllerType.None)
			{
				switch (ss.Port1)
				{
					case ControllerType.Gamepad:
						schemas.Add(StandardController(1));
						break;
					case ControllerType.Mouse:
						schemas.Add(Mouse(1));
						break;
				}

				int controllerNum = ss.Port1 != ControllerType.None ? 2 : 1;
				switch (ss.Port2)
				{
					case ControllerType.Gamepad:
						schemas.Add(StandardController(controllerNum));
						break;
					case ControllerType.Mouse:
						schemas.Add(Mouse(controllerNum));
						break;
				}
			}

			return schemas;
		}

		private static PadSchema StandardController(int controller)
		{
			return new PadSchema
			{
				IsConsole = false,
				DefaultSize = new Size(230, 100),
				Buttons = new[]
				{
					ButtonSchema.Up($"P{controller} Up", 34, 17),
					ButtonSchema.Down($"P{controller} Down", 34, 61),
					ButtonSchema.Left($"P{controller} Left", 22, 39),
					ButtonSchema.Right($"P{controller} Right", 44, 39),
					new ButtonSchema
					{
						Name = $"P{controller} Mode 1",
						DisplayName = "Mode 1",
						Location = new Point(74, 17)
					},
					new ButtonSchema
					{
						Name = $"P{controller} Mode 2",
						DisplayName = "Mode 2",
						Location = new Point(74, 40)
					},
					new ButtonSchema
					{
						Name = $"P{controller} Select",
						DisplayName = "s",
						Location = new Point(77, 63)
					},
					new ButtonSchema
					{
						Name = $"P{controller} Run",
						DisplayName = "R",
						Location = new Point(101, 63)
					},

					new ButtonSchema
					{
						Name = $"P{controller} IV",
						DisplayName = "IV",
						Location = new Point(140, 63)
					},
					new ButtonSchema
					{
						Name = $"P{controller} V",
						DisplayName = "V",
						Location = new Point(166, 53)
					},
					new ButtonSchema
					{
						Name = $"P{controller} VI",
						DisplayName = "VI",
						Location = new Point(192, 43)
					},
					new ButtonSchema
					{
						Name = $"P{controller} I",
						DisplayName = "I",
						Location = new Point(140, 40)
					},
					new ButtonSchema
					{
						Name = $"P{controller} II",
						DisplayName = "II",
						Location = new Point(166, 30)
					},
					new ButtonSchema
					{
						Name = $"P{controller} III",
						DisplayName = "III",
						Location = new Point(192, 20)
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
						Name = $"P{controller} Mouse Right",
						DisplayName = "Right",
						Location = new Point(300, 47)
					}
				}
			};
		}
	}
}
