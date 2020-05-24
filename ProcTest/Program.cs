// This file is part of Aximo, a Game Engine written in C#. Web: https://github.com/AximoGames
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aximo.Engine;
using Aximo.Engine.Windows;
using Gtk;
using OpenToolkit;
using OpenToolkit.Mathematics;
using OpenToolkit.Windowing.Common;

namespace Aximo.ProcTest
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var config = new ApplicationConfig
            {
                WindowTitle = "ProcTest",
                WindowSize = new Vector2i(800, 600),
                WindowBorder = WindowBorder.Resizable,
                //RenderFrequency = 490,
                //UpdateFrequency = 490,
                //VSync = VSyncMode.Off,
                // UseGtkUI = true,
                UseConsole = true,
                IsMultiThreaded = false,
            };

            new Startup<ProcTestApplication, GtkUI>(config).Start();
        }
    }
}
