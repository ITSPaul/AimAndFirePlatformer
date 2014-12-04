using System;

namespace AimAndFireExample
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (ChaseandFire game = new ChaseandFire())
            {
                game.Run();
            }
        }
    }
#endif
}

