import request from '@/utils/http'
import { UserDto, UserQueryInput, UserWithDetailsDto, UserUpdateInput } from './types'
export class UserService {
  static apiUrl = '/api/admin/accounts/users'
  static get(id: string) {
    return request.get<UserWithDetailsDto>({
      url: `${this.apiUrl}/${id}`
    })
  }

  static getList(input: UserQueryInput) {
    return request.get<Api.Query.PagedResult<UserDto>>({
      url: this.apiUrl,
      params: input
    })
  }

  static update(id: string, input: UserUpdateInput) {
    return request.put<UserWithDetailsDto>({
      url: `${this.apiUrl}/${id}`,
      data: input
    })
  }
}
