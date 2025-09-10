import request from '@/utils/http'
import {
  ReferrerWithDetailsDto,
  ReferrerUpdateInput,
  ReferrerDto,
  ReferrerQueryInput,
  ReferrerCreateInput,
  RecommendeeViewDto,
  RecommendeeViewQueryInput
} from './types'

export class ReferrerService {
  static apiUrl = '/api/admin/referrals/referrers'

  static getList(params?: ReferrerQueryInput) {
    return request.get<Api.Query.PagedResult<ReferrerDto>>({
      url: this.apiUrl,
      params
    })
  }

  static get(id: string) {
    return request.get<ReferrerWithDetailsDto>({
      url: `${this.apiUrl}/${id}`
    })
  }

  static create(input: ReferrerCreateInput) {
    return request.post<ReferrerWithDetailsDto>({
      url: this.apiUrl,
      data: input
    })
  }
  
  static update(id: string, input: ReferrerUpdateInput) {
    return request.put<ReferrerWithDetailsDto>({
      url: `${this.apiUrl}/${id}`,
      data: input
    })
  }

  static getRecommendees(params?: RecommendeeViewQueryInput) {
    return request.get<Api.Query.PagedResult<RecommendeeViewDto>>({
      url: `${this.apiUrl}/recommendees`,
      params
    })
  }
}
