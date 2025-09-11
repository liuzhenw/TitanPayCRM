import request from '@/utils/http'
import { SettingGroupsDto, SettingItemDto, SettingUpdateInput } from './types'
export class SettingService {
  static apiUrl = '/api/admin/settings'
  static get(name: string) {
    return request.get<SettingItemDto>({
      url: `${this.apiUrl}/${name}`
    })
  }

  static getList() {
    return request.get<SettingGroupsDto>({
      url: this.apiUrl
    })
  }

  static update(input: SettingUpdateInput) {
    return request.put<void>({
      url: this.apiUrl,
      data: input
    })
  }

  static sendTestEmail(email: string) {
    return request.post<void>({
      url: `${this.apiUrl}/test/email`,
      data: { receiverEmail: email }
    })
  }
}
