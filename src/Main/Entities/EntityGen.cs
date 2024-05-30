using Main.Components;
using Main.CoreGame.Base;

namespace Main.Entities;
internal static class EntityGen
{
    public static Entity Person(int ageInYears) => GameGlobals.CurrentGameState.Components.RegisterNewEntity(
            new Health { AgeInSeconds = GameConstants.SECONDS_IN_YEAR * ageInYears + GameRandom.NextInt(GameConstants.SECONDS_IN_YEAR) },
            new Hunger(),
            new Job());
}
