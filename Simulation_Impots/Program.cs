using System;
using Gtk;

namespace Simulation_Impots
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Application.Init();
            MainWindow win = new MainWindow();
            win.Show();
            win.Title = "Calcul d'impôts au Sénégal";
            Application.Run();
        }
    }
}
