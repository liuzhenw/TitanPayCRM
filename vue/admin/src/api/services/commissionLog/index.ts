import request from '@/utils/http'
import { CommissionLogDto, CommissionLogQueryInput } from './types'

export class CommissionLogService {
  static apiUrl = '/api/admin/referrals/commission-logs'

  static getList(params?: CommissionLogQueryInput) {
    return request.get<Api.Query.PagedResult<CommissionLogDto>>({
      url: this.apiUrl,
      params
    })
  }
}
