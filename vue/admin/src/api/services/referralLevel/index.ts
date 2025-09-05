import request from '@/utils/http'
import { ReferralLevelDto, ReferralLevelCreateInput, ReferralLevelUpdateInput } from './types'

export class ReferralLevelService {
  static apiUrl = '/api/admin/referrals/levels'

  // Referral Level methods
  static getList() {
    return request.get<ReferralLevelDto[]>({
      url: this.apiUrl
    })
  }

  static create(input: ReferralLevelCreateInput) {
    return request.post<ReferralLevelDto>({
      url: this.apiUrl,
      data: input
    })
  }

  static update(id: string, input: ReferralLevelUpdateInput) {
    return request.put<ReferralLevelDto>({
      url: `${this.apiUrl}/${id}`,
      data: input
    })
  }

  static delete(id: string) {
    return request.del<void>({
      url: `${this.apiUrl}/${id}`
    })
  }

  static async getOptions() {
    const levels = await this.getList()
    return levels
      .sort((a, b) => a.size - b.size)
      .map((level) => ({
        label: level.name,
        value: level.id
      }))
  }
}
