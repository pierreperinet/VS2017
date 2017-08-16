using System.Collections.Generic;

namespace MankalaLib
{
    public class Game
    {
        public int ID { get; set; }
        public Box A1 { get; set; }
        public Box A2 { get; set; }
        public Box A3 { get; set; }
        public Box A4 { get; set; }
        public Box A5 { get; set; }
        public Box A6 { get; set; }
        public Box AG { get; set; }
        public Box B1 { get; set; }
        public Box B2 { get; set; }
        public Box B3 { get; set; }
        public Box B4 { get; set; }
        public Box B5 { get; set; }
        public Box B6 { get; set; }
        public Box BG { get; set; }
        public string Message { get; set; }

        public List<BoxName> Steps { get; set; }

        public Game()
        {
            A1 = new Box();
            A2 = new Box();
            A3 = new Box();
            A4 = new Box();
            A5 = new Box();
            A6 = new Box();
            AG = new Box();
            B1 = new Box();
            B2 = new Box();
            B3 = new Box();
            B4 = new Box();
            B5 = new Box();
            B6 = new Box();
            BG = new Box();

            A1.Name = BoxName.A1;
            A2.Name = BoxName.A2;
            A3.Name = BoxName.A3;
            A4.Name = BoxName.A4;
            A5.Name = BoxName.A5;
            A6.Name = BoxName.A6;
            AG.Name = BoxName.AG;
            B1.Name = BoxName.B1;
            B2.Name = BoxName.B2;
            B3.Name = BoxName.B3;
            B4.Name = BoxName.B4;
            B5.Name = BoxName.B5;
            B6.Name = BoxName.B6;
            BG.Name = BoxName.BG;

            A1.Next = A2;
            A2.Next = A3;
            A3.Next = A4;
            A4.Next = A5;
            A5.Next = A6;
            A6.Next = AG;
            AG.Next = B1;
            B1.Next = B2;
            B2.Next = B3;
            B3.Next = B4;
            B4.Next = B5;
            B5.Next = B6;
            B6.Next = BG;
            BG.Next = A1;

            A1.Goal = AG;
            A2.Goal = AG;
            A3.Goal = AG;
            A4.Goal = AG;
            A5.Goal = AG;
            A6.Goal = AG;
            AG.Goal = null;
            B1.Goal = BG;
            B2.Goal = BG;
            B3.Goal = BG;
            B4.Goal = BG;
            B5.Goal = BG;
            B6.Goal = BG;
            BG.Goal = null;

            A1.Link = B6;
            A2.Link = B5;
            A3.Link = B4;
            A4.Link = B3;
            A5.Link = B2;
            A6.Link = B1;
            AG.Link = null;
            B1.Link = A6;
            B2.Link = A5;
            B3.Link = A4;
            B6.Link = A1;
            B4.Link = A3;
            B5.Link = A2;
            BG.Link = null;

            Steps = new List<BoxName>();

            Reset();
        }

        public Box GetBox(BoxName boxName)
        {
            var box = A1;
            do
            {
                if (boxName == box.Name)
                {
                    return box;
                }
                box = box.Next;
            }
            while (box != A1);
            return box;
        }

        public BoxName? GetWinner()
        {
            if (A1.Value == 0 &&
                A2.Value == 0 &&
                A3.Value == 0 &&
                A4.Value == 0 &&
                A5.Value == 0 &&
                A6.Value == 0 ||
                B1.Value == 0 &&
                B2.Value == 0 &&
                B3.Value == 0 &&
                B4.Value == 0 &&
                B5.Value == 0 &&
                B6.Value == 0)
            {
                if (GetAGains() >= GetBGains())
                    return BoxName.AG;
                return BoxName.BG;
            }
            return null;
        }

        public int GetAGains()
        {
            return  A1.Value +
                    A2.Value +
                    A3.Value +
                    A4.Value +
                    A5.Value +
                    A6.Value +
                    AG.Value;
        }

        public int GetBGains()
        {
            return  B1.Value +
                    B2.Value +
                    B3.Value +
                    B4.Value +
                    B5.Value +
                    B6.Value +
                    BG.Value;
        }

        public void Reset()
        {
            A1.Value = 4;
            A2.Value = 4;
            A3.Value = 4;
            A4.Value = 4;
            A5.Value = 4;
            A6.Value = 4;
            AG.Value = 0;
            B1.Value = 4;
            B2.Value = 4;
            B3.Value = 4;
            B4.Value = 4;
            B5.Value = 4;
            B6.Value = 4;
            BG.Value = 0;
            Message = "Your turn!";
        }
    }
}