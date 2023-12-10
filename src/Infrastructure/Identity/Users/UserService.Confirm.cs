using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Squares.Application.Common.Exceptions;
using Squares.Application.Common.Mailing;
using Squares.Application.Identity.Account.Requests;
using Squares.Infrastructure.Common;
using Squares.Shared.Multitenancy;
using System.Text;

namespace Squares.Infrastructure.Identity;
internal partial class UserService
{
    public async Task ConfirmEmailAsync(int userId, string code, CancellationToken token)
    {
        EnsureValidTenant();

        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId && !x.EmailConfirmed, token);
        if (user != null)
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded) return;
        }

        throw new InternalServerException(_localizer["Si è verificato un errore"]);
    }

    public async Task ReconfirmEmailAsync(ConfirmEmailRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email.Normalize());
        if (user != null && !await _userManager.IsEmailConfirmedAsync(user))
        {
            await SendInvitationEmailAsync(user);
        }
    }

    private async Task SendInvitationEmailAsync(ApplicationUser user)
    {
        string uri = await GetPasswordResetUriAsync(user);

        var email = new EmailModel()
        {
            Title = _localizer["Conferma account"],
            Text = _localizer["Ciao, è un piacere averti tra di noi!<br/>Clicca sul seguente link per confermare il tuo account"],
            ButtonText = _localizer["Conferma account"],
            ButtonLink = uri,
            Footer = string.Format(_localizer["Se il pulsante non funziona, copia e incolla il seguente URL nel browser: {0}"], uri)
        };

        var mailRequest = new MailRequest(
            new List<string> { user.Email! },
            _localizer["Conferma account"],
            _templateService.GenerateEmailTemplate("identity", email));

        _jobService.Enqueue(() => _mailService.SendAsync(mailRequest, CancellationToken.None));
    }

    private async Task<string> GetPasswordResetUriAsync(ApplicationUser user)
    {
        var tenant = await GetTenant();
        string uri = new Uri($"{tenant.Domain}/reset-password/").ToString();
        string token = await _userManager.GeneratePasswordResetTokenAsync(user);
        token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        uri = QueryHelpers.AddQueryString(uri, QueryStringKeys.Email, user.Email!);
        uri = QueryHelpers.AddQueryString(uri, QueryStringKeys.Token, token);
        uri = QueryHelpers.AddQueryString(uri, MultitenancyConstants.TenantIdName, _currentTenant.Id!);
        return uri;
    }
}