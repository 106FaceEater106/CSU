using System;
using System.Text;
 
namespace hashes
{
    public class GhostsTask :
        IFactory<Document>, IFactory<Vector>, 
        IFactory<Segment>, IFactory<Cat>,
        IFactory<Robot>, IMagic
    {
        byte[] arr = { 2, 2, 3 };
        Cat cat = new Cat("Monya", "Sphynx", DateTime.MaxValue);
        Robot bender = new Robot("Bender");
        Document page;
        Segment bit;
        Vector vektor = new Vector(0, 0);
 
        public GhostsTask()
        {
            bit = new Segment(vektor, vektor);
            page = new Document("Technical direction", Encoding.Unicode, arr);
        }
 
        public void DoMagic()
        {
            arr[0] = 15;
            cat.Rename("Goretz");
            vektor.Add(new Vector(3, 28));
            Robot.BatteryCapacity++;
        }
  
        Document IFactory<Document>.Create()
        {
            return page;
        }
 
        Vector IFactory<Vector>.Create()
        {
            return vektor;
        }

        Segment IFactory<Segment>.Create()
        {
            return bit;
        }
 
        Cat IFactory<Cat>.Create()
        {
            return cat;
        }
 
        Robot IFactory<Robot>.Create()
        {
            return bender;
        }
    }
}