using Ardalis.Specification;
using Squares.Application.Common.Models;
using Squares.Application.Common.Specification;
using Squares.Application.Identity.Users.Requests;

namespace Squares.Infrastructure.Identity.Users;

public class SearchUserSpec : SearchSpec<ApplicationUser>
{
    public SearchUserSpec(SearchUserRequest request)
        : base(request)
    {
        string? fullName = request.FullName?.Trim();

        Query.Where(
                x => (x.User.LastName + x.User.FirstName).Contains(fullName!) ||
                (x.User.FirstName + x.User.LastName).Contains(fullName!),
                fullName != null)
            .Where(x => x.IsActive == request.IsActive, request.IsActive != null)
            .OrderBy(x => x.User.FirstName, !request.HasOrderBy())
            .ThenBy(x => x.User.LastName, !request.HasOrderBy())
            .Include(x => x.User)
            .AsNoTracking();
    }
}