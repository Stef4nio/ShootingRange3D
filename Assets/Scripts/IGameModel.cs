using System;
using System.Collections.Generic;

public interface IGameModel
{
    List<List<TargetModel>> Targets { get; }
    event EventHandler TargetDestroyed;
}