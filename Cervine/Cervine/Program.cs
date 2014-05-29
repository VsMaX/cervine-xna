using System;

namespace Cervine
{
#if WINDOWS || XBOX
    /// <summary>
    /// Entry point of whole game
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (CervineGame game = new CervineGame())
            {
                game.Run();
            }
        }
    }
#endif
}

