namespace Sandland.Domains.Core.Interfaces.Services
{
    public interface IRandomService
    {
        void Reset(int seed);
        void Reset();
        
        int NextInt();
        int NextInt(int maxValue);
        int NextInt(int minValue, int maxValue);
        
        float NextFloat();
        float NextFloat(float maxValue);
        float NextFloat(float minValue, float maxValue);
    }
}