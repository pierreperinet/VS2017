using System.Collections.Generic;

namespace MankalaLib
{
    public enum BoxName { A1, A2, A3, A4, A5, A6, AG, B1, B2, B3, B4, B5, B6, BG }

    public class Box
    {
        public int Value { get; set; }
        public BoxName Name { get; set; }
        public Box Next { get; set; }
        public Box Goal { get; set; }
        public Box Link { get; set; }

        private static IList<BoxName> ABoxNames = new List<BoxName> { BoxName.A1, BoxName.A2, BoxName.A3, BoxName.A4, BoxName.A5, BoxName.A6, BoxName.AG };
        private static IList<BoxName> BBoxNames = new List<BoxName> { BoxName.B1, BoxName.B2, BoxName.B3, BoxName.B4, BoxName.B5, BoxName.B6, BoxName.BG };

        public BoxName Click(BoxName who)
        {
            var content = Value;
            Value = 0;
            return Next.Play(content, who);
        }

        /// <summary>
        /// Play this box with n stones.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="who"></param>
        /// <returns>who must play next</returns>
        private BoxName Play(int n, BoxName who)
        {
            if (n == 0)
            {
                return ABoxNames.Contains(who) ? BoxName.BG : BoxName.AG;
            }
            if (Goal == null || Link == null)
            {
                // this box is a goal
                if (ABoxNames.Contains(Name) && !ABoxNames.Contains(who) || BBoxNames.Contains(Name) && !BBoxNames.Contains(who))
                {
                    // Skip goal
                    return Next.Play(n, who);
                }
                Value = Value + 1;
                if (n == 1)
                {
                    return ABoxNames.Contains(who) ? BoxName.AG : BoxName.BG;
                }
                return Next.Play(n - 1, who);
            }
            var content = Value;
            if (n == 1 && content == 0 && (ABoxNames.Contains(Name) && ABoxNames.Contains(who) || BBoxNames.Contains(Name) && BBoxNames.Contains(who)))
            {
                // collect opposite content
                var g = Goal.Value;
                var l = Link.Value;
                Link.Value = 0;
                Goal.Value = g + l + 1;
                return ABoxNames.Contains(who) ? BoxName.BG : BoxName.AG;
            }
            Value = content + 1;
            return Next.Play(n - 1, who);
        }
    }
}