using System.Diagnostics;

namespace GOALWorker
{
    public static class LightNotifier
    {
        //TODO: Jared Implement call to each light here for their team
        public static void NotifyLightsForTeam(string teamname)
        {
            Trace.TraceInformation("GOALWorker is Sending Light Request for {0}", teamname);
        }
    }
}
