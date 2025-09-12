import request from '@/utils/http'
import { AncestorDto, RecommendeeDto, RecommendeeQueryInput, ReferralRelationDto, ReferralRelationQueryInput } from './types'
export class ReferralRelationService {
  static apiUrl = '/api/admin/referrals/relations'

  static getList(input: ReferralRelationQueryInput) {
    return request.get<Api.Query.PagedResult<ReferralRelationDto>>({
      url: this.apiUrl,
      params: input
    })
  }

  static getRecommendees(id:string, params?: RecommendeeQueryInput) {
    return request.get<Api.Query.PagedResult<RecommendeeDto>>({
      url: `${this.apiUrl}/${id}/recommendees`,
      params
    })
  }

  static getAncestors(id: string) {
    return request.get<AncestorDto[]>({
      url: `${this.apiUrl}/${id}/ancestors`
    })
  }
}
