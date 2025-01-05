using Logistics_Supplier1_API.Models;
using Logistics_Supplier1_API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Logistics_Supplier1_API.Handlers;

/// <summary>
/// Handles API requests related to invite codes.
/// </summary>
public class InviteCodeHandler
{

    /// <summary>
    /// Gets all invite codes.
    /// </summary>
    /// <param name="inviteCodeRepository">The repository for accessing invite code data.</param>
    /// <returns>A list of all invite codes.</returns>
    [Authorize(Roles = "admin")]
    public static async Task<IResult> GetAllInviteCodes(IInviteCodeRepository inviteCodeRepository)
    {
        var codes = await inviteCodeRepository.GetAllInviteCodesAsync();
        if (codes == null || codes.Count == 0)
        {
            return Results.Ok("No invite codes to retreive");
        }
        return Results.Ok(codes);
    }

    /// <summary>
    /// Generates a new invite code.
    /// </summary>
    /// <param name="inviteCodeRepository">The repository for accessing invite code data.</param>
    /// <returns>The generated invite code.</returns>
    [Authorize(Roles = "admin")]
    public static async Task<IResult> GenerateInviteCode(IInviteCodeRepository inviteCodeRepository)
    {
        var result = await inviteCodeRepository.GenerateInviteCodeAsync();
        if (result == null)
        {
            return Results.BadRequest("Invite code generation failed, please contact an administrator.");
        }

        return Results.Ok(result);

    }

}