using Main.CoreGame.Base;

namespace Main.CoreGame;
internal class GameManager
{
    public static int SequenceNumber { get; set; } = 0;

    public static Entity CreateEntity()
    {
        var e = new Entity
        {
            Id = GenerateEntityId()
        };

        GameGlobals.CurrentGameState.Entities.Register(e);

        return e;
    }

    public static ulong GenerateEntityId() =>
        ((ulong)GameGlobals.CurrentGameState.FramesPassed * 1000) + (ulong)SequenceNumber;
}
