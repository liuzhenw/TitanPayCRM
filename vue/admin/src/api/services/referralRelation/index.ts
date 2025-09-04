import request from '@/utils/http'
import { ReferralRelationDto, ReferralRelationQueryInput } from './types'
export class ReferralRelationService {
  static apiUrl = '/api/admin/referrals/relations'

  static getList(input: ReferralRelationQueryInput) {
    return request.get<Api.Query.PagedResult<ReferralRelationDto>>({
      url: this.apiUrl,
      params: input
    })
  }
}
