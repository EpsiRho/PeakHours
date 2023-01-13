using PeakHoursServer.Classes;
public class Program
{
    private static void Main(string[] args)
    {
        // Start display thread
        Thread d = new Thread(Display.Loop);
        d.Start();
        
        // Load records if needed, initalize records if no file is found.
        Records.LoadEntries();

        // Start Beacon
        Thread b = new Thread(Networking.Beacon);
        b.Start();
    }
}