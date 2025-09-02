using System;
using Astra;

namespace Crm.Referrals;

public interface IWithdrawalRequestRepository :
    IAstraBasicWithPagedBasicRepository<WithdrawalRequest, Guid, WithdrawalRequestPagedParameter> { }