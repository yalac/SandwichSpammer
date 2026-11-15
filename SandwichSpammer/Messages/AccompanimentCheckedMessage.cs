using CommunityToolkit.Mvvm.Messaging.Messages;
using SandwichSpammer.Models;

namespace SandwichSpammer.Messages;

public sealed class AccompanimentCheckedMessage
    : ValueChangedMessage<Accompaniment>
{
    public AccompanimentCheckedMessage(Accompaniment accompaniment)
        : base(accompaniment)
    {
    }
}