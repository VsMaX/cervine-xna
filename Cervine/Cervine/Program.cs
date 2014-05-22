using System;

namespace Cervine
{
#if WINDOWS || XBOX
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

