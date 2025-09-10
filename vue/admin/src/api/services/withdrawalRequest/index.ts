import request from '@/utils/http'
import {
  WithdrawalRequestDto,
  WithdrawalRequestQueryInput,
  WithdrawalRequestApproveInput,
  WithdrawalRequestRejectInput
} from './types'

export class WithdrawalRequestService {
  static apiUrl = '/api/admin/referrals/withdrawal-requests'

  static getList(params?: WithdrawalRequestQueryInput) {
    return request.get<Api.Query.PagedResult<WithdrawalRequestDto>>({
      url: this.apiUrl,
      params
    })
  }

  static get(id: string) {
    return request.get<WithdrawalRequestDto>({
      url: `${this.apiUrl}/${id}`
    })
  }

  static approve(id: string, input: WithdrawalRequestApproveInput) {
    return request.put<WithdrawalRequestDto>({
      url: `${this.apiUrl}/${id}/approve`,
      data: input
    })
  }

  static reject(id: string, input: WithdrawalRequestRejectInput) {
    return request.put<WithdrawalRequestDto>({
      url: `${this.apiUrl}/${id}/reject`,
      data: input
    })
  }
}