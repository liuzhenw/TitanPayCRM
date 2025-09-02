using System.Threading.Tasks;
using Astra;

namespace Crm.Referrals;

public interface IReferrerLevelRepository : IAstraBasicRepository<ReferralLevel, string>
{
    Task<bool> ExistsAsync(uint size);
}