using Squares.Domain.Identity;

namespace Squares.Domain.Games;
public class Game : BaseEntity
{
    public int UserId { get; set; }
    public int Score { get; set; } = default!;
    public DateTime DateTime { get; set; } = default!;

    public User User { get; set; } = default!;
}
