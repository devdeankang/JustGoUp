using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StrategyFactory
{
    static Dictionary<Type, Func<StrategyConfig, IObstacleStrategy>> strategyMap = 
        new Dictionary<Type, Func<StrategyConfig, IObstacleStrategy>>();

    static StrategyFactory()
    {
        RegisterStrategy<SpikesConfig>(config => new SpikesStrategy(config as SpikesConfig));
        RegisterStrategy<SwingConfig>(config => new SwingStrategy(config as SwingConfig));
        RegisterStrategy<PushConfig>(config => new PushStrategy(config as PushConfig));
        RegisterStrategy<DisappearConfig>(config => new DisappearStrategy(config as DisappearConfig));
        RegisterStrategy<TurningGrinderConfig>(config => new TurningGrinderStrategy(config as TurningGrinderConfig));

        RegisterStrategy<TimeLimitedConfig>(config =>
        {
            var timeLimitedConfig = config as TimeLimitedConfig;
            var innerStrategy = CreateStrategy(timeLimitedConfig.innerStrategyConfig);
            return new TimeLimitedStrategy(innerStrategy, timeLimitedConfig.timeLimit);
        });
    }

    public static void RegisterStrategy<T>(Func<StrategyConfig, IObstacleStrategy> factoryMethod) where T : StrategyConfig
    {
        strategyMap[typeof(T)] = factoryMethod;
    }

    public static IObstacleStrategy CreateStrategy(StrategyConfig config)
    {
        if (config == null) return null;

        var configType = config.GetType();
        if (strategyMap.TryGetValue(configType, out var factoryMethod))
        {
            return factoryMethod(config);
        }

        Debug.LogWarning($"StrategyConfig Type is not registered : {configType}");
        return null;
    }

    public static TimeLimitedConfig CreateTimeLimitedConfig(StrategyConfig innerConfig, float timeLimit)
    {
        TimeLimitedConfig config = ScriptableObject.CreateInstance<TimeLimitedConfig>();
        config.innerStrategyConfig = innerConfig;
        config.timeLimit = timeLimit;
        return config;
    }
}
