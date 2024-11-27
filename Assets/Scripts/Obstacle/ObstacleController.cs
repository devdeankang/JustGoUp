using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public List<StrategyConfig> strategiesConfig;
    private List<IObstacleStrategy> strategies = new List<IObstacleStrategy>();

    void Start()
    {
        InitializeStrategies();
    }

    void Update()
    {
        ExecuteStrategies();
    }

    private void InitializeStrategies()
    {
        foreach (var config in strategiesConfig)
        {
            var strategy = StrategyFactory.CreateStrategy(config);
            if (strategy != null)
            {
                strategies.Add(strategy);
            }
        }
    }

    private void ExecuteStrategies()
    {
        foreach (var strategy in strategies)
        {
            strategy.Execute(this);
        }
    }

    public void LoadStageStrategies(string stageName)
    {
        strategies.Clear();

        var configs = Resources.LoadAll<StrategyConfig>($"ObstacleConfigs/{stageName}"); // ##
        foreach (var config in configs)
        {
            var strategy = StrategyFactory.CreateStrategy(config);
            if (strategy != null)
            {
                strategies.Add(strategy);
            }
        }
    }

    public void AddTimeLimitedStrategy(StrategyConfig innerConfig, float timeLimit)
    {
        TimeLimitedConfig timeLimitedConfig = StrategyFactory.CreateTimeLimitedConfig(innerConfig, timeLimit);
        strategiesConfig.Add(timeLimitedConfig);
        var strategy = StrategyFactory.CreateStrategy(timeLimitedConfig);
        if (strategy != null)
        {
            strategies.Add(strategy);
        }
    }
}