using System;

namespace TheLegendOfZigmundREVAMP
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (TheLegendOfZigmund game = new TheLegendOfZigmund())
            {
                game.Run();
            }
        }
    }
#endif
}

