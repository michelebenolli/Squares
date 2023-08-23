using Squares.Application.Common.Exceptions;
using Squares.Application.Common.Mailing;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Squares.Application.Identity.Account.Requests;

namespace Squares.Infrastructure.Identity;

internal partial class UserService
{
    public async Task ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
    {
        EnsureValidTenant();

        var user = await _userManager.FindByEmailAsync(request.Email.Normalize());
        if (user != null && await _userManager.IsEmailConfirmedAsync(user))
        {
            await SendPasswordResetEmailAsync(user, origin);
        }
    }

    public async Task ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email.Normalize());
        if (user == null) return;

        request.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

        if (result.Succeeded)
        {
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _userManager.ConfirmEmailAsync(user, token);
            return;
        }

        throw new InternalServerException(_localizer["Operazione non riuscita"]);
    }

    public async Task ChangePasswordAsync(ChangePasswordRequest model, int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        _ = user ?? throw new NotFoundException(_localizer["Utente non trovato"]);

        var result = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
        if (!result.Succeeded)
        {
            throw new InternalServerException(_localizer["Operazione non riuscita"]);
        }
    }

    private async Task SendPasswordResetEmailAsync(ApplicationUser user, string origin)
    {
        string uri = await GetPasswordResetUriAsync(user, origin);

        var email = new EmailModel()
        {
            Title = _localizer["Hai dimenticato la password?"],
            Text = _localizer["Ciao, abbiamo ricevuto una richiesta di ripristino della password per il tuo account. Clicca sul seguente link per cambiarla."],
            ButtonText = _localizer["Reset password"],
            ButtonLink = uri
        };
        var mailRequest = new MailRequest(
            new List<string> { user.Email! },
            _localizer["Reset password"],
            _templateService.GenerateEmailTemplate("identity", email));

        _jobService.Enqueue(() => _mailService.SendAsync(mailRequest, CancellationToken.None));
    }
}