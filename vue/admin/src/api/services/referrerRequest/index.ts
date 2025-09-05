import request from '@/utils/http'
import { ReferrerRequestDto, ReferrerRequestQueryInput, ReferrerRequestRejectInput } from './types'
export class ReferrerRequestService {
  static apiUrl = '/api/admin/referrals/requests'

  static getList(params?: ReferrerRequestQueryInput) {
    return request.get<Api.Query.PagedResult<ReferrerRequestDto>>({
      url: this.apiUrl,
      params
    })
  }

  static approve(id: string) {
    return request.put<ReferrerRequestDto>({
      url: `${this.apiUrl}/${id}/approve`
    })
  }

  static reject(id: string, data: ReferrerRequestRejectInput) {
    return request.put<ReferrerRequestDto>({
      url: `${this.apiUrl}/${id}/reject`,
      data
    })
  }

  static getStatusOptions() {
    return [
      { label: '待处理', value: 'Pending' },
      { label: '已通过', value: 'Approved' },
      { label: '已拒绝', value: 'Rejected' },
      { label: '已禁用', value: 'Disabled' }
    ]
  }
}
