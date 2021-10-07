using SimpleCampaignModule.CampaignModuleStarter.Persistence;

namespace SimpleCampaignModule.Tests.Persistence
{
    public abstract class IMockDatabase: IDatabase
    {
        public abstract void ClearCaches();        
    }
}