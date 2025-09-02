using System;
using Astra;

namespace Crm.Referrals;

public interface IReferrerRequestRepository :
    IAstraBasicWithPagedBasicRepository<ReferrerRequest, Guid, ReferrerRequestPagedParameter> { }