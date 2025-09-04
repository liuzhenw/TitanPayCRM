import request from '@/utils/http'
import {
  ReferrerWithDetailsDto,
  ReferrerUpdateInput,
  ReferrerDto,
  ReferrerQueryInput
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

  static update(id: string, input: ReferrerUpdateInput) {
    return request.put<ReferrerWithDetailsDto>({
      url: `${this.apiUrl}/${id}`,
      data: input
    })
  }
}
