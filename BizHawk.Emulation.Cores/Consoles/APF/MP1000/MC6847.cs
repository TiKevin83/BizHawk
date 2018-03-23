﻿using System;
using BizHawk.Emulation.Common;
using BizHawk.Common.NumberExtensions;
using BizHawk.Common;

namespace BizHawk.Emulation.Cores.APF.MP1000
{
	// Emulates the MC6847 graphics chip
	public class MC6847
	{
		public MP1000 Core { get; set; }

		public MC6847()
		{
			build_char_set_func();
			Reset();
		}

		public int[] _palette;
		public byte[] char_set = new byte[2048];

		// the graphics chip can directly access memory
		public Func<ushort, byte> ReadMemory;

		public int cycle;
		public int scanline;

		// variables for drawing a pixel
		int color;
		int local_GFX_index;
		int temp_palette;
		int temp_bit_0;
		int temp_bit_1;
		int disp_mode;
		int pixel;

		// each frame contains 263 scanlines
		// each scanline consists of 454 ppu cycles

		public void RunFrame()
		{

		}

		public void Reset()
		{

		}


		public void SyncState(Serializer ser)
		{
			ser.BeginSection("Maria");

			ser.EndSection();
		}

		public void build_char_set_func()
		{
			//the first half of the character set is normal characters, the second half is color inverted ones
			for (int i = 0; i < 0x40; i++)
			{
				for (int j = 0; j < 16; j++)
				{
					if (j < 2)
					{
						char_set[i * 16 + j] = 0;
					}
					else if (j < 9)
					{
						char_set[i * 16 + j] = char_set_builder[i * 7 + (j - 2)];
					}
					else if (j < 12)
					{
						char_set[i * 16 + j] = 0;
					}
					else
					{
						char_set[i * 16 + j] = 0xFF;
					}				
				}
			}

			for (int i = 0; i < 0x40; i++)
			{
				for (int j = 0; j < 16; j++)
				{
					if (j < 2)
					{
						char_set[i * 16 + j + 1024] = 0x3F;
					}
					else if (j < 9)
					{
						char_set[i * 16 + j + 1024] = (byte)(~char_set[i * 16 + j] & 0x3F);
					}
					else if (j < 12)
					{
						char_set[i * 16 + j + 1024] = 0x3F;
					}
					else
					{
						char_set[i * 16 + j + 1024] = 0xFF;
					}
				}
			}
		}

		// NOTE: This is NOT the official character set, it's just a generic 7x5 ascii character set
		// it may not be representative of the MC6847 character set bitmaps

		// only store the non-zero parts of the character set, since we are building the complete thing with inverted
		// versions at startup
		public static byte[] char_set_builder = {0x1C,0x22,0x2E,0x2A,0x2A,0x26,0x1C,
												0x08,0x14,0x14,0x22,0x3E,0x22,0x22,
												0x3C,0x22,0x22,0x3C,0x22,0x22,0x3C,
												0x1C,0x22,0x20,0x20,0x20,0x22,0x1C,
												0x38,0x24,0x22,0x22,0x22,0x24,0x38,
												0x3E,0x20,0x20,0x3C,0x20,0x20,0x3E,
												0x3E,0x20,0x20,0x38,0x20,0x20,0x20,
												0x1C,0x20,0x20,0x2E,0x22,0x22,0x1C,
												0x22,0x22,0x22,0x3E,0x22,0x22,0x22,
												0x1C,0x08,0x08,0x08,0x08,0x08,0x1C,
												0x1C,0x08,0x08,0x08,0x08,0x28,0x10,
												0x22,0x24,0x28,0x30,0x28,0x24,0x22,
												0x20,0x20,0x20,0x20,0x20,0x20,0x3C,
												0x22,0x36,0x2A,0x2A,0x22,0x22,0x22,
												0x22,0x32,0x2A,0x2A,0x26,0x22,0x22,
												0x1C,0x22,0x22,0x22,0x22,0x22,0x1C,
												0x3C,0x22,0x22,0x3C,0x20,0x20,0x20,
												0x1C,0x22,0x22,0x22,0x2A,0x24,0x1A,
												0x3C,0x22,0x22,0x3C,0x28,0x24,0x22,
												0x1C,0x22,0x20,0x1C,0x02,0x22,0x1C,
												0x3E,0x08,0x08,0x08,0x08,0x08,0x08,
												0x22,0x22,0x22,0x22,0x22,0x22,0x1C,
												0x22,0x22,0x22,0x22,0x14,0x14,0x08,
												0x22,0x22,0x22,0x2A,0x2A,0x36,0x22,
												0x22,0x14,0x14,0x08,0x14,0x14,0x22,
												0x22,0x22,0x14,0x14,0x08,0x08,0x08,
												0x3E,0x02,0x04,0x08,0x10,0x20,0x3E,
												0x1C,0x10,0x10,0x10,0x10,0x10,0x1C,
												0x20,0x10,0x10,0x08,0x04,0x04,0x02,
												0x1C,0x04,0x04,0x04,0x04,0x04,0x1C,
												0x08,0x1C,0x2A,0x08,0x08,0x08,0x08,
												0x08,0x10,0x3E,0x10,0x08,0x00,0x00,
												0x00,0x00,0x00,0x00,0x00,0x00,0x00,
												0x08,0x08,0x08,0x08,0x08,0x00,0x08,
												0x14,0x14,0x14,0x00,0x00,0x00,0x00,
												0x14,0x14,0x3E,0x14,0x3E,0x14,0x14,
												0x08,0x1E,0x20,0x1C,0x02,0x3C,0x08,
												0x22,0x24,0x04,0x08,0x10,0x12,0x22,
												0x10,0x28,0x28,0x10,0x2A,0x24,0x1A,
												0x18,0x18,0x00,0x00,0x00,0x00,0x00,
												0x08,0x10,0x20,0x20,0x20,0x10,0x08,
												0x10,0x08,0x04,0x04,0x04,0x08,0x10,
												0x00,0x08,0x1C,0x3E,0x1C,0x08,0x00,
												0x00,0x08,0x08,0x3E,0x08,0x08,0x00,
												0x00,0x00,0x00,0x00,0x04,0x04,0x08,
												0x00,0x00,0x00,0x3E,0x00,0x00,0x00,
												0x00,0x00,0x00,0x00,0x00,0x18,0x18,
												0x02,0x04,0x04,0x08,0x10,0x10,0x20,
												0x0C,0x12,0x12,0x12,0x12,0x12,0x0C,
												0x08,0x18,0x08,0x08,0x08,0x08,0x1C,
												0x1C,0x22,0x02,0x0C,0x10,0x20,0x3E,
												0x1C,0x02,0x02,0x1C,0x02,0x02,0x1C,
												0x24,0x24,0x24,0x3E,0x04,0x04,0x04,
												0x3E,0x20,0x20,0x1C,0x02,0x22,0x1C,
												0x1C,0x20,0x20,0x3C,0x22,0x22,0x1C,
												0x3E,0x02,0x04,0x08,0x10,0x20,0x20,
												0x1C,0x22,0x22,0x1C,0x22,0x22,0x1C,
												0x1C,0x22,0x22,0x1E,0x02,0x02,0x1C,
												0x00,0x18,0x18,0x00,0x18,0x18,0x00,
												0x00,0x18,0x18,0x00,0x18,0x08,0x10,
												0x02,0x04,0x08,0x10,0x08,0x04,0x02,
												0x00,0x00,0x3E,0x00,0x3E,0x00,0x00,
												0x20,0x10,0x08,0x04,0x08,0x10,0x20,
												0x1C,0x22,0x22,0x04,0x08,0x00,0x08};
	}
}
