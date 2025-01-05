using Logistics_Supplier1_API.Data;
using Logistics_Supplier1_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Logistics_Supplier1_API.Repositories;

/// <summary>
/// Provides methods for interacting with InviteCode data in the database.
/// </summary>
public class InviteCodeRepository : IInviteCodeRepository
{
    private readonly MyDbContext _context;

    /// <summary>
    /// Initializes a new instance of the InviteCodeRepository class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public InviteCodeRepository(MyDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets all invite codes asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task result contains a list of all invite codes.</returns>
    public async Task<List<InviteCode>> GetAllInviteCodesAsync()
    {
        var inviteCodes = await _context.InviteCodes.ToListAsync();
        return inviteCodes;
    }

    /// <summary>
    /// Checks if an invite code is valid asynchronously and invalidates the code.
    /// </summary>
    /// <param name="inviteCode">The invite code to check.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains true if the invite code is valid, otherwise false.</returns>
    public async Task<bool> IsValidInviteCodeAsync(string inviteCode)
    {
        var fetchedCode = await _context.InviteCodes.FirstOrDefaultAsync(c => c.Code == inviteCode);

        // Null code
        if (fetchedCode == null)
        {
            return false;
        }

        // Expired
        if (fetchedCode.ExpirationDate != null && fetchedCode.ExpirationDate < DateTime.Now)
        {
            return false;
        }

        // Used before
        if (fetchedCode.UsageCount != 0)
        {
            return false;
        }

        try
        {
            
            fetchedCode.updateUsageCount(fetchedCode.UsageCount + 1);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return false;
        }
        
        return true;
    }

    /// <summary>
    /// Generates a new invite code asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task result contains the generated invite code.</returns>
    public async Task<InviteCode> GenerateInviteCodeAsync()
    {
        string newCode = Guid.NewGuid().ToString();
        DateTime expiryDate = DateTime.Now.AddDays(3);
        var inviteCode = new InviteCode().CreateNewInviteCode(newCode, 0, expiryDate);
        try
        {
            await _context.InviteCodes.AddAsync(inviteCode);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return null;
        }

        return inviteCode;
    }
}