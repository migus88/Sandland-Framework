using System;
using Sandland.Domains.Core.Interfaces.Services;

namespace Sandland.Domains.Core.Services
{
    internal class RandomService : IRandomService
    {
        private Random _random;

        public RandomService(int seed)
        {
            Reset(seed);
        }

        public void Reset(int seed)
        {
            _random = new Random(seed);
        }

        public void Reset()
        {
            _random = new Random();
        }

        public int NextInt()
        {
            return _random.Next();
        }

        public int NextInt(int maxValue)
        {
            return _random.Next(maxValue);
        }

        public int NextInt(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }

        public float NextFloat()
        {
            return (float)_random.NextDouble();
        }

        public float NextFloat(float maxValue)
        {
            return (float)_random.NextDouble() * maxValue;
        }

        public float NextFloat(float minValue, float maxValue)
        {
            return (float)_random.NextDouble() * (maxValue - minValue) + minValue;
        }

        public void Dispose()
        {
            // TODO release managed resources here
        }
    }
}