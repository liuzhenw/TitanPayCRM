import request from '@/utils/http'
import { RoleDto } from './types'
export class RoleService {
  static apiUrl = '/api/admin/accounts/roles'
  static getList() {
    return request.get<RoleDto[]>({
      url: this.apiUrl
    })
  }
}
