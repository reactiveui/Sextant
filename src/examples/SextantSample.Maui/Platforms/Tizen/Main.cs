using System;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace SextantSample.Maui;
    /// <summary>Starts the native application host.</summary>
internal class Program : MauiApplication
    {
        /// <inheritdoc />
protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        static void Main(string[] args)
        {
            var app = new Program();
            app.Run(args);
        }
    }
