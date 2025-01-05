using Logistics_Supplier1_API.Models;

namespace Logistics_Supplier1_API.Repositories;

// IInviteCodeRepository.cs

/// <summary>
/// Defines methods for interacting with InviteCode data.
/// </summary>
public interface IInviteCodeRepository
{
    /// <summary>
    /// Gets all invite codes asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task result contains a list of all invite codes.</returns>
    Task<List<InviteCode>> GetAllInviteCodesAsync();

    /// <summary>
    /// Checks if an invite code is valid asynchronously.
    /// </summary>
    /// <param name="inviteCode">The invite code to check.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains true if the invite code is valid, otherwise false.</returns>
    Task<bool> IsValidInviteCodeAsync(string inviteCode);

    /// <summary>
    /// Generates a new invite code asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task result contains the generated invite code.</returns>
    Task<InviteCode> GenerateInviteCodeAsync();
    
}