using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;

namespace Logistics_Supplier1_API.Models;

/// <summary>
/// Represents an invite code in the system.
/// </summary>
public class InviteCode
{
    /// <summary>
    /// Gets or sets the unique identifier of the invite code.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Gets or sets the code itself.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Code { get; private set; }

    /// <summary>
    /// Gets or sets the number of times the code has been used.
    /// </summary>
    public int UsageCount { get; private set; }

    /// <summary>
    /// Gets or sets the expiration date of the invite code.
    /// </summary>
    public DateTime ExpirationDate { get; private set; }

    /// <summary>
    /// Creates a new invite code with the specified details.
    /// </summary>
    /// <param name="newCode">The code itself.</param>
    /// <param name="usageCount">The initial usage count.</param>
    /// <param name="expirationDate">The expiration date of the code.</param>
    /// <returns>The created invite code instance.</returns>
    public InviteCode CreateNewInviteCode(string newCode, int usageCount, DateTime expirationDate)
    {
        Code = newCode;
        UsageCount = usageCount;
        ExpirationDate = expirationDate;

        return this;
    }

    /// <summary>
    /// Updates invite codes usage count
    /// </summary>
    /// <param name="usageCount">The amount of times a code has been used.</param>
    public void updateUsageCount(int usageCount)
    {
        UsageCount = usageCount;
    }
}