using Squares.Application.Identity.Users;
using Squares.Domain.Games;

namespace Squares.Application.Games;

[AutoMap(typeof(Game))]
public class GameDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int Score { get; set; } = default!;
    public DateTime DateTime { get; set; } = default!;

    public UserDto User { get; set; } = default!;
}
