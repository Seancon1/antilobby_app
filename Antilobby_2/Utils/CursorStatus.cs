namespace Antilobby_2.Utils
{
    class CursorStatus
    {
        int x = 0;
        int y = 0;
        int idleCount = 0;

        public CursorStatus()
        {

        }

        /// <summary>
        /// Compares previous coordinate with actual coordinates. Increases tick count if previous location stays the same.
        /// </summary>
        /// <param name="x">X coordinate for cursor location</param>
        /// <param name="y">Y coordinate for cursor location</param>
        /// <returns></returns>
        public bool isCursorIdle(int x, int y)
        {

            if (this.x == x && this.y == y)
            {
                idleCount++;
                return true;
            }
            else
            {
                this.x = x;
                this.y = y;
                idleCount = 0;
            }

            return false;
        }

        public int getIdleCount() { return idleCount; }

    }
}
