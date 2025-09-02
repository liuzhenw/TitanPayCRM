using System;
using Astra;

namespace Crm.Referrals;

public interface ICommissionLogRepository :
    IAstraBasicWithPagedBasicRepository<CommissionLog, Guid, CommissionLogPagedParameter> { }